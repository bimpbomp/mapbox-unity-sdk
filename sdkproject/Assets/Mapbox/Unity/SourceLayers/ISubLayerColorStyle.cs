using UnityEngine;

namespace Mapbox.Unity.Map
{
	public interface ISubLayerColorStyle : ISubLayerStyle
	{
		Color FeatureColor { get; set; }
		void SetAsStyle(Color featureColor);
	}
}
