using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition
{
	/// <summary>
	/// KEY_ITEM :: Prefab 아이템 리스트
	/// </summary>
	public static class ItemList
	{
		/// <summary>
		/// Item으로 분류된 Prefab 객체를 생선, 반환한다.
		/// </summary>
		/// <param name="_fCode">Item 기능 코드</param>
		/// <returns>생성된 Item 객체 반환</returns>
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
