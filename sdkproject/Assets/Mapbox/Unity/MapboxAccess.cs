using System;
using System.IO;
using Mapbox.Geocoding;
using Mapbox.Map;
using Mapbox.MapMatching;
using Mapbox.Platform;
using Mapbox.Platform.Cache;
using Mapbox.Platform.TilesetTileJSON;
using Mapbox.Tokens;
using Mapbox.Unity.Telemetry;
using MapboxAccountsUnity;
using UnityEngine;

namespace Mapbox.Unity
{
	/// <summary>
	///     Object for retrieving an API token and making http requests.
	///     Contains a lazy <see cref="T:Mapbox.Geocoding.Geocoder">Geocoder</see> and a lazy
	///     <see cref="T:Mapbox.Directions.Directions">Directions</see> for convenience.
	/// </summary>
	public class MapboxAccess : IFileSource
	{
		public delegate void TokenValidationEvent(MapboxTokenStatus response);

		private static MapboxAccess _instance;


		public static bool Configured;
		public static string ConfigurationJSON;


		private Directions.Directions _directions;
		private CachingWebFileSource _fileSource;


		private Geocoder _geocoder;

		private MapMatcher _mapMatcher;
		private ITelemetryLibrary _telemetryLibrary;


		private TileJSON _tileJson;

		private readonly string _tokenNotSetErrorMessage =
			"No configuration file found! Configure your access token from the Mapbox > Setup menu.";


		private MapboxTokenApi _tokenValidator;

		private MapboxAccess()
		{
			LoadAccessToken();
			if (null == Configuration || string.IsNullOrEmpty(Configuration.AccessToken))
				Debug.LogError(_tokenNotSetErrorMessage);
		}

		/// <summary>
		///     The singleton instance.
		/// </summary>
		public static MapboxAccess Instance
		{
			get
			{
				if (_instance == null) _instance = new MapboxAccess();
				return _instance;
			}
		}

		/// <summary>
		///     The Mapbox API access token.
		/// </summary>
		public MapboxConfiguration Configuration { get; private set; }

		/// <summary>
		///     Lazy geocoder.
		/// </summary>
		public Geocoder Geocoder
		{
			get
			{
				if (_geocoder == null)
					_geocoder = new Geocoder(new FileSource(Instance.Configuration.GetMapsSkuToken,
						Configuration.AccessToken));
				return _geocoder;
			}
		}

		/// <summary>
		///     Lazy Directions.
		/// </summary>
		public Directions.Directions Directions
		{
			get
			{
				if (_directions == null)
					_directions = new Directions.Directions(new FileSource(Instance.Configuration.GetMapsSkuToken,
						Configuration.AccessToken));
				return _directions;
			}
		}

		/// <summary>
		///     Lazy Map Matcher.
		/// </summary>
		public MapMatcher MapMatcher
		{
			get
			{
				if (_mapMatcher == null)
					_mapMatcher =
						new MapMatcher(
							new FileSource(Instance.Configuration.GetMapsSkuToken, Configuration.AccessToken),
							Configuration.DefaultTimeout);
				return _mapMatcher;
			}
		}

		/// <summary>
		///     Lazy token validator.
		/// </summary>
		public MapboxTokenApi TokenValidator
		{
			get
			{
				if (_tokenValidator == null) _tokenValidator = new MapboxTokenApi();
				return _tokenValidator;
			}
		}

		/// <summary>
		///     Lazy TileJSON wrapper: https://www.mapbox.com/api-documentation/maps/#retrieve-tilejson-metadata
		/// </summary>
		public TileJSON TileJSON
		{
			get
			{
				if (_tileJson == null)
					_tileJson = new TileJSON(
						new FileSource(Instance.Configuration.GetMapsSkuToken, Configuration.AccessToken),
						Configuration.DefaultTimeout);
				return _tileJson;
			}
		}

		/// <summary>
		///     Makes an asynchronous url query.
		/// </summary>
		/// <returns>The request.</returns>
		/// <param name="url">URL.</param>
		/// <param name="callback">Callback.</param>
		public IAsyncRequest Request(
			string url
			, Action<Response> callback
			, int timeout = 10
			, CanonicalTileId tileId = new()
			, string tilesetId = null
		)
		{
			return _fileSource.Request(url, callback, Configuration.DefaultTimeout, tileId, tilesetId);
		}

		public event TokenValidationEvent OnTokenValidation;

		public void SetConfiguration(MapboxConfiguration configuration, bool throwExecptions = true)
		{
			if (configuration == null)
				if (throwExecptions)
					throw new InvalidTokenException(_tokenNotSetErrorMessage);

			if (null == configuration || string.IsNullOrEmpty(configuration.AccessToken))
			{
				Debug.LogError(_tokenNotSetErrorMessage);
			}
			else
			{
				TokenValidator.Retrieve(configuration.GetMapsSkuToken, configuration.AccessToken, response =>
				{
					if (OnTokenValidation != null) OnTokenValidation(response.Status);

					if (response.Status != MapboxTokenStatus.TokenValid
					    && throwExecptions)
					{
						configuration.AccessToken = string.Empty;
						Debug.LogError(new InvalidTokenException(response.Status.ToString().ToString()));
					}
				});

				Configuration = configuration;

				ConfigureFileSource();
				ConfigureTelemetry();

				Configured = true;
			}
		}


