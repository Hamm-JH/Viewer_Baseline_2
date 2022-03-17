using System.Collections;
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
		/// 개별 UI 요소에서 받은 이벤트를 UI 패널 레벨에서 분배처리
		/// </summary>
		/// <param name="_uType"></param>
		/// <param name="_setter"></param>
		
		public abstract void GetUIEvent(UIEventType _uType, UI_Selectable _setter);
	}
}
