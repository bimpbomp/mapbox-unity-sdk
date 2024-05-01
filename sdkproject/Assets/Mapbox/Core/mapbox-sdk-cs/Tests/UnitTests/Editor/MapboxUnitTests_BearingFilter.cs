//-----------------------------------------------------------------------
// <copyright file="BearingFilterTest.cs" company="Mapbox">
//     Copyright (c) 2016 Mapbox. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using Mapbox.Utils;
using NUnit.Framework;

namespace Mapbox.MapboxSdkCs.UnitTest
{
	[TestFixture]
	internal class BearingFilterTest
	{
		[SetUp]
		public void SetUp()
		{
			_bearingFilter = new BearingFilter(10, 10);
		}

		private BearingFilter _bearingFilter;

		public void BearingTooLarge()
		{
			_bearingFilter = new BearingFilter(361, 10);
		}

		public void BearingTooSmall()
		{
			_bearingFilter = new BearingFilter(-1, 10);
		}

		public void RangeTooLarge()
		{
			_bearingFilter = new BearingFilter(10, 181);
		}

		public void RangeTooSmall()
		{
			_bearingFilter = new BearingFilter(10, -1);
		}

		[Test]
		public void InvalidValues()
		{
			Assert.Throws<Exception>(BearingTooLarge);
			Assert.Throws<Exception>(BearingTooSmall);
			Assert.Throws<Exception>(RangeTooSmall);
			Assert.Throws<Exception>(RangeTooLarge);
		}

		[Test]
		public void ToStringTest()
		{
			Assert.AreEqual(_bearingFilter.ToString(), "10,10");

			_bearingFilter = new BearingFilter(null, null);
			Assert.AreEqual(_bearingFilter.ToString(), string.Empty);
		}
	}
}
