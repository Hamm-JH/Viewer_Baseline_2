using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition
{
	/// <summary>
	/// KEY_ITEM :: Prefab 아이템 리스트
	/// TODO :: 0309 Module_UI 코드와 코드뎁스 정비 (같이 item을 로드하는 관계인데 AUI는 모듈에 속해있는 문제)
	/// </summary>
	public static class ItemList
	{
		public static GameObject Load(FunctionCode _fCode)
		{
			GameObject result = null;

			switch(_fCode)
			{
				case FunctionCode.Item_LocationGuide:
					result = Resources.Load<GameObject>("Items/LocationGuide");
					break;

				case FunctionCode.Item_Compass:
					result = Resources.Load<GameObject>("Items/CompassController");
					break;
			}

			return result;
		}
	}
}
