using Mapbox.Unity.Map;
using UnityEditor;
using UnityEngine;

namespace Mapbox.Editor
{
	[CustomPropertyDrawer(typeof(TerrainSideWallOptions))]
	public class TerrainSideWallOptionsDrawer : PropertyDrawer
	{
		private static readonly float lineHeight = EditorGUIUtility.singleLineHeight;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var isSidewallActiveProp = property.FindPropertyRelative("isActive");
			EditorGUI.BeginProperty(position, label, property);

			EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, lineHeight), isSidewallActiveProp,
				new GUIContent("Show Sidewalls"));
			if (isSidewallActiveProp.boolValue)
			{
				EditorGUI.indentLevel++;
				position.y += lineHeight;
				EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, lineHeight),
					property.FindPropertyRelative("wallHeight"));
				position.y += lineHeight;
				EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, lineHeight),
					property.FindPropertyRelative("wallMaterial"));
				EditorGUI.indentLevel--;
			}

			EditorGUI.EndProperty();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			// Reserve space for the total visible properties.
			var isSidewallActiveProp = property.FindPropertyRelative("isActive");
			if (isSidewallActiveProp.boolValue) return 3.0f * lineHeight;
			return 1.0f * lineHeight;
		}
	}
}
