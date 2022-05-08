using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOrbit : MonoBehaviour
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



    void Update()
    {
        //targetPosition = target.position + targetOffset;

        float axisX = Input.GetAxis("Mouse X");
        float axisY = Input.GetAxis("Mouse Y");
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (Input.GetMouseButtonUp(0))
		{
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit, 100))
			{
                target.gameObject.SetActive(true);
                target.position = hit.point;
			}
			else
			{
                target.gameObject.SetActive(true);
			}
		}

        if (target != null)
        {
            targetPosition = target.position + targetOffset;

            // Left Mouse to Orbit
            if (Input.GetMouseButton(0))
            {
                transform.RotateAround(target.position, Vector3.up, axisX * orbitSpeed);
                //transform.RotateAround(targetPosition, Vector3.up, Input.GetAxis("Mouse X") * orbitSpeed);

                float pitchAngle = Vector3.Angle(Vector3.up, transform.forward);
                float pitchDelta = -axisY * orbitSpeed;
                float newAngle = Mathf.Clamp(pitchAngle + pitchDelta, 0f, 180f);
                pitchDelta = newAngle - pitchAngle;
                transform.RotateAround(target.position, transform.right, pitchDelta);
                //transform.RotateAround(targetPosition, transform.right, pitchDelta);
            }
            // Right Mouse To Pan
            if (Input.GetMouseButton(1))
            {
                Vector3 offset = transform.right * -axisX * panSpeed + transform.up * -axisY * panSpeed;
                Vector3 newTargetOffset = Vector3.ClampMagnitude(targetOffset + offset, maxOffsetDistance);
                transform.position += newTargetOffset - targetOffset;
                targetOffset = newTargetOffset;
            }


            // Scroll to Zoom
            transform.position += transform.forward * scroll * zoomSpeed;

        }
    }
}// CLASS ```