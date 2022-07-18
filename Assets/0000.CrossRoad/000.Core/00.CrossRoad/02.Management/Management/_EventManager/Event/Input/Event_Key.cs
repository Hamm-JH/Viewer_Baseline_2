using Definition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Management.Events.Inputs
{
	public class Event_Key : EventData_Input
	{
		public List<KeyData> m_keys;
		UnityEvent<List<KeyData>> m_keyEvent;

		/// <summary>
		/// Ű���� �Է� �̺�Ʈ ������
		/// </summary>
		/// <param name="_eventType">�̺�Ʈ �з�</param>
		/// <param name="_kData">Ű���� ����Ʈ</param>
		/// <param name="_camera">ī�޶�</param>
		/// <param name="_grRaycaster">�׷��� ����ĳ����</param>
		/// <param name="_event">Ű���� ����Ʈ�� �ʿ�� �ϴ� �̺�Ʈ</param>
		public Event_Key(InputEventType _eventType,
			List<KeyData> _kData,
			Camera _camera, GraphicRaycaster _grRaycaster,
			UnityEvent<List<KeyData>> _event)
		{
			StatusCode = Status.Ready;

			EventType = _eventType;
			m_camera = _camera;
			m_grRaycaster = _grRaycaster;
			m_keys = _kData;
			m_keyEvent = _event;
		}

		/// <summary>
		/// �̺�Ʈ ��ó��
		/// </summary>
		/// <param name="_mList">��� ����Ʈ</param>
		public override void OnProcess(List<ModuleCode> _mList)
		{
			if (m_keys != null && m_keys.Count != 0)
			{
				StatusCode = Status.Update;
			}
			// KeyData ����Ʈ�� null�� ���
			else
			{
				StatusCode = Status.Update;
			}
		}

		/// <summary>
		/// �̺�Ʈ ��ó��
		/// </summary>
		/// <param name="_sEvents">���� �̺�Ʈ ����Ʈ</param>
		public override void DoEvent(Dictionary<InputEventType, AEventData> _sEvents)
		{
			// ���� �Էµ� Ű�� 1�� �̻��� ���
			if (m_keys != null && m_keys.Count != 0)
			{
				// Ű ������ ������Ʈ
				if (_sEvents.ContainsKey(InputEventType.Input_key))
				{
					_sEvents[InputEventType.Input_key] = this;
				}
				else
				{
					_sEvents.Add(InputEventType.Input_key, this);
				}
			}
			else
			{
				// ���� ����� Ű �����
				if (_sEvents.ContainsKey(InputEventType.Input_key))
				{
					_sEvents.Remove(InputEventType.Input_key);
				}
			}

			m_keyEvent.Invoke(m_keys);
		}
	}
}
