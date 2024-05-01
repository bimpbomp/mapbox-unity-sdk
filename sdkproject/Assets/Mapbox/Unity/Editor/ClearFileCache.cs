using Mapbox.Unity;
using UnityEditor;
using UnityEngine;

namespace Mapbox.Editor
{
	[InitializeOnLoad]
	public class ClearFileCache : MonoBehaviour
	{
		[MenuItem("Mapbox/Clear File Cache")]
		public static void ClearAllCachFiles()
		{
			MapboxAccess.Instance.ClearAllCacheFiles();
		}
	}
}
