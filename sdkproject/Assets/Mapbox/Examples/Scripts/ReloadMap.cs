﻿using System.Collections;
using Mapbox.Geocoding;
using Mapbox.Unity.Map;
using UnityEngine;
using UnityEngine.UI;

namespace Mapbox.Examples
{
	public class ReloadMap : MonoBehaviour
	{
		[SerializeField] private ForwardGeocodeUserInput _forwardGeocoder;

		[SerializeField] private Slider _zoomSlider;

		private Camera _camera;
		private Vector3 _cameraStartPos;

		private HeroBuildingSelectionUserInput[] _heroBuildingSelectionUserInput;
		private AbstractMap _map;

		private Coroutine _reloadRoutine;

		private WaitForSeconds _wait;

		private void Awake()
		{
			_camera = Camera.main;
			_cameraStartPos = _camera.transform.position;
			_map = FindObjectOfType<AbstractMap>();
			if (_map == null)
			{
				Debug.LogError("Error: No Abstract Map component found in scene.");
				return;
			}

			if (_zoomSlider != null)
			{
				_map.OnUpdated += () => { _zoomSlider.value = _map.Zoom; };
				_zoomSlider.onValueChanged.AddListener(Reload);
			}

			if (_forwardGeocoder != null) _forwardGeocoder.OnGeocoderResponse += ForwardGeocoder_OnGeocoderResponse;
			_heroBuildingSelectionUserInput = GetComponentsInChildren<HeroBuildingSelectionUserInput>();
			if (_heroBuildingSelectionUserInput != null)
				for (var i = 0; i < _heroBuildingSelectionUserInput.Length; i++)
					_heroBuildingSelectionUserInput[i].OnGeocoderResponse += ForwardGeocoder_OnGeocoderResponse;
			_wait = new WaitForSeconds(.3f);
		}

		private void ForwardGeocoder_OnGeocoderResponse(ForwardGeocodeResponse response)
		{
			if (null != response.Features && response.Features.Count > 0)
			{
				var zoom = _map.AbsoluteZoom;
				_map.UpdateMap(response.Features[0].Center, zoom);
			}
		}

		private void ForwardGeocoder_OnGeocoderResponse(ForwardGeocodeResponse response, bool resetCamera)
		{
			if (response == null) return;
			if (resetCamera) _camera.transform.position = _cameraStartPos;
			ForwardGeocoder_OnGeocoderResponse(response);
		}

		private void Reload(float value)
		{
			if (_reloadRoutine != null)
			{
				StopCoroutine(_reloadRoutine);
				_reloadRoutine = null;
			}

			_reloadRoutine = StartCoroutine(ReloadAfterDelay((int)value));
		}

		private IEnumerator ReloadAfterDelay(int zoom)
		{
			yield return _wait;
			_camera.transform.position = _cameraStartPos;
			_map.UpdateMap(_map.CenterLatitudeLongitude, zoom);
			_reloadRoutine = null;
		}
	}
}
