using UnityEngine;

namespace Mapbox.Unity.Location
{
	/// <summary>
	///     Wrap Unity's LocationService into MapboxLocationService
	/// </summary>
	public class MapboxLocationServiceUnityWrapper : IMapboxLocationService
	{
		public bool isEnabledByUser => Input.location.isEnabledByUser;


		public LocationServiceStatus status => Input.location.status;


		public IMapboxLocationInfo lastData => new MapboxLocationInfoUnityWrapper(Input.location.lastData);


		public void Start(float desiredAccuracyInMeters, float updateDistanceInMeters)
		{
			Input.location.Start(desiredAccuracyInMeters, updateDistanceInMeters);
		}


		public void Stop()
		{
			Input.location.Stop();
		}
	}
}
