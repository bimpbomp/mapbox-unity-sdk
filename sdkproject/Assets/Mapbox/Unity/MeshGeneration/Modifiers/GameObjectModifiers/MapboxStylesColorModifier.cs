using Mapbox.Unity.MeshGeneration.Data;
using UnityEngine;

namespace Mapbox.Unity.MeshGeneration.Modifiers
{
	public class MapboxStylesColorModifier : GameObjectModifier
	{
		private const string _BASE_COLOR_NAME = "_BaseColor";
		private const string _DETAIL_ONE_COLOR_NAME = "_DetailColor1";
		private const string _DETAIL_TWO_COLOR_NAME = "_DetailColor2";

		public ScriptablePalette m_scriptablePalette;

		private int _baseColorId;
		private int _detailOneColorId;
		private int _detailTWoColorId;

		public override void Initialize()
		{
			if (m_scriptablePalette == null) return;

			_baseColorId = Shader.PropertyToID(_BASE_COLOR_NAME);
			_detailOneColorId = Shader.PropertyToID(_DETAIL_ONE_COLOR_NAME);
			_detailTWoColorId = Shader.PropertyToID(_DETAIL_TWO_COLOR_NAME);
		}

		private Color GetRandomColorFromPalette()
		{
			var color = Color.white;
			if (m_scriptablePalette.m_colors.Length > 0)
				color = m_scriptablePalette.m_colors[Random.Range(0, m_scriptablePalette.m_colors.Length)];
			return color;
		}

		public override void Run(VectorEntity ve, UnityTile tile)
		{
			if (m_scriptablePalette == null) return;

			var propBlock = new MaterialPropertyBlock();

			ve.MeshRenderer.GetPropertyBlock(propBlock);

			var baseColor = m_scriptablePalette.m_setBaseColor_Override
				? m_scriptablePalette.m_baseColor_Override
				: GetRandomColorFromPalette();
			var detailColor1 = m_scriptablePalette.m_setDetailColor1_Override
				? m_scriptablePalette.m_detailColor1_Override
				: GetRandomColorFromPalette();
			var detailColor2 = m_scriptablePalette.m_setDetailColor2_Override
				? m_scriptablePalette.m_detailColor2_Override
				: GetRandomColorFromPalette();

			propBlock.SetColor(_baseColorId, baseColor);
			propBlock.SetColor(_detailOneColorId, detailColor1);
			propBlock.SetColor(_detailTWoColorId, detailColor2);

			ve.MeshRenderer.SetPropertyBlock(propBlock);
		}
	}
}
