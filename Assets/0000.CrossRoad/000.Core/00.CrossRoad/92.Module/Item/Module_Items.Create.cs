using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.Item
{
	using Definition;
    using Management;
    using Module.Graphic;
    using Module.Model;

    public partial class Module_Items : AModule
	{
		/// <summary>
		/// 함수코드 리스트 기반 객체 생성
		/// </summary>
		/// <param name="_fCodes">함수 분류 리스트</param>
		public void CreateItems(List<FunctionCode> _fCodes)
        {
			_fCodes.ForEach(x => CreateItem(x));
        }

		/// <summary>
		/// 아이템 리스트 기반 객체 생성
		/// </summary>
		/// <param name="_fCodes">함수 분류 리스트</param>
		public void CreateItems_After(List<FunctionCode> _fCodes)
        {
			_fCodes.ForEach(x => CreateItem_After(x));
        }

		/// <summary>
		/// 아이템 생성
		/// </summary>
		/// <param name="_fCode"></param>
		public void CreateItem(FunctionCode _fCode)
		{
			GameObject obj = null;
			
			// guide 코드 
			if (_fCode == Definition.FunctionCode.Item_LocationGuide)
			{
				obj = Instantiate<GameObject>(ItemList.Load(_fCode));
				m_guide = obj.GetComponent<Items.Controller_LocationGuide>();

				// 관리를 위해 아이템리스트 업데이트
				m_itemList.Add(m_guide);
			}
			else if(_fCode == FunctionCode.Item_Compass)
            {
				return;
            }

			// Item들을 이 모듈 아래로 모음
			obj.transform.SetParent(transform);
		}

		/// <summary>
		/// 아이템 생성
		/// </summary>
		/// <param name="_fCode"></param>
		public void CreateItem_After(FunctionCode _fCode)
        {
			GameObject obj = null;

			PlatformCode pCode = MainManager.Instance.Platform;

			// guide 코드 
			if (_fCode == Definition.FunctionCode.Item_LocationGuide)
			{
				return;
			}
			else if (_fCode == FunctionCode.Item_Compass)
			{
				//if(Platforms.IsDemoWebViewer(pCode))
				if(Platforms.IsTunnelPlatform(pCode))
                {
					Canvas canvas = ContentManager.Instance._Canvas;
					Camera cam = MainManager.Instance.MainCamera;
					// 컴퍼스 초기화용

					Module_Graphic graphic = ContentManager.Instance.Module<Module_Graphic>();

					// 나침반 인스턴스 생성
					GameObject uiCompassRoot = Instantiate<GameObject>(Resources.Load<GameObject>("Items/Compass"), canvas.transform);

					// 아이템 관리자 생성
					obj = Instantiate<GameObject>(ItemList.Load(_fCode));
					m_compass = obj.GetComponent<Items.Controller_Compass>();	// 할당

					// 플레이 객체의 위치 할당
					m_compass.Me = cam.transform;
					// 나침반 root 할당
					m_compass.CompassUIRoot = uiCompassRoot;
					// 나침반 기울기 할당
					m_compass.CompassPitch = 70;

					Module.UI.Element_Compass eCompass = uiCompassRoot.GetComponent<Module.UI.Element_Compass>();


					StartCoroutine(Try_GetTrsTunnel(m_compass, eCompass));

					// 아이템 모듈에 이 아이템 할당
					m_itemList.Add(m_compass);
                }
				else
                {
					return;
                }
			}

			// Item들을 이 모듈 아래로 모음
			obj.transform.SetParent(transform);
		}

		/// <summary>
		/// 터널 에서 나침반 객체 생성
		/// </summary>
		/// <param name="_compass"></param>
		/// <param name="eCompass"></param>
		/// <returns></returns>
		private IEnumerator Try_GetTrsTunnel(Items.Controller_Compass _compass, Module.UI.Element_Compass eCompass)
        {
			Module_Graphic graphic = ContentManager.Instance.Module<Module_Graphic>();

			while(true)
            {
				List<Transform> trs = ContentManager.Instance.Module<Module_Model>().Trs_tunnel;
				if (trs != null && trs.Count == 2)
                {
					_compass.AddCompass(trs, graphic, eCompass);
					break;
                }
				yield return new WaitForEndOfFrame();
            }

			yield break;
        }
	}
}
