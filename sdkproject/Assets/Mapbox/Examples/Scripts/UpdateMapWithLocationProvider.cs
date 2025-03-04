﻿using System.Collections;
using Mapbox.Unity.Location;
using Mapbox.Utils;
using UnityEngine;

namespace Mapbox.Unity.Map
{
	public class UpdateMapWithLocationProvider : MonoBehaviour
	{
		[SerializeField] private AbstractMap _map;

		/// <summary>
		///     The time taken to move from the start to finish positions
		/// </summary>
		public float timeTakenDuringLerp = 1f;

		private Vector2d _endLatlong;
		private Vector3 _endPosition;

		//Whether we are currently interpolating or not
		private bool _isLerping;
		private bool _isMapInitialized;

		private ILocationProvider _locationProvider;

		private Vector2d _startLatLong;

		//The start and finish positions for the interpolation
		private Vector3 _startPosition;
		private Vector3 _targetPosition;

		//The Time.time value when we started the interpolation
		private float _timeStartedLerping;

		private void Awake()
		{
			// Prevent double initialization of the map. 
			_map.InitializeOnStart = false;
		}

		private IEnumerator Start()
		{
			yield return null;
			_locationProvider = LocationProviderFactory.Instance.DefaultLocationProvider;
			_locationProvider.OnLocationUpdated += LocationProvider_OnFirstLocationUpdate;
		}

		//We do the actual interpolation in FixedUpdate(), since we're dealing with a rigidbody
		private void LateUpdate()
		{
			if (_isMapInitialized && _isLerping)
			{
				//We want percentage = 0.0 when Time.time = _timeStartedLerping
				//and percentage = 1.0 when Time.time = _timeStartedLerping + timeTakenDuringLerp
				//In other words, we want to know what percentage of "timeTakenDuringLerp" the value
				//"Time.time - _timeStartedLerping" is.
				var timeSinceStarted = Time.time - _timeStartedLerping;
				var percentageComplete = timeSinceStarted / timeTakenDuringLerp;

				//Perform the actual lerping.  Notice that the first two parameters will always be the same
				//throughout a single lerp-processs (ie. they won't change until we hit the space-bar again
				//to start another lerp)
				_startPosition = _map.GeoToWorldPosition(_startLatLong, false);
				_endPosition = _map.GeoToWorldPosition(_endLatlong, false);
				var position = Vector3.Lerp(_startPosition, _endPosition, percentageComplete);
				var latLong = _map.WorldToGeoPosition(position);
				_map.UpdateMap(latLong, _map.Zoom);

				//When we've completed the lerp, we set _isLerping to false
				if (percentageComplete >= 1.0f) _isLerping = false;
			}
		}

		private void LocationProvider_OnFirstLocationUpdate(Location.Location location)
		{
			_locationProvider.OnLocationUpdated -= LocationProvider_OnFirstLocationUpdate;
			_map.OnInitialized += () =>
			{
				_isMapInitialized = true;
				_locationProvider.OnLocationUpdated += LocationProvider_OnLocationUpdated;
			};
			_map.Initialize(location.LatitudeLongitude, _map.AbsoluteZoom);
		}

		private void LocationProvider_OnLocationUpdated(Location.Location location)
		{
			if (_isMapInitialized && location.IsLocationUpdated) StartLerping(location);
		}

		/// <summary>
		///     Called to begin the linear interpolation
		/// </summary>
		private void StartLerping(Location.Location location)
		{
			_isLerping = true;
			_timeStartedLerping = Time.time;
			timeTakenDuringLerp = Time.deltaTime;

			//We set the start position to the current position
			_startLatLong = _map.CenterLatitudeLongitude;
			_endLatlong = location.LatitudeLongitude;
			_startPosition = _map.GeoToWorldPosition(_startLatLong, false);
			_endPosition = _map.GeoToWorldPosition(_endLatlong, false);
		}
	}
}
