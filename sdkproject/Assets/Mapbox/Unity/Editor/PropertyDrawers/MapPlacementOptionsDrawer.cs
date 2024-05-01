using Mapbox.Unity.Map;
using Mapbox.VectorTile.ExtensionMethods;
using UnityEditor;
using UnityEngine;

namespace Mapbox.Editor
{
	[CustomPropertyDrawer(typeof(MapPlacementOptions))]
	public class MapPlacementOptionsDrawer : PropertyDrawer
	{
		private bool isGUIContentSet;
		private GUIContent[] placementTypeContent;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var placementType = property.FindPropertyRelative("placementType");
			var snapMapToTerrain = property.FindPropertyRelative("snapMapToZero");

			var displayNames = placementType.enumDisplayNames;
			var count = placementType.enumDisplayNames.Length;
			if (!isGUIContentSet)
			{
				placementTypeContent = new GUIContent[count];
				for (var extIdx = 0; extIdx < count; extIdx++)
					placementTypeContent[extIdx] = new GUIContent
					{
						text = displayNames[extIdx], tooltip = ((MapPlacementType)extIdx).Description()
					};
				isGUIContentSet = true;
			}

			placementType.enumValueIndex = EditorGUILayout.Popup(
				new GUIContent { text = label.text, tooltip = "Placement of Map root." }, placementType.enumValueIndex,
				placementTypeContent);
			EditorGUILayout.PropertyField(snapMapToTerrain,
				new GUIContent
				{
					text = snapMapToTerrain.displayName,
					tooltip = "If checked, map's root will be snapped to zero. "
				});
		}
	}
}
