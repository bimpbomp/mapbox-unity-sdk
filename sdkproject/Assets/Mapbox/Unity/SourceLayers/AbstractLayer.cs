using System;
using Mapbox.Unity.MeshGeneration.Factories;
using Mapbox.Unity.MeshGeneration.Interfaces;
using Mapbox.Unity.MeshGeneration.Modifiers;

namespace Mapbox.Unity.Map
{
	public class LayerUpdateArgs : EventArgs
	{
		public bool effectsVectorLayer;
		public AbstractTileFactory factory;
		public MapboxDataProperty property;
	}

	public class VectorLayerUpdateArgs : LayerUpdateArgs
	{
		public ModifierBase modifier;
		public LayerVisualizerBase visualizer;
	}

	public class AbstractLayer
	{
		public event EventHandler UpdateLayer;

		protected virtual void NotifyUpdateLayer(LayerUpdateArgs layerUpdateArgs)
		{
			var handler = UpdateLayer;
			if (handler != null) handler(this, layerUpdateArgs);
		}

		protected virtual void NotifyUpdateLayer(AbstractTileFactory factory, MapboxDataProperty prop,
			bool effectsVectorLayer = false)
		{
			var handler = UpdateLayer;
			if (handler != null)
			{
				var layerUpdateArgs =
					factory is VectorTileFactory
						? new VectorLayerUpdateArgs
						{
							factory = factory, effectsVectorLayer = effectsVectorLayer, property = prop
						}
						: new LayerUpdateArgs
						{
							factory = factory, effectsVectorLayer = effectsVectorLayer, property = prop
						};
				handler(this, layerUpdateArgs);
			}
		}
	}
}
