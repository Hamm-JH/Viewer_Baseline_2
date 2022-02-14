using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management.Events
{
	using Definition;
	using Platform.Feature._Input;
	using UnityEngine.Events;

	/// <summary>
	/// 주 관리자에서 관리하는 입력 이벤트 모음
	/// 입력의 발생만을 처리한다.
	/// </summary>
	[System.Serializable]
	public class InputEvents : IEvents
	{
		public InputEvents(Mouse.Data _mouse, Keyboard.Data _keyboard)
		{
			this.clickEvent = new UnityEvent<InputEventType, int, Vector3>();
			this.dragEvent = new UnityEvent<InputEventType, int, Vector2>();
			this.focusEvent = new UnityEvent<InputEventType, Vector3, float>();
			this.keyEvent = new UnityEvent<InputEventType, List<KeyCode>>();

			MouseData = _mouse;
			KeyboardData = _keyboard;
		}

		/// <summary>
		/// InputEventType	:: 이벤트 형식
		/// int				:: 버튼(마우스) 번호
		/// Vector3			:: 클릭(터치) 위치
		/// </summary>
		public UnityEvent<InputEventType, int, Vector3> clickEvent;

		/// <summary>
		/// InputEventType	:: 이벤트 형식
		/// int				:: 버튼(마우스) 번호
		/// Vector2			:: 드래그 정도
		/// </summary>
		public UnityEvent<InputEventType, int, Vector2> dragEvent;

		/// <summary>
		/// InputEventType	:: 이벤트 형식
		/// Vector3			:: 포커스 위치
		/// float			:: 포커스 정도
		/// </summary>
		public UnityEvent<InputEventType, Vector3, float> focusEvent;

		/// <summary>
		/// InputEventType	:: 이벤트 형식
		/// List[KeyCode]	:: 키 리스트
		/// </summary>
		public UnityEvent<InputEventType, List<KeyCode>> keyEvent;

		~InputEvents()
		{
			this.clickEvent = null;
			this.dragEvent = null;
			this.focusEvent = null;
			this.keyEvent = null;
		}
	}
}
