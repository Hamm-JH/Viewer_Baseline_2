// Copyright (c) 2020 Nementic Games GmbH.

namespace Nementic.SelectionUtility
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text.RegularExpressions;
	using UnityEditor;
	using UnityEditor.IMGUI.Controls;
	using UnityEngine;

	/// <summary>
	/// Holds the collection of GameObjects that can be picked and
	/// applies search string filtering to it.
	/// </summary>
	/// <remarks>
	/// Remember that GameObjects in these lists can be destroyed and become null at any time.
	/// </remarks>
	internal class DataSource
	{
		/// <summary>
		/// The original collection of all GameObjects at the mouse position.
		/// </summary>
		public IList<GameObject> items;

		/// <summary>
		/// The collection of GameObjects after the search filter has been applied.
		/// </summary>
		public List<GameObject> filteredItems;

		private string searchString;
		private readonly SearchField searchField = new SearchField();

		public DataSource(IList<GameObject> options)
		{
			this.items = options;
			this.filteredItems = options.ToList();
		}

		public void FocusSearch()
		{
			if (UserPrefs.ShowSearchField)
				searchField.SetFocus();
		}

		public Rect SearchFieldGUI(Rect rect, float height)
		{
			if (UserPrefs.ShowSearchField)
			{
				Rect searchRect = rect;
				searchRect.height = height;
				searchRect.yMin += 3;
				searchRect.xMin += 4;
				searchRect.xMax -= 4;

				EditorGUI.BeginChangeCheck();
				searchString = searchField.OnToolbarGUI(searchRect, searchString);
				if (EditorGUI.EndChangeCheck())
					OnSearchChanged();

				rect.yMin = searchRect.yMax + 2;
			}
			return rect;
		}

		private void OnSearchChanged()
		{
			if (searchString == null)
				searchString = string.Empty;

			// To polish the search experience:
			// - Remove white space at the start and end of the search string.
			// - Ignore multiple spaces in a row by collapsing them down to a single one.
			// - Ignore letter case.
			string value = Regex.Replace(searchString.Trim(), @"[ ]+", " ");

			// Extract any component type query. User must enter 't:' followed by a type name,
			// with or without spaces in between. Also match if no type name is specified (yet)
			// so that the search window shows all entries until the type name is started.
			var match = Regex.Match(value, @"t:\s*(\w*)");
			string typeName = null;

			if (match.Success)
			{
				value = value.Replace(match.Value, string.Empty).Trim();
				typeName = match.Groups[1].Value.Trim().ToLower();
			}

			filteredItems.Clear();
			for (int i = 0; i < items.Count; i++)
			{
				if (items[i] == null)
					continue;

				// It would be possible to use GetComponent(string)
				// to check for type match, but this would be case-sensitive.
				// For case-insensitive lookup, compare each component name.
				bool hasMatchingComponent = false;
				if (typeName != null)
				{
					var components = items[i].GetComponents<Component>();
					foreach (var comp in components)
					{
						if (comp != null && comp.GetType().Name.ToLower() == typeName)
						{
							hasMatchingComponent = true;
							break;
						}
					}
				}

				if (items[i].name.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0 &&
					(typeName == null || hasMatchingComponent))
				{
					filteredItems.Add(items[i]);
				}
			}
		}

		public int CountValidItems(IList<GameObject> items)
		{
			if (items == null)
				return 0;

			int totalCount = items.Count;
			int count = 0;

			for (int i = 0; i < totalCount; i++)
			{
				if (items[i] != null)
					count++;
			}

			return count;
		}
	}
}
