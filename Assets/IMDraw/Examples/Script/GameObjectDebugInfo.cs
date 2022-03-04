using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

#if UNITY_EDITOR
using UnityEditor;
#endif // UNITY_EDITOR

#pragma warning disable 649, 414 // Disable false warnings "never assigned to, and will always have it's default value null"

[AddComponentMenu("IMDraw/Examples/GameObjectDebugInfo"), DisallowMultipleComponent]
public class GameObjectDebugInfo : MonoBehaviour
{
	private static readonly Color DEFAULT_NAME_TAG_COLOR = Color.white; //new Color(0.5f, 1f, 1f, 1f);
	private static readonly Color DEFAULT_BOUNDS_COLOR = new Color(0.25f, 0.63f, 1f, 1f);
	private static readonly Color DEFAULT_COLLIDERS_COLOR = new Color(0.5f, 1f, 0.5f, 1.0f);

	private const int POSITION_ROUNDING_DECIMALS = 2;
	private const int ROTATION_ROUNDING_DECIMALS = 1;

	[Header("Labels")]

	[SerializeField]
	private bool m_ShowName;

	[SerializeField]
	private bool m_ShowTag;

	[SerializeField]
	private bool m_ShowPosition;

	[SerializeField]
	private bool m_ShowEulerRotation;

	[SerializeField]
	private Color m_NameColor = DEFAULT_NAME_TAG_COLOR;

	[SerializeField]
	private Color m_TagColor = DEFAULT_NAME_TAG_COLOR;

	[Header("Renderer bounds visualisation")]

	[SerializeField]
	private List<Renderer> m_Renderers;

	[SerializeField]
	private Color m_BoundsColor = DEFAULT_BOUNDS_COLOR;

	[SerializeField]
	private bool m_ShowRendererBounds;

	[Header("Collider visualiser")]

	[SerializeField]
	private List<Collider> m_Colliders;

	[SerializeField]
	private Color m_ColliderColor = DEFAULT_COLLIDERS_COLOR;

	[SerializeField]
	private bool m_ShowWireframeColliders;

	[SerializeField]
	private bool m_ShowSolidColliders;

	[SerializeField]
	private float m_ColliderScaleOffset;


	private static StringBuilder m_StringBuilder = new StringBuilder (64);

	private Transform m_Transform;

	private Vector3 m_GameObjectPosition;
	private Vector3 m_GameObjectEulerRotation;

	private string m_PositionString = "0, 0, 0";
	private string m_RotationString = string.Empty;

	void Start ()
	{
		m_Transform = transform;
    }

