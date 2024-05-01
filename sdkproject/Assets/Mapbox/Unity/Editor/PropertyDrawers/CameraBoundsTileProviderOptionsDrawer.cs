﻿using Mapbox.Unity.Map;
using UnityEditor;
using UnityEngine;

namespace Mapbox.Editor
{
	[CustomPropertyDrawer(typeof(CameraBoundsTileProviderOptions))]
	public class CameraBoundsTileProviderOptionsDrawer : PropertyDrawer
	{
		private static readonly float _lineHeight = EditorGUIUtility.singleLineHeight;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var camera = property.FindPropertyRelative("camera");
			EditorGUI.PropertyField(position, camera,
				new GUIContent { text = camera.displayName, tooltip = "Camera to control map extent." });
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return 1 * _lineHeight;
		}
	}
}
