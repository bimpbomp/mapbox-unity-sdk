using System;
using UnityEngine;

namespace Mapbox.Unity.Map
{
	[Serializable]
	public class RangeTileProviderOptions : ExtentOptions
	{
		[Range(0, 10)] public int west = 1;

		[Range(0, 10)] public int north = 1;

		[Range(0, 10)] public int east = 1;

		[Range(0, 10)] public int south = 1;

		public override void SetOptions(ExtentOptions extentOptions)
		{
			var options = extentOptions as RangeTileProviderOptions;
			if (options != null)
			{
				west = options.west;
				north = options.north;
				east = options.east;
				south = options.south;
			}
			else
			{
				Debug.LogError("ExtentOptions type mismatch : Using " + extentOptions.GetType() +
				               " to set extent of type " + GetType());
			}
		}

		public void SetOptions(int northRange = 1, int southRange = 1, int eastRange = 1, int westRange = 1)
		{
			west = westRange;
			north = northRange;
			east = eastRange;
			south = southRange;
		}
	}
}
