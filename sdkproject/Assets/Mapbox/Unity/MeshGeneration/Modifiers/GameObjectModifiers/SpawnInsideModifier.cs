﻿// HACK:
// This will work out of the box, but it's intended to be an example of how to approach
// procedural decoration like this.
// A better approach would be to operate on the geometry itself.

using System.Collections.Generic;
using Mapbox.Unity.MeshGeneration.Data;
using UnityEngine;

namespace Mapbox.Unity.MeshGeneration.Modifiers
{
	[CreateAssetMenu(menuName = "Mapbox/Modifiers/Spawn Inside Modifier")]
	public class SpawnInsideModifier : GameObjectModifier
	{
		[SerializeField] private int _spawnRateInSquareMeters;

		[SerializeField] private int _maxSpawn = 1000;

		[SerializeField] private GameObject[] _prefabs;

		[SerializeField] private LayerMask _layerMask;

		[SerializeField] private bool _scaleDownWithWorld;

		[SerializeField] private bool _randomizeScale;

		[SerializeField] private bool _randomizeRotation;

		private Dictionary<GameObject, List<GameObject>> _objects;
		private Queue<GameObject> _pool;

		private int _spawnedCount;

		public override void Initialize()
		{
			if (_objects == null || _pool == null)
			{
				_objects = new Dictionary<GameObject, List<GameObject>>();
				_pool = new Queue<GameObject>();
			}
		}

		public override void Run(VectorEntity ve, UnityTile tile)
		{
			_spawnedCount = 0;
			var bounds = ve.Mesh.bounds;
			var center = ve.Transform.position + bounds.center;
			center.y = 0;

			var area = (int)(bounds.size.x * bounds.size.z);
			var spawnCount = Mathf.Min(area / _spawnRateInSquareMeters, _maxSpawn);
			while (_spawnedCount < spawnCount)
			{
				var x = Random.Range(-bounds.extents.x, bounds.extents.x);
				var z = Random.Range(-bounds.extents.z, bounds.extents.z);
				var ray = new Ray(center + new Vector3(x, 100, z), Vector3.down * 2000);

				RaycastHit hit;
				if (Physics.Raycast(ray, out hit, 150, _layerMask))
				{
					var index = Random.Range(0, _prefabs.Length);
					var transform = GetObject(index, ve.GameObject).transform;
					transform.position = hit.point;
					if (_randomizeRotation) transform.localEulerAngles = new Vector3(0, Random.Range(-180f, 180f), 0);
					if (!_scaleDownWithWorld) transform.localScale = Vector3.one / tile.TileScale;

					if (_randomizeScale)
					{
						var scale = transform.localScale;
						var y = Random.Range(scale.y * .7f, scale.y * 1.3f);
						scale.y = y;
						transform.localScale = scale;
					}
				}

				_spawnedCount++;
			}
		}

		public override void OnPoolItem(VectorEntity vectorEntity)
		{
			if (_objects.ContainsKey(vectorEntity.GameObject))
			{
				foreach (var item in _objects[vectorEntity.GameObject])
				{
					item.SetActive(false);
					_pool.Enqueue(item);
				}

				_objects[vectorEntity.GameObject].Clear();
				_objects.Remove(vectorEntity.GameObject);
			}
		}

		public override void Clear()
		{
			foreach (var go in _pool) go.Destroy();
			_pool.Clear();
			foreach (var tileObject in _objects)
			foreach (var go in tileObject.Value)
				if (Application.isEditor && !Application.isPlaying)
					DestroyImmediate(go);
				else
					Destroy(go);
			_objects.Clear();
		}

		private GameObject GetObject(int index, GameObject go)
		{
			GameObject ob;
			if (_pool.Count > 0)
			{
				ob = _pool.Dequeue();
				ob.SetActive(true);
				ob.transform.SetParent(go.transform);
			}
			else
			{
				ob = Instantiate(_prefabs[index], go.transform, false);
			}

			if (_objects.ContainsKey(go))
				_objects[go].Add(ob);
			else
				_objects.Add(go, new List<GameObject> { ob });
			return ob;
		}
	}
}
