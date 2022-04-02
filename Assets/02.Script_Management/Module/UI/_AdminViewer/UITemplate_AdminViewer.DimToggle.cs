using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.UI
{
	using AdminViewer;
	using Data.API;
	using Definition;
	using Management;
	using System.Linq;
	using UnityEngine.Events;
	using UnityEngine.UI;
	using View;

	public partial class UITemplate_AdminViewer : AUI
	{
		private void ToggleDimension()
		{
			GameObject selected = EventManager.Instance._SelectedObject;
			if(selected == null)
			{
				Debug.LogError("현재 선택된 객체가 없음");
				return;
			}

			//Debug.Log("Hello");

			string name = GetName(selected.name);
			Debug.Log(name);

			PlatformCode pCode = MainManager.Instance.Platform;
			if(Platforms.IsTunnelPlatform(pCode))
			{
				Debug.LogError("dim 터널 작업 필요");
			}
			else if(Platforms.IsBridgePlatform(pCode))
			{
				Transform lv2 = ContentManager.Instance.Container.m_dimView.dimLevel2;
				List<Transform> lv4 = ContentManager.Instance.Container.m_dimView.dimLevel4;

				// 매칭되는 대상 찾기
				Transform _find = lv4.FindAll(x => name.Contains(x.name)).Last();

				// 활성화
				lv2.gameObject.SetActive(true);
				lv4.ForEach(x => x.gameObject.SetActive(false));

				if(_find != null)
				{
					_find.gameObject.SetActive(true);
				}
			}
		}

		private string GetName(string _value)
		{
			string result = "";

			PlatformCode pCode = MainManager.Instance.Platform;
			if(Platforms.IsTunnelPlatform(pCode))
			{
				Debug.LogError("이름 작업 필요");
			}
			else if(Platforms.IsBridgePlatform(pCode))
			{
				result = _value.Split(',')[0];
			}

			return result;
		}
	}
}
