using System;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace MGS.UCamera
{
    /// <summary>
    /// Camera rotate around and align to target gameobject.
    /// </summary>
    [AddComponentMenu("MGS/UCamera/AroundAlignCamera")]
    public class AroundAlignCamera : AroundCamera
    {
        public bool isCanControl;

        /// <summary>
        /// Damper for align.
        /// </summary>
        [Range(0f, 5f)]
        public float alignDamper = 2f;

        /// <summary>
        /// Threshold of linear adsorbent.
        /// </summary>
        [Range(0f, 1f)]
        public float threshold = 0.1f;

        /// <summary>
        /// Last rotate angle of camera.
        /// </summary>
        protected Vector3 lastAngles;

        /// <summary>
        /// Direction record of camera.
        /// </summary>
        protected Vector3 currentDirection;

        /// <summary>
        /// Direction record of camera.
        /// </summary>
        protected Vector3 targetDirection;

        /// <summary>
        /// Direction record of camera.
        /// </summary>
        protected Vector3 lastDirection;

        /// <summary>
        /// Last distance frome camera to target.
        /// </summary>
        protected float lastDistance;

        /// <summary>
        /// Speed of camera move.
        /// </summary>
        protected float anglesSpeed;

        /// <summary>
        /// Speed of camera move.
        /// </summary>
        protected float directionSpeed;

        /// <summary>
        /// Speed of camera move.
        /// </summary>
        protected float distanceSpeed;

        /// <summary>
        /// Offset record.
        /// </summary>
        protected float anglesOffset;

        /// <summary>
        /// Offset record.
        /// </summary>
        protected float directionOffset;

        /// <summary>
        /// Offset record.
        /// </summary>
        protected float distanceOffset;

        /// <summary>
        /// MoveTowards to linear adsorbent align?
        /// </summary>
        protected bool linearAdsorbent;

        /// <summary>
        /// Camera is auto aligning.
        /// </summary>
        public bool IsAligning
        {
            get;
            protected set;
        }

        public void SwitchControl(bool isOn)
        {
            isCanControl = isOn;
        }

        public AroundAlignCamera()
        {
        }
        public float ClampAngle(float angle)
        {
            if (angle < -360)
                angle += 360;
            if (angle > 360)
                angle -= 360;

            return angle;
        }

        public void AlignViewToTarget(Vector3 center, Vector2 angles, float distance)
        {
            float y = ClampAngle(angles.y);

            var rotation = Quaternion.Euler(y, angles.x, 0);
            var position = rotation * new Vector3(0.0f, 0.0f, -distance) + center;

            transform.rotation = rotation;
            transform.position = position;
        }

        /// <summary>
        /// Align camera veiw to target.
        /// </summary>
        /// <param name="center">Around center.</param>
        /// <param name="angles">Rotate angles.</param>
        /// <param name="distance">Distance from camera to target.</param>
        public void AlignVeiwToTarget(Transform center, Vector2 angles, float distance)
        {
            //if (target && Input.GetMouseButton(0))
            //{
                //x += Input.GetAxis("Mouse X") * xSpeed * 0.08f;
                //y -= Input.GetAxis("Mouse Y") * ySpeed * 0.08f;
                float y = ClampAngle(angles.y);

                var rotation = Quaternion.Euler(y, angles.x, 0);
                var position = rotation * new Vector3(0.0f, 0.0f, -distance) + center.position;

                transform.rotation = rotation;
                transform.position = position;
            //}



            //this.target = center;
            //this.targetAngles = angles;
            //this.targetDistance = distance;
            //while (this.targetAngles.y - base.CurrentAngles.y > 180f)
            //{
            //    this.targetAngles.y -= 360f;
            //}
            //while (this.targetAngles.y - base.CurrentAngles.y < -180f)
            //{
            //    this.targetAngles.y += 360f;
            //}
            //Vector3 vector3 = base.transform.position - this.target.position;
            //this.currentDirection = vector3.normalized;
            //vector3 = Quaternion.Euler(this.targetAngles) * Vector3.back;
            //this.targetDirection = vector3.normalized;
            //base.CurrentDistance = Vector3.Distance(base.transform.position, this.target.position);
            //Vector2 currentAngles = this.targetAngles - base.CurrentAngles;
            //this.anglesOffset = Mathf.Max(currentAngles.magnitude, 1E-05f);
            //vector3 = this.targetDirection - this.currentDirection;
            //this.directionOffset = Mathf.Max(vector3.magnitude, 1E-05f);
            //this.distanceOffset = Mathf.Max(Mathf.Abs(this.targetDistance - base.CurrentDistance), 1E-05f);
            //this.linearAdsorbent = false;
            //this.IsAligning = true;
            //if (this.OnAlignStart != null)
            //{
            //    this.OnAlignStart();
            //}
        }

        /// <summary>
        /// Align camera veiw to target.
        /// </summary>
        /// <param name="alignTarget">Target of camera align.</param>
        public void AlignVeiwToTarget(AlignTarget alignTarget)
        {
            this.AlignVeiwToTarget(alignTarget.center, alignTarget.angles, alignTarget.distance);
            this.angleRange = alignTarget.angleRange;
            this.distanceRange = alignTarget.distanceRange;
        }

        /// <summary>
        /// Auto align camera veiw to target.
        /// </summary>
        protected void AutoAlignView()
        {
            Vector2 currentAngles = this.targetAngles - base.CurrentAngles;
            float single = currentAngles.magnitude;
            Vector3 vector3 = this.targetDirection - this.currentDirection;
            float single1 = vector3.magnitude;
            float single2 = Mathf.Abs(this.targetDistance - base.CurrentDistance);
            if (single >= 1E-05f || single1 >= 1E-05f || single2 >= 1E-05f)
            {
                if (!this.linearAdsorbent)
                {
                    this.lastAngles = base.CurrentAngles;
                    this.lastDirection = this.currentDirection;
                    this.lastDistance = base.CurrentDistance;
                    base.CurrentAngles = Vector2.Lerp(base.CurrentAngles, this.targetAngles, this.alignDamper * Time.deltaTime);
                    this.currentDirection = Vector3.Lerp(this.currentDirection, this.targetDirection, this.alignDamper * Time.deltaTime);
                    base.CurrentDistance = Mathf.Lerp(base.CurrentDistance, this.targetDistance, this.alignDamper * Time.deltaTime);
                    if (single / this.anglesOffset < this.threshold && single1 / this.directionOffset < this.threshold && single2 / this.distanceOffset < this.threshold)
                    {
                        currentAngles = base.CurrentAngles - this.lastAngles;
                        this.anglesSpeed = currentAngles.magnitude / Time.deltaTime;
                        vector3 = this.currentDirection - this.lastDirection;
                        this.directionSpeed = vector3.magnitude / Time.deltaTime;
                        this.distanceSpeed = Mathf.Abs(base.CurrentDistance - this.lastDistance) / Time.deltaTime;
                        this.linearAdsorbent = true;
                    }
                }
                else
                {
                    base.CurrentAngles = Vector2.MoveTowards(base.CurrentAngles, this.targetAngles, this.anglesSpeed * Time.deltaTime);
                    this.currentDirection = Vector3.MoveTowards(this.currentDirection, this.targetDirection, this.directionSpeed * Time.deltaTime);
                    base.CurrentDistance = Mathf.MoveTowards(base.CurrentDistance, this.targetDistance, this.distanceSpeed * Time.deltaTime);
                }
                base.transform.position = this.target.position + (this.currentDirection.normalized * base.CurrentDistance);
                base.transform.rotation = Quaternion.Euler(base.CurrentAngles);
            }
            else
            {
                this.IsAligning = false;
                if (this.OnAlignEnd != null)
                {
                    this.OnAlignEnd();
                    return;
                }
            }
        }

        public void Init()
        {
            Initialize();
        }

        /// <summary>
        /// Late update component.
        /// </summary>
        protected override void LateUpdate()
        {
            if(isCanControl)
            {
                if (this.IsAligning)
                {
                    this.AutoAlignView();
                    return;
                }
                base.AroundByMouse();
                Trargetsize();
            }
        }

        public void UpdateView()
        {
            if (this.IsAligning)
            {
                this.AutoAlignView();
                return;
            }
            base.AroundByMouse();
            Trargetsize();
        }

        /// <summary>
        /// End align event.
        /// </summary>
        public event Action OnAlignEnd;

        /// <summary>
        /// Start align event.
        /// </summary>
        public event Action OnAlignStart;
    }
}