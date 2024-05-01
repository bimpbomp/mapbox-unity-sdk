﻿using System;
using Mapbox.Unity.Map.Interfaces;
using UnityEngine;

namespace Mapbox.Unity.Map
{
	[Serializable]
	public class MapScalingOptions : MapboxDataProperty
	{
		public MapScalingType scalingType = MapScalingType.Custom;

		[Tooltip("Size of each tile in Unity units.")]
		public float unityTileSize = 100f;

		public IMapScalingStrategy scalingStrategy;
	}
}
