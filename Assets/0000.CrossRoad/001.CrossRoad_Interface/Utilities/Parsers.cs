using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
	using Definition;

	public static class Parsers
	{
        /// <summary>
        /// ��ü �Ľ�
        /// </summary>
        /// <typeparam name="T1">Ÿ�� 1</typeparam>
        /// <param name="_v1">Ÿ�� 1 ���� ��ü</param>
        /// <returns>�Ľ� �Ϸ�� ������</returns>
		public static T1 Parse<T1>(object _v1)
		{
			return (T1)_v1;
		}

        /// <summary>
        /// ȸ������ ���ڿ����� �ڵ�� �з�
        /// </summary>
        /// <param name="_value"></param>
        public static ViewRotations GetStringToViewCode(string _value)
        {
            ViewRotations result = ViewRotations.Null;

            switch (_value)
            {
                case "Top":
                case "TO":
                    result = ViewRotations.Top;
                    break;

                case "Bottom":
                case "BO":
                    result = ViewRotations.Bottom;
                    break;

                case "Front":
                case "FR":
                    result = ViewRotations.Front;
                    break;

                case "Back":
                case "BA":
                    result = ViewRotations.Back;
                    break;

                case "Left":
                case "LE":
                    result = ViewRotations.Left;
                    break;

                case "Right":
                case "RI":
                    result = ViewRotations.Right;
                    break;
            }

            return result;
        }

        /// <summary>
        /// UIEvent ��ȯ
        /// </summary>
        /// <param name="_vCode"></param>
        /// <returns></returns>
        public static UIEventType OnParse(ViewRotations _vCode)
		{
			UIEventType uType = UIEventType.Null;

			switch (_vCode)
			{
				case ViewRotations.Top: uType = UIEventType.Viewport_ViewMode_TOP; break;
				case ViewRotations.Bottom: uType = UIEventType.Viewport_ViewMode_BOTTOM; break;
				case ViewRotations.Front: uType = UIEventType.Viewport_ViewMode_SIDE_FRONT; break;
				case ViewRotations.Back: uType = UIEventType.Viewport_ViewMode_SIDE_BACK; break;
				case ViewRotations.Left: uType = UIEventType.Viewport_ViewMode_SIDE_LEFT; break;
				case ViewRotations.Right: uType = UIEventType.Viewport_ViewMode_SIDE_RIGHT; break;
			}

			return uType;
		}

        /// <summary>
        /// UIEvent ��ȯ
        /// </summary>
        /// <param name="_value"></param>
        /// <returns></returns>
        public static UIEventType OnParse(string _value)
		{
            UIEventType uType = UIEventType.Null;

            switch (_value)
            {
                case "Top":     uType = UIEventType.Viewport_ViewMode_TOP; break;
                case "Bottom":  uType = UIEventType.Viewport_ViewMode_BOTTOM; break;
                case "Front":   uType = UIEventType.Viewport_ViewMode_SIDE_FRONT; break;
                case "Back":    uType = UIEventType.Viewport_ViewMode_SIDE_BACK; break;
                case "Left":    uType = UIEventType.Viewport_ViewMode_SIDE_LEFT; break;
                case "Right":   uType = UIEventType.Viewport_ViewMode_SIDE_RIGHT; break;
            }

            return uType;
		}

        /// <summary>
        /// Ư�� ��ü�� ��� ����
        /// </summary>
        /// <param name="_obj">��ǥ ��ü</param>
        /// <returns>���</returns>
        public static Bounds Calculate(GameObject _obj)
		{
            Bounds result = default(Bounds);

            MeshRenderer render;
            if(_obj.TryGetComponent<MeshRenderer>(out render))
			{
                result = render.bounds;
			}
            else
			{
                Debug.LogError("This Object not contains MeshRenderer");
			}

            return result;
		}
	}
}
