using Mapbox.Unity.Map;
using Mapbox.VectorTile.ExtensionMethods;
using UnityEditor;
using UnityEngine;

namespace Mapbox.Editor
{
	[CustomPropertyDrawer(typeof(CoreVectorLayerProperties))]
	public class CoreVectorLayerPropertiesDrawer : PropertyDrawer
	{
		private bool _isGUIContentSet;
		private GUIContent[] _primitiveTypeContent;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, null, property);

			var primitiveType = property.FindPropertyRelative("geometryType");

			var primitiveTypeLabel = new GUIContent
			{
				text = "Primitive Type",
				tooltip = "Primitive geometry type of the visualizer, allowed primitives - point, line, polygon."
			};

			var displayNames = primitiveType.enumDisplayNames;
			var count = primitiveType.enumDisplayNames.Length;

			if (!_isGUIContentSet)
			{
				_primitiveTypeContent = new GUIContent[count];
				for (var extIdx = 0; extIdx < count; extIdx++)
					_primitiveTypeContent[extIdx] = new GUIContent
					{
						text = displayNames[extIdx], tooltip = ((VectorPrimitiveType)extIdx).Description()
					};
				_isGUIContentSet = true;
			}

			EditorGUI.BeginChangeCheck();
			primitiveType.enumValueIndex =
				EditorGUILayout.Popup(primitiveTypeLabel, primitiveType.enumValueIndex, _primitiveTypeContent);
			if (EditorGUI.EndChangeCheck()) EditorHelper.CheckForModifiedProperty(property);
			EditorGUI.EndProperty();
		}
	}
}
