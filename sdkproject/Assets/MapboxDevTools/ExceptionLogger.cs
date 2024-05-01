using UnityEngine;

namespace Mapbox.Unity.Utilities.DebugTools
{
	public class ExceptionLogger : MonoBehaviour
	{
		private void Start()
		{
			Application.logMessageReceived += Application_logMessageReceived;
		}

		private void OnDisable()
		{
			Application.logMessageReceived -= Application_logMessageReceived;
		}

		private void Application_logMessageReceived(string condition, string stackTrace, LogType type)
		{
			var msg = string.Format("{0}: {1}\n{2}", type, condition, stackTrace);
			Debug.LogErrorFormat(msg);
			Console.Instance.Log(msg, "red");
		}
	}
}
