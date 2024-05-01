//-----------------------------------------------------------------------
// <copyright file="HeroBuildingSelectionUserInput.cs" company="Mapbox">
//     Copyright (c) 2018 Mapbox. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using Mapbox.Geocoding;
using Mapbox.Unity;
using Mapbox.Unity.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Mapbox.Examples
{
	public class HeroBuildingSelectionUserInput : MonoBehaviour
	{
		[Geocode] public string location;

		[SerializeField] private Vector3 _cameraPosition;

		[SerializeField] private Vector3 _cameraRotation;

		private Button _button;

		private Camera _camera;

		private ForwardGeocodeResource _resource;

		public bool HasResponse { get; private set; }

		public ForwardGeocodeResponse Response { get; private set; }

		private void Awake()
		{
			_button = GetComponent<Button>();
			_button.onClick.AddListener(HandleUserInput);
			_resource = new ForwardGeocodeResource("");
			_camera = Camera.main;
		}

		public event Action<ForwardGeocodeResponse, bool> OnGeocoderResponse = delegate { };

		private void TransformCamera()
		{
			_camera.transform.position = _cameraPosition;
			_camera.transform.localEulerAngles = _cameraRotation;
		}

		private void HandleUserInput()
		{
			HasResponse = false;
			if (!string.IsNullOrEmpty(location))
			{
				_resource.Query = location;
				MapboxAccess.Instance.Geocoder.Geocode(_resource, HandleGeocoderResponse);
			}
		}

		private void HandleGeocoderResponse(ForwardGeocodeResponse res)
		{
			HasResponse = true;
			Response = res;
			TransformCamera();
			OnGeocoderResponse(res, false);
		}

		public void BakeCameraTransform()
		{
			_cameraPosition = _camera.transform.position;
			_cameraRotation = _camera.transform.localEulerAngles;
		}
	}
}
