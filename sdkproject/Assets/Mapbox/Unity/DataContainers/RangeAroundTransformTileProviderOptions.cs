﻿using System;
using UnityEngine;

namespace Mapbox.Unity.Map
{
	[Serializable]
	public class RangeAroundTransformTileProviderOptions : ExtentOptions
	{
		public Transform targetTransform;
		public int visibleBuffer;
		public int disposeBuffer;

		public override void SetOptions(ExtentOptions extentOptions)
		{
			var options = extentOptions as RangeAroundTransformTileProviderOptions;
			if (options != null)
				SetOptions(options.targetTransform, options.visibleBuffer, options.disposeBuffer);
			else
				Debug.LogError("ExtentOptions type mismatch : Using " + extentOptions.GetType() +
				               " to set extent of type " + GetType());
		}

		public void SetOptions(Transform tgtTransform = null, int visibleRange = 1, int disposeRange = 1)
		{
			targetTransform = tgtTransform;
			visibleBuffer = visibleRange;
			disposeBuffer = disposeRange;
		}
	}
}
