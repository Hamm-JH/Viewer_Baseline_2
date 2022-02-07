using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management.Events
{
	using Control._Input;
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
			this.clickEvent = new UnityEvent<Vector3>();
			this.dragEvent = new UnityEvent<int, Vector2>();
			this.focusEvent = new UnityEvent<Vector3, float>();
			this.keyEvent = new UnityEvent<List<KeyCode>>();

			MouseData = _mouse;
			KeyboardData = _keyboard;
		}

		/// <summary>
		/// Main // 클릭 이벤트
		/// Vec3 :: 마우스 클릭 위치
		/// // GameObject :: 클릭된 대상
		/// // Definition.ObjectType :: 객체유형 (Object, UI)
		/// </summary>
		public UnityEvent<Vector3> clickEvent;
		//public UnityEvent<Definition.ObjectType, GameObject> clickEvent;

		/// <summary>
		/// Main // 드래그 이벤트
		/// int :: 마우스 버튼
		/// Vec2 :: 드래그 정도
		/// </summary>
		public UnityEvent<int, Vector2> dragEvent;

		/// <summary>
		/// Main // 포커스 이벤트
		/// Vec3 :: 포커스 위치
		/// float :: 포커스 정도
		/// </summary>
		public UnityEvent<Vector3, float> focusEvent;

		/// <summary>
		/// Main // 키 입력 이벤트
		/// </summary>
		public UnityEvent<List<KeyCode>> keyEvent;

		~InputEvents()
		{
			this.clickEvent = null;
			this.dragEvent = null;
			this.focusEvent = null;
			this.keyEvent = null;
		}
	}
}
