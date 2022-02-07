using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Control._Input
{
	using Management.Events;

	public partial class Mouse : IInput
	{

		/// <summary>
		/// ���콺���� �� �������� �Է� �̺�Ʈ�� �Ҵ��Ѵ�.
		/// </summary>
		/// <param name="inputEvents"></param>
		/// <returns></returns>
		public override bool OnStart(ref InputEvents inputEvents)
		{
			// �ʱ�ȭ
			onClick = false;

			m_InputEvents = inputEvents;

			// ���콺 ��� ������ �Ҵ�
			SetDefinition<Data>(inputEvents.MouseData);

			return true;
		}

		/// <summary>
		/// OnStart �޼��忡�� ������ MainManager.InputEvents ���ο� ���ǵ� �̺�Ʈ �����͸� �����´�.
		/// </summary>
		/// <typeparam name="TValue"></typeparam>
		/// <param name="_data"></param>
		protected override void SetDefinition<TValue>(TValue _data) where TValue : class
		{
			Data _instance = _data as Data;

			if (_instance != null)
			{
				defData = _instance;
			}
		}
	}
}
