using System;
using Mapbox.Unity;
using Mapbox.Unity.Telemetry;
using UnityEngine;

public class TestTurnstile : MonoBehaviour
{
	[SerializeField] public string accessToken;

	[SerializeField] public bool _sendEvent;

	private ITelemetryLibrary _telemetryLibrary;

	// Start is called before the first frame update
	private void Start()
	{
	}

	// Update is called once per frame
	private void Update()
	{
		if (_sendEvent)
		{
			SendTurnstileEvent();
			_sendEvent = false;
		}
	}

	public void SendTurnstileEvent()
	{
		try
		{
			_telemetryLibrary = TelemetryFactory.GetTelemetryInstance();
			_telemetryLibrary.Initialize(accessToken);
			//_telemetryLibrary.SetLocationCollectionState(GetTelemetryCollectionState());
			_telemetryLibrary.SendTurnstile();
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Error initializing telemetry: {0}", ex);
		}
	}

	private bool GetTelemetryCollectionState()
	{
		if (!PlayerPrefs.HasKey(Constants.Path.SHOULD_COLLECT_LOCATION_KEY))
			PlayerPrefs.SetInt(Constants.Path.SHOULD_COLLECT_LOCATION_KEY, 1);
		return PlayerPrefs.GetInt(Constants.Path.SHOULD_COLLECT_LOCATION_KEY) != 0;
	}
}
