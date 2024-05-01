using System.Collections;
using Mapbox.Unity.Location;
using UnityEngine;

namespace Mapbox.Unity.Map
{
	public class InitializeMapWithLocationProvider : MonoBehaviour
	{
		[SerializeField] private AbstractMap _map;

		private ILocationProvider _locationProvider;

		private void Awake()
		{
			// Prevent double initialization of the map. 
			_map.InitializeOnStart = false;
		}

		protected virtual IEnumerator Start()
		{
			yield return null;
			_locationProvider = LocationProviderFactory.Instance.DefaultLocationProvider;
			_locationProvider.OnLocationUpdated += LocationProvider_OnLocationUpdated;
			;
		}

		private void LocationProvider_OnLocationUpdated(Location.Location location)
		{
			_locationProvider.OnLocationUpdated -= LocationProvider_OnLocationUpdated;
			_map.Initialize(location.LatitudeLongitude, _map.AbsoluteZoom);
		}
	}
}
