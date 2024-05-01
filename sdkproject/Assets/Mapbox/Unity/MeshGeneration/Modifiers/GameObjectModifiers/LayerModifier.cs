using Mapbox.Unity.MeshGeneration.Data;
using UnityEngine;

namespace Mapbox.Unity.MeshGeneration.Modifiers
{
	[CreateAssetMenu(menuName = "Mapbox/Modifiers/Layer Modifier")]
	public class LayerModifier : GameObjectModifier
	{
		[SerializeField] private int _layerId;

		public override void Run(VectorEntity ve, UnityTile tile)
		{
			ve.GameObject.layer = _layerId;
		}
	}
}
