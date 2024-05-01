using System.IO;
using UnityEditor;
using UnityEngine;

namespace Mapbox.Tests
{
	public class CopyEditModeTests : MonoBehaviour
	{
		/// <summary>
		///     Copies EditMode tests to a *not* 'Editior' folder to make them available in PlayMode
		/// </summary>
		[MenuItem("Mapbox/DevTools/Copy EditMode tests to PlayMode tests")]
		private static void CopyEditModeTestFiles()
		{
			// check if destination folder exists
			var destinationFolderName = "DoNotRenameOrRemove_MapboxPlayModeTests";
			var destinationFolderGuids = AssetDatabase.FindAssets(destinationFolderName);
			if (null == destinationFolderGuids || 0 == destinationFolderGuids.Length)
			{
				Debug.LogErrorFormat("destination folder not found: [{0}]", destinationFolderName);
				return;
			}

			if (destinationFolderGuids.Length > 1)
			{
				Debug.LogErrorFormat("several destination folders found: [{0}]", destinationFolderName);
				return;
			}

			var destinationFolderPath = AssetDatabase.GUIDToAssetPath(destinationFolderGuids[0]);
			Debug.LogFormat("destination folder: [{0}]", destinationFolderPath);


			// delete test files already existing in destintation folder
			var oldTestAssetGuids = AssetDatabase.FindAssets("Tests t:Script", new[] { destinationFolderPath });
			if (null != oldTestAssetGuids && oldTestAssetGuids.Length > 0)
				foreach (var oldTestAssetGuid in oldTestAssetGuids)
				{
					var oldTestAssetPath = AssetDatabase.GUIDToAssetPath(oldTestAssetGuid);
					Debug.LogFormat("deleting old test file: [{0}]", oldTestAssetPath);

					if (!AssetDatabase.DeleteAsset(oldTestAssetPath))
						Debug.LogErrorFormat("failed to delete: [{0}]", oldTestAssetPath);
				}


			// copy test files according to naming convention
			var editModeTestAssetGuids = AssetDatabase.FindAssets("MapboxUnitTests_ t:Script");
			foreach (var testAssetGuid in editModeTestAssetGuids)
			{
				var testAssetSourcePath = AssetDatabase.GUIDToAssetPath(testAssetGuid);
				var fileName = Path.GetFileName(testAssetSourcePath);
				Debug.LogFormat("copying [{0}]", testAssetSourcePath);
				if (!AssetDatabase.CopyAsset(testAssetSourcePath, destinationFolderPath + "/" + fileName))
					Debug.LogErrorFormat("failed to copy [{0}]", testAssetSourcePath);
			}
		}
	}
}
