using Data.API;
using Definition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View;

namespace Module.UI
{
	public class UITemplate : AUI
	{
		public override void OnStart()
		{
			Debug.LogWarning("UITemplate OnStart");
		}

		public override void OnModuleComplete()
		{
			Debug.LogError("load complete");
		}

		/// <summary>
		/// 이벤트 재실행
		/// </summary>
		/// <exception cref="System.NotImplementedException"></exception>
		public override void ReInvokeEvent()
		{
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// UIEvent 가져오기
		/// </summary>
		/// <param name="_uType">이벤트 분류</param>
		/// <param name="_setter">UISelectable 처리</param>
		/// <exception cref="System.NotImplementedException"></exception>
		public override void GetUIEvent(UIEventType _uType, Interactable _setter)
		{
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// UIEvent 가져오기
		/// </summary>
		/// <param name="_uType">이벤트 분류</param>
		/// <param name="_setter">Interactable 객체</param>
		/// <exception cref="System.NotImplementedException"></exception>
        public override void GetUIEvent(Inspect_EventType _uType, Interactable _setter)
        {
            throw new System.NotImplementedException();
        }

		/// <summary>
		/// UIEvent 가져오기
		/// </summary>
		/// <param name="_value"></param>
		/// <param name="_uType"></param>
		/// <param name="_setter"></param>
		/// <exception cref="System.NotImplementedException"></exception>
        public override void GetUIEvent(float _value, UIEventType _uType, Interactable _setter)
		{
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// UIEvent 가져오기
		/// </summary>
		/// <param name="_value"></param>
		/// <param name="_uType"></param>
		/// <param name="_setter"></param>
		/// <exception cref="System.NotImplementedException"></exception>
        public override void GetUIEvent(float _value, Inspect_EventType _uType, Interactable _setter)
        {
            throw new System.NotImplementedException();
        }

		/// <summary>
		/// 터널 객체정보 할당
		/// </summary>
		/// <param name="selected">선택된 객체</param>
        public override void SetObjectData_Tunnel(GameObject selected)
		{
			
		}

		/// <summary>
		/// 패널 리스트 토글
		/// </summary>
		/// <param name="_index"></param>
		/// <param name="_exclusive"></param>
		/// <exception cref="System.NotImplementedException"></exception>
		public override void TogglePanelList(int _index, GameObject _exclusive)
		{
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// 서버에 주소정보 요청
		/// </summary>
		/// <param name="_data"></param>
		/// <exception cref="System.NotImplementedException"></exception>
        public override void API_GetAddress(AAPI _data)
        {
            throw new System.NotImplementedException();
        }
    }
}
