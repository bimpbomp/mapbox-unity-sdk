using System.IO;
using UnityEditor;
using UnityEngine;

namespace Mapbox.Unity.Utilities.DebugTools
{
	public static class NavigationBuilder
	{
		[MenuItem("Mapbox/Serialize Example Scenes")]
		public static void AddExampleScenesToBuildSettings()
		{
			var allScenes = PathHelpers.AllScenes;
			var buildScenes = new EditorBuildSettingsScene[allScenes.Count + 1];

			var mainScenes = AssetDatabase.FindAssets("main t:Scene");
			var mainScene = AssetDatabase.GUIDToAssetPath(mainScenes[0]);
			buildScenes[0] = new EditorBuildSettingsScene(mainScene, true);

			for (var i = 0; i < allScenes.Count; i++)
			{
				var sceneToAdd = new EditorBuildSettingsScene(allScenes[i], true);
				buildScenes[i + 1] = sceneToAdd;
			}

			EditorBuildSettings.scenes = buildScenes;
			SaveSceneList();
			AssetDatabase.Refresh();
		}

		private static void SaveSceneList()
		{
			var list = ScriptableObject.CreateInstance<ScenesList>();
			AssetDatabase.CreateAsset(list, Constants.Path.SCENELIST);

			var scenes = EditorBuildSettings.scenes;
			list.SceneList = new SceneData[scenes.Length - 1];
			for (var i = 0; i < scenes.Length - 1; ++i)
			{
				var scenePath = scenes[i + 1].path;
				var name = Path.GetFileNameWithoutExtension(scenePath);
				var imagePath = Directory.GetParent(scenePath) + "/Screenshots/" + name + ".png";
				Texture2D image = null;
				if (File.Exists(imagePath))
					image = (Texture2D)AssetDatabase.LoadAssetAtPath(imagePath, typeof(Texture2D));

				//todo text
				TextAsset text = null;

				var scene = ScriptableObject.CreateInstance<SceneData>();
				scene.name = name;
				scene.Name = name;
				scene.ScenePath = scenePath;
				scene.Text = text;
				scene.Image = image;

				AssetDatabase.AddObjectToAsset(scene, list);
				list.SceneList[i] = scene;
			}

			EditorUtility.SetDirty(list);
			AssetDatabase.SaveAssets();
		}

		private static void Verify(string path)
		{
			Debug.Log("NavigationBuilder: " + path);
			var scenes = Resources.Load<ScenesList>("Mapbox/ScenesList").SceneList;
			foreach (var scene in scenes) Debug.Log("NavigationBuilder: " + scene);
		}
	}
}
