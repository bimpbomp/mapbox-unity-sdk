﻿using System;
using System.Collections.Generic;
using System.IO;
using Mapbox.Unity;
using Mapbox.Unity.Map;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Mapbox.Editor
{
	/// <summary>
	///     Pop up menu for selecting, creating and assigning modifier instances to AbstractMap.
	/// </summary>
	public class PopupSelectionMenu : PopupWindowContent
	{
		private readonly SerializedProperty _finalize;

		private List<Type> _modTypes;

		private Vector2 _scrollPos;

		private readonly Type _type;

		/// <summary>
		///     Initializes a new instance of the <see cref="T:Mapbox.Editor.PopupSelectionMenu" /> class.
		/// </summary>
		/// <param name="t">T.</param>
		/// <param name="p">P.</param>
		public PopupSelectionMenu(Type t, SerializedProperty p)
		{
			_type = t;
			_finalize = p;
		}

		public override Vector2 GetWindowSize()
		{
			return new Vector2(250, 250);
		}

		public override void OnGUI(Rect rect)
		{
			if (_modTypes == null || _modTypes.Count == 0)
			{
				_modTypes = new List<Type>();

				var currentDomain = AppDomain.CurrentDomain;
				var assemblies = currentDomain.GetAssemblies();
				for (var i = 0; i < assemblies.Length; i++)
				{
					var types = assemblies[i].GetTypes();
					for (var j = 0; j < types.Length; j++)
						if (types[j].IsSubclassOf(_type))
							_modTypes.Add(types[j]);
				}
			}

			GUILayout.Label(string.Format("{0}s", _type.Name), EditorStyles.boldLabel);
			var st = new GUIStyle();
			st.padding = new RectOffset(0, 0, 15, 15);
			_scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, st);

			for (var i = 0; i < _modTypes.Count; i++)
			{
				var asset = _modTypes[i];
				if (asset == null) //yea turns out this can happen
					continue;
				var style = GUI.skin.button;
				style.alignment = TextAnchor.MiddleLeft;
				var shortTypeName = GetShortTypeName(asset.ToString());
				if (GUILayout.Button(shortTypeName, style))
				{
					CreateNewModiferInstance(asset, shortTypeName);
					editorWindow.Close();
				}
			}

			EditorGUILayout.EndScrollView();
		}

		/// <summary>
		///     Gets the short name of the type.
		/// </summary>
		/// <returns>The short type name.</returns>
		/// <param name="input">Input.</param>
		private string GetShortTypeName(string input)
		{
			var pos = input.LastIndexOf(".", StringComparison.CurrentCulture) + 1;
			return input.Substring(pos, input.Length - pos);
		}

		/// <summary>
		///     Creates the new modifer instance.
		/// </summary>
		/// <param name="type">Type.</param>
		/// <param name="name">Name.</param>
		private void CreateNewModiferInstance(Type type, string name)
		{
			var modifierInstance = ScriptableObject.CreateInstance(type);

			var pathCandidate = Constants.Path.MAPBOX_USER_MODIFIERS;
			if (!Directory.Exists(pathCandidate))
			{
				var userFolder = Constants.Path.MAPBOX_USER;
				if (!Directory.Exists(userFolder))
				{
					var parentPath = Path.Combine("Assets", "Mapbox");
					AssetDatabase.CreateFolder(parentPath, "User");
				}

				AssetDatabase.CreateFolder(userFolder, "Modifiers");
			}

			foreach (var obj in Selection.GetFiltered(typeof(Object), SelectionMode.Assets))
			{
				pathCandidate = AssetDatabase.GetAssetPath(obj);
				if (!string.IsNullOrEmpty(pathCandidate) && File.Exists(pathCandidate))
				{
					pathCandidate = Path.GetDirectoryName(pathCandidate);
					break;
				}
			}

			var combinedPath = string.Format("{0}{1}.asset", Path.Combine(pathCandidate, "New"), name);
			var uniqueAssetPath = AssetDatabase.GenerateUniqueAssetPath(combinedPath);

			modifierInstance.name = name;

			AssetDatabase.CreateAsset(modifierInstance, uniqueAssetPath);

			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();

			AddNewInstanceToArray(modifierInstance);

			Debug.Log(string.Format("Created new {0} modifer at {1}", name, uniqueAssetPath));
		}

		/// <summary>
		///     Adds the new instance to array.
		/// </summary>
		/// <param name="obj">Object.</param>
		public void AddNewInstanceToArray(object obj)
		{
			var asset = obj as ScriptableObject;

			_finalize.arraySize++;
			_finalize.GetArrayElementAtIndex(_finalize.arraySize - 1).objectReferenceValue = asset;

			var mapboxDataProperty = (MapboxDataProperty)EditorHelper.GetTargetObjectWithProperty(_finalize);
			if (_finalize.serializedObject.ApplyModifiedProperties() && mapboxDataProperty != null)
				mapboxDataProperty.HasChanged = true;
		}
	}
}
