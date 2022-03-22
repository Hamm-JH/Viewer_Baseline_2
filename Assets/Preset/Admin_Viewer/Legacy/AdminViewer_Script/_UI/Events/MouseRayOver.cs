using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// EventSystem : 마우스가 패널 위에 포인터 위치시 이벤트 변경하는 코드
/// </summary>
public class MouseRayOver : MonoBehaviour
{
    RaycastHit hit;
    GraphicRaycaster uiRay;

    [SerializeField] Canvas m_canvas;
    [SerializeField] GraphicRaycaster m_gr;
    [SerializeField] PointerEventData m_ped;

    // Start is called before the first frame update
    void Start()
    {
        m_ped = new PointerEventData(null);
    }

    // Update is called once per frame
    void Update()
    {

        List<RaycastResult> results = HoveringCheck();
        //m_ped.position = Input.mousePosition;
        //List<RaycastResult> results = new List<RaycastResult>();
        //m_gr.Raycast(m_ped, results);

        if(results.Count > 0)
        {
            RuntimeData.RootContainer.Instance.mainCam.isCanControl = false;
        }
        else
        {
            RuntimeData.RootContainer.Instance.mainCam.isCanControl = true;
        }
    }

    public List<RaycastResult> HoveringCheck()
    {
        m_ped.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        m_gr.Raycast(m_ped, results);

        return results;
    }
}
