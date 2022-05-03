using Definition.Control;
using Management;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platform.Feature.Camera
{
	public partial class BIMCamera : ICamera
	{
        public Transform target;

        public float maxOffsetDistance = 2000f;
        public float orbitSpeed = 15f;
        public float freeSpeed = 15f;
        public float panSpeed = .5f;
        public float zoomSpeed = 10f;
        [SerializeField] private Vector3 targetOffset = Vector3.zero;
        [SerializeField] private Vector3 targetPosition;
        [SerializeField] private float m_distance;

        [SerializeField] public float _Distance
		{
            get
			{
                if(target)
				{
                    return Vector3.Distance(transform.position, target.position);
				}
                else
				{
                    return Vector3.Distance(transform.position, ContentManager.Instance._CenterBounds.center);
				}
			}
		}

        // Use this for initialization
        void Start()
        {
            if (target != null) transform.LookAt(target);
        }

        /// <summary>
        /// DRAG 발생
        /// </summary>
        /// <param name="btn"></param>
        /// <param name="delta"></param>
        private void InDrag(int btn, Vector2 delta)
		{
            Transform _target;
            if(target == null)
			{
                Default.transform.position = ContentManager.Instance._CenterBounds.center;

                _target = Default.transform;

                if(ContentManager.Instance != null)
				{
                    targetPosition = ContentManager.Instance._CenterBounds.center;
				}
                //Debug.LogWarning("cam target is null");
                //_target = Default.transform;
                //_target.position = transform.TransformDirection(Vector3.forward * 10f);
			}
            else
			{
                _target = target;

                MeshRenderer render;
                if(_target.TryGetComponent<MeshRenderer>(out render))
			    {
                    targetPosition = render.bounds.center;
			    }
                else
			    {
                    targetPosition = _target.position;
			    }
			}


            //targetPosition = target.position + targetOffset;

            // 일반 BIM mode
            // 0 : 회전, 1 : 패닝
            
            if(btn == 0)
			{
                if(CamMode == CameraModes.BIM_ISO)
				{
                    OnOrbitRotate(targetPosition, delta, orbitSpeed);
				}
                else if(CamMode == CameraModes.OnlyRotate)
                {
                    OnSelfRotate(delta, freeSpeed);
                }
            }
            else if(btn == 1)
			{
                if(CamMode == CameraModes.BIM_ISO ||
                    CamMode == CameraModes.BIM_Top ||
                    CamMode == CameraModes.BIM_Bottom ||
                    CamMode == CameraModes.BIM_FRONT ||
                    CamMode == CameraModes.BIM_BACK ||
                    CamMode == CameraModes.BIM_LEFT ||
                    CamMode == CameraModes.BIM_RIGHT)
				{
                    OnPanning(targetPosition, delta, panSpeed, targetOffset, maxOffsetDistance);
				}
                // 제자리 회전만 하겠다.
                else if(CamMode == CameraModes.OnlyRotate)
                {

                }
            }
		}

        /// <summary>
        /// 궤도회전
        /// </summary>
        /// <param name="_tPos"></param>
        /// <param name="_delta"></param>
        /// <param name="_orbitSpeed"></param>
        private void OnOrbitRotate(Vector3 _tPos, Vector2 _delta, float _orbitSpeed)
		{
            transform.RotateAround(_tPos, Vector3.up, _delta.x * _orbitSpeed);

            float pitchAngle = Vector3.Angle(Vector3.up, transform.forward);
            float pitchDelta = _delta.y * orbitSpeed;
            float newAngle = Mathf.Clamp(pitchAngle + pitchDelta, 0f, 180f);
            pitchDelta = newAngle - pitchAngle;
            transform.RotateAround(_tPos, transform.right, pitchDelta);
        }

        /// <summary>
        /// 자유회전
        /// </summary>
        /// <param name="_delta"></param>
        /// <param name="_freeSpeed"></param>
        private void OnSelfRotate(Vector2 _delta, float _freeSpeed)
        {
            //float pitchAngle = Vector3.Angle(Vector3.up, new Vector3(_delta.x, 0, 0));

            //Vector3 moveDelta = new Vector3(_delta.y, _delta.x);
            //transform.Rotate(Vector3.up);

            Vector3 angle = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(
                angle.x + _delta.y * freeSpeed,
                angle.y + _delta.x * freeSpeed,
                angle.z);

            //float pitchAngle = Vector3.Angle(Vector3.up, transform.forward);
            //float pitchDelta = _delta.y * freeSpeed;
            //float newAngle = Mathf.Clamp(pitchAngle + pitchDelta, 0f, 180f);
            //pitchDelta = newAngle - pitchAngle;

            ////float pitch = 
            //transform.Rotate(transform.right, pitchDelta);


        }

        /// <summary>
        /// 패닝 이동
        /// </summary>
        /// <param name="_tPos"></param>
        /// <param name="_delta"></param>
        /// <param name="_panSpeed"></param>
        /// <param name="_targetOffset"></param>
        /// <param name="_maxOffsetDistance"></param>
        private void OnPanning(Vector3 _tPos, Vector2 _delta, float _panSpeed, Vector3 _targetOffset, float _maxOffsetDistance)
		{
            float distance = _Distance / 10;

            Vector3 offset = transform.right * -_delta.x * _panSpeed * distance + transform.up * _delta.y * _panSpeed * distance;
            Vector3 newTargetOffset = Vector3.ClampMagnitude(_targetOffset + offset, _maxOffsetDistance);
            transform.position += newTargetOffset - _targetOffset;
            targetOffset = newTargetOffset;
        }

        private void InFocus(Vector3 mousePos, float delta)
		{
            float scroll = delta;

            UnityEngine.Camera cam = this.GetComponent<UnityEngine.Camera>();

            if(cam.orthographic)
			{
                // 직교 뷰 크기 포커스 변경
                this.GetComponent<UnityEngine.Camera>().orthographicSize -= delta;
			}
            else
			{
                // 투영 뷰 포커스 변경
                transform.position += transform.forward * scroll * zoomSpeed;
			}

        }
    }
}
