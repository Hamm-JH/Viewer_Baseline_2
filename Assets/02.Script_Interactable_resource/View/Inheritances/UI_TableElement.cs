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
			// 버튼이 존재한다면, 버튼의 클릭 이벤트를 Conditional Branch에 연결한다.
			if (gameObject.TryGetComponent<Button>(out m_btn))
			{
				m_btn.onClick.AddListener(new UnityAction(OnSelect));
			}

			// 슬라이더가 존재한다면, 슬라이더의 슬라이딩 이벤트를 Conditional Branch에 연결한다.
			if (gameObject.TryGetComponent<Slider>(out m_slider))
			{
				m_slider.onValueChanged.AddListener(new UnityAction<float>(OnChangeValue));
			}
		}

		/// <summary>
		/// 슬라이더 변경시 실행
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
		/// 버튼 클릭시 실행
		/// </summary>
		public override void OnSelect()
		{
			throw new System.NotImplementedException();
		}
	}
}
