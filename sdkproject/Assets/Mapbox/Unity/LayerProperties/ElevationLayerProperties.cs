using System;

namespace Mapbox.Unity.Map
{
	[Serializable]
	public class ElevationLayerProperties : LayerProperties
	{
		public ElevationSourceType sourceType = ElevationSourceType.MapboxTerrain;

		public LayerSourceOptions sourceOptions = new()
		{
			layerSource = new Style { Id = "mapbox.terrain-rgb" }, isActive = true
		};

		public ElevationLayerType elevationLayerType = ElevationLayerType.FlatTerrain;
		public ElevationRequiredOptions requiredOptions = new();
		public TerrainColliderOptions colliderOptions = new();
		public ElevationModificationOptions modificationOptions = new();
		public UnityLayerOptions unityLayerOptions = new();
		public TerrainSideWallOptions sideWallOptions = new();

		public override bool NeedsForceUpdate()
		{
			return true;
		}
	}
}
