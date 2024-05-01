using UnityEngine;

namespace Mapbox.Unity.Map
{
	public interface ISubLayerCustomStyleTiled : ISubLayerCustomStyleOptions, ISubLayerStyle
	{
		void SetMaterials(Material TopMaterial, Material SideMaterial);
		void SetAsStyle(Material TopMaterial, Material SideMaterial = null);
	}
}
