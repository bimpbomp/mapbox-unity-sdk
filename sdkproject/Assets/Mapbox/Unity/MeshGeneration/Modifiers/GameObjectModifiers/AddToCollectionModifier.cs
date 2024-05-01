using Mapbox.Unity.MeshGeneration.Data;
using UnityEngine;

namespace Mapbox.Unity.MeshGeneration.Modifiers
{
	[CreateAssetMenu(menuName = "Mapbox/Modifiers/Add To Collection Modifier")]
	public class AddToCollectionModifier : GameObjectModifier
	{
		[SerializeField] private FeatureCollectionBase _collection;

		public override void Initialize()
		{
			base.Initialize();
			_collection.Initialize();
		}

		public override void Run(VectorEntity ve, UnityTile tile)
		{
			_collection.AddFeature(new double[] { ve.Transform.position.x, ve.Transform.position.z }, ve);
		}
	}
}
