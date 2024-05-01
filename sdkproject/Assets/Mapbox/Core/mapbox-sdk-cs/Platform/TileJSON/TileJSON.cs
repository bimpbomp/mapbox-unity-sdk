using System;
using System.Text;
using Mapbox.Json;
using Mapbox.Utils;

namespace Mapbox.Platform.TilesetTileJSON
{
	public class TileJSON
	{
		private readonly int _timeout;


		public TileJSON(IFileSource fileSource, int timeout)
		{
			FileSource = fileSource;
			_timeout = timeout;
		}


		public IFileSource FileSource { get; }


		public IAsyncRequest Get(string tilesetName, Action<TileJSONResponse> callback)
		{
			var url = string.Format(
				"{0}v4/{1}.json?secure"
				, Constants.BaseAPI
				, tilesetName
			);

			return FileSource.Request(
				url
				, response =>
				{
					var json = Encoding.UTF8.GetString(response.Data);
					var tileJSONResponse = JsonConvert.DeserializeObject<TileJSONResponse>(json);
					if (tileJSONResponse != null) tileJSONResponse.Source = tilesetName;
					callback(tileJSONResponse);
				}
				, _timeout
			);
		}
	}
}
