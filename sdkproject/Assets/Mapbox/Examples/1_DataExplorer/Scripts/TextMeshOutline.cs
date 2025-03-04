﻿using UnityEngine;
using UnityEngine.Rendering;

namespace Mapbox.Examples
{
	/// <summary>
	///     Text mesh outline. Script source: https://answers.unity.com/questions/542646/3d-text-strokeoutline.html
	/// </summary>
	public class TextMeshOutline : MonoBehaviour
	{
		public float pixelSize = 1;
		public Color outlineColor = Color.black;
		public bool resolutionDependant;
		public int doubleResolution = 1024;
		private MeshRenderer meshRenderer;

		private TextMesh textMesh;

		private void Start()
		{
			textMesh = GetComponent<TextMesh>();
			meshRenderer = GetComponent<MeshRenderer>();

			for (var i = 0; i < 8; i++)
			{
				var outline = new GameObject("outline", typeof(TextMesh));
				outline.transform.parent = transform;
				outline.transform.localScale = new Vector3(1, 1, 1);

				var otherMeshRenderer = outline.GetComponent<MeshRenderer>();
				otherMeshRenderer.material = new Material(meshRenderer.material);
				otherMeshRenderer.shadowCastingMode = ShadowCastingMode.Off;
				otherMeshRenderer.receiveShadows = false;
				otherMeshRenderer.sortingLayerID = meshRenderer.sortingLayerID;
				otherMeshRenderer.sortingLayerName = meshRenderer.sortingLayerName;
			}
		}

		private void LateUpdate()
		{
			var screenPoint = Camera.main.WorldToScreenPoint(transform.position);

			outlineColor.a = textMesh.color.a * textMesh.color.a;

			// copy attributes
			for (var i = 0; i < transform.childCount; i++)
			{
				var other = transform.GetChild(i).GetComponent<TextMesh>();
				other.color = outlineColor;
				other.text = textMesh.text;
				other.alignment = textMesh.alignment;
				other.anchor = textMesh.anchor;
				other.characterSize = textMesh.characterSize;
				other.font = textMesh.font;
				other.fontSize = textMesh.fontSize;
				other.fontStyle = textMesh.fontStyle;
				other.richText = textMesh.richText;
				other.tabSize = textMesh.tabSize;
				other.lineSpacing = textMesh.lineSpacing;
				other.offsetZ = textMesh.offsetZ;

				var doublePixel = resolutionDependant &&
				                  (Screen.width > doubleResolution || Screen.height > doubleResolution);
				var pixelOffset = GetOffset(i) * (doublePixel ? 2.0f * pixelSize : pixelSize);
				var worldPoint = Camera.main.ScreenToWorldPoint(screenPoint + pixelOffset);
				other.transform.position = worldPoint;

				var otherMeshRenderer = transform.GetChild(i).GetComponent<MeshRenderer>();
				otherMeshRenderer.sortingLayerID = meshRenderer.sortingLayerID;
				otherMeshRenderer.sortingLayerName = meshRenderer.sortingLayerName;
			}
		}

		private Vector3 GetOffset(int i)
		{
			switch (i % 8)
			{
				case 0: return new Vector3(0, 1, 0);
				case 1: return new Vector3(1, 1, 0);
				case 2: return new Vector3(1, 0, 0);
				case 3: return new Vector3(1, -1, 0);
				case 4: return new Vector3(0, -1, 0);
				case 5: return new Vector3(-1, -1, 0);
				case 6: return new Vector3(-1, 0, 0);
				case 7: return new Vector3(-1, 1, 0);
				default: return Vector3.zero;
			}
		}
	}
}
