﻿using System;
using UnityEngine;

namespace Mapbox.Unity.Location
{
	public abstract class AbstractLocationProvider : MonoBehaviour, ILocationProvider
	{
		protected Location _currentLocation;

		/// <summary>
		///     Gets the last known location.
		/// </summary>
		/// <value>The current location.</value>
		public Location CurrentLocation => _currentLocation;

		public event Action<Location> OnLocationUpdated = delegate { };

		protected virtual void SendLocation(Location location)
		{
			OnLocationUpdated(location);
		}
	}
}
