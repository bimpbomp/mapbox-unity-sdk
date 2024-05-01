using UnityEngine;

[CreateAssetMenu(menuName = "Mapbox/ScriptablePalette")]
public class ScriptablePalette : ScriptableObject
{
	public int m_numColors = 3;

	public Color m_keyColor = Color.white;

	public Color[] m_colors;

	public float m_hueRange;
	public float m_saturationRange;
	public float m_valueRange;

	public bool m_setBaseColor_Override;
	public bool m_setDetailColor1_Override;
	public bool m_setDetailColor2_Override;

	public Color m_baseColor_Override = Color.white;
	public Color m_detailColor1_Override = Color.white;
	public Color m_detailColor2_Override = Color.white;

	public void GeneratePalette()
	{
		var hue = 0.0f;
		var sat = 0.0f;
		;
		var val = 0.0f;

		Color.RGBToHSV(m_keyColor, out hue, out sat, out val);

		var hueMin = hue - m_hueRange;
		var hueMax = hue + m_hueRange;

		var satMin = Mathf.Clamp(sat - m_saturationRange, 0.0f, 1.0f);
		var satMax = Mathf.Clamp(sat + m_saturationRange, 0.0f, 1.0f);

		var valMin = Mathf.Clamp(val - m_valueRange, 0.0f, 1.0f);
		var valMax = Mathf.Clamp(val + m_valueRange, 0.0f, 1.0f);

		m_colors = new Color[m_numColors];

		for (var i = 0; i < m_numColors; i++)
			m_colors[i] = Random.ColorHSV(hueMin, hueMax, satMin, satMax, valMin, valMax);
	}
}
