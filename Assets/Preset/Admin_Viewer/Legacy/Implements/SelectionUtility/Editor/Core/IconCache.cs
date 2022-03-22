// Copyright (c) 2020 Nementic Games GmbH.

namespace Nementic.SelectionUtility
{
	using System.Collections.Generic;
	using System.Linq;
	using UnityEditor;
	using UnityEngine;

	internal class IconCache
	{
		/// <summary>
		/// Caches the list of component icons for each GameObject to improve performance.
		/// </summary>
		private Dictionary<GameObject, Texture2D[]> iconLookup = new Dictionary<GameObject, Texture2D[]>(64);

		/// <summary>
		/// A lookup to avoid showing the same icon multiple times.
		/// </summary>
		private HashSet<Texture2D> displayedIcons = new HashSet<Texture2D>();

		private GameObject currentTarget;

		private readonly List<Component> components = new List<Component>(8);

		private readonly int defaultAssetHash = "DefaultAsset".GetHashCode();

		public int CacheIcons(GameObject gameObject)
		{
			gameObject.GetComponents<Component>(components);
			BeginCache(gameObject);

			for (int j = 0; j < components.Count; j++)
			{
				// Ignore missing scripts.
				if (components[j] == null)
					continue;

				CacheIcon(components[j]);
			}

			return EndCache();
		}

		/// <summary>
		/// Begins icon caching for the provided GameObject in a temporary buffer.
		/// </summary>
		private void BeginCache(GameObject gameObject)
		{
			displayedIcons.Clear();
			currentTarget = gameObject;
		}

		private void CacheIcon(Component component)
		{
			var type = component.GetType();

			if (UserPrefs.HiddenIconTypeNames.Contains(type.Name))
				return;

			Texture2D icon = AssetPreview.GetMiniThumbnail(component);

			// TODO: Should this be a user setting?
			// Ignore duplicates.
			if (displayedIcons.Contains(icon))
				return;

			// The default asset icon is returned if nothing else was found,
			// and since it doesn't add much info, omit it.
			if (icon.name.GetHashCode() == defaultAssetHash)
				return;

			displayedIcons.Add(icon);
		}

		/// <summary>
		/// Ends icon caching for the current target and returns the number of cached icon.s
		/// </summary>
		private int EndCache()
		{
			iconLookup.Add(currentTarget, displayedIcons.ToArray());
			return displayedIcons.Count;
		}

		/// <summary>
		/// Returns the collection of icons for the provided GameObject.
		/// </summary>
		public Texture2D[] ForGameObject(GameObject gameObject)
		{
			return iconLookup[gameObject];
		}
	}
}
