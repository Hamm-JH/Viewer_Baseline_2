using Definition.Control;
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
        public float panSpeed = .5f;
        public float zoomSpeed = 10f;
        [SerializeField] private Vector3 targetOffset = Vector3.zero;
        [SerializeField] private Vector3 targetPosition;

        // Use this for initialization
        void Start()
        {
            if (target != null) transform.LookAt(target);
        }

        private void InDrag(int btn, Vector2 delta)
		{
            float axisX = delta.x;
            float axisY = delta.y;

            if(target == null)
			{
                Debug.LogWarning("cam target is null");
                return;
			}

            targetPosition = target.position + targetOffset;

            if(btn == 0)
			{
                transform.RotateAround(target.position, Vector3.up, axisX * orbitSpeed);

                float pitchAngle = Vector3.Angle(Vector3.up, transform.forward);
                float pitchDelta = axisY * orbitSpeed;
                float newAngle = Mathf.Clamp(pitchAngle + pitchDelta, 0f, 180f);
                pitchDelta = newAngle - pitchAngle;
                transform.RotateAround(target.position, transform.right, pitchDelta);
            }
            else if(btn == 1)
			{
                Vector3 offset = transform.right * axisX * panSpeed + transform.up * -axisY * panSpeed;
                Vector3 newTargetOffset = Vector3.ClampMagnitude(targetOffset + offset, maxOffsetDistance);
                transform.position += newTargetOffset - targetOffset;
                targetOffset = newTargetOffset;
            }
		}

        private void InFocus(Vector3 mousePos, float delta)
		{
            float scroll = delta;

            transform.position += transform.forward * scroll * zoomSpeed;
        }
    }
}
