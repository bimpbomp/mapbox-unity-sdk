using UnityEngine;

namespace Mapbox.Examples
{
	public class ChangeShadowDistance : MonoBehaviour
	{
		public int ShadowDistance;

		private void Start()
		{
			QualitySettings.shadowDistance = ShadowDistance;
		}
	}
}
