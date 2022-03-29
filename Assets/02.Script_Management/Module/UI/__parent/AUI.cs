﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.UI
{
	using Definition;
	using View;

	[System.Serializable]
	public abstract class AUI : AModule
	{
		public new void OnCreate(ModuleID _id, FunctionCode _code)
		{
			base.OnCreate(_id, _code);
		}

		/// <summary>
		/// TODO AModule 단으로 올리기
		/// 컨텐츠 관리 단계에서 모든 초기화가 끝날시 실행
		/// </summary>
		public abstract void OnModuleComplete();

		/// <summary>
		/// 터널 모델에서 선택된 객체의 정보를 설정한다.
		/// </summary>
		/// <param name="selected"></param>
		public abstract void SetObjectData_Tunnel(GameObject selected);

		/// <summary>
		/// 패널 리스트 토글
		/// </summary>
		/// <param name="index"></param>
		public abstract void TogglePanelList(int _index, GameObject _exclusive);

		/// <summary>
		/// ModuleStatus 기반 이벤트 재시작
		/// </summary>
		public abstract void ReInvokeEvent();

		/// <summary>
		/// 개별 UI 요소에서 받은 이벤트를 UI 패널 레벨에서 분배처리
		/// </summary>
		/// <param name="_uType"></param>
		/// <param name="_setter"></param>
		
		public abstract void GetUIEvent(UIEventType _uType, UI_Selectable _setter);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="_value"></param>
		/// <param name="_uType"></param>
		/// <param name="_setter"></param>
		public abstract void GetUIEvent(float _value, UIEventType _uType, UI_Selectable _setter);
	}
}
