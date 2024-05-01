using Mapbox.Json;

namespace Mapbox.Unity.Map
{
	public class TileStats
	{
		[JsonProperty("account")] public string account;

		[JsonProperty("layers")] public LayerStats[] layers;

		[JsonProperty("tilesetid")] public string tilesetid;
	}

	public class LayerStats
	{
		[JsonProperty("account")] public string account;

		[JsonProperty("attributes")] public AttributeStats[] attributes;

		[JsonProperty("count")] public string count;

		[JsonProperty("geometry")] public string geometry;

		[JsonProperty("layer")] public string layer;

		[JsonProperty("tilesetid")] public string tilesetid;
	}

	public class AttributeStats
	{
		[JsonProperty("attribute")] public string attribute;

		[JsonProperty("type")] public string type;

		[JsonProperty("values")] public string[] values;
	}
}
