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
		}

		public override void OnDeselect()
		{
			Debug.Log($"OnDeselect : {this.name}");
		}

		public override void OnSelect()
		{
			m_rootUI.GetUIEvent(eventType, this);
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
			m_rootUI.GetUIEvent(_value, eventType, this);
		}

		#region Enable Disable

		private void OnEnable()
		{
			m_uiHoverElements.ForEach(x => x.SetActive(false));
		}

		private void OnDisable()
		{
			m_uiHoverElements.ForEach(x => x.SetActive(false));
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
