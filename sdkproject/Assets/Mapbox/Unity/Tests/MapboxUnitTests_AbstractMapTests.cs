using System.Collections;
using Mapbox.Unity.Map;
using Mapbox.Utils;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Mapbox.Unity.Tests
{
	[TestFixture]
	internal class AbstractMapTests
	{
		private GameObject _map;

		[UnityTest]
		public IEnumerator SetUpDefaultMap()
		{
			var go = new GameObject("Map");
			var _map = go.AddComponent<AbstractMap>();
			_map.OnInitialized += () => { Assert.IsNotNull(_map); };

			yield return new WaitForFixedUpdate();
			;
			_map.Initialize(new Vector2d(37.7749, -122.4194), 15);
		}
	}
}
