using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition
{
	public static class Template
	{
		/// <summary>
		/// UI 템플릿을 가져온다.
		/// </summary>
		/// <param name="_pCode">런타임 중의 플랫폼 코드</param>
		/// <returns></returns>
		/// <exception cref="Definition.Exceptions.PlatformNotDefinedException">이 코드에서 정의되지 않은 플랫폼 코드 발생시 오류 발생</exception>
		public static List<GameObject> GetUITemplates(PlatformCode _pCode)
		{
			List<GameObject> list = new List<GameObject>();

			if(Platforms.IsDemoWebViewer(_pCode))
            {
                list.Add(Resources.Load<GameObject>("UI/BottomBar - DemoWeb"));
                //list.Add(Resources.Load<GameObject>("UI/UIElement/BottomBar"));
			}
			else if (Platforms.IsDemoAdminViewer(_pCode))
			{
				list.Add(Resources.Load<GameObject>("UI/UITemplate_0328 2"));
				list.Add(Resources.Load<GameObject>("UI/UI_AdminViewer"));
			}
			else if(Platforms.IsSmartInspectPlatform(_pCode))
			{
				//list.Add(Resources.Load<GameObject>("UI/UITemplate_0302"));
				list.Add(Resources.Load<GameObject>("UI/SmartInspect"));
			}
			else
            {
				throw new Definition.Exceptions.PlatformNotDefinedException(_pCode);
            }

			return list;
		}
	}
}
