using System;
using Mapbox.Unity.Map.Interfaces;

namespace Mapbox.Unity.Map
{
	[Serializable]
	public class MapPlacementOptions : MapboxDataProperty
	{
		public MapPlacementType placementType = MapPlacementType.AtLocationCenter;
		public bool snapMapToZero;
		public IMapPlacementStrategy placementStrategy;
	}
}
