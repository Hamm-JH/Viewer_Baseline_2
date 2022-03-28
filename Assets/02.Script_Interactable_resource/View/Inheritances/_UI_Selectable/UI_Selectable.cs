using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace View
{
	using Definition;
	using Management;
	using Module.UI;
	using UnityEngine.EventSystems;
	using UnityEngine.UI;

	public partial class UI_Selectable : Interactable,
		IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
	{
		public override GameObject Target
		{
			get => gameObject;
		}

		public override List<GameObject> Targets => throw new System.NotImplementedException();

		public GameObject ChildPanel { get => childPanel; set => childPanel=value; }

		Button m_btn;
		Slider m_slider;

		[SerializeField] AUI m_rootUI;
		[SerializeField] UIEventType eventType;
		[SerializeField] UniqueUIEventType m_unique;

		[Header("Linked UI Elements")]
		/// <summary>
		/// �ڽ� �г� ��ü
		/// </summary>
		[SerializeField] GameObject childPanel;

		/// <summary>
		/// ui ȿ�� ���
		/// </summary>
		[SerializeField] List<GameObject> m_uiFXs;

		/// <summary>
		/// ui ȣ���� ���� ��ۿ��
		/// </summary>
		[SerializeField] List<GameObject> m_uiHoverElements;

		/// <summary>
		/// ǥ�� ��ư�� �����ϴ� ����
		/// </summary>
		[Header("Test Values")]
		public string t_surfaceCode;

		private void Start()
		{
			// ��ư�� �����Ѵٸ�, ��ư�� Ŭ�� �̺�Ʈ�� Conditional Branch�� �����Ѵ�.
			if (gameObject.TryGetComponent<Button>(out m_btn))
			{
				m_btn.onClick.AddListener(new UnityAction(OnSelect));
			}

			// �����̴��� �����Ѵٸ�, �����̴��� �����̵� �̺�Ʈ�� Conditional Branch�� �����Ѵ�.
			if (gameObject.TryGetComponent<Slider>(out m_slider))
			{
				m_slider.onValueChanged.AddListener(new UnityAction<float>(OnChangeValue));
			}
			//m_btn.
		}

		public override void OnDeselect()
		{
			Debug.Log($"OnDeselect : {this.name}");
		}

		public override void OnSelect()
		{
			//Debug.Log($"OnSelect : {this.name}");

			ConditionalBranch(eventType);
		}

		public override void OnDeselect<T1, T2>(T1 t1, T2 t2)
		{

		}

		/// <summary>
		/// �����̴� �� ����� ����
		/// </summary>
		/// <param name="_value"></param>
		public override void OnChangeValue(float _value)
		{
			ConditionalBranch(_value, eventType);
		}

		#region Conditional Branch

		/// <summary>
		/// Slider Branch
		/// </summary>
		/// <param name="_value"></param>
		/// <param name="_eventType"></param>
		private void ConditionalBranch(float _value, UIEventType _eventType)
		{
			m_rootUI.GetUIEvent(_value, _eventType, this);
		}

		/// <summary>
		/// Button Branch
		/// </summary>
		/// <param name="_eventType"></param>
		private void ConditionalBranch(UIEventType _eventType)
		{
			PlatformCode pCode = MainManager.Instance.Platform;

			//bool isDemoAdminViewer = false;
			//if (Platforms.IsDemoAdminViewer(pCode)) isDemoAdminViewer = true;

			m_rootUI.GetUIEvent(_eventType, this);

			//// ����� �����ں�� ����϶�
			//if (isDemoAdminViewer)
			//{
			//	// ��Ʈ UI���� �̺�Ʈ �й�
			//	m_rootUI.GetUIEvent(_eventType, this);
			//}
			//// ��� ����, �ͳο� �̺�Ʈ �ڷ� �α�
			//else
			//{
			//	m_rootUI.GetUIEvent(_eventType, this);
			//}
		}

		#endregion

		#region Enable Disable

		private void OnEnable()
		{
			m_uiHoverElements.ForEach(x => x.SetActive(false));

			//if (m_unique == UniqueUIEventType.SetChild_Highlight)
			//{
			//	((Image)m_btn.targetGraphic).sprite = m_btn.spriteState.disabledSprite;
			//}
		}

		private void OnDisable()
		{
			m_uiHoverElements.ForEach(x => x.SetActive(false));

			//if (m_unique == UniqueUIEventType.SetChild_Highlight)
			//{
			//	((Image)m_btn.targetGraphic).sprite = m_btn.spriteState.disabledSprite;
			//}
		}

		#endregion

		#region Mouse region

		/// <summary>
		/// UI ����� Ŭ���� �ߵ�?
		/// </summary>
		/// <param name="eventData"></param>
		public void OnPointerUp(PointerEventData eventData)
		{
			m_uiHoverElements.ForEach(x => x.SetActive(false));
		}

		/// <summary>
		/// ���콺 ������ ���Խ� �ߵ�
		/// </summary>
		/// <param name="eventData"></param>
		public void OnPointerEnter(PointerEventData eventData)
		{

			m_uiHoverElements.ForEach(x => x.SetActive(true));
		}

		/// <summary>
		/// ���콺 ������ ������ �ߵ�
		/// </summary>
		/// <param name="eventData"></param>
		public void OnPointerExit(PointerEventData eventData)
		{
			m_uiHoverElements.ForEach(x => x.SetActive(false));
		}

		#endregion
	}
}
