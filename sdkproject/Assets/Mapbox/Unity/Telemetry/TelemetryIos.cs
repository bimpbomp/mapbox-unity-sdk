﻿#if UNITY_IOS
namespace Mapbox.Unity.Telemetry
{
	using System.Runtime.InteropServices;

	public class TelemetryIos : ITelemetryLibrary
	{
		[DllImport("__Internal")]
		private static extern void initialize(string accessToken, string userAgentBase, string hostSDKVersion);

		[DllImport("__Internal")]
		static extern void sendTurnstileEvent();

		[DllImport("__Internal")]
		private static extern void setLocationCollectionState(bool enable);

		[DllImport("__Internal")]
		private static extern void setSkuId(string skuId);

		static ITelemetryLibrary _instance = new TelemetryIos();
		public static ITelemetryLibrary Instance
		{
			get
			{
				return _instance;
			}
		}

		public void Initialize(string accessToken)
		{
			initialize(accessToken, "MapboxEventsUnityiOS", Constants.SDK_VERSION);
		}

		public void SendTurnstile()
		{
			setSkuId(Constants.SDK_SKU_ID);
			sendTurnstileEvent();
		}

		public void SetLocationCollectionState(bool enable)
		{
			setLocationCollectionState(enable);
		}
	}
}
#endif
