using System;
using System.Collections.Generic;
using Mapbox.Platform.TilesetTileJSON;
using Mapbox.Unity.Map;
using Mapbox.VectorTile.ExtensionMethods;
using UnityEditor;
using UnityEngine;

namespace Mapbox.Editor
{
	[CustomPropertyDrawer(typeof(GeometryExtrusionOptions))]
	public class GeometryExtrusionOptionsDrawer : PropertyDrawer
	{
		private static List<string> _propertyNamesList = new();
		private static TileJsonData tileJsonData = new();

		private static TileJSONResponse tileJsonResponse;

		//indices for tileJSON lookup
		private int _propertyIndex;
		private GUIContent[] _propertyNameContent;

		private GUIContent[] extrusionTypeContent;
		private bool isGUIContentSet;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var extrusionTypeProperty = property.FindPropertyRelative("extrusionType");
			var displayNames = extrusionTypeProperty.enumDisplayNames;
			var count = extrusionTypeProperty.enumDisplayNames.Length;

			if (!isGUIContentSet)
			{
				extrusionTypeContent = new GUIContent[count];
				for (var extIdx = 0; extIdx < count; extIdx++)
					extrusionTypeContent[extIdx] = new GUIContent
					{
						text = displayNames[extIdx], tooltip = ((ExtrusionType)extIdx).Description()
					};
				isGUIContentSet = true;
			}

			var extrusionTypeLabel = new GUIContent { text = "Extrusion Type", tooltip = "Type of geometry extrusion" };

			EditorGUI.BeginChangeCheck();
			extrusionTypeProperty.enumValueIndex = EditorGUILayout.Popup(extrusionTypeLabel,
				extrusionTypeProperty.enumValueIndex, extrusionTypeContent);
			if (EditorGUI.EndChangeCheck()) EditorHelper.CheckForModifiedProperty(property);

			var sourceTypeValue = (ExtrusionType)extrusionTypeProperty.enumValueIndex;

			var minHeightProperty = property.FindPropertyRelative("minimumHeight");
			var maxHeightProperty = property.FindPropertyRelative("maximumHeight");

			var extrusionGeometryType = property.FindPropertyRelative("extrusionGeometryType");
			var extrusionGeometryGUI = new GUIContent
			{
				text = "Geometry Type",
				tooltip = ((ExtrusionGeometryType)extrusionGeometryType.enumValueIndex).Description()
			};
			EditorGUI.indentLevel++;

			EditorGUI.BeginChangeCheck();

			switch (sourceTypeValue)
			{
				case ExtrusionType.None:
					break;
				case ExtrusionType.PropertyHeight:
					EditorGUILayout.PropertyField(extrusionGeometryType, extrusionGeometryGUI);
					DrawPropertyDropDown(property, position);
					break;
				case ExtrusionType.MinHeight:
					EditorGUILayout.PropertyField(extrusionGeometryType, extrusionGeometryGUI);
					DrawPropertyDropDown(property, position);
					break;
				case ExtrusionType.MaxHeight:
					EditorGUILayout.PropertyField(extrusionGeometryType, extrusionGeometryGUI);
					DrawPropertyDropDown(property, position);
					break;
				case ExtrusionType.RangeHeight:
					EditorGUILayout.PropertyField(extrusionGeometryType, extrusionGeometryGUI);
					DrawPropertyDropDown(property, position);
					EditorGUILayout.PropertyField(minHeightProperty);
					EditorGUILayout.PropertyField(maxHeightProperty);
					if (minHeightProperty.floatValue > maxHeightProperty.floatValue)
						EditorGUILayout.HelpBox("Maximum Height less than Minimum Height!", MessageType.Error);
					break;
				case ExtrusionType.AbsoluteHeight:
					EditorGUILayout.PropertyField(extrusionGeometryType, extrusionGeometryGUI);
					EditorGUILayout.PropertyField(maxHeightProperty, new GUIContent { text = "Height" });
					break;
			}

			if (EditorGUI.EndChangeCheck()) EditorHelper.CheckForModifiedProperty(property);

			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(property.FindPropertyRelative("extrusionScaleFactor"),
				new GUIContent { text = "Scale Factor" });
			if (EditorGUI.EndChangeCheck()) EditorHelper.CheckForModifiedProperty(property);

			EditorGUI.indentLevel--;
		}

		private void DrawPropertyDropDown(SerializedProperty property, Rect position)
		{
			var selectedLayerName = property.FindPropertyRelative("_selectedLayerName").stringValue;

			var serializedMapObject = property.serializedObject;
			var mapObject = (AbstractMap)serializedMapObject.targetObject;
			tileJsonData = mapObject.VectorData.GetTileJsonData();

			DrawPropertyName(property, position, selectedLayerName);
		}

