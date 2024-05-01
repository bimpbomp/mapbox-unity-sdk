//-----------------------------------------------------------------------
// <copyright file="FileSourceTest.cs" company="Mapbox">
//     Copyright (c) 2016 Mapbox. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

// TODO: figure out how run tests outside of Unity with .NET framework, something like '#if !UNITY'

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Mapbox.CheapRulerCs;
using NUnit.Framework;
using UnityEngine;

namespace Mapbox.ProbeExtractorCs.UnitTest
{
	[TestFixture]
	internal class ProbeExtractorCsTest
	{
		[SetUp]
		public void SetUp()
		{
			_trace = loadTraceFixture("ProbeExtractorCs_fixture_trace");
			_footTrace = loadTraceFixture("ProbeExtractorCs_fixture_trace-foot");
			_probes = loadProbeFixture("ProbeExtractorCs_fixture_probes");
		}

		private List<TracePoint> _trace;
		private List<TracePoint> _footTrace;
		private List<Probe> _probes;


		[Test]
		[Order(1)]
		public void FixturesLoaded()
		{
			Assert.AreEqual(14, _trace.Count);
			Assert.AreEqual(12, _probes.Count);
		}


		[Test]
		public void ExtractProbes()
		{
			var ruler = CheapRuler.FromTile(49, 7);

			var options = new ProbeExtractorOptions
			{
				MinTimeBetweenProbes = 1, // seconds
				MaxDistanceRatioJump =
					3, // do not include probes when the distance is 3 times bigger than the previous one
				MaxDurationRatioJump =
					3, // do not include probes when the duration is 3 times bigger than the previous one
				MaxAcceleration = 15, // meters per second per second
				MaxDeceleration = 18 // meters per second per second
			};

			var extractor = new ProbeExtractor(ruler, options);
			var extractedProbes = extractor.ExtractProbes(_trace);

			Assert.AreEqual(12, extractedProbes.Count, "12 probes were expected to be extracted");

			for (var i = 0; i < extractedProbes.Count; i++)
			{
				var fp = _probes[i]; // fixture probe
				var ep = extractedProbes[i]; // extracted probe

				Assert.AreEqual(fp.Longitude, ep.Longitude, 0.001, "probe[" + i + "]: longitude doesn't match");
				Assert.AreEqual(fp.Latitude, ep.Latitude, 0.001, "probe[" + i + "]: latitude doesn't match");
				Assert.AreEqual(fp.StartTime, ep.StartTime, "probe[" + i + "]: start time doesn't match");
				Assert.AreEqual(fp.Duration, ep.Duration, "probe[" + i + "]: duration doesn't match");
				Assert.AreEqual(fp.Speed, ep.Speed, 0.001, "probe[" + i + "]: speed doesn't match");
				Assert.AreEqual(fp.Bearing, ep.Bearing, 0.001, "probe[" + i + "]: bearing doesn't match");
				Assert.AreEqual(fp.Distance, ep.Distance, 0.001, "probe[" + i + "]: distance doesn't match");
				Assert.AreEqual(fp.IsGood, ep.IsGood, "probe[" + i + "]: longitude doesn't match");
			}


			options.MinTimeBetweenProbes = 2;
			extractor = new ProbeExtractor(ruler, options);
			extractedProbes = extractor.ExtractProbes(_trace);

			Assert.AreEqual(5, extractedProbes.Count, "5 probes were expected to be extracted");


			options.OutputBadProbes = true;
			extractor = new ProbeExtractor(ruler, options);
			extractedProbes = extractor.ExtractProbes(_trace);

			Assert.AreEqual(13, extractedProbes.Count, "13 probes were expected to be extracted");
		}


		[Test]
		public void ExtractFootTrace()
		{
			var ruler = new CheapRuler(_footTrace[0].Latitude);
			var options = new ProbeExtractorOptions();

			var extractor = new ProbeExtractor(ruler, options);
			var extractedProbes = extractor.ExtractProbes(_footTrace);

			Assert.AreEqual(40, extractedProbes.Count);
		}


