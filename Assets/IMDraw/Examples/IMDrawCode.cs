using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test
{
	public class IMDrawCode : MonoBehaviour
	{
		private static readonly Color DEFAULT_NAME_TAG_COLOR = Color.white; //new Color(0.5f, 1f, 1f, 1f);
		private static readonly Color DEFAULT_BOUNDS_COLOR = new Color(0.25f, 0.63f, 1f, 1f);
		private static readonly Color DEFAULT_COLLIDERS_COLOR = new Color(0.5f, 1f, 0.5f, 1.0f);

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



		private Transform m_Transform;

		public List<Collider> Colliders { get => m_Colliders; set => m_Colliders=value; }
		public List<Renderer> Renderers { get => m_Renderers; set => m_Renderers=value; }
		public bool ShowWireframeColliders { get => m_ShowWireframeColliders; set => m_ShowWireframeColliders=value; }

		// Start is called before the first frame update
		void Start()
		{
			m_Transform = transform;
		}

		private void LateUpdate()
		{
			//Vector3 position = m_Transform.position;

			if (m_ShowRendererBounds && Renderers != null && Renderers.Count > 0)
			{
				for (int i = 0; i < Renderers.Count; ++i)
				{
					if (Renderers[i] != null)
					{
						IMDraw.Bounds(Renderers[i], m_BoundsColor);
					}
				}
			}

			if (ShowWireframeColliders && Colliders != null && Colliders.Count > 0)
			{
				for (int i = 0; i < Colliders.Count; ++i)
				{
					if (Colliders[i] != null)
					{
						IMDraw.Collider(Colliders[i], m_ColliderColor, m_ColliderScaleOffset, m_ShowSolidColliders);
					}
				}
			}
		}
	}
}
