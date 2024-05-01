using System;
using UnityEngine;

namespace Mapbox.Examples
{
	public class DragableDirectionWaypoint : MonoBehaviour
	{
		public Transform MoveTarget;
		private Plane _yPlane;
		public Action MouseDown = () => { };
		public Action MouseDraging = () => { };
		public Action MouseDrop = () => { };
		private Vector3 offset;
		private Vector3 screenPoint;

		public void Start()
		{
			_yPlane = new Plane(Vector3.up, Vector3.zero);
		}

		private void OnMouseDown()
		{
			MouseDown();
		}

		private void OnMouseDrag()
		{
			MouseDraging();
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			var enter = 0.0f;
			if (_yPlane.Raycast(ray, out enter)) MoveTarget.position = ray.GetPoint(enter);
		}

		private void OnMouseUp()
		{
			MouseDrop();
		}
	}
}
