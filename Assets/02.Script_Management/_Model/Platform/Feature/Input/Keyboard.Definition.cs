using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platform.Feature._Input
{
	using Management.Events;

	public partial class Keyboard : IInput
	{
		/// <summary>
		/// 키보드에서 주 관리자의 입력 이벤트를 할당한다.
		/// </summary>
		/// <param name="inputEvents"></param>
		/// <returns></returns>
		public override bool OnStart(ref InputEvents inputEvents)
		{
			keyCodes = new List<KeyCode>();

			m_InputEvents = inputEvents;

			// 키보드 사용 데이터 할당
			SetDefinition<Data>(inputEvents.KeyboardData);

			return true;
		}

		/// <summary>
		/// OnStart 메서드에서 가져온 MainManager.InputEvents 내부에 정의된 이벤트 데이터를 가져온다.
		/// </summary>
		/// <typeparam name="TValue"></typeparam>
		/// <param name="_data"></param>
		protected override void SetDefinition<TValue>(TValue _data)
		{
			Data _instance = _data as Data;

			if (_instance != null)
			{
				defData = _instance;
			}
		}
	}
}
