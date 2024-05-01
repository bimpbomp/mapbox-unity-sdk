using System;
using Mapbox.Json;
using Mapbox.Utils;

namespace Mapbox.Platform.TilesetTileJSON
{
	public class TileJSONResponse
	{
		private double[] _bounds;


		private double[] _center;


		private long? _created;


		private long? _modified;


		[JsonProperty("attribution")] public string Attribution { get; set; }


		[JsonProperty("autoscale")] public bool AutoScale { get; set; }

		[JsonProperty("bounds")]
		public double[] Bounds
		{
			get => _bounds;
			set
			{
				_bounds = value;
				BoundsParsed = new Vector2dBounds(
					new Vector2d(Bounds[1], Bounds[0])
					, new Vector2d(Bounds[3], Bounds[2])
				);
			}
		}


		[JsonIgnore] public Vector2dBounds BoundsParsed { get; private set; }

		[JsonProperty("center")]
		public double[] Center
		{
			get => _center;
			set
			{
				_center = value;
				CenterParsed = new Vector2d(_center[1], _center[0]);
			}
		}

		[JsonIgnore] public Vector2d CenterParsed { get; private set; }

		/// <summary>Concatenated tilesets don't have a created property </summary>
		[JsonProperty("created")]
		public long? Created
		{
			get => _created;
			set
			{
				_created = value;
				if (_created.HasValue)
					CreatedUtc = UnixTimestampUtils.From(_created.Value);
				else
					CreatedUtc = null;
			}
		}


		/// <summary>Concatenated tilesets don't have a created property </summary>
		[JsonIgnore]
		public DateTime? CreatedUtc { get; private set; }


		[JsonProperty("description")] public string Description { get; set; }


		/// <summary>Can be empty</summary>
		[JsonProperty("format")]
		public string Format { get; set; }


		/// <summary>Can be empty (for concatenated tilesets)</summary>
		[JsonProperty("id")]
		public string Id { get; set; }


		[JsonProperty("maxzoom")] public int MaxZoom { get; set; }


		[JsonProperty("minzoom")] public int MinZoom { get; set; }

		/// <summary>Unmodified tilesets don't have a modfied property </summary>
		[JsonProperty("modified")]
		public long? Modified
		{
			get => _modified;
			set
			{
				_modified = value;
				if (_modified.HasValue)
					ModifiedUtc = UnixTimestampUtils.From(_modified.Value);
				else
					ModifiedUtc = null;
			}
		}


		/// <summary>Unmodified tilesets don't have a modfied property </summary>
		[JsonIgnore]
		public DateTime? ModifiedUtc { get; private set; }


		[JsonProperty("name")] public string Name { get; set; }


		[JsonProperty("private")] public bool Private { get; set; }


		[JsonProperty("scheme")] public string Scheme { get; set; }


		/// <summary>Can be empty</summary>
		[JsonProperty("source")]
		public string Source { get; set; }


		[JsonProperty("tilejson")] public string TileJSONVersion { get; set; }


		[JsonProperty("tiles")] public string[] Tiles { get; set; }


		[JsonProperty("vector_layers")] public TileJSONObjectVectorLayer[] VectorLayers { get; set; }


		/// <summary>Can be empty (for concatenated tilesets)</summary>
		[JsonProperty("webpage")]
		public string WebPage { get; set; }
	}
}
