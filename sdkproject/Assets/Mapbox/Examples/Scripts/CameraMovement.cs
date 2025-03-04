using System;
using Mapbox.Unity.Map;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Mapbox.Examples
{
	public class CameraMovement : MonoBehaviour
	{
		[SerializeField] private AbstractMap _map;

		[SerializeField] private float _panSpeed = 20f;

		[SerializeField] private float _zoomSpeed = 50f;

		[SerializeField] private Camera _referenceCamera;

		private Vector3 _delta;
		private Vector3 _origin;

		private Quaternion _originalRotation;
		private bool _shouldDrag;

		private void Awake()
		{
			_originalRotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);

			if (_referenceCamera == null)
			{
				_referenceCamera = GetComponent<Camera>();
				if (_referenceCamera == null) throw new Exception("You must have a reference camera assigned!");
			}

			if (_map == null)
			{
				_map = FindObjectOfType<AbstractMap>();
				if (_map == null) throw new Exception("You must have a reference map assigned!");
			}
		}

		private void LateUpdate()
		{
			if (Input.touchSupported && Input.touchCount > 0)
				HandleTouch();
			else
				HandleMouseAndKeyBoard();
		}

		private void HandleTouch()
		{
			var zoomFactor = 0.0f;
			//pinch to zoom. 
			switch (Input.touchCount)
			{
				case 1:
				{
					HandleMouseAndKeyBoard();
				}
					break;
				case 2:
				{
					// Store both touches.
					var touchZero = Input.GetTouch(0);
					var touchOne = Input.GetTouch(1);

					// Find the position in the previous frame of each touch.
					var touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
					var touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

					// Find the magnitude of the vector (the distance) between the touches in each frame.
					var prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
					var touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

					// Find the difference in the distances between each frame.
					zoomFactor = 0.05f * (touchDeltaMag - prevTouchDeltaMag);
				}
					ZoomMapUsingTouchOrMouse(zoomFactor);
					break;
			}
		}

		private void ZoomMapUsingTouchOrMouse(float zoomFactor)
		{
			var y = zoomFactor * _zoomSpeed;
			transform.localPosition += transform.forward * y;
		}

		private void HandleMouseAndKeyBoard()
		{
			if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
			{
				var mousePosition = Input.mousePosition;
				mousePosition.z = _referenceCamera.transform.localPosition.y;
				_delta = _referenceCamera.ScreenToWorldPoint(mousePosition) - _referenceCamera.transform.localPosition;
				_delta.y = 0f;
				if (_shouldDrag == false)
				{
					_shouldDrag = true;
					_origin = _referenceCamera.ScreenToWorldPoint(mousePosition);
				}
			}
			else
			{
				_shouldDrag = false;
			}

			if (_shouldDrag)
			{
				var offset = _origin - _delta;
				offset.y = transform.localPosition.y;
				transform.localPosition = offset;
			}
			else
			{
				if (EventSystem.current.IsPointerOverGameObject()) return;

				var x = Input.GetAxis("Horizontal");
				var z = Input.GetAxis("Vertical");
				var y = Input.GetAxis("Mouse ScrollWheel") * _zoomSpeed;
				if (!(Mathf.Approximately(x, 0) && Mathf.Approximately(y, 0) && Mathf.Approximately(z, 0)))
				{
					transform.localPosition += transform.forward * y +
					                           _originalRotation * new Vector3(x * _panSpeed, 0, z * _panSpeed);
					_map.UpdateMap();
				}
			}
		}
	}
}
