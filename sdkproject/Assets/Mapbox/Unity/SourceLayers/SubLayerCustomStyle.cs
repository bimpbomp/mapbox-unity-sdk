namespace Mapbox.Unity.Map
{
	public class SubLayerCustomStyle : ISubLayerCustomStyle
	{
		private readonly GeometryMaterialOptions _materialOptions;

		private SubLayerCustomStyleAtlas _textureAtlas;

		private SubLayerCustomStyleAtlasWithColorPallete _textureAtlasPallete;
		private SubLayerCustomStyleTiled _tiled;

		public SubLayerCustomStyle(GeometryMaterialOptions materialOptions)
		{
			_materialOptions = materialOptions;
		}

		public UvMapType TexturingType
		{
			get => _materialOptions.texturingType;

			set
			{
				if (_materialOptions.texturingType != value)
				{
					_materialOptions.texturingType = value;
					_materialOptions.HasChanged = true;
				}
			}
		}

		public ISubLayerCustomStyleTiled Tiled
		{
			get
			{
				if (_tiled == null) _tiled = new SubLayerCustomStyleTiled(_materialOptions);
				return _tiled;
			}
		}

		public ISubLayerCustomStyleAtlas TextureAtlas
		{
			get
			{
				if (_textureAtlas == null) _textureAtlas = new SubLayerCustomStyleAtlas(_materialOptions);
				return _textureAtlas;
			}
		}

		public ISubLayerCustomStyleAtlasWithColorPallete TextureAtlasWithColorPallete
		{
			get
			{
				if (_textureAtlasPallete == null)
					_textureAtlasPallete = new SubLayerCustomStyleAtlasWithColorPallete(_materialOptions);
				return _textureAtlasPallete;
			}
		}
	}
}
