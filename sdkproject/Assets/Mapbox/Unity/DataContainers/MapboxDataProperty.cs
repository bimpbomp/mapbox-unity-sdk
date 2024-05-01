using System;
using Mapbox.Unity.MeshGeneration.Data;

namespace Mapbox.Unity.Map
{
	public abstract class MapboxDataProperty
	{
		public virtual bool HasChanged
		{
			set
			{
				if (value) OnPropertyHasChanged(null /*Pass args here */);
			}
		}

		public event EventHandler PropertyHasChanged;

		protected virtual void OnPropertyHasChanged(EventArgs e)
		{
			var handler = PropertyHasChanged;
			if (handler != null) handler(this, e);
		}

		public virtual bool NeedsForceUpdate()
		{
			return false;
		}

		public virtual void UpdateProperty(UnityTile tile)
		{
		}
	}
}
