//-----------------------------------------------------------------------
// <copyright file="CanonicalTileIdTest.cs" company="Mapbox">
//     Copyright (c) 2016 Mapbox. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Mapbox.Map;
using Mapbox.Utils;
using NUnit.Framework;

namespace Mapbox.MapboxSdkCs.UnitTest
{
	[TestFixture]
	internal class CanonicalTileIdTest
	{
		[Test]
		public void ToVector2d()
		{
			var set = TileCover.Get(Vector2dBounds.World(), 5);

			foreach (var tile in set)
			{
				var reverse = TileCover.CoordinateToTileId(tile.ToVector2d(), 5);

				Assert.AreEqual(tile.Z, reverse.Z);
				Assert.AreEqual(tile.X, reverse.X);
				Assert.AreEqual(tile.Y, reverse.Y);
			}
		}
	}
}
