using System;
using System.Collections.Generic;
using Mapbox.Unity.Utilities;
using UnityEngine;

namespace Mapbox.Unity.Map
{
	[Serializable]
	public class VectorLayerProperties : LayerProperties
	{
		/// <summary>
		///     Raw tileJSON response received from the requested source tileset id(s)
		/// </summary>
		public TileJsonData tileJsonData = new();

		[SerializeField] protected VectorSourceType _sourceType = VectorSourceType.MapboxStreets;

		public LayerSourceOptions sourceOptions = new()
		{
			isActive = true, layerSource = MapboxDefaultVector.GetParameters(VectorSourceType.MapboxStreets)
		};

		[Tooltip(
			"Use Mapbox style-optimized tilesets, remove any layers or features in the tile that are not represented by a Mapbox style. Style-optimized vector tiles are smaller, served over-the-wire, and a great way to reduce the size of offline caches.")]
		public bool useOptimizedStyle;

		[StyleSearch] public Style optimizedStyle;

		public LayerPerformanceOptions performanceOptions;

		[NodeEditorElementAttribute("Feature Sublayers")]
		public List<VectorSubLayerProperties> vectorSubLayers = new();

		[NodeEditorElementAttribute("POI Sublayers")]
		public List<PrefabItemOptions> locationPrefabList = new();

		public VectorSourceType sourceType
		{
			get => _sourceType;
			set
			{
				if (value != VectorSourceType.Custom) sourceOptions.Id = MapboxDefaultVector.GetParameters(value).Id;

				if (value == VectorSourceType.None)
					sourceOptions.isActive = false;
				else
					sourceOptions.isActive = true;

				_sourceType = value;
			}
		}


		public override bool NeedsForceUpdate()
		{
			return true;
		}

		#region Events

		public event EventHandler SubLayerPropertyAdded;

		public virtual void OnSubLayerPropertyAdded(EventArgs e)
		{
			var handler = SubLayerPropertyAdded;
			if (handler != null) handler(this, e);
		}

		public event EventHandler SubLayerPropertyRemoved;

		public virtual void OnSubLayerPropertyRemoved(EventArgs e)
		{
			var handler = SubLayerPropertyRemoved;
			if (handler != null) handler(this, e);
		}

		#endregion
	}
}
