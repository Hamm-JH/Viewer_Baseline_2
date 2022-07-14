using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition
{
	public static class Positions
	{
		/// <summary>
		/// UIEventType에 따라 객체에서 z축으로 이동하는 거리를 결정한다.
		/// </summary>
		/// <param name="_obj">목표 객체</param>
		/// <param name="_eType">UIEventType</param>
		/// <returns>이동 거리 반환</returns>
		public static Vector3 SetLocal(GameObject _obj, UIEventType _eType)
		{
			Vector3 result = default(Vector3);

			Vector3 scale = default(Vector3);
			float boundary = 0.2f;

			MeshRenderer render;
			if(_obj.TryGetComponent<MeshRenderer>(out render))
			{
				scale = render.bounds.extents;
			}

			switch(_eType)
			{
				case UIEventType.Viewport_ViewMode_TOP:
					result = new Vector3(0, 0, -(scale.y + boundary) );
					break;

				case UIEventType.Viewport_ViewMode_BOTTOM:
					result = new Vector3(0, 0, -(scale.y + boundary) );
					break;

				case UIEventType.Viewport_ViewMode_SIDE_FRONT:
					result = new Vector3(0, 0, -(scale.z + boundary));
					break;

				case UIEventType.Viewport_ViewMode_SIDE_BACK:
					result = new Vector3(0, 0, -(scale.z + boundary));
					break;

				case UIEventType.Viewport_ViewMode_SIDE_LEFT:
					result = new Vector3(0, 0, -(scale.x + boundary));
					break;

				case UIEventType.Viewport_ViewMode_SIDE_RIGHT:
					result = new Vector3(0, 0, -(scale.x + boundary));
					break;
			}

			return result;
		}
	}
}
