﻿using System.Collections.Generic;
using Mapbox.Json;

namespace Mapbox.Platform.TilesetTileJSON
{
	public class TileJSONObjectVectorLayer
	{
		[JsonProperty("description")] public string Description { get; set; }


		[JsonProperty("fields")] public Dictionary<string, string> Fields { get; set; }


		[JsonProperty("id")] public string Id { get; set; }


		[JsonProperty("source")] public string Source { get; set; }


		[JsonProperty("source_name")] public string SourceName { get; set; }
	}
}
