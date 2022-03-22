// Copyright (c) 2020 Nementic Games GmbH.

namespace Nementic.SelectionUtility
{
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;

	/// <summary>
	/// Provides access to the tool settings stored on the local machine.
	/// </summary>
	[InitializeOnLoad]
	internal static class UserPrefs
	{
		[Tooltip("True if the tool should be available when clicking in the SceneView.")]
		public static readonly Setting<bool> Enabled;

		[Tooltip("True if the popup should show a search field.")]
		public static readonly Setting<bool> ShowSearchField;

		[Tooltip("Mouse movement between click down and up larger than this value will be interpreted as scene view camera panning" +
			"instead of triggering the selection popup.")]
		public static readonly Setting<int> ContextClickPixelThreshold;

		[Tooltip("These type names are not displayed as component icons in the popup window.")]
		public static readonly StringListSetting HiddenIconTypeNames;

		static UserPrefs()
		{
			settings = new List<ISetting>();
			Enabled = new BoolSetting("Enabled", true);
			ShowSearchField = new BoolSetting("ShowSearchField", true);
			ContextClickPixelThreshold = new IntSetting("ClickDeadZone", defaultValue: 10, min: 0, max: int.MaxValue);

			var types = new List<string> { "Transform", "MeshFilter", "ParticleSystemRenderer" };
			HiddenIconTypeNames = new StringListSetting("HiddenIcons", types);

			LogSettings();
		}

		[System.Diagnostics.Conditional(CustomDebug.conditionString)]
		private static void LogSettings()
		{
			var sb = new System.Text.StringBuilder();
			sb.AppendLine("Initialized user prefs:");
			sb.AppendLine("Enabled: " + Enabled.Value);
			sb.AppendLine("ShowSearchField: " + ShowSearchField.Value);
			sb.AppendLine("ContextClickPixelThreshold: " + ContextClickPixelThreshold.Value);
			sb.AppendLine("HiddenIconTypeNames: " + HiddenIconTypeNames.ToString());

			CustomDebug.Log(sb.ToString());
		}

		/// <summary>
		/// The list to which all settings register to so that they can be iterated.
		/// </summary>
		private static readonly List<ISetting> settings;

		internal static void Register(ISetting setting)
		{
			settings.Add(setting);
		}

#if UNITY_2018_3_OR_NEWER
		[SettingsProvider]
        private static SettingsProvider CreateSettings()
        {
            return new SettingsProvider("Selection Utility", SettingsScope.User)
            {
                guiHandler = (searchContext) =>
                {
					DrawSettings();
				},
                keywords = new HashSet<string>(new[] { "Nementic", "Selection", "Utility" })
            };
        }
#else
		[PreferenceItem("Selection Utility")]
		private static void OnPreferencesGUI()
		{
			DrawSettings();
		}
#endif

		private static void DrawSettings()
		{
			for (int i = 0; i < settings.Count; i++)
				settings[i].DrawProperty();

			EditorGUILayout.Space();

			if (GUILayout.Button("Use Defaults", GUILayout.Width(120)))
				ResetToDefaults();
		}

		private static void ResetToDefaults()
		{
			for (int i = 0; i < settings.Count; i++)
				settings[i].Reset();

			// Stop text editing if any of the input fields are focused, because
			// otherwise they don't update until the user deselects them.
			GUI.FocusControl(null);
		}

		public static void DeleteAll()
		{
			if (settings != null)
			{
				foreach (var setting in settings)
					setting.Delete();
			}
		}
	}
}
