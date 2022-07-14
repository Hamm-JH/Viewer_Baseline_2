using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition
{
	using Management;

	public class AModuleStatus
	{
		/// <summary>
		/// 관리 모드에서 임시 캐싱용 객체 리스트
		/// </summary>
		public List<GameObject> m_cacheObjects;

		/// <summary>
		/// 모듈 상태코드
		/// </summary>
		public ModuleStatus m_moduleStatus;

		public AModuleStatus()
		{
			// 초기값 null
			//m_cacheObject = null;
			m_cacheObjects = null;

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
		/// 선택된 상태의 객체 리스트를 캐싱한다.
		/// </summary>
		/// <param name="_selecteds">선택된 객체끼리 모여있는 리스트</param>
		public void CachingObjects(List<GameObject> _selecteds)
        {
			m_cacheObjects = _selecteds;
        }

		/// <summary>
		/// 캐싱 객체를 가져온다.
		/// </summary>
		/// <returns></returns>
		//public GameObject GetCache()
		//{
		//	return m_cacheObject;
		//}

		public List<GameObject> GetCaches()
        {
			return m_cacheObjects;
        }
		
		/// <summary>
		/// 캐시 리스트 정리
		/// </summary>
		public void RemoveCaches()
        {
			m_cacheObjects = null;
        }
	}
}
