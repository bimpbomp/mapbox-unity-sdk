using System.Collections.Generic;
using Mapbox.Utils;
using UnityEngine;

namespace Mapbox.Unity.MeshGeneration.Data
{
	// TODO: Do we need this class? Why not just use `Mesh`?
	public class MeshData
	{
		public List<int> Edges;
		public Vector2 MercatorCenter;
		public List<Vector3> Normals;
		public Vector3 PositionInTile;
		public List<Vector4> Tangents;
		public RectD TileRect;
		public List<List<int>> Triangles;
		public List<List<Vector2>> UV;
		public List<Vector3> Vertices;

		public MeshData()
		{
			Edges = new List<int>();
			Vertices = new List<Vector3>();
			Normals = new List<Vector3>();
			Tangents = new List<Vector4>();
			Triangles = new List<List<int>>();
			UV = new List<List<Vector2>>();
			UV.Add(new List<Vector2>());
		}

		internal void Clear()
		{
			Edges.Clear();
			Vertices.Clear();
			Normals.Clear();
			Tangents.Clear();

			foreach (var item in Triangles) item.Clear();
			foreach (var item in UV) item.Clear();
		}
	}
}
