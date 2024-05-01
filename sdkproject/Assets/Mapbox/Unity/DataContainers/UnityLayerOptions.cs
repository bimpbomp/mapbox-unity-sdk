using System;
using Mapbox.Unity.MeshGeneration.Data;
using UnityEngine;

namespace Mapbox.Unity.Map
{
	[Serializable]
	public class UnityLayerOptions : MapboxDataProperty
	{
		[Tooltip("Add terrain tiles to Unity Layer")]
		public bool addToLayer;

		[Tooltip("Unity Layer id to which terrain tiles will get added.")]
		public int layerId;

		public override void UpdateProperty(UnityTile tile)
		{
			tile.gameObject.layer = layerId;
		}
	}
}
