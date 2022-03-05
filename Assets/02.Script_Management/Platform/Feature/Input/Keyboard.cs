using Definition;
using Management;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platform.Feature._Input
{
	public partial class Keyboard : IInput
	{
		#region Pre-defined ���� ����

		[System.Serializable]
		public class Data
		{
			/// <summary>
			/// ���� �̵� Ű
			/// </summary>
			public KeyCode keyFront;

			/// <summary>
			/// �Ĺ� �̵� Ű
			/// </summary>
			public KeyCode keyBack;

			/// <summary>
			/// ���� �̵� Ű
			/// </summary>
			public KeyCode keyLeft;

			/// <summary>
			/// ������ �̵� Ű
			/// </summary>
			public KeyCode keyRight;

			/// <summary>
			/// ���� �̵� Ű
			/// </summary>
			public KeyCode keyUp;

			/// <summary>
			/// �Ʒ��� �̵� Ű
			/// </summary>
			public KeyCode keyDown;

			/// <summary>
			/// �ν�Ʈ Ű
			/// </summary>
			public KeyCode keyBoost;

			/// <summary>
			/// ��Ʈ�� Ű
			/// </summary>
			public KeyCode keyCtrl;
		}

		[SerializeField] Data defData;

		#endregion

		private void Update()
		{
			try
			{
				KeyCheck();
			}
			catch
			{
				// TODO :: �ٸ� â���� ����ڰ� �Է��� �Ѿ���� pause �ڵ� �߰��ϱ�
				Debug.LogError("�ٸ� â ����ΰ� �ٸ� �� �ϸ� �̺�Ʈ�� Ƣ�� ��찡 ����.");
			}
		}

		#region fields

		/// <summary>
		/// Ű �Է� ����Ʈ
		/// </summary>
		List<KeyData> keyCodes;
		List<KeyData> newKeyCodes;

		#endregion

		#region Ű üũ

		private KeyData CheckKeyData(KeyCode _kCode)
		{
			if (Input.GetKeyDown(_kCode))
			{
				return new KeyData
				{
					m_keyState = KeyState.Down,
					m_keyCode = _kCode
				};
			}
			else if(Input.GetKeyUp(_kCode))
			{
				return new KeyData
				{
					m_keyState = KeyState.Up,
					m_keyCode = _kCode
				};
			}
			else if (Input.GetKey(_kCode))
			{
				return new KeyData
				{
					m_keyState = KeyState.Stay,
					m_keyCode = _kCode
				};
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Ű���� ���� Ű�� �����ϴ� �̺�Ʈ üũ ����
		/// </summary>
		private void KeyCheck()
		{
			newKeyCodes.Clear();

			if(Input.anyKey)
			{
				KeyData _front = CheckKeyData(defData.keyFront);
				KeyData _back = CheckKeyData(defData.keyBack);
				KeyData _left = CheckKeyData(defData.keyLeft);
				KeyData _right = CheckKeyData(defData.keyRight);
				KeyData _up = CheckKeyData(defData.keyUp);
				KeyData _down = CheckKeyData(defData.keyDown);
				KeyData _boost = CheckKeyData(defData.keyBoost);
				KeyData _ctrl = CheckKeyData(defData.keyCtrl);

				if (_front != null) newKeyCodes.Add(_front);
				if (_back != null) newKeyCodes.Add(_back);
				if (_left != null) newKeyCodes.Add(_left);
				if (_right != null) newKeyCodes.Add(_right);
				if (_up != null) newKeyCodes.Add(_up);
				if (_down != null) newKeyCodes.Add(_down);
				if (_boost != null) newKeyCodes.Add(_boost);
				if (_ctrl != null) newKeyCodes.Add(_ctrl);
			}

			if(newKeyCodes.Count != 0)
			{
				// clickDown
				//if(keyCodes.Count == 0) { }
				
				// stay
				m_InputEvents.keyEvent.Invoke(Definition.InputEventType.Input_key, newKeyCodes);
			}
			// ���� Ű �Է� �־��� && ���� Ű �Է� ���� :: ClickUp (��� Ű �Է� ���� ����)
			else if(keyCodes.Count != 0 && newKeyCodes.Count == 0)
			{
				m_InputEvents.keyEvent.Invoke(InputEventType.Input_key, null);
			}

			// ������Ʈ
			keyCodes = newKeyCodes;
		}

		#endregion
	}
}
