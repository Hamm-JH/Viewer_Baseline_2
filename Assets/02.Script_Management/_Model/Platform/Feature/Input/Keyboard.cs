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
		}

		[SerializeField] Data defData;

		#endregion

		private void Update()
		{
			KeyCheck();
		}

		#region fields

		/// <summary>
		/// 키 입력 리스트
		/// </summary>
		List<KeyCode> keyCodes;

		#endregion

		#region 키 체크

		/// <summary>
		/// 키보드 설정 키에 대응하는 이벤트 체크 수행
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
