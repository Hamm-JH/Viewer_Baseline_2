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

			// TODO 0309 Module_UI �ڵ�� �ڵ嵪�� ���� (���� item�� �ε��ϴ� �����ε� AUI�� ��⿡ �����ִ� ����)
			// TODO 0309 �ջ����� ��ϴܰ� �̺�Ʈ ó��

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
