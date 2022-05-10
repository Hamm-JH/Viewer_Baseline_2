using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.Item
{
	using Definition;
	using Management;

	public partial class Module_Items : AModule
	{
		public void OnAfterInitialize()
        {
			CreateItems_After(Functions);
        }

		/// <summary>
		/// TODO :: 추후 Items 리스트는 n개 이상의 리소스를 받아올 예정.
		/// 여러개의 functionCode를 처리할 수 있게 코드 업데이트 필요함.
		/// </summary>
		public override void OnStart()
		{
			Debug.LogWarning($"{this.GetType().ToString()} OnStart");

			m_itemList = new List<Items.AItem>();

			CreateItems(Functions);
			//CreateItem(m_currentFunction);

			// TODO 0309 코드 리디렉션 정리
			OnUpdateState(EventManager.Instance._ModuleList);

			ContentManager.Instance.CheckInitModuleComplete(ID);
		}

		/// <summary>
		/// 이벤트 관리자에서 상태코드 업데이트
		/// </summary>
		/// <param name="_mList"></param>
		public void OnUpdateState(List<ModuleCode> _mList)
		{
			m_itemList.ForEach(x => x.UpdateState(_mList));
		}

		/// <summary>
		/// 개별 아이템에 SetGuide 이벤트 실행
		/// (현) LocationGuide만 구현되어있음
		/// </summary>
		/// <param name="_target"></param>
		/// <param name="_baseAngle"></param>
		/// <param name="_uType"></param>
		public void SetGuide(GameObject _target, Vector3 _baseAngle, UIEventType _uType)
		{
			m_itemList.ForEach(x => x.SetGuide(_target, _baseAngle, _uType));
		}
	}
}
