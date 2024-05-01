using Mapbox.Unity.MeshGeneration.Factories;
using UnityEditor;

namespace Mapbox.Editor
{
	[CustomEditor(typeof(AbstractTileFactory))]
	public class FactoryEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
		}
	}
}
