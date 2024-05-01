using Mapbox.Unity.Map;
using UnityEditor;
using UnityEngine;

namespace Mapbox.Editor
{
	[CustomPropertyDrawer(typeof(LayerPerformanceOptions))]
	public class LayerPerformanceOptionsDrawer : PropertyDrawer
	{
		private SerializedProperty isActiveProperty;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			isActiveProperty = property.FindPropertyRelative("isEnabled");

			isActiveProperty.boolValue =
				EditorGUILayout.Toggle(new GUIContent("Enable Coroutines"), isActiveProperty.boolValue);

			if (isActiveProperty.boolValue)
			{
				EditorGUI.indentLevel++;
				EditorGUILayout.PropertyField(property.FindPropertyRelative("entityPerCoroutine"), true);
				EditorGUI.indentLevel--;
			}
		}
	}
}
