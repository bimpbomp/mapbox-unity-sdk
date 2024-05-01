using UnityEditor;
using UnityEngine;

namespace Mapbox.Unity.Utilities.DebugTools
{
	[CustomEditor(typeof(ScenesList))]
	public class ScenesListEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			var e = target as ScenesList;

			if (GUILayout.Button("Link Listed Scenes")) e.LinkScenes();
		}
	}
}
