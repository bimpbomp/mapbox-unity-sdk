using Mapbox.Unity.MeshGeneration.Data;
using UnityEngine;

namespace Mapbox.Unity.Map
{
	public class SubLayerCustomStyleAtlasWithColorPallete : ISubLayerCustomStyleAtlasWithColorPallete
	{
		private readonly GeometryMaterialOptions _materialOptions;

		public SubLayerCustomStyleAtlasWithColorPallete(GeometryMaterialOptions materialOptions)
		{
			_materialOptions = materialOptions;
		}

		public AtlasInfo UvAtlas
		{
			get => _materialOptions.atlasInfo;

			set
			{
				if (_materialOptions.atlasInfo != value)
				{
					_materialOptions.atlasInfo = value;
					_materialOptions.HasChanged = true;
				}
			}
		}

		public Material TopMaterial
		{
			get => _materialOptions.materials[0].Materials[0];
			set
			{
				if (_materialOptions.materials[0].Materials[0] != value)
				{
					_materialOptions.materials[0].Materials[0] = value;
					_materialOptions.HasChanged = true;
				}
			}
		}

		public Material SideMaterial
		{
			get => _materialOptions.materials[1].Materials[0];
			set
			{
				if (_materialOptions.materials[1].Materials[0] != value)
				{
					_materialOptions.materials[1].Materials[0] = value;
					_materialOptions.HasChanged = true;
				}
			}
		}

		public ScriptablePalette ColorPalette
		{
			get => _materialOptions.colorPalette;

			set
			{
				if (_materialOptions.colorPalette != value)
				{
					_materialOptions.colorPalette = value;
					_materialOptions.HasChanged = true;
				}
			}
		}

		public void SetAsStyle(Material topMaterial, Material sideMaterial, AtlasInfo uvAtlas,
			ScriptablePalette palette)
		{
			_materialOptions.texturingType = UvMapType.Atlas;
			_materialOptions.materials[0].Materials[0] = topMaterial;
			_materialOptions.materials[1].Materials[0] = sideMaterial;
			_materialOptions.atlasInfo = uvAtlas;
			_materialOptions.colorPalette = palette;
			_materialOptions.HasChanged = true;
		}

		public void SetAsStyle()
		{
			_materialOptions.SetDefaultAssets(UvMapType.AtlasWithColorPalette);
			_materialOptions.HasChanged = true;
		}
	}
}
