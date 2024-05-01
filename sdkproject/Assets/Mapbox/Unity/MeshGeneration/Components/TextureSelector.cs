using Mapbox.Unity.MeshGeneration.Data;
using UnityEngine;

namespace Mapbox.Unity.MeshGeneration.Components
{
	using Random = Random;

	[RequireComponent(typeof(MeshRenderer))]
	public class TextureSelector : MonoBehaviour
	{
		private MeshRenderer _meshRenderer;
		private Material[] _sideTextures;
		private bool _textureSides;
		private bool _textureTop;

		private UnityTile _tile;
		private Material[] _topTextures;
		private bool _useSatelliteRoof;

		public void Initialize(VectorEntity ve, bool doTextureTop, bool useSatelliteRoof, Material[] topTextures,
			bool doTextureSides, Material[] sideTextures)
		{
			_useSatelliteRoof = useSatelliteRoof;
			_textureTop = doTextureTop;
			_textureSides = doTextureSides;

			_tile = GetComponent<UnityTile>();
			var t = transform;
			while (_tile == null && t.parent != null)
			{
				t = t.parent;
				_tile = t.GetComponent<UnityTile>();
			}

			_topTextures = topTextures;
			_sideTextures = sideTextures;
			_meshRenderer = GetComponent<MeshRenderer>();

			if (_textureSides && _sideTextures.Length > 0)
				_meshRenderer.materials = new Material[2]
				{
					_topTextures[Random.Range(0, _topTextures.Length)],
					_sideTextures[Random.Range(0, _sideTextures.Length)]
				};
			else if (_textureTop)
				_meshRenderer.materials = new Material[1] { _topTextures[Random.Range(0, _topTextures.Length)] };

			if (_useSatelliteRoof)
			{
				_meshRenderer.materials[0].mainTexture = _tile.GetRasterData();
				_meshRenderer.materials[0].mainTextureScale = new Vector2(1f, 1f);
			}
		}
	}
}
