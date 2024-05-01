//-----------------------------------------------------------------------
// <copyright file="Utils.cs" company="Mapbox">
//     Copyright (c) 2016 Mapbox. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Collections.Generic;
using Mapbox.Map;
using Mapbox.Utils;

namespace Mapbox.MapboxSdkCs.UnitTest
{
	internal static class Utils
	{
		internal class VectorMapObserver : IObserver<Map.VectorTile>
		{
			public List<Map.VectorTile> Tiles { get; } = new();

			public void OnNext(Map.VectorTile tile)
			{
				if (tile.CurrentState == Tile.State.Loaded) Tiles.Add(tile);
			}
		}

		internal class RasterMapObserver : IObserver<RasterTile>
		{
			public List<byte[]> Tiles { get; } = new();

			public void OnNext(RasterTile tile)
			{
				if (tile.CurrentState == Tile.State.Loaded && !tile.HasError) Tiles.Add(tile.Data);
			}
		}

		internal class ClassicRasterMapObserver : IObserver<ClassicRasterTile>
		{
			public List<byte[]> Tiles { get; } = new();

			public void OnNext(ClassicRasterTile tile)
			{
				if (tile.CurrentState == Tile.State.Loaded && !tile.HasError) Tiles.Add(tile.Data);
			}
		}
	}
}