		private List<TracePoint> loadTraceFixture(string fixtureName)
		{
			var fixtureAsset = Resources.Load<TextAsset>(fixtureName);
			var trace = new List<TracePoint>();
			using (var sr = new StringReader(fixtureAsset.text))
			{
				// skip header
				sr.ReadLine();
				string line;
				while (null != (line = sr.ReadLine()))
				{
					var tokens = line.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
					if (tokens.Length != 4)
					{
						Debug.LogWarning("trace.csv has wrong number of columns");
						continue;
					}

					double lng;
					double lat;
					double bearing;
					long timestamp;

					if (!double.TryParse(tokens[0], NumberStyles.Any, CultureInfo.InvariantCulture, out lng))
					{
						Debug.LogWarning("could not parse longitude");
						continue;
					}

					if (!double.TryParse(tokens[1], NumberStyles.Any, CultureInfo.InvariantCulture, out lat))
					{
						Debug.LogWarning("could not parse latitude");
						continue;
					}

					if (!double.TryParse(tokens[2], NumberStyles.Any, CultureInfo.InvariantCulture, out bearing))
					{
						Debug.LogWarning("could not parse bearing");
						continue;
					}

					if (!long.TryParse(tokens[3], NumberStyles.Any, CultureInfo.InvariantCulture, out timestamp))
					{
						Debug.LogWarning("could not parse timestamp");
						continue;
					}

					trace.Add(new TracePoint
					{
						Longitude = lng, Latitude = lat, Bearing = bearing, Timestamp = timestamp
					});
				}
			}

			return trace;
		}


		private List<Probe> loadProbeFixture(string fixtureName)
		{
			var fixtureAsset = Resources.Load<TextAsset>(fixtureName);
			var probes = new List<Probe>();
			using (var sr = new StringReader(fixtureAsset.text))
			{
				// skip header
				sr.ReadLine();
				string line;
				while (null != (line = sr.ReadLine()))
				{
					var tokens = line.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
					if (tokens.Length != 8)
					{
						Debug.LogWarning("probes.csv has wrong number of columns");
						continue;
					}

					double lng;
					double lat;
					long startTime;
					long duration;
					double speed;
					double bearing;
					double distance;
					bool isGood;

					if (!double.TryParse(tokens[0], NumberStyles.Any, CultureInfo.InvariantCulture, out lng))
					{
						Debug.LogWarning("could not parse longitude");
						continue;
					}

					if (!double.TryParse(tokens[1], NumberStyles.Any, CultureInfo.InvariantCulture, out lat))
					{
						Debug.LogWarning("could not parse latitude");
						continue;
					}

					if (!long.TryParse(tokens[2], NumberStyles.Any, CultureInfo.InvariantCulture, out startTime))
					{
						Debug.LogWarning("could not parse timestamp");
						continue;
					}

					if (!long.TryParse(tokens[3], NumberStyles.Any, CultureInfo.InvariantCulture, out duration))
					{
						Debug.LogWarning("could not parse duration");
						continue;
					}

					if (!double.TryParse(tokens[4], NumberStyles.Any, CultureInfo.InvariantCulture, out speed))
					{
						Debug.LogWarning("could not parse speed");
						continue;
					}

					if (!double.TryParse(tokens[5], NumberStyles.Any, CultureInfo.InvariantCulture, out bearing))
					{
						Debug.LogWarning("could not parse bearing");
						continue;
					}

					if (!double.TryParse(tokens[6], NumberStyles.Any, CultureInfo.InvariantCulture, out distance))
					{
						Debug.LogWarning("could not parse distance");
						continue;
					}

					if (!bool.TryParse(tokens[7], out isGood))
					{
						Debug.LogWarning("could not parse good");
						continue;
					}

					probes.Add(new Probe
					{
						Longitude = lng,
						Latitude = lat,
						StartTime = startTime,
						Duration = duration,
						Speed = speed,
						Bearing = bearing,
						Distance = distance,
						IsGood = isGood
					});
				}
			}

			return probes;
		}


		// quick hack for visualizing output of ProbeExtractor on http://geojson.io
		private string probesToGeojson(List<Probe> probes)
		{
			var sb = new StringBuilder();

			// line
			sb.Append(
				"{\"type\":\"FeatureCollection\",\"features\":[{\"type\":\"Feature\",\"properties\":{},\"geometry\":{\"type\":\"LineString\",\"coordinates\":[");
			sb.Append(string.Join(",",
				probes.Select(p => string.Format(CultureInfo.InvariantCulture, "[{0},{1}]", p.Longitude, p.Latitude))
					.ToArray()));
			sb.Append("]}}");

			sb.Append("]}");
			return sb.ToString();
		}
	}
}
