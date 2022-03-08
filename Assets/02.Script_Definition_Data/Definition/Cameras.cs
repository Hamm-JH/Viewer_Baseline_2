using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition
{
    using Management;
	using Utilities;

	public static class Cameras
	{
        /// <summary>
        /// 카메라 세팅
        /// </summary>
        /// <param name="_code"></param>
        public static void SetCamera(string _code)
        {
            GameObject obj = ContentManager.Instance._SelectedObj;

            ViewRotations vCode = ViewRotations.Null;
            Vector3 angle = obj.transform.parent.parent.rotation.eulerAngles;

            vCode = Parsers.GetStringToViewCode(_code);

            if (vCode != ViewRotations.Null)
            {
                ContentManager.Instance.SetCameraMode(obj, vCode, angle);
            }
        }
    }
}
