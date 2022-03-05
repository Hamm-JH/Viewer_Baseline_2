using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platform.Feature._Input
{
	using Definition;
	using Management.Events;

	public partial class Keyboard : IInput
	{
		/// <summary>
		/// Ű���忡�� �� �������� �Է� �̺�Ʈ�� �Ҵ��Ѵ�.
		/// </summary>
		/// <param name="inputEvents"></param>
		/// <returns></returns>
		public override bool OnStart(ref InputEvents inputEvents)
		{
			keyCodes = new List<KeyData>();
			newKeyCodes = new List<KeyData>();

			m_InputEvents = inputEvents;

			// Ű���� ��� ������ �Ҵ�
			SetDefinition<Data>(inputEvents.KeyboardData);

			return true;
		}

		/// <summary>
		/// OnStart �޼��忡�� ������ MainManager.InputEvents ���ο� ���ǵ� �̺�Ʈ �����͸� �����´�.
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
