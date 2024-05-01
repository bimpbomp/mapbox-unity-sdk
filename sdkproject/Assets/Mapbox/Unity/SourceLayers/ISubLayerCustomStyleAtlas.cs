using Mapbox.Unity.MeshGeneration.Data;
using UnityEngine;

namespace Mapbox.Unity.Map
{
	public interface ISubLayerCustomStyleAtlas : ISubLayerCustomStyleOptions, ISubLayerStyle
	{
		AtlasInfo UvAtlas { get; set; }
		void SetAsStyle(Material TopMaterial, Material SideMaterial, AtlasInfo uvAtlas);
	}
}
