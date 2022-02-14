using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management.Events
{
	using Definition;
	using Platform.Feature._Input;
	using UnityEngine.Events;

	/// <summary>
	/// �� �����ڿ��� �����ϴ� �Է� �̺�Ʈ ����
	/// �Է��� �߻����� ó���Ѵ�.
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
		/// InputEventType	:: �̺�Ʈ ����
		/// int				:: ��ư(���콺) ��ȣ
		/// Vector3			:: Ŭ��(��ġ) ��ġ
		/// </summary>
		public UnityEvent<InputEventType, int, Vector3> clickEvent;

		/// <summary>
		/// InputEventType	:: �̺�Ʈ ����
		/// int				:: ��ư(���콺) ��ȣ
		/// Vector2			:: �巡�� ����
		/// </summary>
		public UnityEvent<InputEventType, int, Vector2> dragEvent;

		/// <summary>
		/// InputEventType	:: �̺�Ʈ ����
		/// Vector3			:: ��Ŀ�� ��ġ
		/// float			:: ��Ŀ�� ����
		/// </summary>
		public UnityEvent<InputEventType, Vector3, float> focusEvent;

		/// <summary>
		/// InputEventType	:: �̺�Ʈ ����
		/// List[KeyCode]	:: Ű ����Ʈ
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
