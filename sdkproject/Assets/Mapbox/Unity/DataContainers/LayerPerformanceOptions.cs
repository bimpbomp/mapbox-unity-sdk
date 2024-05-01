﻿using System;
using UnityEngine;

namespace Mapbox.Unity.Map
{
	[Serializable]
	public class LayerPerformanceOptions : MapboxDataProperty
	{
		[Tooltip(
			"Enable Coroutines to distribute tile loading using coroutines, reduces the load on the main thread and keeps applications responsive. First load may be slower but subsequent loading will be faster. ")]
		public bool isEnabled = true;

		[Tooltip("Number of feature entities to group in one single coroutine call. ")]
		public int entityPerCoroutine = 20;

		public override bool HasChanged
		{
			set
			{
				if (value) OnPropertyHasChanged(new VectorLayerUpdateArgs { property = this });
			}
		}
	}
}
