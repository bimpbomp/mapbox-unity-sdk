using System.Collections.Generic;
using System.Linq;
using Mapbox.Editor;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Mapbox.Unity.Map
{
	public class PointsOfInterestSubLayerPropertiesDrawer
	{
		private static readonly float _lineHeight = EditorGUIUtility.singleLineHeight;
		public bool isLayerAdded;

		private FeatureSubLayerTreeView layerTreeView;

		private bool m_Initialized;

		[SerializeField] private MultiColumnHeaderState m_MultiColumnHeaderState;

		[SerializeField] private TreeViewState m_TreeViewState;

		private string objectId = "";
		private IList<int> selectedLayers = new List<int>();

		private TreeModel<FeatureTreeElement> treeModel;

		private int SelectionIndex
		{
			get => EditorPrefs.GetInt(objectId + "LocationPrefabsLayerProperties_selectionIndex");
			set => EditorPrefs.SetInt(objectId + "LocationPrefabsLayerProperties_selectionIndex", value);
		}

		public void DrawUI(SerializedProperty property)
		{
			objectId = property.serializedObject.targetObject.GetInstanceID().ToString();
			var prefabItemArray = property.FindPropertyRelative("locationPrefabList");
			var layersRect = EditorGUILayout.GetControlRect(
				GUILayout.MinHeight(Mathf.Max(prefabItemArray.arraySize + 1, 1) * _lineHeight +
				                    MultiColumnHeader.DefaultGUI.defaultHeight),
				GUILayout.MaxHeight((prefabItemArray.arraySize + 1) * _lineHeight +
				                    MultiColumnHeader.DefaultGUI.defaultHeight));

			if (!m_Initialized)
			{
				var firstInit = m_MultiColumnHeaderState == null;
				var headerState = FeatureSubLayerTreeView.CreateDefaultMultiColumnHeaderState();
				if (MultiColumnHeaderState.CanOverwriteSerializedFields(m_MultiColumnHeaderState, headerState))
					MultiColumnHeaderState.OverwriteSerializedFields(m_MultiColumnHeaderState, headerState);
				m_MultiColumnHeaderState = headerState;

				var multiColumnHeader = new FeatureSectionMultiColumnHeader(headerState);

				if (firstInit) multiColumnHeader.ResizeToFit();

				treeModel = new TreeModel<FeatureTreeElement>(GetData(prefabItemArray));
				if (m_TreeViewState == null) m_TreeViewState = new TreeViewState();

				if (layerTreeView == null)
					layerTreeView = new FeatureSubLayerTreeView(m_TreeViewState, multiColumnHeader, treeModel,
						FeatureSubLayerTreeView.uniqueIdPoI);
				layerTreeView.multiColumnHeader = multiColumnHeader;
				m_Initialized = true;
			}


			layerTreeView.Layers = prefabItemArray;
			layerTreeView.Reload();
			layerTreeView.OnGUI(layersRect);

			if (layerTreeView.hasChanged)
			{
				EditorHelper.CheckForModifiedProperty(property);
				layerTreeView.hasChanged = false;
			}

			selectedLayers = layerTreeView.GetSelection();
			//if there are selected elements, set the selection index at the first element.
			//if not, use the Selection index to persist the selection at the right index.
			if (selectedLayers.Count > 0)
			{
				//ensure that selectedLayers[0] isn't out of bounds
				if (selectedLayers[0] - FeatureSubLayerTreeView.uniqueIdPoI > prefabItemArray.arraySize - 1)
					selectedLayers[0] = prefabItemArray.arraySize - 1 + FeatureSubLayerTreeView.uniqueIdPoI;

				SelectionIndex = selectedLayers[0];
			}
			else
			{
				selectedLayers = new int[1] { SelectionIndex };
				if (SelectionIndex > 0 && SelectionIndex - FeatureSubLayerTreeView.uniqueIdPoI <=
				    prefabItemArray.arraySize - 1) layerTreeView.SetSelection(selectedLayers);
			}


			GUILayout.Space(EditorGUIUtility.singleLineHeight);
			EditorGUILayout.BeginHorizontal();

			if (GUILayout.Button(new GUIContent("Add Layer"), "minibuttonleft"))
			{
				prefabItemArray.arraySize++;

				var prefabItem = prefabItemArray.GetArrayElementAtIndex(prefabItemArray.arraySize - 1);
				var prefabItemName = prefabItem.FindPropertyRelative("coreOptions.sublayerName");

				prefabItemName.stringValue = "New Location";

				// Set defaults here because SerializedProperty copies the previous element.
				prefabItem.FindPropertyRelative("coreOptions.isActive").boolValue = true;
				prefabItem.FindPropertyRelative("coreOptions.snapToTerrain").boolValue = true;
				prefabItem.FindPropertyRelative("presetFeatureType").enumValueIndex = (int)PresetFeatureType.Points;
				var categories = prefabItem.FindPropertyRelative("categories");
				categories.intValue = (int)LocationPrefabCategories.AnyCategory; //To select any category option

				var density = prefabItem.FindPropertyRelative("density");
				density.intValue = 15; //To select all locations option

				//Refreshing the tree
				layerTreeView.Layers = prefabItemArray;
				layerTreeView.AddElementToTree(prefabItem);
				layerTreeView.Reload();

				selectedLayers = new int[1] { prefabItemArray.arraySize - 1 };
				layerTreeView.SetSelection(selectedLayers);

				if (EditorHelper.DidModifyProperty(property)) isLayerAdded = true;
			}

			if (GUILayout.Button(new GUIContent("Remove Selected"), "minibuttonright"))
			{
				foreach (var index in selectedLayers.OrderByDescending(i => i))
					if (layerTreeView != null)
					{
						var poiSubLayer =
							prefabItemArray.GetArrayElementAtIndex(index - FeatureSubLayerTreeView.uniqueIdPoI);

						var vectorLayerProperties =
							(VectorLayerProperties)EditorHelper.GetTargetObjectOfProperty(property);
						var poiSubLayerProperties =
							(PrefabItemOptions)EditorHelper.GetTargetObjectOfProperty(poiSubLayer);

						vectorLayerProperties.OnSubLayerPropertyRemoved(
							new VectorLayerUpdateArgs { property = poiSubLayerProperties });

						layerTreeView.RemoveItemFromTree(index);
						prefabItemArray.DeleteArrayElementAtIndex(index - FeatureSubLayerTreeView.uniqueIdPoI);
						layerTreeView.treeModel.SetData(GetData(prefabItemArray));
					}

				selectedLayers = new int[0];
				layerTreeView.SetSelection(selectedLayers);
			}

			EditorGUILayout.EndHorizontal();

			if (selectedLayers.Count == 1 && prefabItemArray.arraySize != 0 &&
			    selectedLayers[0] - FeatureSubLayerTreeView.uniqueIdPoI >= 0)
			{
				//ensure that selectedLayers[0] isn't out of bounds
				if (selectedLayers[0] - FeatureSubLayerTreeView.uniqueIdPoI > prefabItemArray.arraySize - 1)
					selectedLayers[0] = prefabItemArray.arraySize - 1 + FeatureSubLayerTreeView.uniqueIdPoI;
				SelectionIndex = selectedLayers[0];

				var layerProperty =
					prefabItemArray.GetArrayElementAtIndex(SelectionIndex - FeatureSubLayerTreeView.uniqueIdPoI);

				layerProperty.isExpanded = true;
				var subLayerCoreOptions = layerProperty.FindPropertyRelative("coreOptions");
				var isLayerActive = subLayerCoreOptions.FindPropertyRelative("isActive").boolValue;
				if (!isLayerActive) GUI.enabled = false;
				DrawLayerLocationPrefabProperties(layerProperty, property);
				if (!isLayerActive) GUI.enabled = true;
			}
			else
			{
				GUILayout.Space(15);
				GUILayout.Label("Select a visualizer to see properties");
			}
		}

		private void DrawLayerLocationPrefabProperties(SerializedProperty layerProperty, SerializedProperty property)
		{
			EditorGUILayout.PropertyField(layerProperty);
		}

		private IList<FeatureTreeElement> GetData(SerializedProperty subLayerArray)
		{
			var elements = new List<FeatureTreeElement>();
			var name = string.Empty;
			var type = string.Empty;
			var id = 0;
			var root = new FeatureTreeElement("Root", -1, 0);
			elements.Add(root);
			for (var i = 0; i < subLayerArray.arraySize; i++)
			{
				var subLayer = subLayerArray.GetArrayElementAtIndex(i);
				name = subLayer.FindPropertyRelative("coreOptions.sublayerName").stringValue;
				id = i + FeatureSubLayerTreeView.uniqueIdPoI;
				type = PresetFeatureType.Points.ToString();
				var element = new FeatureTreeElement(name, 0, id);
				element.Name = name;
				element.name = name;
				element.Type = type;
				elements.Add(element);
			}

			return elements;
		}
	}
}
