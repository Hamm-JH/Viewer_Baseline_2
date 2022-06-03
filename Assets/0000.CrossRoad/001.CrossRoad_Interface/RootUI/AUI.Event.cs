using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.UI
{
	using Definition;
    using System;
    using View;

    public abstract partial class AUI : AModule
    {
		
		/// <summary>
		/// 버튼 이벤트 분배
		/// </summary>
		/// <typeparam name="T"> 이벤트 타입 </typeparam>
		/// <param name="_type"> 이벤트 </param>
		/// <param name="_setter"> 상호작용 가능 개체 </param>
		public virtual void GetUIEvent<T>(T _type, Interactable _setter)
		{
			switch(typeof(T))
            {
				case Type type when type == typeof(UIEventType):
					GetUIEvent((UIEventType)(object)_type, _setter);
					break;

				case Type type when type == typeof(Inspect_EventType):
					GetUIEvent((Inspect_EventType)(object)_type, _setter);
					break;

				case Type type when type == typeof(BottomBar_EventType):
					GetUIEvent((BottomBar_EventType)(object)_type, _setter);
					break;
            }
		}

		/// <summary>
		/// 슬라이더 이벤트 분배
		/// </summary>
		/// <typeparam name="T"> 이벤트 타입 </typeparam>
		/// <param name="_value"> 실수 값 </param>
		/// <param name="_type"> 이벤트 </param>
		/// <param name="_setter"> 상호작용 가능 개체 </param>
		public virtual void GetUIEvent<T>(float _value, T _type, Interactable _setter)
		{
			switch(typeof(T))
            {
				case Type type when type == typeof(UIEventType):
					GetUIEvent(_value, (UIEventType)(object)_type, _setter);
					break;

				case Type type when type == typeof(Inspect_EventType):
					GetUIEvent(_value, (Inspect_EventType)(object)_type, _setter);
					break;

				case Type type when type == typeof(BottomBar_EventType):
					GetUIEvent(_value, (BottomBar_EventType)(object)_type, _setter);
					break;

			}
		}

		/// <summary>
		/// 개별 UI 요소에서 받은 이벤트를 UI 패널 레벨에서 분배처리
		/// </summary>
		/// <param name="_uType"></param>
		/// <param name="_setter"></param>
		public abstract void GetUIEvent(UIEventType _uType, Interactable _setter);
		public abstract void GetUIEvent(Inspect_EventType _uType, Interactable _setter);
		public virtual void GetUIEvent(BottomBar_EventType _type, Interactable _setter) { }


		/// <summary>
		/// 슬라이더 이벤트 분배
		/// </summary>
		/// <param name="_value"></param>
		/// <param name="_uType"></param>
		/// <param name="_setter"></param>
		public abstract void GetUIEvent(float _value, UIEventType _uType, Interactable _setter);

		public abstract void GetUIEvent(float _value, Inspect_EventType _uType, Interactable _setter);
		public virtual void GetUIEvent(float _value, BottomBar_EventType _type, Interactable _setter) { }

		/// <summary>
		/// 전달변수가 V인 이벤트 발생
		/// </summary>
		/// <typeparam name="T"> 이벤트 타입</typeparam>
		/// <typeparam name="V"> 전달변수 타입 </typeparam>
		/// <param name="_type"> 이벤트 </param>
		/// <param name="_value"> 전달변수 </param>
		/// <param name="_setter"> 이벤트 실행자 </param>
		public virtual void GetUIEvent<T, V>(T _type, V _value, Interactable _setter) { }

		/// <summary>
		/// 전달변수만 존재하는 이벤트 발생
		/// </summary>
		/// <typeparam name="T"> 이벤트 타입 </typeparam>
		/// <typeparam name="V"> 전달변수 타입 </typeparam>
		/// <param name="_type"> 이벤트 </param>
		/// <param name="_value"> 전달변수 </param>
		public virtual void GetUIEvent<T, V>(T _type, V _value) { }

		/// <summary>
		/// 이벤트 타입만 존재하는 이벤트 발생
		/// </summary>
		/// <typeparam name="T"> 이벤트 타입 </typeparam>
		/// <param name="_type"> 이벤트 </param>
		public virtual void GetUIEvent<T>(T _type) { }
	}
}
