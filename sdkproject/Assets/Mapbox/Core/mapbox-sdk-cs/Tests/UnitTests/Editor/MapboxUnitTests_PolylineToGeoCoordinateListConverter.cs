//-----------------------------------------------------------------------
// <copyright file="PolylineToVector2dListConverterTest.cs" company="Mapbox">
//     Copyright (c) 2016 Mapbox. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Collections.Generic;
using Mapbox.Json;
using Mapbox.Utils;
using Mapbox.Utils.JsonConverters;
using NUnit.Framework;

namespace Mapbox.MapboxSdkCs.UnitTest
{
	[TestFixture]
	internal class PolylineToVector2dListConverterTest
	{
		// (38.5, -120.2), (40.7, -120.95), (43.252, -126.453)
		private readonly List<Vector2d> _polyLineObj = new()
		{
			new(38.5, -120.2), new(40.7, -120.95), new(43.252, -126.453)
		};

		private readonly string _polyLineString = "\"_p~iF~ps|U_ulLnnqC_mqNvxq`@\"";


		[Test]
		public void Deserialize()
		{
			var deserializedLine =
				JsonConvert.DeserializeObject<List<Vector2d>>(_polyLineString, JsonConverters.Converters);
			Assert.AreEqual(_polyLineObj, deserializedLine);
		}


		[Test]
		public void Serialize()
		{
			var serializedLine = JsonConvert.SerializeObject(_polyLineObj, JsonConverters.Converters);
			Assert.AreEqual(_polyLineString, serializedLine);
		}
	}
}
