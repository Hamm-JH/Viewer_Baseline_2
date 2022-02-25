using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management.Events
{
	using Definition;
	using View;

	public class EventData_UI : EventData
	{
		private List<GameObject> m_modelObj;

		public EventData_UI(InputEventType _eventType,
			UIEventType _uiEvent, ToggleType _toggle, List<GameObject> _modelObj)
		{
			EventType = _eventType;
			UiEventType = _uiEvent;
			ToggleType = _toggle;
			m_modelObj = _modelObj;
		}

		public override void DoEvent()
		{
			
		}

		public override void DoEvent(List<GameObject> _objs)
		{
			switch(UiEventType)
			{
				case UIEventType.Mode_Hide:
				case UIEventType.Mode_Isolate:
					{
						bool isHide = false;
						if (UiEventType == UIEventType.Mode_Hide) isHide = true;
						
						foreach(GameObject obj in m_modelObj)
						{
							// ¸ðµ¨ ¸®½ºÆ® Áß¿¡¼­ ÇöÀç ¼±ÅÃµÈ ¸ðµ¨¸®½ºÆ®ÀÇ ¿ä¼Ò¿Í °°Àº ¿ä¼Ò°¡ Á¸ÀçÇÏ´Â°¡?
							bool isSelectedModel = _objs.Find(x => x == obj) == null ? false : true;

							float alpha = 0.1f;
							bool thisHide = false;
							// ÀÌ °´Ã¼°¡ ¸ÂÀ½, ¼û°Ü¾ßµÊ			true true -> alpha = 0.1
							// ÀÌ °´Ã¼°¡ ¸ÂÀ½, Á¦¿Ü ¼û°Ü¾ßµÊ	true false -> alpha = 1
							// ÀÌ °´Ã¼ ¾Æ´Ô, ¼û°Ü¾ßµÊ			false true -> alpha = 1
							// ÀÌ °´Ã¼ ¾Æ´Ô, Á¦¿Ü ¼û°Ü¾ßµÊ		false false -> alpha = 0.1

							if(isSelectedModel)
							{
								alpha = isHide ? 0.1f : 1f;
								thisHide = isHide ? true : false;
							}
							else
							{
								alpha = isHide ? 1f : 0.1f;
								thisHide = isHide ? false : true;
							}

							Obj_Selectable selectable;
							if (obj.TryGetComponent<Obj_Selectable>(out selectable))
							{
								selectable.OnDeselect<UIEventType, bool>(UiEventType, thisHide);
							}
						}
					}
					break;
			}
		}

		public override void OnProcess(GameObject _cObj)
		{
			if (EventType != InputEventType.UI_Invoke)
			{
				StatusCode = Status.Skip;
				return;
			}

			switch (UiEventType)
			{
				case UIEventType.Mode_Hide:
				case UIEventType.Mode_Isolate:
					StatusCode = Status.Update;
					break;
			}
		}
	}
}
