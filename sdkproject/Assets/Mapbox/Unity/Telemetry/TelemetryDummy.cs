namespace Mapbox.Unity.Telemetry
{
	public class TelemetryDummy : ITelemetryLibrary
	{
		public static ITelemetryLibrary Instance { get; } = new TelemetryDummy();

		public void Initialize(string accessToken)
		{
			// empty.
		}

		public void SendTurnstile()
		{
			// empty.
		}

		public void SetLocationCollectionState(bool enable)
		{
			// empty.
		}
	}
}
