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
		}

		[SerializeField] Data defData;

		#endregion

		private void Update()
		{
			KeyCheck();
		}

		#region fields

		/// <summary>
		/// Ű �Է� ����Ʈ
		/// </summary>
		List<KeyCode> keyCodes;

		#endregion

		#region Ű üũ

		/// <summary>
		/// Ű���� ���� Ű�� �����ϴ� �̺�Ʈ üũ ����
		/// </summary>
		private void KeyCheck()
		{
			keyCodes.Clear();

			if(Input.GetKey(defData.keyFront))
			{
				keyCodes.Add(defData.keyFront);
			}

			if(Input.GetKey(defData.keyBack))
			{
				keyCodes.Add(defData.keyBack);
			}

			if(Input.GetKey(defData.keyLeft))
			{
				keyCodes.Add(defData.keyLeft);
			}

			if(Input.GetKey(defData.keyRight))
			{
				keyCodes.Add(defData.keyRight);
			}

			if(Input.GetKey(defData.keyUp))
			{
				keyCodes.Add(defData.keyUp);
			}

			if(Input.GetKey(defData.keyDown))
			{
				keyCodes.Add(defData.keyDown);
			}

			if(Input.GetKey(defData.keyBoost))
			{
				keyCodes.Add(defData.keyBoost);
			}

			if(keyCodes.Count != 0)
			{
				m_InputEvents.keyEvent.Invoke(Definition.InputEventType.Input_key, keyCodes);
			}
		}

		#endregion
	}
}
