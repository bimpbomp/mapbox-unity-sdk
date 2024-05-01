﻿using System;
using UnityEngine;

namespace Mapbox.Unity.Map
{
	[Serializable]
	public class TerrainSideWallOptions
	{
		[Tooltip("Adds side walls to terrain meshes, reduces visual artifacts.")]
		public bool isActive;

		[Tooltip("Height of side walls.")] public float wallHeight = 10;

		[Tooltip("Unity material to use for side walls.")]
		public Material wallMaterial;
	}
}
