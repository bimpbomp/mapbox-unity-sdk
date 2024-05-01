using Mapbox.Unity.Map;
using Mapbox.VectorTile.ExtensionMethods;
using UnityEditor;
using UnityEngine;

namespace Mapbox.Editor
{
	[CustomPropertyDrawer(typeof(ColliderOptions))]
	public class ColliderOptionsDrawer : PropertyDrawer
	{
		private static readonly float lineHeight = EditorGUIUtility.singleLineHeight;
		private GUIContent[] colliderTypeContent;
		private bool isGUIContentSet;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, null, property);
			var colliderTypeLabel = new GUIContent
			{
				text = "Collider Type", tooltip = "The type of collider added to game objects in this layer."
			};
			var colliderTypeProperty = property.FindPropertyRelative("colliderType");

			var displayNames = colliderTypeProperty.enumDisplayNames;
			var count = colliderTypeProperty.enumDisplayNames.Length;

			if (!isGUIContentSet)
			{
				colliderTypeContent = new GUIContent[count];
				for (var extIdx = 0; extIdx < count; extIdx++)
					colliderTypeContent[extIdx] = new GUIContent
					{
						text = displayNames[extIdx], tooltip = ((ColliderType)extIdx).Description()
					};
				isGUIContentSet = true;
			}

			EditorGUI.BeginChangeCheck();
			colliderTypeProperty.enumValueIndex = EditorGUILayout.Popup(colliderTypeLabel,
				colliderTypeProperty.enumValueIndex, colliderTypeContent);
			if (EditorGUI.EndChangeCheck()) EditorHelper.CheckForModifiedProperty(property);

			EditorGUI.EndProperty();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return lineHeight;
		}
	}
}
