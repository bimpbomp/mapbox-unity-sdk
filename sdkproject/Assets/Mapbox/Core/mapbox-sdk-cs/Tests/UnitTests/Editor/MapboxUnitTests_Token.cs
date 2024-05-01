//-----------------------------------------------------------------------
// <copyright file="TokenTest.cs" company="Mapbox">
//     Copyright (c) 2017 Mapbox. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections;
using Mapbox.Tokens;
using Mapbox.Unity;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Mapbox.MapboxSdkCs.UnitTest
{
	[TestFixture]
	internal class TokenTest
	{
		[SetUp]
		public void SetUp()
		{
			_tokenApi = new MapboxTokenApi();
			_configAccessToken = MapboxAccess.Instance.Configuration.AccessToken;
			_configSkuToken = MapboxAccess.Instance.Configuration.GetMapsSkuToken;
		}


		private MapboxTokenApi _tokenApi;
		private string _configAccessToken;
		private Func<string> _configSkuToken;


		[UnityTest]
		public IEnumerator RetrieveConfigToken()
		{
			MapboxToken token = null;

			_tokenApi.Retrieve(
				_configSkuToken,
				_configAccessToken,
				tok => { token = tok; }
			);

			while (null == token) yield return null;

			Assert.IsNull(token.ErrorMessage);
			Assert.IsFalse(token.HasError);
			Assert.AreEqual(MapboxTokenStatus.TokenValid, token.Status, "Config token is not valid");
		}


		[UnityTest]
		public IEnumerator TokenMalformed()
		{
			MapboxToken token = null;

			_tokenApi.Retrieve(
				_configSkuToken,
				"yada.yada",
				tok => { token = tok; }
			);

			while (null == token) yield return null;

			Assert.IsNull(token.ErrorMessage);
			Assert.IsFalse(token.HasError);
			Assert.AreEqual(MapboxTokenStatus.TokenMalformed, token.Status, "token is malformed");
		}


		[UnityTest]
		public IEnumerator TokenInvalid()
		{
			MapboxToken token = null;

			_tokenApi.Retrieve(
				_configSkuToken,
				"pk.12345678901234567890123456789012345.0123456789012345678901",
				tok => { token = tok; }
			);

			while (null == token) yield return null;

			Assert.IsNull(token.ErrorMessage);
			Assert.IsFalse(token.HasError);
			Assert.AreEqual(MapboxTokenStatus.TokenInvalid, token.Status, "token is invalid");
		}
	}
}
