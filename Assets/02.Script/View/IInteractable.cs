﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace View
{
	/// <summary>
	/// 상호작용 가능한 객체들이 상속받아야할 인터페이스
	/// </summary>
	public interface IInteractable
	{
		/// <summary>
		/// 인스턴스 선택
		/// </summary>
		void OnSelect();


		/// <summary>
		/// 인스턴스 선택해제
		/// </summary>
		void OnDeselect();

	}
}
