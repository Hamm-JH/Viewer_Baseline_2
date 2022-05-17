using Definition;
using Management;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platform.Feature._Input
{
	public partial class Keyboard : IInput
	{
		#region Pre-defined 사전 정의

		[System.Serializable]
		public class Data
		{
			/// <summary>
			/// 전방 이동 키
			/// </summary>
			public KeyCode keyFront;

			/// <summary>
			/// 후방 이동 키
			/// </summary>
			public KeyCode keyBack;

			/// <summary>
			/// 왼쪽 이동 키
			/// </summary>
			public KeyCode keyLeft;

			/// <summary>
			/// 오른쪽 이동 키
			/// </summary>
			public KeyCode keyRight;

			/// <summary>
			/// 위쪽 이동 키
			/// </summary>
			public KeyCode keyUp;

			/// <summary>
			/// 아래쪽 이동 키
			/// </summary>
			public KeyCode keyDown;

			/// <summary>
			/// 부스트 키
			/// </summary>
			public KeyCode keyBoost;

			/// <summary>
			/// 컨트롤 키
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
				// TODO :: 다른 창으로 사용자가 입력이 넘어갔을때 pause 코드 추가하기. 다른 창 열어두고 다른 짓 하면 이벤트가 튀는 경우가 있음
				newKeyCodes = new List<KeyData>();
			}
		}

		#region fields

		/// <summary>
		/// 키 입력 리스트
		/// </summary>
		List<KeyData> keyCodes;
		List<KeyData> newKeyCodes;

		#endregion

		#region 키 체크

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
		/// 키보드 설정 키에 대응하는 이벤트 체크 수행
		/// </summary>
		private void KeyCheck()
		{
			//string str = "";

			//str += "1";
			newKeyCodes.Clear();
			//str += "2";
			if (Input.anyKey)
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
			//str += "3";
			if (newKeyCodes.Count != 0)
			{
				// clickDown
				//if(keyCodes.Count == 0) { }
				
				// stay
				m_InputEvents.keyEvent.Invoke(Definition.InputEventType.Input_key, newKeyCodes);
			}
			// 이전 키 입력 있었음 && 현재 키 입력 없음 :: ClickUp (모든 키 입력 없는 상태)
			else if(keyCodes.Count != 0 && newKeyCodes.Count == 0)
			{
				m_InputEvents.keyEvent.Invoke(InputEventType.Input_key, null);
			}
			//str += "4";

			// 업데이트
			keyCodes = newKeyCodes;
			//Debug.LogWarning(str);
		}

		#endregion
	}
}
