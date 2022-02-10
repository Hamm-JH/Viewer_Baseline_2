using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Bearroll.UltimateDecals {

	public class Bearroll_Editor: Editor {

		Dictionary<string, SerializedProperty> propCache = new Dictionary<string, SerializedProperty>();
		Dictionary<string, GUIContent> guiContentCache = new Dictionary<string, GUIContent>();

		protected new SerializedObject serializedObject;

		GUIContent c(string s) {
			if (!guiContentCache.ContainsKey(s)) {
				guiContentCache[s] = new GUIContent(s);
			}

			return guiContentCache[s];
		}

		SerializedProperty p(string name) {
			if (serializedObject == null) return null;

			if (!propCache.ContainsKey(name)) {
				propCache[name] = serializedObject.FindProperty(name);
			}

			return propCache[name];
		}

		protected void FastPropertyField(string name, string label) {

			if (serializedObject == null) {
				serializedObject = base.serializedObject;
			}

			var prop = p(name);

			if (prop == null) {
				Debug.Log("Property not found: " + name);
				return;
			}

			EditorGUILayout.PropertyField(prop, c(label));
		}

		protected void FastSliderField(string name, string label, int min, int max) {

			if (serializedObject == null) {
				serializedObject = base.serializedObject;
			}

			var prop = p(name);

			if (prop == null) {
				Debug.Log("Property not found: " + name);
				return;
			}

			EditorGUILayout.IntSlider(prop, min, max, c(label));
		}

		protected bool FastPropertyFieldCheck(string name, string label) {

			EditorGUI.BeginChangeCheck();

			EditorGUILayout.PropertyField(p(name), c(label));

			var hasChanged = EditorGUI.EndChangeCheck();

			if (hasChanged) {
				serializedObject.ApplyModifiedProperties();
			}

			return hasChanged;

		}

	}

}