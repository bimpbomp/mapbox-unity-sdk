using System;

namespace Mapbox.Unity.Map
{
	[Serializable]
	public class MapExtentOptions : MapboxDataProperty
	{
		public MapExtentType extentType = MapExtentType.CameraBounds;
		public DefaultMapExtents defaultExtents = new();

		public MapExtentOptions(MapExtentType type)
		{
			extentType = type;
		}

		public ExtentOptions GetTileProviderOptions()
		{
			var options = new ExtentOptions();
			switch (extentType)
			{
				case MapExtentType.CameraBounds:
					options = defaultExtents.cameraBoundsOptions;
					break;
				case MapExtentType.RangeAroundCenter:
					options = defaultExtents.rangeAroundCenterOptions;
					break;
				case MapExtentType.RangeAroundTransform:
					options = defaultExtents.rangeAroundTransformOptions;
					break;
			}

			return options;
		}
	}


	[Serializable]
	public class DefaultMapExtents : MapboxDataProperty
	{
		public CameraBoundsTileProviderOptions cameraBoundsOptions = new();
		public RangeTileProviderOptions rangeAroundCenterOptions = new();
		public RangeAroundTransformTileProviderOptions rangeAroundTransformOptions = new();
	}
}
