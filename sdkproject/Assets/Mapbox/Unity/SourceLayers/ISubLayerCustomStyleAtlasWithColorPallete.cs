using Mapbox.Unity.MeshGeneration.Data;
using UnityEngine;

namespace Mapbox.Unity.Map
{
	public interface ISubLayerCustomStyleAtlasWithColorPallete : ISubLayerCustomStyleOptions, ISubLayerStyle
	{
		ScriptablePalette ColorPalette { get; set; }
		void SetAsStyle(Material TopMaterial, Material SideMaterial, AtlasInfo uvAtlas, ScriptablePalette palette);
	}
}
