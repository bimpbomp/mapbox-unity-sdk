using Mapbox.Unity.Map;
using UnityEditor;
using UnityEngine;

namespace Mapbox.Editor
{
	[CustomPropertyDrawer(typeof(MapLocationOptions))]
	public class MapLocationOptionsDrawer : PropertyDrawer
	{
		private static readonly float _lineHeight = EditorGUIUtility.singleLineHeight;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.indentLevel++;
			GUILayout.Space(-1f * _lineHeight);
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(property.FindPropertyRelative("latitudeLongitude"));
			if (EditorGUI.EndChangeCheck()) EditorHelper.CheckForModifiedProperty(property);

			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(property.FindPropertyRelative("zoom"), GUILayout.Height(_lineHeight));
			if (EditorGUI.EndChangeCheck()) EditorHelper.CheckForModifiedProperty(property);
			EditorGUI.indentLevel--;
		}
	}
}
