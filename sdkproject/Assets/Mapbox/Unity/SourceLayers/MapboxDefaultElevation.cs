using System;

namespace Mapbox.Unity.Map
{
	public static class MapboxDefaultElevation
	{
		public static Style GetParameters(ElevationSourceType defaultElevation)
		{
			var defaultStyle = new Style();
			switch (defaultElevation)
			{
				case ElevationSourceType.MapboxTerrain:
					defaultStyle = new Style { Id = "mapbox.terrain-rgb", Name = "Mapbox Terrain" };

					break;
				case ElevationSourceType.Custom:
					throw new Exception("Invalid type : Custom");
			}

			return defaultStyle;
		}
	}
}
