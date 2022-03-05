using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.UI
{
	using Definition;

	[System.Serializable]
	public abstract class AUI : AModule
	{
		public new void OnCreate(ModuleID _id, FunctionCode _code)
		{
			base.OnCreate(_id, _code);
		}

		/// <summary>
		/// 터널 모델에서 선택된 객체의 정보를 설정한다.
		/// </summary>
		/// <param name="selected"></param>
		public abstract void SetObjectData_Tunnel(GameObject selected);

		/// <summary>
		/// 패널 리스트 토글
		/// </summary>
		/// <param name="index"></param>
		public abstract void TogglePanelList(int _index, GameObject _exclusive);
	}
}
