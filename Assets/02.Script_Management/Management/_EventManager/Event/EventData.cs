using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management.Events
{
	using Definition;
	using System.Linq;
	using UnityEngine.EventSystems;
	using View;

	/// <summary>
	/// ���� �����Ϳ� �����Ϳ� �����ϱ� ���� �޼��� ���� ����
	/// </summary>
	[System.Serializable]
	public abstract class EventData
	{
		//protected IInteractable m_element;
		protected List<IInteractable> m_elements;
		protected InputEventType m_eventType;
		private Status status;

		/// <summary>
		/// ��ȣ�ۿ� ������ ���
		/// </summary>
		//public IInteractable Element { get => m_element; set => m_element=value; }
		public List<IInteractable> Elements { get => m_elements; set => m_elements=value; }

		public List<GameObject> objects;

		/// <summary>
		/// �߻��� �̺�Ʈ�� ����
		/// </summary>
		public InputEventType EventType { get => m_eventType; set => m_eventType=value; }

		/// <summary>
		/// �̺�Ʈ ó�� ���
		/// </summary>
		public Status StatusCode { get => status; set => status=value; }

		//----------------------------------------------------------------------------------------------

		[Header("Private 3D")]
		protected GameObject m_selected3D = null;
		protected RaycastHit m_hit = default(RaycastHit);
		protected List<RaycastResult> m_results = new List<RaycastResult>();
		// ���콺 ��ư ��ȣ
		protected int m_btn;

		public GameObject Selected3D { get => m_selected3D; set => m_selected3D=value; }
		public RaycastHit Hit { get => m_hit; set => m_hit=value; }
		public List<RaycastResult> Results { get => m_results; set => m_results=value; }
		public int BtnIndex { get => m_btn; set => m_btn=value; }

		//----------------------------------------------------------------------------------------------

		[Header("Private UI")]
		protected UIEventType m_uiEventType;
		protected ToggleType m_toggleType;

		public UIEventType UiEventType { get => m_uiEventType; set => m_uiEventType=value; }
		public ToggleType ToggleType { get => m_toggleType; set => m_toggleType=value; }
		


		/// <summary>
		/// �̺�Ʈ ó�� �޼���
		/// </summary>
		public abstract void OnProcess(GameObject _cObj);


		/// <summary>
		/// �̺�Ʈ ����ó�� �Ϸ��� �ܺ� �̺�Ʈ �߻�ó��
		/// </summary>
		public abstract void DoEvent();
		public abstract void DoEvent(List<GameObject> _objs);

		public abstract void DoEvent(Dictionary<InputEventType, EventData> _sEvents);
		
		public static bool IsEqual(EventData A, EventData B)
		{
			if(A.Elements.Last().Target == B.Elements.Last().Target)
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
