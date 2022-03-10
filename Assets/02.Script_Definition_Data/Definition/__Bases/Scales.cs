using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition
{
	public static class Scales
	{
		/// <summary>
		/// 가이드큐브(쿼드 크기변경)
		/// </summary>
		/// <param name="_tar"></param>
		/// <param name="_uType"></param>
		/// <returns></returns>
		public static Vector3 SetQuad(GameObject _tar, UIEventType _uType)
		{
			Vector3 result = default(Vector3);

			Bounds bound = _tar.GetComponent<MeshRenderer>().bounds;
			Vector3 tgScale = bound.size;
			//Vector3 tgScale = _tar.transform.localScale;

			switch (_uType)
			{
				case UIEventType.Viewport_ViewMode_TOP:
				case UIEventType.Viewport_ViewMode_BOTTOM:
					result = new Vector3(tgScale.x, tgScale.z, 1);
					break;

				case UIEventType.Viewport_ViewMode_SIDE_FRONT:
				case UIEventType.Viewport_ViewMode_SIDE_BACK:
					result = new Vector3(tgScale.x, tgScale.y, 1);
					break;

				case UIEventType.Viewport_ViewMode_SIDE_LEFT:
				case UIEventType.Viewport_ViewMode_SIDE_RIGHT:
					result = new Vector3(tgScale.z, tgScale.y, 1);
					break;
			}

			return result;
		}
	}
}
