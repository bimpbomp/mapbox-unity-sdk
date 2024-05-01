using System.Linq;
using Mapbox.Unity.MeshGeneration.Data;
using UnityEngine;

namespace Mapbox.Unity.MeshGeneration.Components
{
	public class FeatureBehaviour : MonoBehaviour
	{
		public Transform Transform;

		[Multiline(5)] public string DataString;

		public VectorFeatureUnity Data;
		public VectorEntity VectorEntity;

		public void ShowDebugData()
		{
			DataString = string.Join("\r\n", Data.Properties.Select(x => x.Key + " - " + x.Value).ToArray());
		}

		public void ShowDataPoints()
		{
			foreach (var item in VectorEntity.Feature.Points)
				for (var i = 0; i < item.Count; i++)
				{
					var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
					go.name = i.ToString();
					go.transform.SetParent(transform, false);
					go.transform.localPosition = item[i];
				}
		}

		public void Initialize(VectorEntity ve)
		{
			VectorEntity = ve;
			Transform = transform;
			Data = ve.Feature;
		}

		public void Initialize(VectorFeatureUnity feature)
		{
			Transform = transform;
			Data = feature;
		}
	}
}
