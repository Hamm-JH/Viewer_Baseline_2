// Copyright (c) 2020 Nementic Games GmbH.

namespace Nementic.SelectionUtility
{
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;

	/// <summary>
	/// A popup which displays a list of selectable GameObjects.
	/// </summary>
	internal sealed class SelectionPopup : PopupWindowContent
	{
		private readonly DataSource dataSource;
		private readonly IconCache iconCache;

		private float buttonAndIconsWidth;
		private float buttonWidth;
		private float iconWidth;

		private bool styleNeedsUpdate;
		private Styles styles;
		private Vector2 scroll;
		private Rect contentRect;

		public SelectionPopup(IList<GameObject> options)
		{
			this.dataSource = new DataSource(options);
			this.iconCache = new IconCache();
		}

		public override void OnOpen()
		{
			base.OnOpen();
			editorWindow.wantsMouseMove = true;
			styles = new Styles();
			// Queue a rebuild of the styles during 
			// the next OnGUI to handle skin changes.
			styleNeedsUpdate = true;
			PrecalculateRequiredSizes();
			dataSource.FocusSearch();
		}

		private void PrecalculateRequiredSizes()
		{
			buttonWidth = 0;

			IList<GameObject> items = dataSource.items;

			for (int i = 0; i < items.Count; i++)
			{
				if (items[i] == null)
					continue;

				var style = styles.LabelStyle(items[i]);
				float width = items[i] != null ? style.CalcSize(new GUIContent(items[i].name)).x : 0f;

				// If a GameObject name is excessively long, clip it.
				int maxWidth = 300;
				if (width > maxWidth)
					width = maxWidth;

				if (width > this.buttonWidth)
					this.buttonWidth = width;
			}

			// After button, add small space.
			this.buttonWidth += EditorGUIUtility.standardVerticalSpacing;

			iconWidth = 0;

			for (int i = 0; i < items.Count; i++)
			{
				if (items[i] == null)
					continue;

				int iconCount = iconCache.CacheIcons(items[i]);

				float iconWidth = (18 * iconCount);

				if (iconWidth > this.iconWidth)
					this.iconWidth = iconWidth;
			}

			this.buttonAndIconsWidth = this.buttonWidth + this.iconWidth + EditorGUIUtility.standardVerticalSpacing;
		}

		public override void OnGUI(Rect rect)
		{
			if (styleNeedsUpdate)
			{
				// Especially the prefab label style must be recreated
				// on the next OnGUI call after a skin change.
				styles = new Styles();
				styleNeedsUpdate = false;
			}

			styles.Update();

			Event current = Event.current;
			bool repaint = current.type == EventType.Repaint;

			if (repaint && dataSource.CountValidItems(dataSource.items) == 0)
			{
				// If all GameObjects have been destroyed while the popup was open,
				// e.g. during scene change, the popup can be closed.
				ClosePopup();
				return;
			}

			// Account for the 1px gray border at the top of the window.
			rect.yMin += 1;

			rect = dataSource.SearchFieldGUI(rect, RowHeight);

			scroll = GUI.BeginScrollView(rect, scroll, contentRect, GUIStyle.none, GUI.skin.verticalScrollbar);

			rect.height = RowHeight;
			rect.xMin += 2;
			rect.xMax -= 2;

			IList<GameObject> items = dataSource.filteredItems;
			int count = dataSource.CountValidItems(dataSource.filteredItems);

			using (new EditorGUIUtility.IconSizeScope(styles.iconSize))
			{
				for (int i = 0; i < count; i++)
				{
					if (items[i] == null)
						continue;

					DrawRow(rect, current, items[i]);

					if (i < count && repaint)
						DrawSplitter(rect);

					rect.y += RowHeight;
				}
			}

			GUI.EndScrollView();

			if (current.type == EventType.MouseMove)
				editorWindow.Repaint();
		}

		private void ClosePopup()
		{
			if (editorWindow)
				editorWindow.Close();

			GUIUtility.ExitGUI();
		}

		private void DrawSplitter(Rect rect)
		{
			rect.height = 1;
			rect.y -= 1;
			rect.xMin = 0;
			rect.width += 4;
			EditorGUI.DrawRect(rect, styles.splitterColor);
		}

		private void DrawRow(Rect rect, Event current, GameObject item)
		{
			if (rect.Contains(current.mousePosition) &&
				current.type != EventType.MouseDrag)
			{
				Rect background = rect;
				background.xMin -= 1;
				background.xMax += 1;
				EditorGUI.DrawRect(background, styles.rowHoverColor);
			}

			Rect originalRect = rect;
			var icon = AssetPreview.GetMiniThumbnail(item);
			Rect iconRect = rect;
			iconRect.width = 20;

			EditorGUI.LabelField(iconRect, styles.TempContent(null, icon));

			rect.x = iconRect.xMax;
			rect.width = buttonWidth;

			var nameContent = styles.TempContent(item != null ? item.name : "Null", null);
			EditorGUI.LabelField(rect, nameContent, styles.LabelStyle(item));

			if (ObjectSelector.TrySelectObject(current, originalRect, item))
				ClosePopup();

			if (item == null)
				return;

			Rect componentIconRect = rect;
			componentIconRect.x = rect.xMax;
			componentIconRect.width = rect.height;

			Texture2D[] icons = iconCache.ForGameObject(item);
			for (int i = 0; i < icons.Length; i++)
			{
				componentIconRect.width = 16;
				GUI.DrawTexture(componentIconRect, icons[i], ScaleMode.ScaleToFit, true);
				componentIconRect.x = componentIconRect.xMax + 2;
			}
		}

		private float RowHeight
		{
			get { return 20; }
		}

		public override Vector2 GetWindowSize()
		{
			float totalHeight = 0;

			if (UserPrefs.ShowSearchField)
				totalHeight += RowHeight + 2;

			int itemCount = dataSource.CountValidItems(dataSource.filteredItems);
			totalHeight += RowHeight * itemCount;

			float iconBeforeLabelWidth = 22;
			Vector2 windowSize = new Vector2(iconBeforeLabelWidth + buttonAndIconsWidth, totalHeight);

			// Content refers to all item rows without the search field and is used by the scroll view.
			Vector2 contentSize = new Vector2(windowSize.x, windowSize.y - (UserPrefs.ShowSearchField ? RowHeight + 2 : 0));

			// Account for the offset by the search field and the border line at the top of the window.
			float contentYOffset = (UserPrefs.ShowSearchField ? RowHeight + 1 : 0) + 1;
			this.contentRect = new Rect(new Vector2(0, contentYOffset), contentSize);

			// The popup window has a 1px gray border that covers the top and bottom.
			// Make enough room for the content to fit perfectly within.
			totalHeight += 2;
			windowSize.y = totalHeight;

			int maxHeight = Mathf.Min(Screen.currentResolution.height, 700);

			if (totalHeight > maxHeight)
			{
				// Window is clamped and must show scroll bars for its content.
				windowSize.y = maxHeight;

				// Extra size to fit vertical scroll bar without clipping icons.
				windowSize.x += 14;
			}

			return windowSize;
		}
	}
}
