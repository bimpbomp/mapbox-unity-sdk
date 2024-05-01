using System.Collections.Generic;
using Mapbox.Unity.MeshGeneration.Data;
using Mapbox.Unity.Utilities;
using UnityEngine;

namespace Mapbox.Unity.MeshGeneration.Modifiers
{
	public class ModifierStackBase : ScriptableObject
	{
		[NodeEditorElement("Mesh Modifiers")] public List<MeshModifier> MeshModifiers = new();

		[NodeEditorElement("Game Object Modifiers")]
		public List<GameObjectModifier> GoModifiers = new();

		public virtual GameObject Execute(UnityTile tile, VectorFeatureUnity feature, MeshData meshData,
			GameObject parent = null, string type = "")
		{
			return null;
		}

		public virtual void Initialize()
		{
		}

		public void UnregisterTile(UnityTile tile)
		{
			OnUnregisterTile(tile);
		}

		public virtual void OnUnregisterTile(UnityTile tile)
		{
		}

		public virtual void Clear()
		{
		}
	}
}
