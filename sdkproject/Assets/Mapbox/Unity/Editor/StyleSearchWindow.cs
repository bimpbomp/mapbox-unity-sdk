﻿using System.Collections;
using System.Collections.Generic;
using Mapbox.Json;
using Mapbox.Unity;
using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using Constants = Mapbox.Utils.Constants;

namespace Mapbox.Editor
{
	public class StyleSearchWindow : EditorWindow
	{
		private const string searchFieldName = "searchField";
		private const float width = 320f;
		private const float height = 300f;
		private string _errorString = "";

		private bool _isSearching;
		private SerializedProperty _property;

		private List<Style> _styles;

		private string _username = "";
		private Vector2 scrollPos;

		private void OnEnable()
		{
			EditorApplication.playModeStateChanged += OnModeChanged;
		}

		private void OnDisable()
		{
			EditorApplication.playModeStateChanged -= OnModeChanged;
		}

		private void OnGUI()
		{
			var st = new GUIStyle();
			st.padding = new RectOffset(15, 15, 15, 15);
			EditorGUILayout.Space();
			EditorGUILayout.Space();
			EditorGUILayout.HelpBox(
				"User styles are bound to usernames, enter your Mapbox Username to search for your personal styles.",
				MessageType.Info);
			_username = EditorGUILayout.TextField("Mapbox Username: ", _username);


			scrollPos = EditorGUILayout.BeginScrollView(scrollPos, st);

			if (_username.Length == 0)
			{
				GUILayout.Label("Enter a user name");
			}
			else
			{
				if (!_isSearching)
					if (GUILayout.Button("Search"))
					{
						Search(_username);
						_property.FindPropertyRelative("UserName").stringValue = _username;
						_property.serializedObject.ApplyModifiedProperties();
					}

				if (_styles != null && _styles.Count > 0)
				{
					GUILayout.Label("Results:");
					for (var i = 0; i < _styles.Count; i++)
					{
						var style = _styles[i];
						if (GUILayout.Button(style.Name))
						{
							_property.FindPropertyRelative("Name").stringValue = style.Name;
							_property.FindPropertyRelative("Id").stringValue = style.Id;
							_property.FindPropertyRelative("Modified").stringValue = style.Modified;
							_property.FindPropertyRelative("UserName").stringValue = style.UserName;
							_property.serializedObject.ApplyModifiedProperties();
							EditorUtility.SetDirty(_property.serializedObject.targetObject);

							Close();
						}
					}
				}
				else
				{
					if (_isSearching)
					{
						GUI.enabled = false;
						if (GUILayout.Button("Searching..."))
						{
						}

						GUI.enabled = true;
					}
					else if (!string.IsNullOrEmpty(_errorString))
					{
						EditorGUILayout.Space();
						EditorGUILayout.Space();
						EditorGUILayout.Space();
						EditorGUILayout.Space();
						EditorGUILayout.HelpBox(_errorString, MessageType.Error);
						if (GUILayout.Button("Check username/get token with styles:list support"))
							Application.OpenURL("https://www.mapbox.com/studio/account/tokens/");
					}
					else
					{
						GUILayout.Label("No search results");
					}
				}
			}

			EditorGUILayout.EndScrollView();
		}

		public static void Open(SerializedProperty property)
		{
			var window = GetWindow<StyleSearchWindow>(true, "Search for style");

			window._property = property;
			window._username = property.FindPropertyRelative("UserName").stringValue;
			if (!string.IsNullOrEmpty(window._username)) window.Search(window._username);

			Runnable.EnableRunnableInEditor();
			var e = Event.current;
			var mousePos = GUIUtility.GUIToScreenPoint(e.mousePosition);
			window.position = new Rect(mousePos.x - width, mousePos.y, width, height);
			window.minSize = new Vector2(400, 500);
		}

		private void OnModeChanged(PlayModeStateChange state)
		{
			Close();
		}

		private void Search(string searchString)
		{
			_errorString = "";
			if (!string.IsNullOrEmpty(searchString) && !_isSearching)
			{
				Runnable.Run(ListStyles(MapboxAccess.Instance.Configuration.AccessToken));
				_isSearching = true;
			}
		}

		private IEnumerator ListStyles(string token)
		{
#if UNITY_2017_1_OR_NEWER
			var webRequest =
				new UnityWebRequest(Constants.BaseAPI +
				                    string.Format("styles/v1/{0}?access_token={1}", _username, token))
				{
					downloadHandler = new DownloadHandlerBuffer()
				};
			yield return webRequest.SendWebRequest();

			while (!webRequest.isDone) yield return 0;
			var json = webRequest.downloadHandler.text;
			if (!string.IsNullOrEmpty(json)) ParseResponse(json);
#else
			// "https://api.mapbox.com/styles/v1/{username}?access_token=your-access-token"
			var www =
 new WWW(Utils.Constants.BaseAPI + string.Format("styles/v1/{0}?access_token={1}", _username, token));
			while (!www.isDone)
			{
				yield return 0;
			}
			var json = www.text;
			if (!string.IsNullOrEmpty(json))
			{
				ParseResponse(json);
			}
#endif
		}

		private void ParseResponse(string json)
		{
			_styles = new List<Style>();
			if (json.Contains("This API requires a token with styles:list scope"))
			{
				_errorString =
					"The Mapbox Access Token you're using at the moment doesn't have \"styles: list\" scope this feature requires.\r\n\r\nYou can create a new token from link below and check \"styles: list\" in the Token Scopes list to enable this feature.";
				_isSearching = false;
				return;
			}

			if (json.Contains("Forbidden"))
			{
				_errorString = "Forbidden!";
				_isSearching = false;
				return;
			}

			var settings = new JsonSerializerSettings();
			settings.DateParseHandling = DateParseHandling.None;
			var styleArray = JsonConvert.DeserializeObject<object[]>(json);
			foreach (var style in styleArray)
			{
				var styleData = JsonConvert.DeserializeObject<Dictionary<string, object>>(style.ToString(), settings);
				var id = string.Format("mapbox://styles/{0}/{1}", _username, styleData["id"]);
				_styles.Add(new Style
				{
					Name = styleData["name"].ToString(),
					Id = id,
					Modified = styleData["modified"].ToString(),
					UserName = _username
				});
			}

			_isSearching = false;
			Repaint();
		}
	}
}