		public void ClearAllCacheFiles()
		{
			// explicity call Clear() to close any connections that might be referenced by the current scene
			var cwfs = _fileSource;
			if (null != cwfs) cwfs.Clear();

			// remove all left over files (eg orphaned .journal) from the cache directory
			var cacheDirectory = Path.Combine(Application.persistentDataPath, "cache");
			if (!Directory.Exists(cacheDirectory)) return;

			foreach (var file in Directory.GetFiles(cacheDirectory))
				try
				{
					File.Delete(file);
				}
				catch (Exception deleteEx)
				{
					Debug.LogErrorFormat("Could not delete [{0}]: {1}", file, deleteEx);
				}

			//reinit caches after clear
			if (null != cwfs) cwfs.ReInit();

			Debug.Log("done clearing caches");
		}

		/// <summary>
		///     Loads the access token from
		///     <see href="https://docs.unity3d.com/Manual/BestPracticeUnderstandingPerformanceInUnity6.html">Resources folder</see>
		///     .
		/// </summary>
		private void LoadAccessToken()
		{
			if (string.IsNullOrEmpty(ConfigurationJSON))
			{
				var configurationTextAsset = Resources.Load<TextAsset>(Constants.Path.MAPBOX_RESOURCES_RELATIVE);
				if (null == configurationTextAsset) throw new InvalidTokenException(_tokenNotSetErrorMessage);
				ConfigurationJSON = configurationTextAsset.text;
			}

#if !WINDOWS_UWP
			var test = JsonUtility.FromJson<MapboxConfiguration>(ConfigurationJSON);
			SetConfiguration(ConfigurationJSON == null ? null : test);
#else
			SetConfiguration(ConfigurationJSON == null ? null : Mapbox.Json.JsonConvert.DeserializeObject<MapboxConfiguration>(ConfigurationJSON));
#endif
		}


		private void ConfigureFileSource()
		{
			_fileSource = new CachingWebFileSource(Configuration.AccessToken, Configuration.GetMapsSkuToken,
						Configuration.AutoRefreshCache)
					.AddCache(new MemoryCache(Configuration.MemoryCacheSize))
#if !UNITY_WEBGL
					.AddCache(new SQLiteCache(Configuration.FileCacheSize))
#endif
				;
		}


		private void ConfigureTelemetry()
		{
			// TODO: enable after token validation has been made async
			//if (
			//	null == _configuration
			//	|| string.IsNullOrEmpty(_configuration.AccessToken)
			//	|| !_tokenValid
			//)
			//{
			//	Debug.LogError(_tokenNotSetErrorMessage);
			//	return;
			//}
			try
			{
				_telemetryLibrary = TelemetryFactory.GetTelemetryInstance();
				_telemetryLibrary.Initialize(Configuration.AccessToken);
				_telemetryLibrary.SetLocationCollectionState(GetTelemetryCollectionState());
				_telemetryLibrary.SendTurnstile();
			}
			catch (Exception ex)
			{
				Debug.LogErrorFormat("Error initializing telemetry: {0}", ex);
			}
		}

		public void SetLocationCollectionState(bool enable)
		{
			PlayerPrefs.SetInt(Constants.Path.SHOULD_COLLECT_LOCATION_KEY, enable ? 1 : 0);
			PlayerPrefs.Save();
			_telemetryLibrary.SetLocationCollectionState(enable);
		}

		private bool GetTelemetryCollectionState()
		{
			if (!PlayerPrefs.HasKey(Constants.Path.SHOULD_COLLECT_LOCATION_KEY))
				PlayerPrefs.SetInt(Constants.Path.SHOULD_COLLECT_LOCATION_KEY, 1);
			return PlayerPrefs.GetInt(Constants.Path.SHOULD_COLLECT_LOCATION_KEY) != 0;
		}


		private class InvalidTokenException : Exception
		{
			public InvalidTokenException(string message) : base(message)
			{
			}
		}
	}

	public class MapboxConfiguration
	{
		public string AccessToken;
		public bool AutoRefreshCache = false;
		public int DefaultTimeout = 30;
		public uint FileCacheSize = 2500;
		[NonSerialized] private readonly MapboxAccounts mapboxAccounts = new();
		public uint MemoryCacheSize = 500;

		public string GetMapsSkuToken()
		{
			return mapboxAccounts.ObtainMapsSkuUserToken(Application.persistentDataPath);
		}
	}
}
