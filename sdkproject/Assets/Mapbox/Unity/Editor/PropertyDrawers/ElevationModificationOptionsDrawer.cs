﻿using Mapbox.Unity.Map;
using UnityEditor;
using UnityEngine;

namespace Mapbox.Editor
{
	[CustomPropertyDrawer(typeof(ElevationModificationOptions))]
	public class ElevationModificationOptionsDrawer : PropertyDrawer
	{
		private static readonly float lineHeight = EditorGUIUtility.singleLineHeight;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);
			EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, lineHeight),
				property.FindPropertyRelative("sampleCount"));
			position.y += lineHeight;
			EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, lineHeight),
				property.FindPropertyRelative("useRelativeHeight"));
			position.y += lineHeight;
			EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, lineHeight),
				property.FindPropertyRelative("earthRadius"));
			EditorGUI.EndProperty();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			// Reserve space for the total visible properties.
			return 3.0f * lineHeight;
		}
	}
}
