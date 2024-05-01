using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mapbox.Unity.Location
{
	/// <summary>
	///     Class to mock Unity's location service Input.location
	/// </summary>
	public class MapboxLocationServiceMock : IMapboxLocationService, IDisposable
	{
		private bool _disposed;
		private bool _isRunning;
		private IEnumerator<Location> _locationEnumerator;


		private LocationLogReader _logReader;


		public MapboxLocationServiceMock(byte[] locationLogFileContents)
		{
			if (null == locationLogFileContents || locationLogFileContents.Length < 1)
				throw new ArgumentNullException("locationLogFileContents");

			_logReader = new LocationLogReader(locationLogFileContents);
			_locationEnumerator = _logReader.GetLocations();
		}


		public bool isEnabledByUser => true;


		public LocationServiceStatus status =>
			_isRunning ? LocationServiceStatus.Running : LocationServiceStatus.Stopped;


		public IMapboxLocationInfo lastData
		{
			get
			{
				if (null == _locationEnumerator) return new MapboxLocationInfoMock();
				// no need to check if 'MoveNext()' returns false as LocationLogReader loops through log file
				_locationEnumerator.MoveNext();
				var currentLocation = _locationEnumerator.Current;

				return new MapboxLocationInfoMock(currentLocation);
			}
		}


		public void Start(float desiredAccuracyInMeters, float updateDistanceInMeters)
		{
			_isRunning = true;
		}


		public void Stop()
		{
			_isRunning = false;
		}


		#region idisposable

		~MapboxLocationServiceMock()
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
					if (null != _locationEnumerator)
					{
						_locationEnumerator.Dispose();
						_locationEnumerator = null;
					}

					if (null != _logReader)
					{
						_logReader.Dispose();
						_logReader = null;
					}
				}

				_disposed = true;
			}
		}

		#endregion
	}
}
