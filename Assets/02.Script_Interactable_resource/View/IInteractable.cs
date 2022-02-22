using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace View
{
	using Definition;

	/// <summary>
	/// 상호작용 가능한 객체들이 상속받아야할 인터페이스
	/// </summary>
	public interface IInteractable
	{
		/// <summary>
		/// 선택된 객체
		/// </summary>
		GameObject Target { get; }

		/// <summary>
		/// 클릭, 터치 지점
		/// </summary>
		RaycastHit Hit { get; set; }

		/// <summary>
		/// 인스턴스 선택
		/// </summary>
		void OnSelect();


		/// <summary>
		/// 인스턴스 선택해제
		/// </summary>
		void OnDeselect();

		/// <summary>
		/// 인스턴스 값 변경
		/// :: 슬라이더
		/// </summary>
		/// <param name="_value"></param>
		void OnChangeValue(float _value);
	}
}
