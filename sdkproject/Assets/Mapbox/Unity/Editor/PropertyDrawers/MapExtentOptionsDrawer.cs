using Mapbox.Unity.Map;
using Mapbox.VectorTile.ExtensionMethods;
using UnityEditor;
using UnityEngine;

namespace Mapbox.Editor
{
	[CustomPropertyDrawer(typeof(MapExtentOptions))]
	public class MapExtentOptionsDrawer : PropertyDrawer
	{
		private static readonly string extTypePropertyName = "extentType";
		private static readonly float _lineHeight = EditorGUIUtility.singleLineHeight;
		private GUIContent[] extentTypeContent;
		private bool isGUIContentSet;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var kindProperty = property.FindPropertyRelative(extTypePropertyName);
			var displayNames = kindProperty.enumDisplayNames;
			var count = kindProperty.enumDisplayNames.Length;
			if (!isGUIContentSet)
			{
				extentTypeContent = new GUIContent[count];
				for (var extIdx = 0; extIdx < count; extIdx++)
					extentTypeContent[extIdx] = new GUIContent
					{
						text = displayNames[extIdx], tooltip = ((MapExtentType)extIdx).Description()
					};
				isGUIContentSet = true;
			}

			// Draw label.
			var extentTypeLabel = new GUIContent
			{
				text = label.text,
				tooltip =
					"Options to determine the geographic extent of the world for which the map tiles will be requested."
			};
			EditorGUI.BeginChangeCheck();
			kindProperty.enumValueIndex = EditorGUILayout.Popup(extentTypeLabel, kindProperty.enumValueIndex,
				extentTypeContent, GUILayout.Height(_lineHeight));

			var kind = (MapExtentType)kindProperty.enumValueIndex;
			if (EditorGUI.EndChangeCheck()) EditorHelper.CheckForModifiedProperty(property);

			EditorGUI.indentLevel++;

			GUILayout.Space(-_lineHeight);
			var defaultExtentsProp = property.FindPropertyRelative("defaultExtents");
			EditorGUI.BeginChangeCheck();

			switch (kind)
			{
				case MapExtentType.CameraBounds:
					GUILayout.Space(_lineHeight);
					EditorGUILayout.PropertyField(defaultExtentsProp.FindPropertyRelative("cameraBoundsOptions"),
						new GUIContent { text = "CameraOptions-" });
					break;
				case MapExtentType.RangeAroundCenter:
					EditorGUILayout.PropertyField(defaultExtentsProp.FindPropertyRelative("rangeAroundCenterOptions"),
						new GUIContent { text = "RangeAroundCenter" });
					break;
				case MapExtentType.RangeAroundTransform:
					GUILayout.Space(_lineHeight);
					EditorGUILayout.PropertyField(
						defaultExtentsProp.FindPropertyRelative("rangeAroundTransformOptions"),
						new GUIContent { text = "RangeAroundTransform" });
					break;
			}

			if (EditorGUI.EndChangeCheck()) EditorHelper.CheckForModifiedProperty(defaultExtentsProp);
			EditorGUI.indentLevel--;
		}
	}
}
