﻿using System.Collections.Generic;
using UnityEngine;

namespace Mapbox.Unity.Location
{
	/// <summary>
	///     <para>
	///         The EditorLocationProvider is responsible for providing mock location data via log file collected with the
	///         'LocationProvider' scene
	///     </para>
	/// </summary>
	public class EditorLocationProviderLocationLog : AbstractEditorLocationProvider
	{
		/// <summary>
		///     The mock "latitude, longitude" location, respresented with a string.
		///     You can search for a place using the embedded "Search" button in the inspector.
		///     This value can be changed at runtime in the inspector.
		/// </summary>
		[SerializeField] private TextAsset _locationLogFile;

		private IEnumerator<Location> _locationEnumerator;


		private LocationLogReader _logReader;


#if UNITY_EDITOR
		protected override void Awake()
		{
			base.Awake();
			_logReader = new LocationLogReader(_locationLogFile.bytes);
			_locationEnumerator = _logReader.GetLocations();
		}
#endif


		private void OnDestroy()
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


		protected override void SetLocation()
		{
			if (null == _locationEnumerator) return;

			// no need to check if 'MoveNext()' returns false as LocationLogReader loops through log file
			_locationEnumerator.MoveNext();
			_currentLocation = _locationEnumerator.Current;
		}
	}
}
