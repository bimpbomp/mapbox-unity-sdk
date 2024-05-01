using Mapbox.Map;
using Mapbox.Unity.Map;
using Mapbox.Unity.MeshGeneration.Data;
using UnityEngine;

namespace Mapbox.Unity.MeshGeneration.Factories.TerrainStrategies
{
	public class TerrainStrategy
	{
		[SerializeField] protected ElevationLayerProperties _elevationOptions = new();

		public virtual int RequiredVertexCount => 0;

		public virtual void Initialize(ElevationLayerProperties elOptions)
		{
			_elevationOptions = elOptions;
		}

		public virtual void RegisterTile(UnityTile tile)
		{
		}

		public virtual void PostProcessTile(UnityTile tile)
		{
		}

		public virtual void UnregisterTile(UnityTile tile)
		{
		}

		public virtual void DataErrorOccurred(UnityTile tile, TileErrorEventArgs e)
		{
		}
	}
}
