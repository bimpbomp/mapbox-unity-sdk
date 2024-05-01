using Mapbox.Unity.MeshGeneration.Modifiers;
using UnityEditor;
using UnityEngine;

namespace Mapbox.Editor
{
	[CustomEditor(typeof(ModifierStack))]
	public class ModifierStackEditor : UnityEditor.Editor
	{
		private Texture2D _magnifier;
		private SerializedProperty _positionType;

		private MonoScript script;

		private void OnEnable()
		{
			script = MonoScript.FromScriptableObject((ModifierStack)target);
			_positionType = serializedObject.FindProperty("moveFeaturePositionTo");
			_magnifier = EditorGUIUtility.FindTexture("d_ViewToolZoom");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			GUI.enabled = false;
			script = EditorGUILayout.ObjectField("Script", script, typeof(MonoScript), false) as MonoScript;
			GUI.enabled = true;

			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(_positionType, new GUIContent("Feature Position"));


			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Mesh Modifiers");
			var meshfac = serializedObject.FindProperty("MeshModifiers");
			for (var i = 0; i < meshfac.arraySize; i++)
			{
				var ind = i;
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.BeginVertical();
				GUILayout.Space(5);
				meshfac.GetArrayElementAtIndex(ind).objectReferenceValue =
					EditorGUILayout.ObjectField(meshfac.GetArrayElementAtIndex(i).objectReferenceValue,
						typeof(MeshModifier), false) as ScriptableObject;
				EditorGUILayout.EndVertical();
				if (GUILayout.Button(_magnifier, "minibuttonleft", GUILayout.Width(30)))
					ScriptableCreatorWindow.Open(typeof(MeshModifier), meshfac, ind);
				if (GUILayout.Button(new GUIContent("-"), "minibuttonright", GUILayout.Width(30), GUILayout.Height(22)))
					meshfac.DeleteArrayElementAtIndex(ind);
				EditorGUILayout.EndHorizontal();
			}

			EditorGUILayout.Space();
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button(new GUIContent("Add New Empty"), "minibuttonleft"))
			{
				meshfac.arraySize++;
				meshfac.GetArrayElementAtIndex(meshfac.arraySize - 1).objectReferenceValue = null;
			}

			if (GUILayout.Button(new GUIContent("Find Asset"), "minibuttonright"))
				ScriptableCreatorWindow.Open(typeof(MeshModifier), meshfac);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Game Object Modifiers");
			var gofac = serializedObject.FindProperty("GoModifiers");
			for (var i = 0; i < gofac.arraySize; i++)
			{
				var ind = i;
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.BeginVertical();
				GUILayout.Space(5);
				gofac.GetArrayElementAtIndex(ind).objectReferenceValue =
					EditorGUILayout.ObjectField(gofac.GetArrayElementAtIndex(i).objectReferenceValue,
						typeof(GameObjectModifier), false) as ScriptableObject;
				EditorGUILayout.EndVertical();

				if (GUILayout.Button(_magnifier, "minibuttonleft", GUILayout.Width(30)))
					ScriptableCreatorWindow.Open(typeof(GameObjectModifier), gofac, ind);
				if (GUILayout.Button(new GUIContent("-"), "minibuttonright", GUILayout.Width(30), GUILayout.Height(22)))
					gofac.DeleteArrayElementAtIndex(ind);
				EditorGUILayout.EndHorizontal();
			}

			EditorGUILayout.Space();
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button(new GUIContent("Add New Empty"), "minibuttonleft"))
			{
				gofac.arraySize++;
				gofac.GetArrayElementAtIndex(gofac.arraySize - 1).objectReferenceValue = null;
			}

			if (GUILayout.Button(new GUIContent("Find Asset"), "minibuttonright"))
				ScriptableCreatorWindow.Open(typeof(GameObjectModifier), gofac);
			EditorGUILayout.EndHorizontal();

			serializedObject.ApplyModifiedProperties();
		}
	}
}
