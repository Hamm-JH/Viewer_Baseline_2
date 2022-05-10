using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition
{
	public static class Positions
	{
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
