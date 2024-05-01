using System;
using System.Collections.Generic;
using Mapbox.Map;
using Mapbox.Unity.Map.Interfaces;
using UnityEngine;

namespace Mapbox.Unity.Map.TileProviders
{
	public class ExtentArgs : EventArgs
	{
		//TODO: Override GetHashCode for UnwrappedTileId
		public HashSet<UnwrappedTileId> activeTiles;
	}

	public abstract class AbstractTileProvider : MonoBehaviour, ITileProvider
	{
		protected ExtentArgs _currentExtent = new();

		protected IMap _map;

		protected ITileProviderOptions _options;
		public event EventHandler<ExtentArgs> ExtentChanged;

		public ITileProviderOptions Options => _options;

		public virtual void Initialize(IMap map)
		{
			_map = map;
			OnInitialized();
		}

		public virtual void SetOptions(ITileProviderOptions options)
		{
			_options = options;
		}

		public virtual void OnExtentChanged()
		{
			if (ExtentChanged != null) ExtentChanged(this, _currentExtent);
		}

		public abstract void OnInitialized();
		public abstract void UpdateTileExtent();

		public abstract bool Cleanup(UnwrappedTileId tile);

		public virtual void UpdateTileProvider()
		{
		}
	}
}
