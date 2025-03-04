﻿using Mapbox.Unity.Location;
using Mapbox.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Mapbox.Examples
{
	public class LocationStatus : MonoBehaviour
	{
		[SerializeField] private Text _statusText;

		private AbstractLocationProvider _locationProvider;

		private void Start()
		{
			if (null == _locationProvider)
				_locationProvider =
					LocationProviderFactory.Instance.DefaultLocationProvider as AbstractLocationProvider;
		}


		private void Update()
		{
			var currLoc = _locationProvider.CurrentLocation;

			if (currLoc.IsLocationServiceInitializing)
			{
				_statusText.text = "location services are initializing";
			}
			else
			{
				if (!currLoc.IsLocationServiceEnabled)
				{
					_statusText.text = "location services not enabled";
				}
				else
				{
					if (currLoc.LatitudeLongitude.Equals(Vector2d.zero))
						_statusText.text = "Waiting for location ....";
					else
						_statusText.text = string.Format("{0}", currLoc.LatitudeLongitude);
				}
			}
		}
	}
}
