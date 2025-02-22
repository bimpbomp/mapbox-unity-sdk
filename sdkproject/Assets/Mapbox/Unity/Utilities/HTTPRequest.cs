//-----------------------------------------------------------------------
// <copyright file="HTTPRequest.cs" company="Mapbox">
//     Copyright (c) 2016 Mapbox. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------


using System;
using System.Collections;
using Mapbox.Platform;
using UnityEngine.Networking;

namespace Mapbox.Unity.Utilities
{
#if UNITY_EDITOR
	using UnityEditor;
#endif

	public enum HttpRequestType
	{
		Get,
		Head
	}


	internal sealed class HTTPRequest : IAsyncRequest
	{
		private readonly Action<Response> _callback;

		private UnityWebRequest _request;
		private int _timeout;

		// TODO: simplify timeout for Unity 5.6+
		// https://docs.unity3d.com/ScriptReference/Networking.UnityWebRequest-timeout.html
		public HTTPRequest(string url, Action<Response> callback, int timeout,
			HttpRequestType requestType = HttpRequestType.Get)
		{
			IsCompleted = false;
			RequestType = requestType;

			switch (RequestType)
			{
				case HttpRequestType.Get:
					_request = UnityWebRequest.Get(url);
					break;
				case HttpRequestType.Head:
					_request = UnityWebRequest.Head(url);
					break;
				default:
					_request = UnityWebRequest.Get(url);
					break;
			}

			_request.timeout = timeout;
			_callback = callback;

#if UNITY_EDITOR
			if (!EditorApplication.isPlaying) Runnable.EnableRunnableInEditor();
#endif
			Runnable.Run(DoRequest());
		}

		public bool IsCompleted { get; private set; }

		public HttpRequestType RequestType { get; }

		public void Cancel()
		{
			if (_request != null) _request.Abort();
		}

		private IEnumerator DoRequest()
		{
#if UNITY_EDITOR
			// otherwise requests don't work in Edit mode, eg geocoding
			// also lot of EditMode tests fail otherwise
#pragma warning disable 0618
			_request.Send();
#pragma warning restore 0618
			while (!_request.isDone) yield return null;
#else
#pragma warning disable 0618
			yield return _request.Send();
#pragma warning restore 0618
#endif

			var response = Response.FromWebResponse(this, _request, null);

			_callback(response);
			_request.Dispose();
			_request = null;
			IsCompleted = true;
		}
	}
}
