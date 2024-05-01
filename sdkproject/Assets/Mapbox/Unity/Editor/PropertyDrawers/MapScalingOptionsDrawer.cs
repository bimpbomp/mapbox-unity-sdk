using Mapbox.Unity.Map;
using Mapbox.VectorTile.ExtensionMethods;
using UnityEditor;
using UnityEngine;

namespace Mapbox.Editor
{
	[CustomPropertyDrawer(typeof(MapScalingOptions))]
	public class MapScalingOptionsDrawer : PropertyDrawer
	{
		private static readonly float lineHeight = EditorGUIUtility.singleLineHeight;
		private bool isGUIContentSet;
		private GUIContent[] scalingTypeContent;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var scalingType = property.FindPropertyRelative("scalingType");
			var displayNames = scalingType.enumDisplayNames;
			var count = scalingType.enumDisplayNames.Length;
			if (!isGUIContentSet)
			{
				scalingTypeContent = new GUIContent[count];
				for (var extIdx = 0; extIdx < count; extIdx++)
					scalingTypeContent[extIdx] = new GUIContent
					{
						text = displayNames[extIdx], tooltip = ((MapScalingType)extIdx).Description()
					};
				isGUIContentSet = true;
			}

			// Draw label.
			var scalingTypeLabel = new GUIContent { text = label.text, tooltip = "Scale of map in game units." };

			scalingType.enumValueIndex =
				EditorGUILayout.Popup(scalingTypeLabel, scalingType.enumValueIndex, scalingTypeContent);

			if ((MapScalingType)scalingType.enumValueIndex == MapScalingType.Custom)
			{
				position.y += lineHeight;
				EditorGUILayout.PropertyField(property.FindPropertyRelative("unityTileSize"));
			}
		}
	}
}
