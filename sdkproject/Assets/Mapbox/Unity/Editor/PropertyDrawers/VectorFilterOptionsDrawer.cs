using System;
using System.Collections.Generic;
using Mapbox.Unity.Map;
using Mapbox.Unity.MeshGeneration.Filters;
using UnityEditor;
using UnityEngine;

namespace Mapbox.Editor
{
	[CustomPropertyDrawer(typeof(VectorFilterOptions))]
	public class VectorFilterOptionsDrawer : PropertyDrawer
	{
		private static readonly float lineHeight = EditorGUIUtility.singleLineHeight;

		//indices for tileJSON lookup
		private int _propertyIndex;
		private GUIContent[] _propertyNameContent;
		private List<string> _propertyNamesList = new();

		private string[] descriptionArray;

		private readonly GUIContent maxValueGui =
			new() { text = "Max", tooltip = "Maximum numeric value to match using the operator.  " };

		private readonly GUIContent minValueGui =
			new() { text = "Min", tooltip = "Minimum numeric value to match using the operator.  " };

		private readonly GUIContent numValueGui =
			new() { text = "Num Value", tooltip = "Numeric value to match using the operator.  " };

		private string objectId = "";

		private readonly GUIContent operatorGui = new() { text = "Operator", tooltip = "Filter operator to apply. " };

		private readonly GUIContent strValueGui =
			new() { text = "Str Value", tooltip = "String value to match using the operator.  " };

		private bool showFilters
		{
			get => EditorPrefs.GetBool(objectId + "VectorSubLayerProperties_showFilters");
			set => EditorPrefs.SetBool(objectId + "VectorSubLayerProperties_showFilters", value);
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			objectId = property.serializedObject.targetObject.GetInstanceID().ToString();
			var options = (VectorFilterOptions)EditorHelper.GetTargetObjectOfProperty(property);

			showFilters = EditorGUILayout.Foldout(showFilters,
				new GUIContent
				{
					text = "Filters", tooltip = "Filter features in a vector layer based on criterion specified.  "
				});
			if (showFilters)
			{
				var propertyFilters = property.FindPropertyRelative("filters");

				for (var i = 0; i < propertyFilters.arraySize; i++)
					DrawLayerFilter(property, propertyFilters, i, options);
				if (propertyFilters.arraySize > 0)
				{
					EditorGUI.BeginChangeCheck();
					EditorGUILayout.PropertyField(property.FindPropertyRelative("combinerType"));
					if (EditorGUI.EndChangeCheck()) EditorHelper.CheckForModifiedProperty(property);
				}

				EditorGUI.indentLevel++;
				EditorGUILayout.BeginHorizontal();
				GUILayout.Space(EditorGUI.indentLevel * 12);

				EditorGUI.BeginChangeCheck();
				if (GUILayout.Button(new GUIContent("Add New Empty"), "minibutton")) options.AddFilter();
				if (EditorGUI.EndChangeCheck()) EditorHelper.CheckForModifiedProperty(property);
				EditorGUILayout.EndHorizontal();
				EditorGUI.indentLevel--;
			}
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return lineHeight;
		}

