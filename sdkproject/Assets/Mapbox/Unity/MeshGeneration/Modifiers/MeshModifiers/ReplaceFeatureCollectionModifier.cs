﻿using System;
using System.Collections.Generic;
using Mapbox.Unity.Map;
using Mapbox.Unity.MeshGeneration.Data;
using Mapbox.Unity.Utilities;
using UnityEngine;

namespace Mapbox.Unity.MeshGeneration.Modifiers
{
	[Serializable]
	public class FeatureBundle
	{
		//public name param will be displayed in inspector list ui instead of element x...
		[HideInInspector] public string Name;

		public bool active;

		public GameObject prefab;
		public bool scaleDownWithWorld = true;

		[Geocode] public List<string> _prefabLocations = new();

		public List<string> _explicitlyBlockedFeatureIds = new();
	}

	/// <summary>
	///     ReplaceFeatureCollectionModifier aggregates multiple ReplaceFeatureModifier objects into one modifier.
	/// </summary>
	[CreateAssetMenu(menuName = "Mapbox/Modifiers/Replace Feature Collection Modifier")]
	public class ReplaceFeatureCollectionModifier : GameObjectModifier, IReplacementCriteria
	{
		public List<FeatureBundle> features = new();

		private List<ReplaceFeatureModifier> _replaceFeatureModifiers;

		//update all names to make inspector look better...
		private void OnValidate()
		{
			for (var i = 0; i < features.Count; i++)
				features[i].Name = features[i].prefab == null ? "Feature" : features[i].prefab.name;
		}

		public bool ShouldReplaceFeature(VectorFeatureUnity feature)
		{
			foreach (var modifier in _replaceFeatureModifiers)
			{
				if (modifier == null) continue;

				if (modifier.ShouldReplaceFeature(feature)) return true;
			}

			return false;
		}

		public override void Initialize()
		{
			base.Initialize();

			if (_replaceFeatureModifiers != null && _replaceFeatureModifiers.Count > 0)
				foreach (var replaceFeatureModifier in _replaceFeatureModifiers)
					if (replaceFeatureModifier != null)
						replaceFeatureModifier.Clear();

			_replaceFeatureModifiers = new List<ReplaceFeatureModifier>();
			foreach (var feature in features)
			{
				var replaceFeatureModifier = CreateInstance<ReplaceFeatureModifier>();

				replaceFeatureModifier.Active = feature.active;
				replaceFeatureModifier.SpawnPrefabOptions = new SpawnPrefabOptions
				{
					prefab = feature.prefab, scaleDownWithWorld = feature.scaleDownWithWorld
				};
				replaceFeatureModifier.PrefabLocations = new List<string>(feature._prefabLocations);
				replaceFeatureModifier.BlockedIds = new List<string>(feature._explicitlyBlockedFeatureIds);
				replaceFeatureModifier.Initialize();

				_replaceFeatureModifiers.Add(replaceFeatureModifier);
			}
		}

		public override void FeaturePreProcess(VectorFeatureUnity feature)
		{
			foreach (var modifier in _replaceFeatureModifiers)
			{
				if (modifier == null) continue;

				modifier.FeaturePreProcess(feature);
			}
		}

		public override void SetProperties(ModifierProperties properties)
		{
			foreach (var modifier in _replaceFeatureModifiers)
			{
				if (modifier == null) continue;

				modifier.SetProperties(properties);
			}
		}

		public override void Run(VectorEntity ve, UnityTile tile)
		{
			foreach (var modifier in _replaceFeatureModifiers) modifier.Run(ve, tile);
		}

		public override void OnPoolItem(VectorEntity vectorEntity)
		{
			foreach (var modifier in _replaceFeatureModifiers) modifier.OnPoolItem(vectorEntity);
		}

		public override void Clear()
		{
			foreach (var subModules in _replaceFeatureModifiers) subModules.Clear();
		}
	}
}
