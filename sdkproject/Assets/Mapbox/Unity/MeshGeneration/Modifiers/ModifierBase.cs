using System;
using Mapbox.Unity.Map;
using Mapbox.Unity.MeshGeneration.Data;
using Mapbox.Unity.MeshGeneration.Interfaces;
using UnityEngine;

namespace Mapbox.Unity.MeshGeneration.Modifiers
{
	[Serializable]
	public abstract class ModifierProperties : MapboxDataProperty
	{
		public abstract Type ModifierType { get; }

		public override bool HasChanged
		{
			set
			{
				if (value) OnPropertyHasChanged(new VectorLayerUpdateArgs { property = this });
			}
		}

		public virtual void UpdateProperty(LayerVisualizerBase layerVisualizer)
		{
		}
	}

	public class ModifierBase : ScriptableObject
	{
		[SerializeField] public bool Active = true;

		public virtual void SetProperties(ModifierProperties properties)
		{
		}

		public virtual void Initialize()
		{
		}

		public virtual void FeaturePreProcess(VectorFeatureUnity feature)
		{
		}

		public virtual void UnbindProperties()
		{
		}

		public virtual void UpdateModifier(object sender, EventArgs layerArgs)
		{
			NotifyUpdateModifier(new VectorLayerUpdateArgs
			{
				property = sender as MapboxDataProperty, modifier = this
			});
		}

		public event EventHandler ModifierHasChanged;

		protected virtual void NotifyUpdateModifier(VectorLayerUpdateArgs layerUpdateArgs)
		{
			var handler = ModifierHasChanged;
			if (handler != null) handler(this, layerUpdateArgs);
		}
	}
}
