using System;
using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using UnityEngine;

namespace Mapbox.Unity.Location
{
	/// <summary>
	///     The EditorLocationProvider is responsible for providing mock location and heading data
	///     for testing purposes in the Unity editor.
	/// </summary>
	public class EditorLocationProvider : AbstractEditorLocationProvider
	{
		/// <summary>
		///     The mock "latitude, longitude" location, respresented with a string.
		///     You can search for a place using the embedded "Search" button in the inspector.
		///     This value can be changed at runtime in the inspector.
		/// </summary>
		[SerializeField] [Geocode] private string _latitudeLongitude;

		/// <summary>
		///     The transform that will be queried for location and heading data & ADDED to the mock latitude/longitude
		///     Can be changed at runtime to simulate moving within the map.
		/// </summary>
		[SerializeField] private Transform _targetTransform;

		private AbstractMap _map;

		private bool _mapInitialized;

		private Vector2d LatitudeLongitude
		{
			get
			{
				if (_mapInitialized)
				{
					var startingLatLong = Conversions.StringToLatLon(_latitudeLongitude);
					var position = Conversions.GeoToWorldPosition(
						startingLatLong,
						_map.CenterMercator,
						_map.WorldRelativeScale
					).ToVector3xz();
					position += _targetTransform.position;
					return position.GetGeoPosition(_map.CenterMercator, _map.WorldRelativeScale);
				}

				return Conversions.StringToLatLon(_latitudeLongitude);
			}
		}

#if UNITY_EDITOR
		protected virtual void Start()
		{
			LocationProviderFactory.Instance.mapManager.OnInitialized += Map_OnInitialized;

			if (_targetTransform == null) _targetTransform = transform;

			base.Awake();
		}
#endif

		private void Map_OnInitialized()
		{
			LocationProviderFactory.Instance.mapManager.OnInitialized -= Map_OnInitialized;
			_mapInitialized = true;
			_map = LocationProviderFactory.Instance.mapManager;
		}

		protected override void SetLocation()
		{
			_currentLocation.UserHeading = _targetTransform.eulerAngles.y;
			_currentLocation.LatitudeLongitude = LatitudeLongitude;
			_currentLocation.Accuracy = _accuracy;
			_currentLocation.Timestamp = UnixTimestampUtils.To(DateTime.UtcNow);
			_currentLocation.IsLocationUpdated = true;
			_currentLocation.IsUserHeadingUpdated = true;
			_currentLocation.IsLocationServiceEnabled = true;
		}
	}
}
