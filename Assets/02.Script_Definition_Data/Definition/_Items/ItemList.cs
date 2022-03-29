using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition
{
	public static class ItemList
	{
		public static GameObject Load(FunctionCode _fCode)
		{
			GameObject result = null;

			// TODO 0309 Module_UI 코드와 코드뎁스 정비 (같이 item을 로드하는 관계인데 AUI는 모듈에 속해있는 문제)
			// TODO 0309 손상정보 등록단계 이벤트 처리

			switch(_fCode)
			{
				case FunctionCode.Item_LocationGuide:
					result = Resources.Load<GameObject>("Items/LocationGuide");
					break;
			}

			return result;
		}
	}
}
