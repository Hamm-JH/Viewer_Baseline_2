using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition
{
	using Definition.Control;
    using DG.Tweening;
    using Management;
	using Platform.Feature.Camera;
	using Utilities;

	public static class Cameras
	{
        /// <summary>
        /// 카메라의 이동 위치를 할당하는 코드
        /// </summary>
        private static Transform m_toTransform;

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

            // 키맵용 bounds 세팅
            Bounds bForKeymap = new Bounds();
            bForKeymap.center = _b.center;
            bForKeymap.size = _b.size * 0.37f;

            Canvas _canvas = ContentManager.Instance._Canvas;
            UIEventType _uType = UIEventType.Viewport_ViewMode_ISO;

            _camCode.CamMode = GetCameraMode(_uType, _camCode);

            SetCameraCenterPosition(_cam, bForKeymap, _canvas, _uType);
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

        #region Set Camera DOTween Position
        public static void SetCameraDOTweenPosition(Camera _cam, GameObject _toGameObject)
        {
            GameObject currObject = _cam.gameObject;

            Bounds _bound = new Bounds();

            // 대상 객체의 경계검출
            MeshRenderer render;
            if(_toGameObject.TryGetComponent<MeshRenderer>(out render))
            {
                _bound = render.bounds;
            }

            // 목표점 할당
            SetToTransform(currObject.transform, _toGameObject.transform, _bound, ContentManager.Instance._Canvas);

            _cam.transform.DOMove(m_toTransform.position, 1);
            _cam.transform.DORotateQuaternion(m_toTransform.rotation, 1);
        }

        /// <summary>
        /// 카메라 이동 목적지를 세팅하는 Transform 변수 세팅
        /// </summary>
        /// <param name="_targetBound"> 목표 객체의 경계 </param>
        /// <param name="_baseAngle"> 카메라의 from angle </param>
        /// <param name="_canvas"></param>
        private static void SetToTransform(Transform _fromTransform, Transform _toTransform, Bounds _targetBound, Canvas _canvas)
        {
            if(m_toTransform == null)
            {
                InitToTransform();
            }
            // 보고자 하는 목표 객체의 중심, 크기를 구한다.
            //Vector3 center = _targetBound.center;
            //Vector3 size = _targetBound.size;

            PlatformCode pCode = MainManager.Instance.Platform;

            if(Platforms.IsBridgePlatform(pCode))
            {
                // 목표 객체를 봐야 하는 대상 객체 _fromTransform과 m_toTransform을 동기화한다.
                m_toTransform.position = _fromTransform.position;
                m_toTransform.rotation = _fromTransform.rotation;

                // from 위치에서 to 위치를 바라본다.
                m_toTransform.LookAt(_toTransform);

                // 목표 회전각도를 검출한다.
                Quaternion targetAngle = m_toTransform.rotation;

                // to 위치로 위치 변경한다.
                m_toTransform.position = _toTransform.position;

                // 스크린 비율 계산
                float ratio = 0f;
                RectTransform _rectT;
                if(_canvas.TryGetComponent<RectTransform>(out _rectT))
                {
                    float width = _rectT.rect.width;
                    float height = _rectT.rect.height;

                    ratio = width > height ? width / height : height / width;
                }

                // size 반절만큼 멀어지게 한다.
                float distance = Vector3.Distance(_targetBound.center, _targetBound.min);

                // 일정 거리만큼 멀어지게 한다.
                m_toTransform.Translate(Vector3.back * distance * 1.4f * (ratio * 0.8f));

                // 목표점 지정완료.
            }
            else if(Platforms.IsTunnelPlatform(pCode))
            {
                // 기준 위치로 이동한다.
                m_toTransform.position = _fromTransform.position;
                m_toTransform.rotation = _fromTransform.rotation;

                // toTransform에서 Segment 객체를 찾는다.
                Transform segment = _toTransform.parent.parent;

                // segment 위치 + y5만큼 이동
                m_toTransform.position = segment.position + new Vector3(0, 2, 0);

                // 이 위치에서 보고자 하는 객체를 쳐다본다.
                m_toTransform.LookAt(_targetBound.center);
            }


            // 교량의 경우는 위의 식으로 종료됨.
            // 하지만 터널의 경우는
            // to_Transform 위치는 경간점
            // to_Transform 각도는 목표 
        }

        private static void InitToTransform()
        {
            GameObject obj = new GameObject("__CameraMover__");

            m_toTransform = obj.transform;
        }
        #endregion

        #region Set CameraMode

        // 카메라 모드를 변경한다.
        public static void SetCameraMode(CameraModes _mode)
        {
            PlatformCode pCode = MainManager.Instance.Platform;

            if(Platforms.IsBridgePlatform(pCode))
            {
                // 교량 플랫폼의 경우 카메라 모드 변화는 없다.
            }
            else if(Platforms.IsTunnelPlatform(pCode))
            {
                MainManager.Instance.UpdateCameraMode(_mode);
                //MainManager.Instance.camera`
                //_mode
            }
        }

        #endregion
    }
}
