﻿using System.Collections.Generic;
using Mapbox.Unity.Map;
using Mapbox.Utils;
using UnityEngine;

namespace Mapbox.Examples
{
	public class POIPlacementScriptExample : MonoBehaviour
	{
		public AbstractMap map;

		//prefab to spawn
		public GameObject prefab;

		//cache of spawned gameobjects
		private List<GameObject> _prefabInstances;

		// Use this for initialization
		private void Start()
		{
			//add layers before initializing the map
			map.VectorData.SpawnPrefabByCategory(prefab, LocationPrefabCategories.ArtsAndEntertainment, 10,
				HandlePrefabSpawned, true, "SpawnFromScriptLayer");
			map.Initialize(new Vector2d(37.784179, -122.401583), 16);
		}

		//handle callbacks
		private void HandlePrefabSpawned(List<GameObject> instances)
		{
			if (instances.Count > 0) Debug.Log(instances[0].name);
		}
	}
}
