﻿//-----------------------------------------------------------------------
// <copyright file="MapUtils.cs" company="Mapbox">
//     Copyright (c) 2016 Mapbox. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using Mapbox.Utils;

namespace Mapbox.Map
{
	/// <summary>
	///     Utilities for working with Map APIs.
	/// </summary>
	public static class MapUtils
	{
		/// <summary>
		///     Normalizes a static style URL.
		/// </summary>
		/// <returns>The static style URL.</returns>
		/// <param name="url">A url, either a Mapbox URI (mapbox://{username}/{styleid}) or a full url to a map.</param>
		public static string NormalizeStaticStyleURL(string url)
		{
			var isMapboxUrl = url.StartsWith("mapbox://", StringComparison.Ordinal);

			// Support full Mapbox URLs by returning here if a mapbox URL is not detected.
			if (!isMapboxUrl) return url;

			var split = url.Split('/');
			var user = split[3];
			var style = split[4];
			var draft = string.Empty;

			if (split.Length > 5) draft = "/draft";

			return Constants.BaseAPI + "styles/v1/" + user + "/" + style + draft + "/tiles";
		}

		/// <summary>
		///     Converts a TilesetId to a URL.
		/// </summary>
		/// <returns>The identifier to URL.</returns>
		/// <param name="id">The style id.</param>
		public static string TilesetIdToUrl(string id)
		{
			// TODO: Validate that id is a real id
			const string MapBaseApi = Constants.BaseAPI + "v4/";

			return MapBaseApi + id;
		}
	}
}
