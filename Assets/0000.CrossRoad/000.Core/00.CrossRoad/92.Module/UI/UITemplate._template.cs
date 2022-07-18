using Data.API;
using Definition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View;

namespace Module.UI
{
	public class UITemplate : AUI
	{
		public override void OnStart()
		{
			Debug.LogWarning("UITemplate OnStart");
		}

		public override void OnModuleComplete()
		{
			Debug.LogError("load complete");
		}

		/// <summary>
		/// �̺�Ʈ �����
		/// </summary>
		/// <exception cref="System.NotImplementedException"></exception>
		public override void ReInvokeEvent()
		{
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// UIEvent ��������
		/// </summary>
		/// <param name="_uType">�̺�Ʈ �з�</param>
		/// <param name="_setter">UISelectable ó��</param>
		/// <exception cref="System.NotImplementedException"></exception>
		public override void GetUIEvent(UIEventType _uType, Interactable _setter)
		{
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// UIEvent ��������
		/// </summary>
		/// <param name="_uType">�̺�Ʈ �з�</param>
		/// <param name="_setter">Interactable ��ü</param>
		/// <exception cref="System.NotImplementedException"></exception>
        public override void GetUIEvent(Inspect_EventType _uType, Interactable _setter)
        {
            throw new System.NotImplementedException();
        }

		/// <summary>
		/// UIEvent ��������
		/// </summary>
		/// <param name="_value"></param>
		/// <param name="_uType"></param>
		/// <param name="_setter"></param>
		/// <exception cref="System.NotImplementedException"></exception>
        public override void GetUIEvent(float _value, UIEventType _uType, Interactable _setter)
		{
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// UIEvent ��������
		/// </summary>
		/// <param name="_value"></param>
		/// <param name="_uType"></param>
		/// <param name="_setter"></param>
		/// <exception cref="System.NotImplementedException"></exception>
        public override void GetUIEvent(float _value, Inspect_EventType _uType, Interactable _setter)
        {
            throw new System.NotImplementedException();
        }

		/// <summary>
		/// �ͳ� ��ü���� �Ҵ�
		/// </summary>
		/// <param name="selected">���õ� ��ü</param>
        public override void SetObjectData_Tunnel(GameObject selected)
		{
			
		}

		/// <summary>
		/// �г� ����Ʈ ���
		/// </summary>
		/// <param name="_index"></param>
		/// <param name="_exclusive"></param>
		/// <exception cref="System.NotImplementedException"></exception>
		public override void TogglePanelList(int _index, GameObject _exclusive)
		{
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// ������ �ּ����� ��û
		/// </summary>
		/// <param name="_data"></param>
		/// <exception cref="System.NotImplementedException"></exception>
        public override void API_GetAddress(AAPI _data)
        {
            throw new System.NotImplementedException();
        }
    }
}
