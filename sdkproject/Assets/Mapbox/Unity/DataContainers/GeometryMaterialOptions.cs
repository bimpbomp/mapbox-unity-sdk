using System;
using System.IO;
using Mapbox.Unity.MeshGeneration.Data;
using Mapbox.Unity.MeshGeneration.Modifiers;
using UnityEngine;

namespace Mapbox.Unity.Map
{
	[Serializable]
	public class CustomStyleBundle
	{
		public UvMapType texturingType = UvMapType.Tiled;
		public MaterialList[] materials = new MaterialList[2];
		public AtlasInfo atlasInfo;
		public ScriptablePalette colorPalette;

		public CustomStyleBundle()
		{
			materials = new MaterialList[2];
			materials[0] = new MaterialList();
			materials[1] = new MaterialList();
		}

		private void AssignAssets(StyleAssetPathBundle styleAssetPathBundle)
		{
			var topMaterial = Resources.Load(styleAssetPathBundle.topMaterialPath, typeof(Material)) as Material;
			var sideMaterial = Resources.Load(styleAssetPathBundle.sideMaterialPath, typeof(Material)) as Material;

			var atlas = Resources.Load(styleAssetPathBundle.atlasPath, typeof(AtlasInfo)) as AtlasInfo;
			var palette =
				Resources.Load(styleAssetPathBundle.palettePath, typeof(ScriptablePalette)) as ScriptablePalette;

			materials[0].Materials[0] = new Material(topMaterial);
			materials[1].Materials[0] = new Material(sideMaterial);
			atlasInfo = atlas;
			colorPalette = palette;
		}

		public void SetDefaultAssets(UvMapType mapType = UvMapType.Atlas)
		{
			var styleAssetPathBundle =
				new StyleAssetPathBundle("Default", Constants.Path.MAP_FEATURE_STYLES_DEFAULT_STYLE_ASSETS);
			texturingType = mapType;
			AssignAssets(styleAssetPathBundle);
		}
	}

	[Serializable]
	public class GeometryMaterialOptions : ModifierProperties, ISubLayerTexturing

	{
		public StyleTypes style;

		public UvMapType texturingType = UvMapType.Tiled;
		public MaterialList[] materials = new MaterialList[2];
		public AtlasInfo atlasInfo;

		public float lightStyleOpacity = 1.0f;
		public float darkStyleOpacity = 1.0f;

		public Color colorStyleColor = Color.white;

		public SamplePalettes samplePalettes;

		public ScriptablePalette colorPalette;

		[SerializeField] public CustomStyleBundle customStyleOptions;

		private SubLayerColorStyle _colorStyle;


		private SubLayerCustomStyle _customStyle;
		private SubLayerDarkStyle _darkStyle;

		private SubLayerFantasyStyle _fantasyStyle;

		private SubLayerLightStyle _lightStyle;

		private SubLayerRealisticStyle _realisticStyle;

		private SubLayerSimpleStyle _simpleStyle;

		public GeometryMaterialOptions()
		{
			materials = new MaterialList[2];
			materials[0] = new MaterialList();
			materials[1] = new MaterialList();
		}

		public override Type ModifierType => typeof(MaterialModifier);

		public ISubLayerDarkStyle DarkStyle
		{
			get
			{
				if (_darkStyle == null) _darkStyle = new SubLayerDarkStyle(this);
				return _darkStyle;
			}
		}

		public ISubLayerLightStyle LightStyle
		{
			get
			{
				if (_lightStyle == null) _lightStyle = new SubLayerLightStyle(this);
				return _lightStyle;
			}
		}

		public ISubLayerColorStyle ColorStyle
		{
			get
			{
				if (_colorStyle == null) _colorStyle = new SubLayerColorStyle(this);
				return _colorStyle;
			}
		}

		public ISubLayerSimpleStyle SimpleStyle
		{
			get
			{
				if (_simpleStyle == null) _simpleStyle = new SubLayerSimpleStyle(this);
				return _simpleStyle;
			}
		}

		public ISubLayerRealisticStyle RealisticStyle
		{
			get
			{
				if (_realisticStyle == null) _realisticStyle = new SubLayerRealisticStyle(this);
				return _realisticStyle;
			}
		}

		public ISubLayerFantasyStyle FantasyStyle
		{
			get
			{
				if (_fantasyStyle == null) _fantasyStyle = new SubLayerFantasyStyle(this);
				return _fantasyStyle;
			}
		}

		public ISubLayerCustomStyle CustomStyle
		{
			get
			{
				if (_customStyle == null) _customStyle = new SubLayerCustomStyle(this);
				return _customStyle;
			}
		}

