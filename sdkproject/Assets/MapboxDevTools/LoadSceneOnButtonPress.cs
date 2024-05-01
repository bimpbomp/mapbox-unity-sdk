using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Utilities
{
	[RequireComponent(typeof(Button))]
	public class LoadSceneOnButtonPress : MonoBehaviour
	{
		private Button _button;

		private void Awake()
		{
			_button = GetComponent<Button>();
			_button.onClick.AddListener(LoadScene);
		}

		private void LoadScene()
		{
			var scenePath = GetComponentInChildren<Text>().text;
			SceneManager.LoadScene(scenePath);
		}
	}
}
