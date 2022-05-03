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
        /// ī�޶��� �̵� ��ġ�� �Ҵ��ϴ� �ڵ�
        /// </summary>
        private static Transform m_toTransform;

        /// <summary>
        /// ī�޶� ����
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

            // Ű�ʿ� bounds ����
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

            // ī�޶� ��ġ ���߾ӿ� �δ� ���
            {
                //CameraPoint.transform.position = center;

                _cam.transform.position = center;
                //mainCam.transform.localPosition = default(Vector3);
                _cam.transform.rotation = Quaternion.Euler(_baseAngle/* + Angle.Set(eventType)*/);
                _cam.transform.Rotate(Angle.Set(_uType));

                // ��ũ�� �������
                float ratio = 0f;
                RectTransform _rectT;
                if (_canvas.TryGetComponent<RectTransform>(out _rectT))
                {
                    float width = _rectT.rect.width;
                    float height = _rectT.rect.height;

                    ratio = width > height ? width / height : height / width;
                }

                // size�� ���� �Ÿ� �̵� ���� ����
                //Debug.LogError("Size�� ���� �Ÿ� �̵� ���� ����"); // O

                // size�� ����, * ��Ʈ2 ��ŭ �־����� ��
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

            // ��� ��ü�� ������
            MeshRenderer render;
            if(_toGameObject.TryGetComponent<MeshRenderer>(out render))
            {
                _bound = render.bounds;
            }

            // ��ǥ�� �Ҵ�
            SetToTransform(currObject.transform, _toGameObject.transform, _bound, ContentManager.Instance._Canvas);

            _cam.transform.DOMove(m_toTransform.position, 1);
            _cam.transform.DORotateQuaternion(m_toTransform.rotation, 1);
        }

        /// <summary>
        /// ī�޶� �̵� �������� �����ϴ� Transform ���� ����
        /// </summary>
        /// <param name="_targetBound"> ��ǥ ��ü�� ��� </param>
        /// <param name="_baseAngle"> ī�޶��� from angle </param>
        /// <param name="_canvas"></param>
        private static void SetToTransform(Transform _fromTransform, Transform _toTransform, Bounds _targetBound, Canvas _canvas)
        {
            if(m_toTransform == null)
            {
                InitToTransform();
            }
            // ������ �ϴ� ��ǥ ��ü�� �߽�, ũ�⸦ ���Ѵ�.
            //Vector3 center = _targetBound.center;
            //Vector3 size = _targetBound.size;

            PlatformCode pCode = MainManager.Instance.Platform;

            if(Platforms.IsBridgePlatform(pCode))
            {
                // ��ǥ ��ü�� ���� �ϴ� ��� ��ü _fromTransform�� m_toTransform�� ����ȭ�Ѵ�.
                m_toTransform.position = _fromTransform.position;
                m_toTransform.rotation = _fromTransform.rotation;

                // from ��ġ���� to ��ġ�� �ٶ󺻴�.
                m_toTransform.LookAt(_toTransform);

                // ��ǥ ȸ�������� �����Ѵ�.
                Quaternion targetAngle = m_toTransform.rotation;

                // to ��ġ�� ��ġ �����Ѵ�.
                m_toTransform.position = _toTransform.position;

                // ��ũ�� ���� ���
                float ratio = 0f;
                RectTransform _rectT;
                if(_canvas.TryGetComponent<RectTransform>(out _rectT))
                {
                    float width = _rectT.rect.width;
                    float height = _rectT.rect.height;

                    ratio = width > height ? width / height : height / width;
                }

                // size ������ŭ �־����� �Ѵ�.
                float distance = Vector3.Distance(_targetBound.center, _targetBound.min);

                // ���� �Ÿ���ŭ �־����� �Ѵ�.
                m_toTransform.Translate(Vector3.back * distance * 1.4f * (ratio * 0.8f));

                // ��ǥ�� �����Ϸ�.
            }
            else if(Platforms.IsTunnelPlatform(pCode))
            {
                // ���� ��ġ�� �̵��Ѵ�.
                m_toTransform.position = _fromTransform.position;
                m_toTransform.rotation = _fromTransform.rotation;

                // toTransform���� Segment ��ü�� ã�´�.
                Transform segment = _toTransform.parent.parent;

                // segment ��ġ + y5��ŭ �̵�
                m_toTransform.position = segment.position + new Vector3(0, 2, 0);

                // �� ��ġ���� ������ �ϴ� ��ü�� �Ĵٺ���.
                m_toTransform.LookAt(_targetBound.center);
            }


            // ������ ���� ���� ������ �����.
            // ������ �ͳ��� ����
            // to_Transform ��ġ�� �氣��
            // to_Transform ������ ��ǥ 
        }

        private static void InitToTransform()
        {
            GameObject obj = new GameObject("__CameraMover__");

            m_toTransform = obj.transform;
        }
        #endregion

        #region Set CameraMode

        // ī�޶� ��带 �����Ѵ�.
        public static void SetCameraMode(CameraModes _mode)
        {
            PlatformCode pCode = MainManager.Instance.Platform;

            if(Platforms.IsBridgePlatform(pCode))
            {
                // ���� �÷����� ��� ī�޶� ��� ��ȭ�� ����.
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
