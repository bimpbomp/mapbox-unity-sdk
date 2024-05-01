using KDTree;
using Mapbox.Unity.MeshGeneration;
using Mapbox.Unity.MeshGeneration.Data;
using UnityEngine;

namespace Mapbox.Examples
{
	public class HighlightBuildings : MonoBehaviour
	{
		public KdTreeCollection Collection;
		public int MaxCount = 100;
		public float Range = 10;
		private Plane groundPlane = new(Vector3.up, Vector3.zero);
		private NearestNeighbour<VectorEntity> pIter;
		private Vector3 pos;
		private Ray ray;
		private float rayDistance;

		private void Update()
		{
			if (Input.GetMouseButton(0))
			{
				ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				if (groundPlane.Raycast(ray, out rayDistance))
				{
					pos = ray.GetPoint(rayDistance);
					pIter = Collection.NearestNeighbors(new double[] { pos.x, pos.z }, MaxCount, Range);
					while (pIter.MoveNext()) pIter.Current.Transform.localScale = Vector3.zero;
				}
			}
		}
	}
}
