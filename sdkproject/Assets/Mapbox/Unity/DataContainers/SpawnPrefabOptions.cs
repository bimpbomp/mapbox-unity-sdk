using System;
using System.Collections.Generic;
using Mapbox.Unity.MeshGeneration.Modifiers;
using UnityEngine;

namespace Mapbox.Unity.Map
{
	[Serializable]
	public class SpawnPrefabOptions : ModifierProperties
	{
		public GameObject prefab;
		public bool scaleDownWithWorld = true;

		[NonSerialized] public Action<List<GameObject>> AllPrefabsInstatiated = delegate { };

		public override Type ModifierType => typeof(PrefabModifier);
	}
}
