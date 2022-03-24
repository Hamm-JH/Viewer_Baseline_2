using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition
{
	using Definition.Control;
	using Management;
	using Platform.Feature.Camera;
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
            Vector3 angle = ContentManager.Instance._SelectedAngle;
            //Vector3 angle = obj.transform.parent.parent.rotation.eulerAngles;

            vCode = Parsers.GetStringToViewCode(_code);

            if (vCode != ViewRotations.Null)
            {
                ContentManager.Instance.SetCameraMode(obj, vCode, angle);
            }
        }

        public static void SetCameraCenter(Camera _cam, ICamera _camCode)
		{
            Bounds _b = ContentManager.Instance._CenterBounds;
            Canvas _canvas = ContentManager.Instance._Canvas;
            UIEventType _uType = UIEventType.Viewport_ViewMode_ISO;

            _camCode.CamMode = GetCameraMode(_uType, _camCode);

            SetCameraCenterPosition(_cam, _b, _canvas, _uType);
		}

        public static CameraModes GetCameraMode(UIEventType _uType, ICamera _camCode)
		{
            CameraModes _modes = _camCode.CamMode;

            int _modeIndex = (int)_modes / 0x10;

            int _typeIndex = 0;

            switch (_uType)
            {
                case UIEventType.Viewport_ViewMode_ISO: _typeIndex = 0x00; break;
                case UIEventType.Viewport_ViewMode_TOP: _typeIndex = 0x01; break;
                case UIEventType.Viewport_ViewMode_SIDE: _typeIndex = 0x02; break;
                case UIEventType.Viewport_ViewMode_BOTTOM: _typeIndex = 0x03; break;
                case UIEventType.Viewport_ViewMode_SIDE_FRONT: _typeIndex = 0x04; break;
                case UIEventType.Viewport_ViewMode_SIDE_BACK: _typeIndex = 0x05; break;
                case UIEventType.Viewport_ViewMode_SIDE_LEFT: _typeIndex = 0x06; break;
                case UIEventType.Viewport_ViewMode_SIDE_RIGHT: _typeIndex = 0x07; break;
            }

            _modes = (CameraModes)(_modeIndex * 0x10 + _typeIndex);

            return _modes;
		}

        public static void SetCameraCenterPosition(Camera _cam, Bounds _cBounds, Canvas _canvas, UIEventType _uType,
            Vector3 _baseAngle = default(Vector3))
		{
            Vector3 center = _cBounds.center;
            Vector3 size = _cBounds.size;

            // 카메라 위치 정중앙에 두는 경우
            {
                //CameraPoint.transform.position = center;

                _cam.transform.position = center;
                //mainCam.transform.localPosition = default(Vector3);
                _cam.transform.rotation = Quaternion.Euler(_baseAngle/* + Angle.Set(eventType)*/);
                _cam.transform.Rotate(Angle.Set(_uType));

                // 스크린 비율계산
                float ratio = 0f;
                RectTransform _rectT;
                if (_canvas.TryGetComponent<RectTransform>(out _rectT))
                {
                    float width = _rectT.rect.width;
                    float height = _rectT.rect.height;

                    ratio = width > height ? width / height : height / width;
                }

                // size에 따라 거리 이동 공식 적용
                //Debug.LogError("Size에 따라 거리 이동 공식 적용"); // O

                // size의 반절, * 루트2 만큼 멀어져야 함
                float distance = Vector3.Distance(center, _cBounds.min);

                _cam.orthographicSize = distance;

                _cam.transform.Translate(Vector3.back * distance * 1.4f * (ratio * 0.8f));
            }
        }
    }
}
