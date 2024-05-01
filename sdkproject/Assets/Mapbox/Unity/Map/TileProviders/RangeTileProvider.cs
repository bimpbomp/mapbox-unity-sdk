using System.Collections.Generic;
using Mapbox.Map;

namespace Mapbox.Unity.Map.TileProviders
{
	public class RangeTileProvider : AbstractTileProvider
	{
		private bool _initialized;
		private RangeTileProviderOptions _rangeTileProviderOptions;

		public override void OnInitialized()
		{
			if (Options != null)
				_rangeTileProviderOptions = (RangeTileProviderOptions)Options;
			else
				_rangeTileProviderOptions = new RangeTileProviderOptions();

			_initialized = true;
			_currentExtent.activeTiles = new HashSet<UnwrappedTileId>();
		}

		public override void UpdateTileExtent()
		{
			if (!_initialized || _rangeTileProviderOptions == null) return;

			_currentExtent.activeTiles.Clear();
			var centerTile = TileCover.CoordinateToTileId(_map.CenterLatitudeLongitude, _map.AbsoluteZoom);
			_currentExtent.activeTiles.Add(new UnwrappedTileId(_map.AbsoluteZoom, centerTile.X, centerTile.Y));

			for (var x = centerTile.X - _rangeTileProviderOptions.west;
			     x <= centerTile.X + _rangeTileProviderOptions.east;
			     x++)
			for (var y = centerTile.Y - _rangeTileProviderOptions.north;
			     y <= centerTile.Y + _rangeTileProviderOptions.south;
			     y++)
				_currentExtent.activeTiles.Add(new UnwrappedTileId(_map.AbsoluteZoom, x, y));

			OnExtentChanged();
		}

		public override bool Cleanup(UnwrappedTileId tile)
		{
			return !_currentExtent.activeTiles.Contains(tile);
		}
	}
}
