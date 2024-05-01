//-----------------------------------------------------------------------
// <copyright file="ReverseGeocodeResourceTest.cs" company="Mapbox">
//     Copyright (c) 2016 Mapbox. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using Mapbox.Geocoding;
using Mapbox.Utils;
using NUnit.Framework;

namespace Mapbox.MapboxSdkCs.UnitTest
{
	[TestFixture]
	internal class ReverseGeocodeResourceTest
	{
		[SetUp]
		public void SetUp()
		{
			_reverseGeocodeResource = new ReverseGeocodeResource(_queryLocation);
		}

		private const string _baseUrl = "https://api.mapbox.com/geocoding/v5/mapbox.places/";
		private readonly Vector2d _queryLocation = new(10, 10);
		private readonly string _expectedQueryString = "10.00000,10.00000";
		private ReverseGeocodeResource _reverseGeocodeResource;

		public void BadType()
		{
			_reverseGeocodeResource.Types = new[] { "fake" };
		}

		public void BadTypeWithGoodType()
		{
			_reverseGeocodeResource.Types = new[] { "place", "fake" };
		}

		[Test]
		public void SetInvalidTypes()
		{
			Assert.Throws<Exception>(BadType);
			Assert.Throws<Exception>(BadTypeWithGoodType);
		}

		[Test]
		public void GetUrl()
		{
			// With only constructor
			Assert.AreEqual(_baseUrl + _expectedQueryString + ".json", _reverseGeocodeResource.GetUrl());

			// With one types
			_reverseGeocodeResource.Types = new[] { "country" };
			Assert.AreEqual(_baseUrl + _expectedQueryString + ".json?types=country", _reverseGeocodeResource.GetUrl());

			// With multiple types
			_reverseGeocodeResource.Types = new[] { "country", "region" };
			// ToLower is need to make test pass on OSX
			Assert.AreEqual((_baseUrl + _expectedQueryString + ".json?types=country%2Cregion").ToLower(),
				_reverseGeocodeResource.GetUrl().ToLower());

			// Set all to null
			_reverseGeocodeResource.Types = null;
			Assert.AreEqual(_baseUrl + _expectedQueryString + ".json", _reverseGeocodeResource.GetUrl());
		}
	}
}