		private void DrawLayerFilter(SerializedProperty originalProperty, SerializedProperty propertyFilters, int index,
			VectorFilterOptions vectorFilterOptions)
		{
			var property = propertyFilters.GetArrayElementAtIndex(index);

			var filterOperatorProp = property.FindPropertyRelative("filterOperator");

			EditorGUILayout.BeginVertical();

			EditorGUILayout.BeginHorizontal();

			EditorGUILayout.LabelField(
				new GUIContent
				{
					text = "Key", tooltip = "Name of the property to use as key. This property is case sensitive."
				}, GUILayout.MaxWidth(150));

			switch ((LayerFilterOperationType)filterOperatorProp.enumValueIndex)
			{
				case LayerFilterOperationType.IsEqual:
				case LayerFilterOperationType.IsGreater:
				case LayerFilterOperationType.IsLess:
					EditorGUILayout.LabelField(operatorGui, GUILayout.MaxWidth(150));
					EditorGUILayout.LabelField(numValueGui, GUILayout.MaxWidth(100));
					break;
				case LayerFilterOperationType.Contains:
					EditorGUILayout.LabelField(operatorGui, GUILayout.MaxWidth(150));
					EditorGUILayout.LabelField(strValueGui, GUILayout.MaxWidth(100));
					break;
				case LayerFilterOperationType.IsInRange:
					EditorGUILayout.LabelField(operatorGui, GUILayout.MaxWidth(150));
					EditorGUILayout.LabelField(minValueGui, GUILayout.MaxWidth(100));
					EditorGUILayout.LabelField(maxValueGui, GUILayout.MaxWidth(100));
					break;
			}

			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();

			var selectedLayerName = originalProperty.FindPropertyRelative("_selectedLayerName").stringValue;

			DrawPropertyDropDown(originalProperty, property);

			EditorGUI.BeginChangeCheck();
			filterOperatorProp.enumValueIndex = EditorGUILayout.Popup(filterOperatorProp.enumValueIndex,
				filterOperatorProp.enumDisplayNames, GUILayout.MaxWidth(150));
			if (EditorGUI.EndChangeCheck()) EditorHelper.CheckForModifiedProperty(property);

			EditorGUI.BeginChangeCheck();
			switch ((LayerFilterOperationType)filterOperatorProp.enumValueIndex)
			{
				case LayerFilterOperationType.IsEqual:
				case LayerFilterOperationType.IsGreater:
				case LayerFilterOperationType.IsLess:
					property.FindPropertyRelative("Min").doubleValue =
						EditorGUILayout.DoubleField(property.FindPropertyRelative("Min").doubleValue,
							GUILayout.MaxWidth(100));
					break;
				case LayerFilterOperationType.Contains:
					property.FindPropertyRelative("PropertyValue").stringValue =
						EditorGUILayout.TextField(property.FindPropertyRelative("PropertyValue").stringValue,
							GUILayout.MaxWidth(150));
					break;
				case LayerFilterOperationType.IsInRange:
					property.FindPropertyRelative("Min").doubleValue =
						EditorGUILayout.DoubleField(property.FindPropertyRelative("Min").doubleValue,
							GUILayout.MaxWidth(100));
					property.FindPropertyRelative("Max").doubleValue =
						EditorGUILayout.DoubleField(property.FindPropertyRelative("Max").doubleValue,
							GUILayout.MaxWidth(100));
					break;
			}

			if (EditorGUI.EndChangeCheck()) EditorHelper.CheckForModifiedProperty(property);

			if (GUILayout.Button(new GUIContent(" X "), "minibuttonright", GUILayout.Width(30)))
			{
				vectorFilterOptions.RemoveFilter(index);
				propertyFilters.DeleteArrayElementAtIndex(index);
			}

			EditorGUILayout.EndHorizontal();

			EditorGUILayout.EndVertical();
		}

		private void DrawPropertyDropDown(SerializedProperty originalProperty, SerializedProperty filterProperty)
		{
			var selectedLayerName = originalProperty.FindPropertyRelative("_selectedLayerName").stringValue;
			var mapObject = (AbstractMap)originalProperty.serializedObject.targetObject;
			var tileJsonData = mapObject.VectorData.GetTileJsonData();

			if (string.IsNullOrEmpty(selectedLayerName) ||
			    !tileJsonData.PropertyDisplayNames.ContainsKey(selectedLayerName))
			{
				DrawWarningMessage();
				return;
			}

			var parsedString = "no property selected";
			var descriptionString = "no description available";
			var propertyDisplayNames = tileJsonData.PropertyDisplayNames[selectedLayerName];
			_propertyNamesList = new List<string>(propertyDisplayNames);

			var propertyString = filterProperty.FindPropertyRelative("Key").stringValue;
			//check if the selection is valid
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
							tileJsonData.LayerPropertyDescriptionDictionary[selectedLayerName][parsedPropertyString]
					};
				}

				//display popup
				EditorGUI.BeginChangeCheck();
				_propertyIndex = EditorGUILayout.Popup(_propertyIndex, _propertyNameContent, GUILayout.MaxWidth(150));
				if (EditorGUI.EndChangeCheck()) EditorHelper.CheckForModifiedProperty(filterProperty);

				//set new string values based on selection
				parsedString = _propertyNamesList[_propertyIndex].Split(new[] { tileJsonData.optionalPropertiesString },
					StringSplitOptions.None)[0].Trim();
				descriptionString = tileJsonData.LayerPropertyDescriptionDictionary[selectedLayerName][parsedString];
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
							tileJsonData.LayerPropertyDescriptionDictionary[selectedLayerName][parsedPropertyString]
					};
				}

				//display popup
				EditorGUI.BeginChangeCheck();
				_propertyIndex = EditorGUILayout.Popup(_propertyIndex, _propertyNameContent, GUILayout.MaxWidth(150));
				if (EditorGUI.EndChangeCheck()) EditorHelper.CheckForModifiedProperty(filterProperty);

				//set new string values based on the offset
				parsedString = _propertyNamesList[_propertyIndex].Split(new[] { tileJsonData.optionalPropertiesString },
					StringSplitOptions.None)[0].Trim();
				descriptionString = "Unavailable in Selected Layer.";
			}

			EditorGUI.BeginChangeCheck();
			filterProperty.FindPropertyRelative("Key").stringValue = parsedString;
			if (EditorGUI.EndChangeCheck()) EditorHelper.CheckForModifiedProperty(filterProperty);
			filterProperty.FindPropertyRelative("KeyDescription").stringValue = descriptionString;
		}

		private void DrawWarningMessage()
		{
			var labelStyle = new GUIStyle(EditorStyles.popup);
			labelStyle.fontStyle = FontStyle.Bold;
			EditorGUILayout.LabelField(new GUIContent(), new GUIContent("No properties"), labelStyle,
				GUILayout.MaxWidth(155));
		}
	}
}
