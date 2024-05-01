//-----------------------------------------------------------------------
// <copyright file="ReverseGeocodeUserInput.cs" company="Mapbox">
//     Copyright (c) 2016 Mapbox. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using Mapbox.Geocoding;
using Mapbox.Unity;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Mapbox.Examples
{
	/// <summary>
	///     Peforms a reverse geocoder request (search by latitude, longitude) whenever the InputField on *this*
	///     gameObject is finished with an edit.
	///     Expects input in the form of "latitude, longitude"
	/// </summary>
	[RequireComponent(typeof(InputField))]
	public class ReverseGeocodeUserInput : MonoBehaviour
	{
		private Vector2d _coordinate;

		private Geocoder _geocoder;

		private InputField _inputField;

		private ReverseGeocodeResource _resource;

		public bool HasResponse { get; private set; }

		public ReverseGeocodeResponse Response { get; private set; }

		private void Awake()
		{
			_inputField = GetComponent<InputField>();
			_inputField.onEndEdit.AddListener(HandleUserInput);
			_resource = new ReverseGeocodeResource(_coordinate);
		}

		private void Start()
		{
			_geocoder = MapboxAccess.Instance.Geocoder;
		}

		public event EventHandler<EventArgs> OnGeocoderResponse;

		/// <summary>
		///     An edit was made to the InputField.
		///     Unity will send the string from _inputField.
		///     Make geocoder query.
		/// </summary>
		/// <param name="searchString">Search string.</param>
		private void HandleUserInput(string searchString)
		{
			HasResponse = false;
			if (!string.IsNullOrEmpty(searchString))
			{
				_coordinate = Conversions.StringToLatLon(searchString);
				_resource.Query = _coordinate;
				_geocoder.Geocode(_resource, HandleGeocoderResponse);
			}
		}

		/// <summary>
		///     Handles the geocoder response by updating coordinates and notifying observers.
		/// </summary>
		/// <param name="res">Res.</param>
		private void HandleGeocoderResponse(ReverseGeocodeResponse res)
		{
			HasResponse = true;
			Response = res;
			if (OnGeocoderResponse != null) OnGeocoderResponse(this, EventArgs.Empty);
		}
	}
}
