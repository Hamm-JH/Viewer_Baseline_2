using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition
{
	using Management;

	public class AModuleStatus
	{
		// TODO :: 관리코드 모듈 영역으로 추상화 필요

		/// <summary>
		/// 관리 모드에서 임시 캐싱용 객체
		/// </summary>
		public GameObject m_cacheObject;

		/// <summary>
		/// 모듈 상태코드
		/// </summary>
		public ModuleStatus m_moduleStatus;

		public AModuleStatus()
		{
			// 초기값 null
			m_cacheObject = null;

			// 초기 값은 view1
			m_moduleStatus = ModuleStatus.Administration_view1;
		}

		/// <summary>
		/// 현재 상태가 기본 관리모드인가?
		/// </summary>
		/// <returns></returns>
		public bool IsDefaultAdministrationMode()
		{
			PlatformCode pCode = MainManager.Instance.Platform;
			if(Platforms.IsDemoAdminViewer(pCode))
			{
				switch(m_moduleStatus)
				{
					case ModuleStatus.Administration_view1:
					case ModuleStatus.Administration_view2:
					case ModuleStatus.Administration_view5:
						return true;

					case ModuleStatus.Administration_view3:
					case ModuleStatus.Administration_view4:
						return false;

					default:
						return false;
				}
			}

			return false;
		}

		/// <summary>
		/// 현재 선택된 객체를 캐싱한다.
		/// </summary>
		/// <param name="_selected"></param>
		public void CachingObject(GameObject _selected)
		{
			m_cacheObject = _selected;
		}

		/// <summary>
		/// 캐싱 객체를 가져온다.
		/// </summary>
		/// <returns></returns>
		public GameObject GetCache()
		{
			return m_cacheObject;
		}

		/// <summary>
		/// 캐시를 지운다.
		/// </summary>
		public void RemoveCache()
		{
			m_cacheObject = null;
		}
	}
}
