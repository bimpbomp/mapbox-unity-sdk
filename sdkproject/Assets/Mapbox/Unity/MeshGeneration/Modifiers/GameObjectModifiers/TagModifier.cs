using Mapbox.Unity.MeshGeneration.Data;
using UnityEngine;

namespace Mapbox.Unity.MeshGeneration.Modifiers
{
	[CreateAssetMenu(menuName = "Mapbox/Modifiers/Tag Modifier")]
	public class TagModifier : GameObjectModifier
	{
		[SerializeField] private string _tag;

		public override void Run(VectorEntity ve, UnityTile tile)
		{
			ve.GameObject.tag = _tag;
		}
	}
}
