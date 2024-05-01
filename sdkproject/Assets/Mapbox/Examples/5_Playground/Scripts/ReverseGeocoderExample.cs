//-----------------------------------------------------------------------
// <copyright file="ForwardGeocoderExample.cs" company="Mapbox">
//     Copyright (c) 2016 Mapbox. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using Mapbox.Json;
using Mapbox.Utils.JsonConverters;
using UnityEngine;
using UnityEngine.UI;

namespace Mapbox.Examples.Playground
{
	public class ReverseGeocoderExample : MonoBehaviour
	{
		[SerializeField] private ReverseGeocodeUserInput _searchLocation;

		[SerializeField] private Text _resultsText;

		private void Awake()
		{
			_searchLocation.OnGeocoderResponse += SearchLocation_OnGeocoderResponse;
		}

		private void OnDestroy()
		{
			if (_searchLocation != null) _searchLocation.OnGeocoderResponse -= SearchLocation_OnGeocoderResponse;
		}

		private void SearchLocation_OnGeocoderResponse(object sender, EventArgs e)
		{
			_resultsText.text = JsonConvert.SerializeObject(_searchLocation.Response, Formatting.Indented,
				JsonConverters.Converters);
		}
	}
}
