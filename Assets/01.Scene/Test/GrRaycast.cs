using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test
{
	using UnityEngine.EventSystems;
	using UnityEngine.UI;

	public class GrRaycast : MonoBehaviour
	{
		public Camera m_camera;
		public GraphicRaycaster m_graphicRaycaster;

		[SerializeField] protected GameObject m_selected3D = null;
		[SerializeField] protected RaycastHit m_hit = default(RaycastHit);
		[SerializeField] protected List<RaycastResult> m_results = new List<RaycastResult>();

		// Start is called before the first frame update
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{
			if(Input.GetMouseButtonDown(0))
			{
				PointerEventData eData = new PointerEventData(EventSystem.current);
				eData.position = Input.mousePosition;

				m_results = new List<RaycastResult>();
				m_graphicRaycaster.Raycast(eData, m_results);

				Debug.Log(m_results.Count);
				m_results.ForEach(x => Debug.Log(x.gameObject.name));
			}
		}
	}
}
