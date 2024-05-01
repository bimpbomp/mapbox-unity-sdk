using System.Collections.Generic;
using Mapbox.Unity.MeshGeneration.Interfaces;
using UnityEngine;

namespace Mapbox.Examples
{
	public class PoiMarkerHelper : MonoBehaviour, IFeaturePropertySettable
	{
		private Dictionary<string, object> _props;

		private void OnMouseUpAsButton()
		{
			foreach (var prop in _props) Debug.Log(prop.Key + ":" + prop.Value);
		}

		public void Set(Dictionary<string, object> props)
		{
			_props = props;
		}
	}
}
