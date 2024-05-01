using Mapbox.Unity.Utilities;
using UnityEditor;
using UnityEngine;

namespace Mapbox.Editor
{
	/// <summary>
	///     Custom property drawer for geocodes
	///     <para />
	///     Includes a search window to enable search of Lat/Lon via geocoder.
	///     Requires a Mapbox token be set for the project
	/// </summary>
	[CustomPropertyDrawer(typeof(GeocodeAttribute))]
	public class GeocodeAttributeDrawer : PropertyDrawer
	{
		private const string searchButtonContent = "Search";

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var buttonWidth = EditorGUIUtility.singleLineHeight * 4;

			var fieldRect = new Rect(position.x, position.y, position.width - buttonWidth,
				EditorGUIUtility.singleLineHeight);
			var buttonRect = new Rect(position.x + position.width - buttonWidth, position.y, buttonWidth,
				EditorGUIUtility.singleLineHeight);

			EditorGUI.PropertyField(fieldRect, property);

			if (GUI.Button(buttonRect, searchButtonContent))
			{
				var objectToUpdate = EditorHelper.GetTargetObjectWithProperty(property);
				GeocodeAttributeSearchWindow.Open(property, objectToUpdate);
			}
		}
	}
}
