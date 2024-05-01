using UnityEngine;
using UnityEngine.UI;

namespace Mapbox.Unity.Utilities.DebugTools
{
	public class BuildExamplesNavigator : MonoBehaviour
	{
		[SerializeField] private GameObject _buttonPrefab;

		private void Awake()
		{
			var scenes = Resources.Load<ScenesList>("Mapbox/ScenesList").SceneList;
			foreach (var scene in scenes)
			{
				var button = Instantiate(_buttonPrefab);
				button.transform.SetParent(GetComponentInChildren<VerticalLayoutGroup>().transform);
				var text = button.GetComponentInChildren<Text>();
				text.text = scene.ScenePath.Replace(".unity", "").Replace("Assets/", "");
			}
		}
	}
}