		/// <summary>
		///     Sets the type of the style.
		/// </summary>
		/// <param name="styleType">Style type.</param>
		public void SetStyleType(StyleTypes styleType)
		{
			style = styleType;
			HasChanged = true;
		}


		/// <summary>
		///     Gets the type of style used in the layer.
		/// </summary>
		/// <returns>The style type.</returns>
		public virtual StyleTypes GetStyleType()
		{
			return style;
		}

		/// <summary>
		///     Sets up default values for GeometryMaterial Options.
		///     If style is set to Custom, user defined values will be used.
		/// </summary>
		public void SetDefaultMaterialOptions()
		{
			var styleName = style.ToString();

			if (customStyleOptions == null)
			{
				customStyleOptions = new CustomStyleBundle();
				customStyleOptions.SetDefaultAssets();
			}

			if (style == StyleTypes.Custom)
			{
				//nothing to do. Use custom settings
			}
			else
			{
				var samplePaletteName = samplePalettes.ToString();

				var path = Path.Combine(Constants.Path.MAP_FEATURE_STYLES_SAMPLES,
					Path.Combine(styleName, Constants.Path.MAPBOX_STYLES_ASSETS_FOLDER));

				var styleAssetPathBundle = new StyleAssetPathBundle(styleName, path, samplePaletteName);

				AssignAssets(styleAssetPathBundle);
			}

			switch (style)
			{
				case StyleTypes.Light:
					var lightColor = materials[0].Materials[0].color;
					lightColor.a = lightStyleOpacity;
					materials[0].Materials[0].color = lightColor;

					lightColor = materials[1].Materials[0].color;
					lightColor.a = lightStyleOpacity;
					materials[1].Materials[0].color = lightColor;
					break;
				case StyleTypes.Dark:
					var darkColor = materials[0].Materials[0].color;
					darkColor.a = darkStyleOpacity;
					materials[0].Materials[0].color = darkColor;

					darkColor = materials[1].Materials[0].color;
					darkColor.a = darkStyleOpacity;
					materials[1].Materials[0].color = darkColor;
					break;
				case StyleTypes.Color:
					var color = colorStyleColor;
					materials[0].Materials[0].color = color;
					materials[1].Materials[0].color = color;
					break;
			}

			if (style == StyleTypes.Satellite)
				texturingType = UvMapType.Tiled;
			else
				texturingType = style != StyleTypes.Custom && style == StyleTypes.Simple
					? UvMapType.AtlasWithColorPalette
					: UvMapType.Atlas;
		}

		private void AssignAssets(StyleAssetPathBundle styleAssetPathBundle)
		{
			var topMaterial = Resources.Load(styleAssetPathBundle.topMaterialPath, typeof(Material)) as Material;
			var sideMaterial = Resources.Load(styleAssetPathBundle.sideMaterialPath, typeof(Material)) as Material;

			var atlas = Resources.Load(styleAssetPathBundle.atlasPath, typeof(AtlasInfo)) as AtlasInfo;
			var palette =
				Resources.Load(styleAssetPathBundle.palettePath, typeof(ScriptablePalette)) as ScriptablePalette;

			var tempMaterials = new Material[2];


			for (var i = 0; i < materials.Length; i++)
				if (materials[i].Materials[0] != null)
				{
					tempMaterials[i] = materials[i].Materials[0];
					materials[i].Materials[0] = null;
				}

			materials[0].Materials[0] = new Material(topMaterial);
			materials[1].Materials[0] = new Material(sideMaterial);

			for (var i = 0; i < materials.Length; i++)
				if (tempMaterials[i] != null)
					tempMaterials[i].Destroy();

			Resources.UnloadUnusedAssets();

			atlasInfo = atlas;
			colorPalette = palette;
		}

		public void SetDefaultAssets(UvMapType mapType = UvMapType.Atlas)
		{
			var styleAssetPathBundle =
				new StyleAssetPathBundle("Default", Constants.Path.MAP_FEATURE_STYLES_DEFAULT_STYLE_ASSETS);
			texturingType = mapType;
			AssignAssets(styleAssetPathBundle);
		}
	}

	[Serializable]
	public class UVModifierOptions : ModifierProperties
	{
		public StyleTypes style;
		public UvMapType texturingType = UvMapType.Tiled;
		public AtlasInfo atlasInfo;

		public override Type ModifierType => typeof(PolygonMeshModifier);

		public GeometryExtrusionWithAtlasOptions ToGeometryExtrusionWithAtlasOptions()
		{
			return new GeometryExtrusionWithAtlasOptions(this);
		}
	}
}
