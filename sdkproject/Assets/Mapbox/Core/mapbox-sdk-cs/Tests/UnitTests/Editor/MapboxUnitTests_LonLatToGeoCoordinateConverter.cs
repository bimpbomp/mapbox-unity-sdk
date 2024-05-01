//-----------------------------------------------------------------------
// <copyright file="LonLatToVector2dConverterTest.cs" company="Mapbox">
//     Copyright (c) 2016 Mapbox. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Mapbox.Json;
using Mapbox.Utils;
using Mapbox.Utils.JsonConverters;
using NUnit.Framework;

namespace Mapbox.MapboxSdkCs.UnitTest
{
	[TestFixture]
	internal class LonLatToVector2dConverterTest
	{
		// Mapbox API returns longitude,latitude
		private readonly string _lonLatStr = "[-77.0295,38.9165]";

		// In Unity, x = latitude, y = longitude
		private Vector2d _latLonObject = new(y: -77.0295, x: 38.9165);


		[Test]
		public void Deserialize()
		{
			var deserializedLonLat = JsonConvert.DeserializeObject<Vector2d>(_lonLatStr, JsonConverters.Converters);
			Assert.AreEqual(_latLonObject.ToString(), deserializedLonLat.ToString());
		}


		[Test]
		public void Serialize()
		{
			var serializedLonLat = JsonConvert.SerializeObject(_latLonObject, JsonConverters.Converters);
			Assert.AreEqual(_lonLatStr, serializedLonLat);
		}
	}
}
