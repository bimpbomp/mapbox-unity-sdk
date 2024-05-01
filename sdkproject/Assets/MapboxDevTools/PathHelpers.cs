using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Mapbox.Unity.Utilities.DebugTools
{
	public static class PathHelpers
	{
		private static readonly string kScenesPath = Path.Combine(Application.dataPath, "Mapbox/Examples");
		private static readonly string arScenesPath = Path.Combine(Application.dataPath, "MapboxAR/Examples");

		public static List<string> AllScenes
		{
			get
			{
				var files = DirSearch(new DirectoryInfo(kScenesPath), "*.unity");
				var arfiles = DirSearch(new DirectoryInfo(arScenesPath), "*.unity");
				if (arfiles != null) files.AddRange(arfiles);
				var assetRefs = new List<string>();
				foreach (var fi in files)
				{
					if (fi.Name.StartsWith(".", StringComparison.Ordinal)) continue;
					assetRefs.Add(GetRelativeAssetPathFromFullPath(fi.FullName));
				}

				return assetRefs;
			}
		}

		private static List<FileInfo> DirSearch(DirectoryInfo d, string searchFor)
		{
			List<FileInfo> founditems = null;

			if (d.Exists)
			{
				founditems = d.GetFiles(searchFor).ToList();

				var dis = d.GetDirectories();
				foreach (var di in dis) founditems.AddRange(DirSearch(di, searchFor));
			}

			return founditems;
		}

		private static string GetRelativeAssetPathFromFullPath(string fullPath)
		{
			fullPath = CleanPathSeparators(fullPath);
			if (fullPath.Contains(Application.dataPath)) return fullPath.Replace(Application.dataPath, "Assets");
			Debug.LogWarning("Path does not point to a location within Assets: " + fullPath);
			return null;
		}

		private static string CleanPathSeparators(string s)
		{
			const string forwardSlash = "/";
			const string backSlash = "\\";
			return s.Replace(backSlash, forwardSlash);
		}
	}
}
