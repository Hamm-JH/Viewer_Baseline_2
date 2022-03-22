using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace MGS.UCamera
{
    public class MouseTranslate : Singleton<MouseTranslate>
    {
        /// <summary>
        /// Target camera for translate direction.
        /// </summary>
        public Transform targetCamera;

        /// <summary>
        /// Settings of mouse button and pointer.
        /// </summary>
        public MouseSettings mouseSettings = new MouseSettings(0, 1f, 0f);

        /// <summary>
        /// Settings of move area.
        /// </summary>
        public PlaneArea areaSettings = new PlaneArea(new Vector3(0, 0, 0), 10f, 10f, 10f);

        /// <summary>
        /// Damper for move.
        /// </summary>
        [Range(0f, 10f)]
        public float damper = 5f;

        /// <summary>
        /// Target offset base area center.
        /// </summary>
        protected Vector3 targetOffset;

        // *************** 추가
        public Transform centerTransform;

        /// <summary>
        /// Current offset base area center.
        /// </summary>
        public Vector3 CurrentOffset
        {
            get;
            protected set;
        }

        public MouseTranslate()
        {
        }

        public void moveTraget(Vector3 targetcenter)
        {
            targetOffset = targetcenter - areaSettings.center;
            CurrentOffset = targetOffset;
        }

        /// <summary>
        /// Initialize component.
        /// </summary>
        protected virtual void Initialize()
        {
            Vector3 vector3 = base.transform.position - this.areaSettings.center;
            Vector3 vector31 = vector3;
            targetOffset = vector3;
            CurrentOffset = vector31;
        }
        
        public virtual void Resetposition()
        {
            transform.position = areaSettings.center;
            Initialize();
        }

        /// <summary>
        /// Component start.
        /// </summary>
        protected virtual void Start()
        {
            Initialize();
        }

        /// <summary>
        /// Translate this gameobject by mouse.
        /// </summary>
        protected void TranslateByMouse()
        {
            if (Input.GetMouseButton(this.mouseSettings.mouseButtonID))
            {
                float axis = Input.GetAxis("Mouse X") * this.mouseSettings.pointerSensitivity;
                float single = Input.GetAxis("Mouse Y") * this.mouseSettings.pointerSensitivity;
                this.targetOffset = this.targetOffset - (this.targetCamera.right * axis);
                this.targetOffset = this.targetOffset - (Vector3.Cross(this.targetCamera.right, Vector3.up) * single);
                this.targetOffset = this.targetOffset - (Vector3.Cross(this.targetCamera.forward, Vector3.left) * single); // 상하 이동 - sjw
                this.targetOffset.x = Mathf.Clamp(this.targetOffset.x, -this.areaSettings.width, this.areaSettings.width);
                this.targetOffset.z = Mathf.Clamp(this.targetOffset.z, -this.areaSettings.length, this.areaSettings.length);
                this.targetOffset.y = Mathf.Clamp(this.targetOffset.y, -this.areaSettings.height, this.areaSettings.height); // 상하 이동 - sjw
            }
            this.CurrentOffset = Vector3.Lerp(this.CurrentOffset, this.targetOffset, this.damper * Time.deltaTime);
            base.transform.position = this.areaSettings.center + this.CurrentOffset;
        }

        /// <summary>
        /// 이동방향 벡터값
        /// </summary>
        /// <param name="m_Direction"></param>
        public virtual void Keytrans(Vector3 m_Direction)
        {
            var dir = Quaternion.Euler(0, AroundCamera.Instance.CurrentAngles.y, 0);

            if (Input.GetKey(KeyCode.LeftShift))
            {
                targetOffset = this.targetOffset - (dir * m_Direction * mouseSettings.pointerSensitivity * 2);
                CurrentOffset = targetOffset;
                transform.position = this.areaSettings.center + CurrentOffset;
            }
            else
            {
                targetOffset = this.targetOffset - (dir * m_Direction * mouseSettings.pointerSensitivity);
                CurrentOffset = targetOffset;
                transform.position = this.areaSettings.center + CurrentOffset;
            }
        }

        /// <summary>
        /// Component update.
        /// </summary>
        protected virtual void Update()
        {
            this.TranslateByMouse();
            areaSettings.center = centerTransform.position;
        }
    }
}