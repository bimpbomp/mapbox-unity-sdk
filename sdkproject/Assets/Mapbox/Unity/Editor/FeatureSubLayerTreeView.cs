﻿using System.Collections.Generic;
using Mapbox.Unity.Map;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Mapbox.Editor
{
	internal class FeatureSubLayerTreeView : TreeViewWithTreeModel<FeatureTreeElement>
	{
		private const float kRowHeights = 15f;
		private const float nameOffset = 15f;
		public static int uniqueIdPoI = 1000;
		public static int uniqueIdFeature = 3000;

		private readonly GUIStyle columnStyle = new()
		{
			alignment = TextAnchor.MiddleCenter, normal = new GUIStyleState { textColor = Color.white }
		};

		public bool hasChanged = false;
		private readonly float kToggleWidth = 18f;
		public SerializedProperty Layers;

		private MultiColumnHeaderState m_MultiColumnHeaderState;
		public int maxElementsAdded = 0;
		public int uniqueId;

		public FeatureSubLayerTreeView(TreeViewState state, MultiColumnHeader multicolumnHeader,
			TreeModel<FeatureTreeElement> model, int uniqueIdentifier = 3000) : base(state, multicolumnHeader, model)
		{
			showAlternatingRowBackgrounds = true;
			showBorder = true;
			customFoldoutYOffset =
				(kRowHeights - EditorGUIUtility.singleLineHeight) *
				0.5f; // center foldout in the row since we also center content. See RowGUI
			extraSpaceBeforeIconAndLabel = kToggleWidth;
			uniqueId = uniqueIdentifier;
			Reload();
		}

		protected override bool CanRename(TreeViewItem item)
		{
			// Only allow rename if we can show the rename overlay with a certain width (label might be clipped by other columns)
			var renameRect = GetRenameRect(treeViewRect, 0, item);
			return renameRect.width > 30;
		}

		protected override void RenameEnded(RenameEndedArgs args)
		{
			if (Layers == null || Layers.arraySize == 0) return;

			if (args.acceptedRename)
			{
				var element = treeModel.Find(args.itemID);
				element.name = string.IsNullOrEmpty(args.newName.Trim()) ? args.originalName : args.newName;
				element.Name = string.IsNullOrEmpty(args.newName.Trim()) ? args.originalName : args.newName;
				var layer = Layers.GetArrayElementAtIndex(args.itemID - uniqueId);
				layer.FindPropertyRelative("coreOptions.sublayerName").stringValue = element.name;
				Reload();
			}
		}

		protected override Rect GetRenameRect(Rect rowRect, int row, TreeViewItem item)
		{
			var cellRect = GetCellRectForTreeFoldouts(rowRect);
			cellRect.xMin = nameOffset;
			CenterRectUsingSingleLineHeight(ref cellRect);
			return base.GetRenameRect(cellRect, row, item);
		}

		public void RemoveItemFromTree(int id)
		{
			treeModel.RemoveElements(new List<int> { id });
		}

		public void AddElementToTree(SerializedProperty subLayer)
		{
			var name = subLayer.FindPropertyRelative("coreOptions.sublayerName").stringValue;
			var id = Layers.arraySize - 1 + uniqueId;

			if (treeModel.Find(id) != null)
			{
				Debug.Log(" found one. exiting");
				return;
			}

			var type = ((PresetFeatureType)subLayer.FindPropertyRelative("presetFeatureType").enumValueIndex)
				.ToString();
			var element = new FeatureTreeElement(name, 0, id);
			element.Name = name;
			element.Type = type;
			treeModel.AddElement(element, treeModel.root, treeModel.numberOfDataElements - 1);
		}

		protected override void RowGUI(RowGUIArgs args)
		{
			var rowItem = (TreeViewItem<FeatureTreeElement>)args.item;
			for (var i = 0; i < args.GetNumVisibleColumns(); ++i)
				CellGUI(args.GetCellRect(i), rowItem, (FeatureSubLayerColumns)args.GetColumn(i), ref args);
		}

		private void CellGUI(Rect cellRect, TreeViewItem<FeatureTreeElement> item, FeatureSubLayerColumns column,
			ref RowGUIArgs args)
		{
			// Center cell rect vertically (makes it easier to place controls, icons etc in the cells)
			if (Layers == null || Layers.arraySize == 0) return;

			if (Layers.arraySize <= args.item.id - uniqueId) return;

			var layer = Layers.GetArrayElementAtIndex(args.item.id - uniqueId);
			CenterRectUsingSingleLineHeight(ref cellRect);
			if (column == FeatureSubLayerColumns.Name)
			{
				var toggleRect = cellRect;
				toggleRect.x += GetContentIndent(item);
				toggleRect.width = kToggleWidth;

				EditorGUI.BeginChangeCheck();
				item.data.isActive = layer.FindPropertyRelative("coreOptions.isActive").boolValue;
				if (toggleRect.xMax < cellRect.xMax)
					item.data.isActive =
						EditorGUI.Toggle(toggleRect, item.data.isActive); // hide when outside cell rect
				layer.FindPropertyRelative("coreOptions.isActive").boolValue = item.data.isActive;
				if (EditorGUI.EndChangeCheck())
				{
					var vectorSubLayerProperties =
						(VectorSubLayerProperties)EditorHelper.GetTargetObjectOfProperty(layer);
					EditorHelper.CheckForModifiedProperty(layer, vectorSubLayerProperties.coreOptions);
				}

				cellRect.xMin += nameOffset; // Adding some gap between the checkbox and the name
				args.rowRect = cellRect;

				layer.FindPropertyRelative("coreOptions.sublayerName").stringValue = item.data.Name;
				//This draws the name property
				base.RowGUI(args);
			}

			if (column == FeatureSubLayerColumns.Type)
			{
				cellRect.xMin += 15f; // Adding some gap between the checkbox and the name

				var typeString =
					((PresetFeatureType)layer.FindPropertyRelative("presetFeatureType").intValue).ToString();
				item.data.Type = typeString;
				EditorGUI.LabelField(cellRect, item.data.Type, columnStyle);
			}
		}

		public static MultiColumnHeaderState CreateDefaultMultiColumnHeaderState()
		{
			var columns = new[]
			{
				//Name column
				new MultiColumnHeaderState.Column
				{
					headerContent = new GUIContent("Name"),
					contextMenuText = "Name",
					headerTextAlignment = TextAlignment.Center,
					autoResize = true,
					canSort = false,
					allowToggleVisibility = false
				},

				//Type column
				new MultiColumnHeaderState.Column
				{
					headerContent = new GUIContent("Type"),
					contextMenuText = "Type",
					headerTextAlignment = TextAlignment.Center,
					autoResize = true,
					canSort = false,
					allowToggleVisibility = false
				}
			};

			var state = new MultiColumnHeaderState(columns);
			return state;
		}

		// All columns
		private enum FeatureSubLayerColumns
		{
			Name,
			Type
		}
	}
}
