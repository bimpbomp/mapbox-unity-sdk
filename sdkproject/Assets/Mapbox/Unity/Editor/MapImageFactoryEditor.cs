using Mapbox.Unity.MeshGeneration.Factories;
using UnityEditor;
using UnityEngine;

namespace Mapbox.Editor
{
	[CustomEditor(typeof(MapImageFactory))]
	public class MapImageFactoryEditor : FactoryEditor
	{
		public SerializedProperty layerProperties;
		private MonoScript script;

		private void OnEnable()
		{
			layerProperties = serializedObject.FindProperty("_properties");
			script = MonoScript.FromScriptableObject((MapImageFactory)target);
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			GUI.enabled = false;
			script = EditorGUILayout.ObjectField("Script", script, typeof(MonoScript), false) as MonoScript;
			GUI.enabled = true;

			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(layerProperties);
			EditorGUILayout.Space();

			serializedObject.ApplyModifiedProperties();
		}
	}
}