	void LateUpdate ()
	{
		Vector3 position = m_Transform.position;

		float labelX = 10f;
		float labelY = 0f;
		const float lineSpacing = 14f;

		IMDraw.LabelShadowed(position, 0f, labelY, m_NameColor, LabelPivot.MIDDLE_CENTER, LabelAlignment.CENTER, "■");

		if (m_ShowName)
		{
			// Note: Accessing the name on a gameobject generates garbage...
			IMDraw.LabelShadowed(position, labelX, labelY, m_NameColor, LabelPivot.MIDDLE_LEFT, LabelAlignment.LEFT, name);
			labelY += lineSpacing;
		}

		if (m_ShowTag)
		{
			// Note: Accessing the tag on a gameobject generates garbage...
			IMDraw.LabelShadowed(position, labelX, labelY, m_TagColor, LabelPivot.MIDDLE_LEFT, LabelAlignment.LEFT, tag);
			labelY += lineSpacing;
		}

		if (m_ShowPosition)
		{
			position.x = (float)Math.Round(position.x, POSITION_ROUNDING_DECIMALS);
			position.y = (float)Math.Round(position.y, POSITION_ROUNDING_DECIMALS);
			position.z = (float)Math.Round(position.z, POSITION_ROUNDING_DECIMALS);
            
			if (position != m_GameObjectPosition) // Only update the position string when the position has changed, to reduce GC pressure
			{
				m_GameObjectPosition = position;

				m_StringBuilder.Length = 0;
				m_StringBuilder.Append("xyz : ");
				m_StringBuilder.Append(position.x);
				m_StringBuilder.Append(", ");
				m_StringBuilder.Append(position.y);
				m_StringBuilder.Append(", ");
				m_StringBuilder.Append(position.z);
				m_PositionString = m_StringBuilder.ToString();
			}

			IMDraw.LabelShadowed(position, labelX, labelY, m_TagColor, LabelPivot.MIDDLE_LEFT, LabelAlignment.LEFT, m_PositionString);
			labelY += lineSpacing;
		}

		if (m_ShowEulerRotation)
		{
			Vector3 eulerRotation = m_Transform.rotation.eulerAngles;
			eulerRotation.x = (float)Math.Round(eulerRotation.x, ROTATION_ROUNDING_DECIMALS);
			eulerRotation.y = (float)Math.Round(eulerRotation.y, ROTATION_ROUNDING_DECIMALS);
			eulerRotation.z = (float)Math.Round(eulerRotation.z, ROTATION_ROUNDING_DECIMALS);

			if (eulerRotation != m_GameObjectEulerRotation)
			{
				m_GameObjectEulerRotation = eulerRotation;

				m_StringBuilder.Length = 0;
				m_StringBuilder.Append("deg : ");
				m_StringBuilder.Append(eulerRotation.x);
				m_StringBuilder.Append(", ");
				m_StringBuilder.Append(eulerRotation.y);
				m_StringBuilder.Append(", ");
				m_StringBuilder.Append(eulerRotation.z);
				m_RotationString = m_StringBuilder.ToString();
			}

			IMDraw.LabelShadowed(position, labelX, labelY, m_TagColor, LabelPivot.MIDDLE_LEFT, LabelAlignment.LEFT, m_RotationString);
			labelY += lineSpacing;
		}

		if (m_ShowRendererBounds && m_Renderers != null && m_Renderers.Count > 0)
		{
			for (int i = 0; i < m_Renderers.Count; ++i)
			{
				if (m_Renderers[i] != null)
				{
					IMDraw.Bounds(m_Renderers[i], m_BoundsColor);
				}
			}
		}

		if (m_ShowWireframeColliders && m_Colliders != null && m_Colliders.Count > 0)
		{
			for (int i = 0; i < m_Colliders.Count; ++i)
			{
				if (m_Colliders[i] != null)
				{
					IMDraw.Collider(m_Colliders[i], m_ColliderColor, m_ColliderScaleOffset, m_ShowSolidColliders);
				}
			}
		}
	}

	#if UNITY_EDITOR
    void OnDrawGizmosSelected ()
	{
		if (m_ShowRendererBounds && m_Renderers != null && m_Renderers.Count > 0)
		{
			Gizmos.color = m_BoundsColor;

			Bounds bounds;

			for (int i = 0; i < m_Renderers.Count; ++i)
			{
				if (m_Renderers[i] == null)
					continue;

				bounds = m_Renderers[i].bounds;

				if (bounds.size.sqrMagnitude > 0f)
				{
					Gizmos.DrawWireCube(bounds.center, bounds.extents * 2f);
				}
			}
		}
	}
	#endif // UNITY_EDITOR
}

#if UNITY_EDITOR // ======================================================================================================================================================================

[CustomEditor(typeof(GameObjectDebugInfo)), CanEditMultipleObjects]
public class GameObjectDebugInfoEditor : Editor
{
	SerializedProperty m_RenderersProperty;
	SerializedProperty m_CollidersProperty;

	void OnEnable()
	{
		m_RenderersProperty = serializedObject.FindProperty("m_Renderers");
		m_CollidersProperty = serializedObject.FindProperty("m_Colliders");
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		DrawDefaultInspector();

		GUILayout.Space(8f);

		if (GUILayout.Button("Auto-detect renderers", GUILayout.ExpandWidth(false)))
		{
			BuildComponentList<Renderer>(m_RenderersProperty);
		}

		if (GUILayout.Button("Auto-detect colliders", GUILayout.ExpandWidth(false)))
		{
			BuildComponentList<Collider>(m_CollidersProperty);
		}

		serializedObject.ApplyModifiedProperties();
	}

	private void BuildComponentList<T>(SerializedProperty listProperty) where T : UnityEngine.Object
	{
		GameObjectDebugInfo thisComponent = (GameObjectDebugInfo)target;

		#if UNITY_5_3_OR_NEWER
		Undo.RecordObject(thisComponent, "Auto-detect component");
		#endif // UNITY_5_3_OR_NEWER

		T [] components = thisComponent.GetComponentsInChildren<T>(true);

		SerializedProperty arrayElement;

		listProperty.ClearArray();

		for(int i = 0; i < components.Length; ++i)
		{
			listProperty.InsertArrayElementAtIndex(i);
			arrayElement = listProperty.GetArrayElementAtIndex(i);
			arrayElement.objectReferenceValue = components[i];
		}
		
		EditorUtility.SetDirty(thisComponent);
	}
}

#endif // UNITY_EDITOR