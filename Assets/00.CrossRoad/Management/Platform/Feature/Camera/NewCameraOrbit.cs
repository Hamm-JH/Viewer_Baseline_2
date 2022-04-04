using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCameraOrbit : MonoBehaviour
{

    private Vector3 currentMouse;
    private Vector3 mouseDelta;
    private Vector3 lastMouse;

    public static bool activeRotate = false;
    public static Vector3 pivotPoint; //

    private void Update()
    {
        mouseDelta = lastMouse - currentMouse;

        if (Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButtonDown(2))
        {
            activeRotate = true;
        }


        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            activeRotate = false;
        }

        currentMouse = Input.mousePosition;

        mouseDelta = lastMouse - currentMouse;
    }


    void LateUpdate()
    {
        if (activeRotate)
        {
            transform.RotateAround(pivotPoint, Vector3.up, mouseDelta.x * -.1f);
            transform.RotateAround(pivotPoint, transform.right, mouseDelta.y * .1f);
        }

        lastMouse = Input.mousePosition;
    }
}
