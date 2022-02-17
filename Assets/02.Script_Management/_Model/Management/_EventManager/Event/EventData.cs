using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management.Events
{
	using Definition;
	using View;

	/// <summary>
	/// ���� �����Ϳ� �����Ϳ� �����ϱ� ���� �޼��� ���� ����
	/// </summary>
	public abstract class EventData
	{
		protected IInteractable m_element;
		protected InputEventType m_eventType;
		private Status status;

		/// <summary>
		/// ��ȣ�ۿ� ������ ���
		/// </summary>
		public IInteractable Element { get => m_element; set => m_element=value; }

		/// <summary>
		/// �߻��� �̺�Ʈ�� ����
		/// </summary>
		public InputEventType EventType { get => m_eventType; set => m_eventType=value; }

		/// <summary>
		/// �̺�Ʈ ó�� ���
		/// </summary>
		public Status StatusCode { get => status; set => status=value; }

		//public EventData() { }

		//public EventData(IInteractable _target, InputEventType _mainEventType)
		//{
		//	Element = _target;
		//	EventType = _mainEventType;
		//}

		/// <summary>
		/// �̺�Ʈ ó�� �޼���
		/// </summary>
		public abstract void OnProcess(GameObject _cObj);

		/// <summary>
		/// �̺�Ʈ ����ó�� �Ϸ��� �ܺ� �̺�Ʈ �߻�ó��
		/// </summary>
		public abstract void DoEvent();
		
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
