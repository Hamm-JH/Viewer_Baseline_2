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
		public void CreateItems(List<FunctionCode> _fCodes)
        {
			_fCodes.ForEach(x => CreateItem(x));
        }

		public void CreateItems_After(List<FunctionCode> _fCodes)
        {
			_fCodes.ForEach(x => CreateItem_After(x));
        }

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
				if(Platforms.IsDemoWebViewer(pCode))
                {
					Canvas canvas = ContentManager.Instance._Canvas;
					Camera cam = MainManager.Instance.MainCamera;
					// 컴퍼스 초기화용

					Module_Graphic graphic = ContentManager.Instance.Module<Module_Graphic>();

					GameObject uiCompassRoot = Instantiate<GameObject>(Resources.Load<GameObject>("Items/Compass"), canvas.transform);

					obj = Instantiate<GameObject>(ItemList.Load(_fCode));
					m_compass = obj.GetComponent<Items.Controller_Compass>();

					m_compass.Me = cam.transform;
					m_compass.CompassUIRoot = uiCompassRoot;
					m_compass.CompassPitch = 60;

					StartCoroutine(Try_GetTrsTunnel(m_compass));

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

		private IEnumerator Try_GetTrsTunnel(Items.Controller_Compass _compass)
        {
			Module_Graphic graphic = ContentManager.Instance.Module<Module_Graphic>();

			while(true)
            {
				List<Transform> trs = ContentManager.Instance.Module<Module_Model>().Trs_tunnel;
				if (trs != null && trs.Count == 2)
                {
					_compass.AddCompass(trs, graphic);
					break;
                }
				yield return new WaitForEndOfFrame();
            }

			yield break;
        }
	}
}
