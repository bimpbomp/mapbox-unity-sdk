using System;
using System.Collections.Generic;
using System.Reflection;
using Mapbox.Unity.Map;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

public class VectorTextureApiTest : MonoBehaviour
{
#if !ENABLE_WINMD_SUPPORT
	private AbstractMap _abstractMap;

	private VectorSubLayerProperties _layer;

	private List<Action> testMethods;
	private readonly List<string> testResults = new();
	private bool _testStarted;

	private void Start()
	{
		_abstractMap = FindObjectOfType<AbstractMap>();
		_layer = _abstractMap.VectorData.FindFeatureSubLayerWithName("test");
		Assert.IsNotNull(_layer, "No layer named test found");

		testMethods = new List<Action>
		{
			SetStyle,
			SetRealisticStyleType,
			SetFantasyStyleType,
			SetSimpleStylePaletteType,
			SetLightStyleOpacity,
			SetDarkStyleOpacity,
			SetColorStyleColor,
			SetCustomTexturingType,
			SetCustomTopMaterial,
			SetCustomSideMaterial,
			SetCustomMaterials
		};
	}

	private void ConductTests()
	{
		for (var i = 0; i < testMethods.Count; i++) testMethods[i]();
		PrintResults();
	}

	private void Update()
	{
		if (_testStarted) return;
		if (Input.GetKeyDown(KeyCode.Space))
		{
			ConductTests();
			_testStarted = true;
		}
	}

	private void AddResultsToList(MethodBase methodBase, bool result)
	{
		var color = result ? "cyan" : "red";
		var printStatement = string.Format("<color={0}>{1} -> {2}</color>", color, result, methodBase.Name);
		testResults.Add(printStatement);
	}

	private void PrintResults()
	{
		Debug.Log("<color=yellow>Vector Texture API Test ///////////////////////////////////////////////////</color>");
		for (var i = 0; i < testResults.Count; i++) Debug.Log(testResults[i]);
	}

	private void SetStyle()
	{
		foreach (StyleTypes style in Enum.GetValues(typeof(StyleTypes)))
		{
			_layer.Texturing.SetStyleType(style);
			AddResultsToList(MethodBase.GetCurrentMethod(), _layer.Texturing.GetStyleType() == style);
		}
	}

	private void SetRealisticStyleType()
	{
		_layer.Texturing.RealisticStyle.SetAsStyle();
		AddResultsToList(MethodBase.GetCurrentMethod(), _layer.Texturing.GetStyleType() == StyleTypes.Realistic);
	}

	private void SetFantasyStyleType()
	{
		_layer.Texturing.FantasyStyle.SetAsStyle();
		AddResultsToList(MethodBase.GetCurrentMethod(), _layer.Texturing.GetStyleType() == StyleTypes.Fantasy);
	}

	private void SetSimpleStylePaletteType()
	{
		foreach (SamplePalettes palette in Enum.GetValues(typeof(SamplePalettes)))
		{
			_layer.Texturing.SimpleStyle.PaletteType = palette;
			AddResultsToList(MethodBase.GetCurrentMethod(), _layer.Texturing.SimpleStyle.PaletteType == palette);
		}
	}

	private void SetLightStyleOpacity()
	{
		var randomVal = Random.value;
		_layer.Texturing.LightStyle.SetAsStyle(randomVal);
		AddResultsToList(MethodBase.GetCurrentMethod(),
			Mathf.Approximately(_layer.Texturing.LightStyle.Opacity, randomVal));
	}

	private void SetDarkStyleOpacity()
	{
		var randomVal = Random.value;
		_layer.Texturing.DarkStyle.SetAsStyle(randomVal);
		AddResultsToList(MethodBase.GetCurrentMethod(),
			Mathf.Approximately(_layer.Texturing.DarkStyle.Opacity, randomVal));
	}

	private void SetColorStyleColor()
	{
		var randomColor = new Color(Random.value, Random.value, Random.value, Random.value);
		_layer.Texturing.ColorStyle.SetAsStyle(randomColor);
		AddResultsToList(MethodBase.GetCurrentMethod(), _layer.Texturing.ColorStyle.FeatureColor == randomColor);
	}

	private void SetCustomTexturingType()
	{
		_layer.Texturing.SetStyleType(StyleTypes.Custom);
		foreach (UvMapType uv in Enum.GetValues(typeof(UvMapType)))
		{
			_layer.Texturing.CustomStyle.TexturingType = uv;
			AddResultsToList(MethodBase.GetCurrentMethod(), _layer.Texturing.CustomStyle.TexturingType == uv);
		}
	}

	private void SetCustomTopMaterial()
	{
		_layer.Texturing.CustomStyle.Tiled.SetAsStyle();
		var myNewMaterial = new Material(Shader.Find("Specular"));
		_layer.Texturing.CustomStyle.Tiled.TopMaterial = myNewMaterial;
		AddResultsToList(MethodBase.GetCurrentMethod(),
			_layer.Texturing.CustomStyle.Tiled.TopMaterial.name == myNewMaterial.name);
	}

	private void SetCustomSideMaterial()
	{
		_layer.Texturing.CustomStyle.Tiled.SetAsStyle();
		var myNewMaterial = new Material(Shader.Find("Specular"));
		_layer.Texturing.CustomStyle.Tiled.SideMaterial = myNewMaterial;
		AddResultsToList(MethodBase.GetCurrentMethod(),
			_layer.Texturing.CustomStyle.Tiled.SideMaterial.name == myNewMaterial.name);
	}

	private void SetCustomMaterials()
	{
		_layer.Texturing.CustomStyle.Tiled.SetAsStyle();
		var myNewMaterialTop = new Material(Shader.Find("Specular"));
		var myNewMaterialSide = new Material(Shader.Find("Specular"));
		_layer.Texturing.CustomStyle.Tiled.SetMaterials(myNewMaterialTop, myNewMaterialSide);
		AddResultsToList(MethodBase.GetCurrentMethod(),
			_layer.Texturing.CustomStyle.Tiled.TopMaterial.name == myNewMaterialTop.name);
		AddResultsToList(MethodBase.GetCurrentMethod(),
			_layer.Texturing.CustomStyle.Tiled.SideMaterial.name == myNewMaterialSide.name);
	}
#endif
}
