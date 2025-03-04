﻿using Mapbox.Unity.MeshGeneration.Modifiers;
using UnityEditor;
using UnityEngine;

namespace Mapbox.Editor
{
	[CustomEditor(typeof(MergedModifierStack))]
	public class MergedModifierStackEditor : UnityEditor.Editor
	{
		private Texture2D _magnifier;
		private MonoScript script;

		private void OnEnable()
		{
			script = MonoScript.FromScriptableObject((MergedModifierStack)target);
			_magnifier = EditorGUIUtility.FindTexture("d_ViewToolZoom");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			GUI.enabled = false;
			script = EditorGUILayout.ObjectField("Script", script, typeof(MonoScript), false) as MonoScript;
			GUI.enabled = true;

			EditorGUILayout.Space();

			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Mesh Modifiers");
			var facs = serializedObject.FindProperty("MeshModifiers");
			for (var i = 0; i < facs.arraySize; i++)
			{
				var ind = i;
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.BeginVertical();
				GUILayout.Space(5);
				facs.GetArrayElementAtIndex(ind).objectReferenceValue =
					EditorGUILayout.ObjectField(facs.GetArrayElementAtIndex(i).objectReferenceValue,
						typeof(MeshModifier), false) as ScriptableObject;
				EditorGUILayout.EndVertical();

				if (GUILayout.Button(_magnifier, "minibuttonleft", GUILayout.Width(30)))
					ScriptableCreatorWindow.Open(typeof(MeshModifier), facs, ind);
				if (GUILayout.Button(new GUIContent("-"), "minibuttonright", GUILayout.Width(30), GUILayout.Height(22)))
					facs.DeleteArrayElementAtIndex(ind);
				EditorGUILayout.EndHorizontal();
			}

			EditorGUILayout.Space();
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button(new GUIContent("Add New Empty"), "minibuttonleft"))
			{
				facs.arraySize++;
				facs.GetArrayElementAtIndex(facs.arraySize - 1).objectReferenceValue = null;
			}

			if (GUILayout.Button(new GUIContent("Find Asset"), "minibuttonright"))
				ScriptableCreatorWindow.Open(typeof(MeshModifier), facs);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Game Object Modifiers");
			var facs2 = serializedObject.FindProperty("GoModifiers");
			for (var i = 0; i < facs2.arraySize; i++)
			{
				var ind = i;
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.BeginVertical();
				GUILayout.Space(5);
				facs2.GetArrayElementAtIndex(ind).objectReferenceValue =
					EditorGUILayout.ObjectField(facs2.GetArrayElementAtIndex(i).objectReferenceValue,
						typeof(GameObjectModifier), false) as ScriptableObject;

				EditorGUILayout.EndVertical();

				if (GUILayout.Button(_magnifier, "minibuttonleft", GUILayout.Width(30)))
					ScriptableCreatorWindow.Open(typeof(GameObjectModifier), facs2, ind);
				if (GUILayout.Button(new GUIContent("-"), "minibuttonright", GUILayout.Width(30), GUILayout.Height(22)))
					facs2.DeleteArrayElementAtIndex(ind);
				EditorGUILayout.EndHorizontal();
			}

			EditorGUILayout.Space();
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button(new GUIContent("Add New Empty"), "minibuttonleft"))
			{
				facs2.arraySize++;
				facs2.GetArrayElementAtIndex(facs2.arraySize - 1).objectReferenceValue = null;
			}

			if (GUILayout.Button(new GUIContent("Find Asset"), "minibuttonright"))
				ScriptableCreatorWindow.Open(typeof(GameObjectModifier), facs2);
			EditorGUILayout.EndHorizontal();

			serializedObject.ApplyModifiedProperties();
		}
	}
}
