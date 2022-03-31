using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace MGS.UCamera
{
    /// <summary>
    /// Camera rotate around target gameobject.
    /// </summary>
    [AddComponentMenu("MGS/UCamera/AroundCamera")]
    [RequireComponent(typeof(Camera))]
    public class AroundCamera : Singleton<AroundCamera>
    {
        /// <summary>
        /// 객체 2D 모드에서 카메라 회전 Lock과 Orthographic size 조절을 위한 변수 선언
        /// </summary>
        //public DimViewManager dimViewManager;
        public bool selectViewMode = false;

        /// <summary>
        /// Around center.
        /// </summary>
        public Transform target;

        /// <summary>
        /// Settings of mouse button, pointer and scrollwheel.
        /// </summary>
        public MouseSettings mouseSettings = new MouseSettings(1, 10f, 10f);

        /// <summary>
        /// Range limit of angle.
        /// </summary>
        public Range angleRange = new Range(-90f, 90f);

        /// <summary>
        /// Range limit of distance.
        /// </summary>
        public Range distanceRange = new Range(1f, 10f);

        /// <summary>
        /// Range limit of distance.
        /// </summary>
        public float orthoSize;
        public Range orthoSizeRange = new Range(1f, 10f);

        /// <summary>
        /// Damper for move and rotate.
        /// </summary>
        [Range(0f, 10f)]
        public float damper = 10f;

        /// <summary>
        /// Camera target angls.
        /// </summary>
        protected Vector3 targetAngles;

        /// <summary>
        /// Target distance from camera to target.
        /// </summary>
        public float targetDistance;

        /// <summary>
        /// Camera current angls.
        /// </summary>
        public Vector3 CurrentAngles
        {
            get;
            set;
        }

        /// <summary>
        /// Current distance from camera to target.
        /// </summary>
        public float CurrentDistance
        {
            get;
            protected set;
        }

        public AroundCamera()
        {
        }

        /// <summary>
        /// Camera around target by mouse.
        /// </summary>
        protected void AroundByMouse()
        {
            if (Input.GetMouseButton(1))
            {
                //ref float axis = ref this.targetAngles.y;
                //axis = axis + Input.GetAxis("Mouse X") * this.mouseSettings.pointerSensitivity;
                //ref float singlePointer = ref this.targetAngles.x;
                //singlePointer = singlePointer - Input.GetAxis("Mouse Y") * this.mouseSettings.pointerSensitivity;
                //this.targetAngles.x = Mathf.Clamp(this.targetAngles.x, this.angleRange.min, this.angleRange.max);

                #region 2D와 3D 모드 변경에 따라서 카메라 조작법 변경
                if (selectViewMode == false)
                {
                    this.targetAngles.y += Input.GetAxis("Mouse X") * this.mouseSettings.pointerSensitivity;
                    this.targetAngles.x -= Input.GetAxis("Mouse Y") * this.mouseSettings.pointerSensitivity;
                    //this.targetAngles.x = Mathf.Clamp(this.targetAngles.x, this.angleRange.min, this.angleRange.max);
                }
                else
                    return;
            }

            //Manager.EventClassifier.Instance.OnEvent<AroundCamera>(Control.Status.Scroll, gameObject.GetComponent<AroundCamera>());


            //if (RuntimeData.RootContainer.Instance.IsScrollInPanel == false)
            //{
            //    ChangeTargetDistance(Input.GetAxis("Mouse ScrollWheel"));

            //    //if (selectViewMode == false)
            //    //    this.targetDistance = this.targetDistance - Input.GetAxis("Mouse ScrollWheel") * this.mouseSettings.wheelSensitivity;
            //    //else
            //    //    Camera.main.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * this.mouseSettings.wheelSensitivity;
            //}

            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, this.orthoSizeRange.min, this.orthoSizeRange.max);
            #endregion

            this.targetDistance = Mathf.Clamp(this.targetDistance, this.distanceRange.min, this.distanceRange.max);
            this.CurrentAngles = Vector2.Lerp(this.CurrentAngles, this.targetAngles, this.damper * Time.deltaTime);
            this.CurrentDistance = Mathf.Lerp(this.CurrentDistance, this.targetDistance, this.damper * Time.deltaTime);
            base.transform.rotation = Quaternion.Euler(this.CurrentAngles);
            base.transform.position = this.target.position - (base.transform.forward * this.CurrentDistance);
        }

        public void ChangeTargetDistance(float velocity)
        {
            if (selectViewMode == false)
                this.targetDistance = this.targetDistance - velocity * this.mouseSettings.wheelSensitivity;
            else
                Camera.main.orthographicSize -= velocity * this.mouseSettings.wheelSensitivity;
        }

        /// <summary>
        /// Initialize component.
        /// </summary>
        protected virtual void Initialize()
        {
            transform.eulerAngles = new Vector3(target.position.z * -2f, 0, 0);

            Vector2 vector2 = transform.eulerAngles;
            Vector2 vector21 = vector2;
            this.targetAngles = vector2;
            this.CurrentAngles = vector21;
            float single = Vector3.Distance(base.transform.position, this.target.position);
            float single1 = single;
            this.targetDistance = single;
            this.CurrentDistance = single1;
        }

        public virtual void ResetAngle()
        {
            Vector2 vector2 = new Vector3(MouseTranslate.Instance.areaSettings.center.z * -2f, 0, 0);
            //CurrentAngles = vector2;
            targetAngles = vector2;
            //targetDistance = distanceRange.max;
        }

        public void ChangeAngle(Vector3 _angle)
        {
            targetAngles = _angle;
        }

        public virtual void DirectionAngle(int cubedic)
        {
            switch (cubedic)
            {
                case 0:
                    CurrentAngles = new Vector3(90, 180, 0); // TO
                    targetAngles = CurrentAngles;
                    break;

                case 1:
                    CurrentAngles = new Vector3(-90, 180, 0); // BO
                    targetAngles = CurrentAngles;
                    break;

                case 2:
                    CurrentAngles = new Vector3(0, 0, 0); // BA
                    targetAngles = CurrentAngles;
                    break;

                case 3:
                    CurrentAngles = new Vector3(0, 180, 0); // FR
                    targetAngles = CurrentAngles;
                    break;

                case 4:
                    CurrentAngles = new Vector3(0, 90, 0); // LE
                    targetAngles = CurrentAngles;
                    break;

                case 5:
                    CurrentAngles = new Vector3(0, -90, 0); // RE
                    targetAngles = CurrentAngles;
                    break;
            }
        }

        public virtual void SetAngle(Vector3 m_issueAng)
        {
            CurrentAngles = m_issueAng;
            targetAngles = CurrentAngles;
        }

        public void Trargetsize()
        {
            if (targetDistance != distanceRange.max)
            {
                Vector3 v = new Vector3(1, 1, 1) * (targetDistance / distanceRange.max);
                MouseTranslate.Instance.transform.localScale = v;
            }
        }

        /// <summary>
        /// Late update component.
        /// </summary>
        protected virtual void LateUpdate()
        {
            this.AroundByMouse();
        }

        /// <summary>
        /// Start component.
        /// </summary>
        protected virtual void Start()
        {
            this.Initialize();
        }

        //private void Update()
        //{
        //    selectViewModeCheck = dimViewManager.selectViewMode;
        //}
    }
}