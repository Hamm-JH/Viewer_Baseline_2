using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition
{
	using Management;

	public class AModuleStatus
	{
		// TODO :: �����ڵ� ��� �������� �߻�ȭ �ʿ�

		/// <summary>
		/// ���� ��忡�� �ӽ� ĳ�̿� ��ü
		/// </summary>
		public GameObject m_cacheObject;

		/// <summary>
		/// ��� �����ڵ�
		/// </summary>
		public ModuleStatus m_moduleStatus;

		public AModuleStatus()
		{
			// �ʱⰪ null
			m_cacheObject = null;

			// �ʱ� ���� view1
			m_moduleStatus = ModuleStatus.Administration_view1;
		}

		/// <summary>
		/// ���� ���°� �⺻ ��������ΰ�?
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
		/// ���� ���õ� ��ü�� ĳ���Ѵ�.
		/// </summary>
		/// <param name="_selected"></param>
		public void CachingObject(GameObject _selected)
		{
			m_cacheObject = _selected;
		}

		/// <summary>
		/// ĳ�� ��ü�� �����´�.
		/// </summary>
		/// <returns></returns>
		public GameObject GetCache()
		{
			return m_cacheObject;
		}

		/// <summary>
		/// ĳ�ø� �����.
		/// </summary>
		public void RemoveCache()
		{
			m_cacheObject = null;
		}
	}
}
