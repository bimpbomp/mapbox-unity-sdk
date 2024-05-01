using System;
using System.IO;
using System.Text;
using Mapbox.Utils;
using UnityEngine;

namespace Mapbox.Unity.Location
{
	/// <summary>
	///     Writes location data into Application.persistentDataPath
	/// </summary>
	public class LocationLogWriter : LocationLogAbstractBase, IDisposable
	{
		private bool _disposed;
		private FileStream _fileStream;
		private long _lineCount;
		private TextWriter _textWriter;


		public LocationLogWriter()
		{
			var fileName = "MBX-location-log-" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".txt";
			var persistentPath = Application.persistentDataPath;
			var fullFilePathAndName = Path.Combine(persistentPath, fileName);
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN || UNITY_WSA
			// use `GetFullPath` on that to sanitize the path: replaces `/` returned by `Application.persistentDataPath` with `\`
			fullFilePathAndName = Path.GetFullPath(fullFilePathAndName);
#endif
			Debug.Log("starting new log file: " + fullFilePathAndName);

			_fileStream = new FileStream(fullFilePathAndName, FileMode.Create, FileAccess.Write);
			_textWriter = new StreamWriter(_fileStream, new UTF8Encoding(false));
			_textWriter.WriteLine("#" + string.Join(Delimiter, HeaderNames));
		}


		public void Write(Location location)
		{
			string[] lineTokens =
			{
				location.IsLocationServiceEnabled.ToString(), location.IsLocationServiceInitializing.ToString(),
				location.IsLocationUpdated.ToString(), location.IsUserHeadingUpdated.ToString(), location.Provider,
				LocationProviderFactory.Instance.DefaultLocationProvider.GetType().Name,
				DateTime.UtcNow.ToString("yyyyMMdd-HHmmss.fff"),
				UnixTimestampUtils.From(location.Timestamp).ToString("yyyyMMdd-HHmmss.fff"),
				string.Format(_invariantCulture, "{0:0.00000000}", location.LatitudeLongitude.x),
				string.Format(_invariantCulture, "{0:0.00000000}", location.LatitudeLongitude.y),
				string.Format(_invariantCulture, "{0:0.0}", location.Accuracy),
				string.Format(_invariantCulture, "{0:0.0}", location.UserHeading),
				string.Format(_invariantCulture, "{0:0.0}", location.DeviceOrientation),
				nullableAsStr(location.SpeedKmPerHour, "{0:0.0}"), nullableAsStr(location.HasGpsFix, "{0}"),
				nullableAsStr(location.SatellitesUsed, "{0}"), nullableAsStr(location.SatellitesInView, "{0}")
			};

			_lineCount++;
			var logMsg = string.Join(Delimiter, lineTokens);
			Debug.Log(logMsg);
			_textWriter.WriteLine(logMsg);
			_textWriter.Flush();
		}


		private string nullableAsStr<T>(T? val, string formatString = null) where T : struct
		{
			if (null == val && null == formatString) return "[not supported by provider]";
			if (null == val && null != formatString)
				return string.Format(_invariantCulture, formatString, "[not supported by provider]");
			if (null != val && null == formatString) return val.Value.ToString();
			return string.Format(_invariantCulture, formatString, val);
		}


		#region idisposable

		~LocationLogWriter()
		{
			Dispose(false);
		}


		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}


		protected virtual void Dispose(bool disposeManagedResources)
		{
			if (!_disposed)
			{
				if (disposeManagedResources)
				{
					Debug.LogFormat("{0} locations logged", _lineCount);
					if (null != _textWriter)
					{
						_textWriter.Flush();
						_fileStream.Flush();
#if !NETFX_CORE
						_textWriter.Close();
#endif
						_textWriter.Dispose();
						_fileStream.Dispose();

						_textWriter = null;
						_fileStream = null;
					}
				}

				_disposed = true;
			}
		}

		#endregion
	}
}
