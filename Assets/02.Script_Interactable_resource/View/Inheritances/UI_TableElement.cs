using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View
{
	using AdminViewer.UI;
	using UnityEngine.Events;
	using UnityEngine.UI;

	public class UI_TableElement : Interactable
	{
		public Ad_TableElement tRootElement;

		Button m_btn;
		Slider m_slider;

		public override GameObject Target => gameObject;

		public override List<GameObject> Targets => throw new System.NotImplementedException();

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

		/// <summary>
		/// �����̴� ����� ����
		/// </summary>
		/// <param name="_value"></param>
		public override void OnChangeValue(float _value)
		{
			throw new System.NotImplementedException();
		}

		public override void OnDeselect()
		{
			throw new System.NotImplementedException();
		}

		public override void OnDeselect<T1, T2>(T1 t1, T2 t2)
		{
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// ��ư Ŭ���� ����
		/// </summary>
		public override void OnSelect()
		{
			throw new System.NotImplementedException();
		}
	}
}
