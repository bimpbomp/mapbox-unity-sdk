﻿using System.Collections.Generic;
using Mapbox.Unity.Map;
using Mapbox.Unity.MeshGeneration.Data;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using UnityEngine;

namespace Mapbox.Unity.MeshGeneration.Factories.TerrainStrategies
{
	public class FlatSphereTerrainStrategy : TerrainStrategy
	{
		public float Radius => _elevationOptions.modificationOptions.earthRadius;

		public override int RequiredVertexCount => _elevationOptions.modificationOptions.sampleCount *
		                                           _elevationOptions.modificationOptions.sampleCount;

		public override void Initialize(ElevationLayerProperties elOptions)
		{
			_elevationOptions = elOptions;
		}

		public override void RegisterTile(UnityTile tile)
		{
			if (_elevationOptions.unityLayerOptions.addToLayer &&
			    tile.gameObject.layer != _elevationOptions.unityLayerOptions.layerId)
				tile.gameObject.layer = _elevationOptions.unityLayerOptions.layerId;

			if ((int)tile.ElevationType != (int)ElevationLayerType.GlobeTerrain ||
			    tile.MeshFilter.sharedMesh.vertexCount != RequiredVertexCount)
			{
				tile.MeshFilter.sharedMesh.Clear();
				tile.ElevationType = TileTerrainType.Globe;
			}

			GenerateTerrainMesh(tile);
		}

		private void GenerateTerrainMesh(UnityTile tile)
		{
			var verts = new List<Vector3>();
			var _sampleCount = _elevationOptions.modificationOptions.sampleCount;
			var _radius = _elevationOptions.modificationOptions.earthRadius;
			for (float x = 0; x < _sampleCount; x++)
			for (float y = 0; y < _sampleCount; y++)
			{
				var xx = Mathf.Lerp((float)tile.Rect.Min.x, (float)tile.Rect.Min.x + (float)tile.Rect.Size.x,
					x / (_sampleCount - 1));
				var yy = Mathf.Lerp((float)tile.Rect.Max.y, (float)tile.Rect.Max.y + (float)tile.Rect.Size.y,
					y / (_sampleCount - 1));

				var ll = Conversions.MetersToLatLon(new Vector2d(xx, yy));

				var latitude = (float)(Mathf.Deg2Rad * ll.x);
				var longitude = (float)(Mathf.Deg2Rad * ll.y);

				var xPos = _radius * Mathf.Cos(latitude) * Mathf.Cos(longitude);
				var zPos = _radius * Mathf.Cos(latitude) * Mathf.Sin(longitude);
				var yPos = _radius * Mathf.Sin(latitude);

				var pp = new Vector3(xPos, yPos, zPos);
				verts.Add(pp);
			}

			var trilist = new List<int>();
			for (var y = 0; y < _sampleCount - 1; y++)
			for (var x = 0; x < _sampleCount - 1; x++)
			{
				trilist.Add(y * _sampleCount + x);
				trilist.Add(y * _sampleCount + x + _sampleCount + 1);
				trilist.Add(y * _sampleCount + x + _sampleCount);

				trilist.Add(y * _sampleCount + x);
				trilist.Add(y * _sampleCount + x + 1);
				trilist.Add(y * _sampleCount + x + _sampleCount + 1);
			}

			var uvlist = new List<Vector2>();
			var step = 1f / (_sampleCount - 1);
			for (var i = 0; i < _sampleCount; i++)
			for (var j = 0; j < _sampleCount; j++)
				uvlist.Add(new Vector2(i * step, j * step));

			tile.MeshFilter.sharedMesh.SetVertices(verts);
			tile.MeshFilter.sharedMesh.SetTriangles(trilist, 0);
			tile.MeshFilter.sharedMesh.SetUVs(0, uvlist);
			tile.MeshFilter.sharedMesh.RecalculateBounds();
			tile.MeshFilter.sharedMesh.RecalculateNormals();

			tile.transform.localPosition = Constants.Math.Vector3Zero;
		}

		public override void UnregisterTile(UnityTile tile)
		{
		}
	}
}
