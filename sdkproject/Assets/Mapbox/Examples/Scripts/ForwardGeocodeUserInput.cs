//-----------------------------------------------------------------------
// <copyright file="ForwardGeocodeUserInput.cs" company="Mapbox">
//     Copyright (c) 2016 Mapbox. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using Mapbox.Geocoding;
using Mapbox.Unity;
using Mapbox.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Mapbox.Examples
{
	[RequireComponent(typeof(InputField))]
	public class ForwardGeocodeUserInput : MonoBehaviour
	{
		private InputField _inputField;

		private ForwardGeocodeResource _resource;

		public Vector2d Coordinate { get; private set; }

		public bool HasResponse { get; private set; }

		public ForwardGeocodeResponse Response { get; private set; }

		private void Awake()
		{
			_inputField = GetComponent<InputField>();
			_inputField.onEndEdit.AddListener(HandleUserInput);
			_resource = new ForwardGeocodeResource("");
		}

		public event Action<ForwardGeocodeResponse> OnGeocoderResponse = delegate { };

		private void HandleUserInput(string searchString)
		{
			HasResponse = false;
			if (!string.IsNullOrEmpty(searchString))
			{
				_resource.Query = searchString;
				MapboxAccess.Instance.Geocoder.Geocode(_resource, HandleGeocoderResponse);
			}
		}

		private void HandleGeocoderResponse(ForwardGeocodeResponse res)
		{
			HasResponse = true;
			if (null == res)
			{
				_inputField.text = "no geocode response";
			}
			else if (null != res.Features && res.Features.Count > 0)
			{
				var center = res.Features[0].Center;
				Coordinate = res.Features[0].Center;
			}

			Response = res;
			OnGeocoderResponse(res);
		}
	}
}
