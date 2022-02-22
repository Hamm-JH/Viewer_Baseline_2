using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management.Events
{
	using Definition;
	using UnityEngine.EventSystems;
	using View;

	/// <summary>
	/// ���� �����Ϳ� �����Ϳ� �����ϱ� ���� �޼��� ���� ����
	/// </summary>
	[System.Serializable]
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

		[Header("Private")]
		protected GameObject m_selected3D = null;
		protected RaycastHit m_hit = default(RaycastHit);
		protected List<RaycastResult> m_results = new List<RaycastResult>();

		public GameObject Selected3D { get => m_selected3D; set => m_selected3D=value; }
		public RaycastHit Hit { get => m_hit; set => m_hit=value; }
		public List<RaycastResult> Results { get => m_results; set => m_results=value; }



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
