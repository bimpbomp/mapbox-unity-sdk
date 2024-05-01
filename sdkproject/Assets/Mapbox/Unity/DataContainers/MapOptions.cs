using System;
using UnityEngine;

namespace Mapbox.Unity.Map
{
	[Serializable]
	public class MapOptions : MapboxDataProperty
	{
		public MapLocationOptions locationOptions = new();
		public MapExtentOptions extentOptions = new(MapExtentType.RangeAroundCenter);
		public MapPlacementOptions placementOptions = new();
		public MapScalingOptions scalingOptions = new();

		[Tooltip("Texture used while tiles are loading.")]
		public Texture2D loadingTexture;

		public Material tileMaterial;
	}

	[Serializable]
	public class EditorPreviewOptions : MapboxDataProperty
	{
		public bool isPreviewEnabled;
	}
}
