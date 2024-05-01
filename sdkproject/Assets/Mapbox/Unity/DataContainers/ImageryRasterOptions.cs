using System;
using UnityEngine;

namespace Mapbox.Unity.Map
{
	[Serializable]
	public class ImageryRasterOptions : MapboxDataProperty
	{
		[Tooltip(
			"Use higher resolution Mapbox imagery for retina displays; better visual quality and larger texture sizes.")]
		public bool useRetina;

		[Tooltip("Use Unity compression for the tile texture.")]
		public bool useCompression;

		[Tooltip("Use texture with Unity generated mipmaps.")]
		public bool useMipMap;

		public override bool NeedsForceUpdate()
		{
			return true;
		}
	}
}
