using System;
using UnityEngine;

namespace Mapbox.Unity.Map
{
	[Serializable]
	public class ElevationModificationOptions
	{
		[Tooltip("Mesh resolution of terrain, results in n x n grid")] [Range(2, 255)]
		public int sampleCount = 10;

		[Tooltip("Use world relative scale to scale terrain height.")]
		public bool useRelativeHeight;

		[Tooltip("Earth radius in Unity units.")]
		public float earthRadius = 1000f;
	}
}
