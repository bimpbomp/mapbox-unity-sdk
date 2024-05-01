using UnityEngine;
using UnityEngine.UI;

namespace Mapbox.Unity.Utilities
{
	[RequireComponent(typeof(Button))]
	public class TelemetryConfigurationButton : MonoBehaviour
	{
		[SerializeField] private bool _booleanValue;

		protected virtual void Awake()
		{
			GetComponent<Button>().onClick.AddListener(SetPlayerPref);
		}

		private void SetPlayerPref()
		{
			MapboxAccess.Instance.SetLocationCollectionState(_booleanValue);
			PlayerPrefs.Save();
		}
	}
}
