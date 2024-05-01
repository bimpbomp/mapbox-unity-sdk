using System;
using Mapbox.Unity.MeshGeneration.Data;
using UnityEngine;

namespace Mapbox.Unity.MeshGeneration.Filters
{
	public class HeightFilter : FilterBase
	{
		public enum HeightFilterOptions
		{
			Above,
			Below
		}

		[SerializeField] private float _height;

		[SerializeField] private HeightFilterOptions _type;

		public override string Key => "height";

		public override bool Try(VectorFeatureUnity feature)
		{
			var hg = Convert.ToSingle(feature.Properties[Key]);
			if (_type == HeightFilterOptions.Above && hg > _height)
				return true;
			if (_type == HeightFilterOptions.Below && hg < _height)
				return true;

			return false;
		}
	}
}
