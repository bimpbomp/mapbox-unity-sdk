using Mapbox.Unity.MeshGeneration.Data;
using UnityEngine;

namespace Mapbox.Unity.MeshGeneration
{
	public class FeatureCollectionBase : ScriptableObject
	{
		public virtual void Initialize()
		{
		}

		public virtual void AddFeature(double[] position, VectorEntity ve)
		{
		}
	}
}
