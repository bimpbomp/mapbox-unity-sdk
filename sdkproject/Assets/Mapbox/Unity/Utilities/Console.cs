using UnityEngine;
using UnityEngine.UI;

namespace Mapbox.Unity.Utilities
{
	public class Console : MonoBehaviour
	{
		[SerializeField] private Button _toggleButton;

		[SerializeField] private GameObject _menu;

		[SerializeField] private Text _logText;

		[SerializeField] private ScrollRect _logScroll;

		private string _log;
		public static Console Instance { get; private set; }


		protected virtual void Awake()
		{
			if (Instance != null) Debug.LogError("Duplicate singleton!", gameObject);
			Instance = this;
			ClearLog();
		}

		private void ClearLog()
		{
			_log = "";
			_logText.text = _log;
			_logScroll.verticalNormalizedPosition = 0f;
		}

		public void Log(string log, string color)
		{
			if (!string.IsNullOrEmpty(_log) && _log.Length > 15000) _log = "";
			_log += string.Format("<color={0}>{1}</color>\n", color, log);
			_logText.text = _log;
			_logScroll.verticalNormalizedPosition = 0f;
		}


		public void ToggleMenu()
		{
			_menu.SetActive(!_menu.activeSelf);
		}
	}
}
