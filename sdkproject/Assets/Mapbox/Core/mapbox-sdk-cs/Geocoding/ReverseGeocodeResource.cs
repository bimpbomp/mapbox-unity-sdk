//-----------------------------------------------------------------------
// <copyright file="ReverseGeocodeResource.cs" company="Mapbox">
//     Copyright (c) 2016 Mapbox. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Collections.Generic;
using Mapbox.Utils;

namespace Mapbox.Geocoding
{
	/// <summary> A reverse geocode request. </summary>
	public sealed class ReverseGeocodeResource : GeocodeResource<Vector2d>
	{
		// Required

		/// <summary> Initializes a new instance of the <see cref="ReverseGeocodeResource" /> class.</summary>
		/// <param name="query"> Location to reverse geocode. </param>
		public ReverseGeocodeResource(Vector2d query)
		{
			Query = query;
		}

		/// <summary> Gets or sets the location. </summary>
		public override Vector2d Query { get; set; }

		/// <summary> Builds a complete reverse geocode URL string. </summary>
		/// <returns> A complete, valid reverse geocode URL string. </returns>
		public override string GetUrl()
		{
			var opts = new Dictionary<string, string>();

			if (Types != null) opts.Add("types", GetUrlQueryFromArray(Types));

			return Constants.BaseAPI +
			       ApiEndpoint +
			       Mode +
			       Query +
			       ".json" +
			       EncodeQueryString(opts);
		}
	}
}
