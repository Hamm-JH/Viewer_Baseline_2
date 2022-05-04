using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test
{
	using Items;
	using System.Linq;
	using View;

	public class Test_ManageLocationGuide : MonoBehaviour
	{
		[SerializeField] Test_LocationGuide m_tester;

		// Update is called once per frame
		void Update()
		{
			if(Input.GetKeyDown(KeyCode.Keypad1))
			{
				m_tester.SetMode(Definition.UIEventType.Viewport_ViewMode_TOP);
			}
			if (Input.GetKeyDown(KeyCode.Keypad2))
			{
				m_tester.SetMode(Definition.UIEventType.Viewport_ViewMode_BOTTOM);
			}
			if (Input.GetKeyDown(KeyCode.Keypad3))
			{
				m_tester.SetMode(Definition.UIEventType.Viewport_ViewMode_SIDE_FRONT);
			}
			if (Input.GetKeyDown(KeyCode.Keypad4))
			{
				m_tester.SetMode(Definition.UIEventType.Viewport_ViewMode_SIDE_BACK);
			}
			if (Input.GetKeyDown(KeyCode.Keypad5))
			{
				m_tester.SetMode(Definition.UIEventType.Viewport_ViewMode_SIDE_LEFT);
			}
			if (Input.GetKeyDown(KeyCode.Keypad6))
			{
				m_tester.SetMode(Definition.UIEventType.Viewport_ViewMode_SIDE_RIGHT);
			}

			if(Input.GetMouseButtonDown(0))
			{
				//RaycastHit hit;
				List<RaycastHit> hits;
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

				hits = Physics.RaycastAll(ray).ToList();
				if(hits.Count != 0)
				{

					LocationElement element = null;
					Obj_Selectable selectable = null;
					hits.Find(x => x.collider.gameObject.TryGetComponent<LocationElement>(out element));
					hits.Find(x => x.collider.gameObject.TryGetComponent<Obj_Selectable>(out selectable));
					
					//element = hits.Find(x => x.collider.gameObject.TryGetComponent<LocationElement>(out element)).collider.GetComponent<LocationElement>();
					//selectable = hits.Find(x => x.collider.TryGetComponent<Obj_Selectable>(out selectable)).collider.GetComponent<Obj_Selectable>();

					if(element && selectable)
					{
						Debug.Log($"element index : {element.Index + 1}, selectable : {selectable.name}");
						//Debug.Log($"element : {element.name}, selectable : {selectable.name}");
					}

				}
				//if(Physics.Raycast(ray, out hit))
				//{
				//	Debug.Log(hit.collider.name);
				//}
			}
		}
	}
}
