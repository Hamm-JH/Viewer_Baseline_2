using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management.Events
{
	using Control._Input;
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
			this.clickEvent = new UnityEvent<Vector3>();
			this.dragEvent = new UnityEvent<int, Vector2>();
			this.focusEvent = new UnityEvent<Vector3, float>();
			this.keyEvent = new UnityEvent<List<KeyCode>>();

			MouseData = _mouse;
			KeyboardData = _keyboard;
		}

		/// <summary>
		/// Main // Ŭ�� �̺�Ʈ
		/// Vec3 :: ���콺 Ŭ�� ��ġ
		/// // GameObject :: Ŭ���� ���
		/// // Definition.ObjectType :: ��ü���� (Object, UI)
		/// </summary>
		public UnityEvent<Vector3> clickEvent;
		//public UnityEvent<Definition.ObjectType, GameObject> clickEvent;

		/// <summary>
		/// Main // �巡�� �̺�Ʈ
		/// int :: ���콺 ��ư
		/// Vec2 :: �巡�� ����
		/// </summary>
		public UnityEvent<int, Vector2> dragEvent;

		/// <summary>
		/// Main // ��Ŀ�� �̺�Ʈ
		/// Vec3 :: ��Ŀ�� ��ġ
		/// float :: ��Ŀ�� ����
		/// </summary>
		public UnityEvent<Vector3, float> focusEvent;

		/// <summary>
		/// Main // Ű �Է� �̺�Ʈ
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
