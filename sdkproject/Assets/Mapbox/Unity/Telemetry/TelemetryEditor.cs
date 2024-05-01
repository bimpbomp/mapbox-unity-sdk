﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Mapbox.Json;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

#if UNITY_EDITOR
namespace Mapbox.Unity.Telemetry
{
	public class TelemetryEditor : ITelemetryLibrary
	{
		private string _url;

		public static ITelemetryLibrary Instance { get; } = new TelemetryEditor();

		public void Initialize(string accessToken)
		{
			_url = string.Format("{0}events/v2?access_token={1}", Utils.Constants.EventsAPI, accessToken);
		}

		public void SendTurnstile()
		{
			var ticks = DateTime.Now.Ticks;
			if (ShouldPostTurnstile(ticks)) Runnable.Run(PostWWW(_url, GetPostBody()));
		}

		public void SetLocationCollectionState(bool enable)
		{
			// Empty.
		}

		private string GetPostBody()
		{
			var eventList = new List<Dictionary<string, object>>();
			var jsonDict = new Dictionary<string, object>();

			var unixTimestamp = (long)UnixTimestampUtils.To(DateTime.UtcNow);

			jsonDict.Add("event", "appUserTurnstile");
			jsonDict.Add("created", unixTimestamp);
			jsonDict.Add("userId", SystemInfo.deviceUniqueIdentifier);
			jsonDict.Add("enabled.telemetry", false);
			jsonDict.Add("sdkIdentifier", GetSDKIdentifier());
			jsonDict.Add("skuId", Constants.SDK_SKU_ID);
			jsonDict.Add("sdkVersion", Constants.SDK_VERSION);
			eventList.Add(jsonDict);

			var jsonString = JsonConvert.SerializeObject(eventList);
			return jsonString;
		}

		private bool ShouldPostTurnstile(long ticks)
		{
			var date = new DateTime(ticks);
			var longAgo = DateTime.Now.AddDays(-100).Ticks.ToString();
			var lastDateString =
				PlayerPrefs.GetString(Constants.Path.TELEMETRY_TURNSTILE_LAST_TICKS_EDITOR_KEY, longAgo);
			long lastTicks = 0;
			long.TryParse(lastDateString, out lastTicks);
			var lastDate = new DateTime(lastTicks);
			var timeSpan = date - lastDate;
			return timeSpan.Days >= 1;
		}

		private IEnumerator PostWWW(string url, string bodyJsonString)
		{
			var bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);

#if UNITY_2017_1_OR_NEWER
			var postRequest = new UnityWebRequest(url, "POST");
			postRequest.SetRequestHeader("Content-Type", "application/json");

			postRequest.downloadHandler = new DownloadHandlerBuffer();
			postRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);

			yield return postRequest.SendWebRequest();

			while (!postRequest.isDone) yield return null;

			if (!postRequest.isNetworkError)
			{
#else
				var headers = new Dictionary<string, string>();
				headers.Add("Content-Type", "application/json");
				headers.Add("user-agent", GetUserAgent());
				var www = new WWW(url, bodyRaw, headers);
				yield return www;

				while (!www.isDone) { yield return null; }

				// www doesn't expose HTTP status code, relay on 'error' property
				if (!string.IsNullOrEmpty(www.error))
				{
#endif
				PlayerPrefs.SetString(Constants.Path.TELEMETRY_TURNSTILE_LAST_TICKS_EDITOR_KEY, "0");
			}
			else
			{
				PlayerPrefs.SetString(Constants.Path.TELEMETRY_TURNSTILE_LAST_TICKS_EDITOR_KEY,
					DateTime.Now.Ticks.ToString());
			}
		}

		private static string GetUserAgent()
		{
			var userAgent = string.Format(
				"{0}/{1}/{2} MapboxEventsUnityEditor/{3}",
				PlayerSettings.applicationIdentifier,
				PlayerSettings.bundleVersion,
#if UNITY_IOS
				PlayerSettings.iOS.buildNumber,
#elif UNITY_ANDROID
				PlayerSettings.Android.bundleVersionCode,
#else
				"0",
#endif
				Constants.SDK_VERSION
			);
			return userAgent;
		}

		private string GetSDKIdentifier()
		{
			var sdkIdentifier = string.Format("MapboxEventsUnity{0}",
				Application.platform
			);
			return sdkIdentifier;
		}
	}
}
#endif
