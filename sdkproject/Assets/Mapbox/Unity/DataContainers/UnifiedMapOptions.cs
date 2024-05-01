using System;
using Mapbox.Unity.Utilities;

namespace Mapbox.Unity.Map
{
	[Serializable]
	public class UnifiedMapOptions
	{
		public MapPresetType mapPreset = MapPresetType.LocationBasedMap;
		public MapOptions mapOptions = new();

		[NodeEditorElement("Image Layer")] public ImageryLayerProperties imageryLayerProperties = new();

		[NodeEditorElement("Terrain Layer")] public ElevationLayerProperties elevationLayerProperties = new();

		[NodeEditorElement("Vector Layer")] public VectorLayerProperties vectorLayerProperties = new();
	}
}
