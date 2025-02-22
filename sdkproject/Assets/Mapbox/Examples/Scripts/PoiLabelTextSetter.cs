using System.Collections.Generic;
using Mapbox.Unity.MeshGeneration.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Mapbox.Examples
{
	public class PoiLabelTextSetter : MonoBehaviour, IFeaturePropertySettable
	{
		[SerializeField] private Text _text;

		[SerializeField] private Image _background;

		public void Set(Dictionary<string, object> props)
		{
			_text.text = "";

			if (props.ContainsKey("name"))
				_text.text = props["name"].ToString();
			else if (props.ContainsKey("house_num"))
				_text.text = props["house_num"].ToString();
			else if (props.ContainsKey("type")) _text.text = props["type"].ToString();
			RefreshBackground();
		}

		public void RefreshBackground()
		{
			var backgroundRect = _background.GetComponent<RectTransform>();
			LayoutRebuilder.ForceRebuildLayoutImmediate(backgroundRect);
		}
	}
}
