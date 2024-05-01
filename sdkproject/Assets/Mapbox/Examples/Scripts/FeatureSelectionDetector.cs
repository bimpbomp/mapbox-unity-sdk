using Mapbox.Unity.MeshGeneration.Data;
using UnityEngine;

namespace Mapbox.Examples
{
	public class FeatureSelectionDetector : MonoBehaviour
	{
		private VectorEntity _feature;
		private FeatureUiMarker _marker;

		public void OnMouseUpAsButton()
		{
			_marker.Show(_feature);
		}

		internal void Initialize(FeatureUiMarker marker, VectorEntity ve)
		{
			_marker = marker;
			_feature = ve;
		}
	}
}
