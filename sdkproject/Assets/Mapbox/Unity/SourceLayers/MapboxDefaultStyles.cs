using System.IO;

namespace Mapbox.Unity.Map
{
	/// <summary>
	///     MapboxDefaultStyles generates a new GeometryMaterialOptions object based on data contained in a
	///     MapFeatureStyleOptions.
	/// </summary>
	public class StyleAssetPathBundle
	{
		public string atlasPath;
		public string palettePath;
		public string sideMaterialPath;
		public string topMaterialPath;

		public StyleAssetPathBundle(string styleName, string path, string samplePaletteName = "")
		{
			var topMaterialName = string.Format("{0}{1}", styleName, Constants.StyleAssetNames.TOP_MATERIAL_SUFFIX);
			var sideMaterialName = string.Format("{0}{1}", styleName, Constants.StyleAssetNames.SIDE_MATERIAL_SUFFIX);
			var atlasInfoName = string.Format("{0}{1}", styleName, Constants.StyleAssetNames.ALTAS_SUFFIX);
			var paletteName = styleName == "Simple"
				? samplePaletteName
				: string.Format("{0}{1}", styleName, Constants.StyleAssetNames.PALETTE_SUFFIX);

			var materialFolderPath = Path.Combine(path, Constants.Path.MAPBOX_STYLES_MATERIAL_FOLDER);
			var atlasFolderPath = Path.Combine(path, Constants.Path.MAPBOX_STYLES_ATLAS_FOLDER);
			var paletteFolderPath = Path.Combine(path, Constants.Path.MAPBOX_STYLES_PALETTES_FOLDER);

			topMaterialPath = Path.Combine(materialFolderPath, topMaterialName);
			sideMaterialPath = Path.Combine(materialFolderPath, sideMaterialName);
			atlasPath = Path.Combine(atlasFolderPath, atlasInfoName);
			palettePath = Path.Combine(paletteFolderPath, paletteName);
		}
	}
}