		private void DrawPropertyName(SerializedProperty property, Rect position, string selectedLayerName)
		{
			var parsedString = "No property selected";
			var descriptionString = "No description available";

			if (string.IsNullOrEmpty(selectedLayerName) || tileJsonData == null ||
			    !tileJsonData.PropertyDisplayNames.ContainsKey(selectedLayerName))
			{
				DrawWarningMessage(position);
			}
			else
			{
				var propertyDisplayNames = tileJsonData.PropertyDisplayNames[selectedLayerName];
				_propertyNamesList = new List<string>(propertyDisplayNames);

				//check if the selection is valid
				var propertyString = property.FindPropertyRelative("propertyName").stringValue;
				if (_propertyNamesList.Contains(propertyString))
				{
					//if the layer contains the current layerstring, set it's index to match
					_propertyIndex = propertyDisplayNames.FindIndex(s => s.Equals(propertyString));


					//create guicontent for a valid layer
					_propertyNameContent = new GUIContent[_propertyNamesList.Count];
					for (var extIdx = 0; extIdx < _propertyNamesList.Count; extIdx++)
					{
						var parsedPropertyString = _propertyNamesList[extIdx]
							.Split(new[] { tileJsonData.optionalPropertiesString }, StringSplitOptions.None)[0].Trim();
						_propertyNameContent[extIdx] = new GUIContent
						{
							text = _propertyNamesList[extIdx],
							tooltip =
								tileJsonData.LayerPropertyDescriptionDictionary[selectedLayerName][
									parsedPropertyString]
						};
					}

					//display popup
					var propertyNameLabel = new GUIContent
					{
						text = "Property Name",
						tooltip =
							"The name of the property in the selected Mapbox layer that will be used for extrusion"
					};

					EditorGUI.BeginChangeCheck();
					_propertyIndex = EditorGUILayout.Popup(propertyNameLabel, _propertyIndex, _propertyNameContent);
					if (EditorGUI.EndChangeCheck()) EditorHelper.CheckForModifiedProperty(property);

					//set new string values based on selection
					parsedString = _propertyNamesList[_propertyIndex]
						.Split(new[] { tileJsonData.optionalPropertiesString }, StringSplitOptions.None)[0].Trim();
					descriptionString =
						tileJsonData.LayerPropertyDescriptionDictionary[selectedLayerName][parsedString];
				}
				else
				{
					//if the selected layer isn't in the source, add a placeholder entry
					_propertyIndex = 0;
					_propertyNamesList.Insert(0, propertyString);

					//create guicontent for an invalid layer
					_propertyNameContent = new GUIContent[_propertyNamesList.Count];

					//first property gets a unique tooltip
					_propertyNameContent[0] = new GUIContent
					{
						text = _propertyNamesList[0], tooltip = "Unavialable in Selected Layer"
					};

					for (var extIdx = 1; extIdx < _propertyNamesList.Count; extIdx++)
					{
						var parsedPropertyString = _propertyNamesList[extIdx]
							.Split(new[] { tileJsonData.optionalPropertiesString }, StringSplitOptions.None)[0].Trim();
						_propertyNameContent[extIdx] = new GUIContent
						{
							text = _propertyNamesList[extIdx],
							tooltip =
								tileJsonData.LayerPropertyDescriptionDictionary[selectedLayerName][
									parsedPropertyString]
						};
					}

					//display popup
					var propertyNameLabel = new GUIContent
					{
						text = "Property Name",
						tooltip =
							"The name of the property in the selected Mapbox layer that will be used for extrusion"
					};

					EditorGUI.BeginChangeCheck();
					_propertyIndex = EditorGUILayout.Popup(propertyNameLabel, _propertyIndex, _propertyNameContent);
					if (EditorGUI.EndChangeCheck()) EditorHelper.CheckForModifiedProperty(property);

					//set new string values based on the offset
					parsedString = _propertyNamesList[_propertyIndex]
						.Split(new[] { tileJsonData.optionalPropertiesString }, StringSplitOptions.None)[0].Trim();
					descriptionString = "Unavailable in Selected Layer.";
				}

				property.FindPropertyRelative("propertyName").stringValue = parsedString;
				property.FindPropertyRelative("propertyDescription").stringValue = descriptionString;
			}

			descriptionString =
				string.IsNullOrEmpty(descriptionString) ? "No description available" : descriptionString;

			var propertyDescriptionPrefixLabel = new GUIContent
			{
				text = "Property Description", tooltip = "Factual information about the selected property"
			};
			EditorGUILayout.LabelField(propertyDescriptionPrefixLabel, new GUIContent(descriptionString),
				"wordWrappedLabel");
		}

		private void DrawWarningMessage(Rect position)
		{
			var labelStyle = new GUIStyle(EditorStyles.popup);
			labelStyle.fontStyle = FontStyle.Bold;
			var layerNameLabel = new GUIContent
			{
				text = "Property Name",
				tooltip = "The name of the property in the selected Mapbox layer that will be used for extrusion"
			};
			EditorGUILayout.LabelField(layerNameLabel, new GUIContent("No properties found in layer"), labelStyle);
		}
	}
}
