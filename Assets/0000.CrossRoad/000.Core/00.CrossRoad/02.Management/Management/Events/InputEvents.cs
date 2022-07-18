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
		/// <summary>
		/// �Է� �̺�Ʈ ������
		/// </summary>
		/// <param name="_mouse">���콺 �Է� �̺�Ʈ ������</param>
		/// <param name="_keyboard">Ű���� �Է� �̺�Ʈ ������</param>
		/// <param name="_touchpad">��ġ�е� �Է� �̺�Ʈ ������</param>
		public InputEvents(Mouse.Data _mouse, Keyboard.Data _keyboard, Touchpad.Data _touchpad)
		{
			this.clickEvent = new UnityEvent<InputEventType, int, Vector3>();
			this.dragEvent = new UnityEvent<InputEventType, int, Vector2>();
			this.focusEvent = new UnityEvent<InputEventType, Vector3, float>();
			this.hoverEvent = new UnityEvent<InputEventType, Vector3>();
			this.keyEvent = new UnityEvent<InputEventType, List<KeyData>>();

			MouseData = _mouse;
			KeyboardData = _keyboard;
			TouchpadData = _touchpad;
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
		/// Vector3			:: ȣ���� üũ ��ġ
		/// </summary>
		public UnityEvent<InputEventType, Vector3> hoverEvent;

		/// <summary>
		/// InputEventType	:: �̺�Ʈ ����
		/// List[KeyCode]	:: Ű ����Ʈ
		/// </summary>
		public UnityEvent<InputEventType, List<KeyData>> keyEvent;

		~InputEvents()
		{
			this.clickEvent = null;
			this.dragEvent = null;
			this.focusEvent = null;
			this.hoverEvent = null;
			this.keyEvent = null;
		}
	}
}
