using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management.Events
{
	using Definition;
	using View;

	public class EventData
	{
		private IInteractable m_element;
		private InputEventType m_eventType;

		/// <summary>
		/// ��ȣ�ۿ� ������ ���
		/// </summary>
		public IInteractable Element { get => m_element; set => m_element=value; }

		/// <summary>
		/// �߻��� �̺�Ʈ�� ����
		/// </summary>
		public InputEventType EventType { get => m_eventType; set => m_eventType=value; }

		public EventData(IInteractable _target, InputEventType _mainEventType)
		{
			Element = _target;
			EventType = _mainEventType;
		}


		public static bool IsEqual(EventData A, EventData B)
		{
			if(A.Element.Target == B.Element.Target)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}
