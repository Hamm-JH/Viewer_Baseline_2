using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Control
{
    public class Keyboard_Mouse : AContoller
    {
        List<Coroutine> MouseEventList;
        GraphicRaycaster graphicRaycaster;

        private void Awake()
        {
            MouseEventList = new List<Coroutine>();
            MouseEventList.Add(StartCoroutine(StanbyMouseButton(0)));
            MouseEventList.Add(StartCoroutine(StanbyMouseButton(2)));
        }

        //private void OnEnable()
        //{
        //}

        //private void OnDisable()
        //{
        //    //if(Application.isPlaying)
        //    //{
        //    //    for (int i = 0; i < MouseEventList.Count; i++)
        //    //    {
        //    //        StopCoroutine(MouseEventList[i]);
        //    //    }
        //    //}
        //}

        private IEnumerator StanbyMouseButton(int buttonIndex)
        {
            while(true)
            {
                yield return new WaitForEndOfFrame();
                yield return new WaitUntil(() => Input.GetMouseButtonDown(buttonIndex));

                //Debug.Log($"{buttonIndex} Click");
                if ((int)Manager.MainManager.Instance.SceneStatus < 3) continue;
                if (graphicRaycaster == null)
                {
                    graphicRaycaster = Manager.UIManager.Instance.GetComponent<GraphicRaycaster>();

                }

                if(buttonIndex == 0)
                {
                    GetClickEvent(Input.mousePosition);
                }
                else if(buttonIndex == 2)
                {
                    MouseEventList.Add(StartCoroutine(DragMouseButton(buttonIndex)));
                }
            }
        }

        #region 왼쪽 클릭 이벤트

        private void GetClickEvent(Vector3 mousePosition)
        {
            if((int)Manager.MainManager.Instance.SceneStatus == 3)
            {
                Data.ClickInfo _info = new Data.ClickInfo(
                    Manager.MainManager.Instance.SceneStatus,
                    Data.MouseEventType.Left,
                    Data.InvokedEventType.Down);

                //=====================================================

                // 물리 레이캐스트
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(mousePosition);
                Physics.Raycast(ray, out hit);

                //=====================================================

                // UI 레이캐스트
                PointerEventData pointerEvent = new PointerEventData(null);
                pointerEvent.position = mousePosition;
                List<RaycastResult> grResults = new List<RaycastResult>();

                graphicRaycaster.Raycast(pointerEvent, grResults);

                //=====================================================

                _info.graphicRaycastResults = grResults;        // 그래픽 레이캐스터의 결과 할당

#if UNITY_EDITOR
                if(hit.collider != null)    EditDebug.PrintCLICKRoutine($"Physics raycast hit : {hit.collider.name}");
                EditDebug.PrintCLICKRoutine($"Graphic raycast hits : {grResults.Count.ToString()}");
#endif

                if(hit.collider != null)
                {
                    //_info.clickHit = hit.transform;
                    //_info.singleHit = hit;

                    if(hit.collider.name == "Skew")
                    {
                        Skew _skew = hit.collider.GetComponent<Skew>();

                        Debug.Log(hit.collider.name);
                        Debug.Log($"Skew name : {_skew.name}");

                        Manager.EventClassifier.Instance.OnEvent<Skew>
                            (Control.Status.Click, _skew);
                    }
                    else if(hit.collider.name == "Curve")
                    {
                        Curve _curve = hit.collider.GetComponent<Curve>();

                        Debug.Log(hit.collider.name);
                        Debug.Log($"Curve name : {_curve.name}");

                        Manager.EventClassifier.Instance.OnEvent<Curve>
                            (Control.Status.Click, _curve);
                    }
                    else if (hit.collider.name == "Interactable")
                    {
                        Interactable _interactable = hit.collider.GetComponent<Interactable>();

                        Debug.Log(hit.collider.name);
                        Debug.Log($"Interactable name : {_interactable.name}");

                        Manager.EventClassifier.Instance.OnEvent<Interactable>
                            (Control.Status.Click, _interactable);
                    }
                    else if (hit.collider.name == "CurveSkew")
                    {
                        CurveSkew _curveSkew = hit.collider.GetComponent<CurveSkew>();

                        Debug.Log(hit.collider.name);
                        Debug.Log($"CurveSkew name : {_curveSkew.name}");

                        Manager.EventClassifier.Instance.OnEvent<CurveSkew>
                            (Control.Status.Click, _curveSkew);
                    }

                    //InvokeMouseEvent(_info);
                }
                else
                {
                    //_info.clickHit = null;
                    //_info.singleHit = hit;

                    //InvokeMouseEvent(_info);
                }
            }
        }

        #endregion

        private void InvokeMouseEvent(Data.ClickInfo _event)
        {
            //Debug.Log()
            if( (_event.mouseEvent == Data.MouseEventType.Left)
                && (_event.eventType == Data.InvokedEventType.Down))
            {
                switch(_event.sceneStatus)
                {
                    case Manager.ViewSceneStatus.Ready:
                        //{
                        //    Transform selectedObj = RuntimeData.RootContainer.Instance.selectedInstance.itemTransform;
                        //    //Transform currentedObj = RuntimeData.RootContainer.Instance.currentSelectObject;

                        //    // 선택한 객체가 존재하는 경우
                        //    if(_event.clickHit != null)
                        //    {
                        //        // 이미 선택된 객체가 있을 경우
                        //        if(selectedObj != null)
                        //        {
                        //            // 선택한 객체와 현재 선택한 객체가 같은 경우
                        //            if(selectedObj == _event.clickHit)
                        //            {
                        //                RuntimeData.RootContainer.Instance.selectedInstance.itemTransform = null;

                        //                // 상태 변경
                        //                Manager.MainManager.Instance.visibleOption = InVisibleOption.interactable;
                        //                Manager.MainManager.Instance.SceneStatus = Manager.ViewSceneStatus.ViewAllDamage;
                        //            }
                        //            // 선택한 객체가 다른 경우
                        //            else
                        //            {
                        //                RuntimeData.RootContainer.Instance.selectedInstance.itemTransform = _event.clickHit;
                        //            }
                        //        }
                        //        // 선택된 객체가 존재하지 않을 경우
                        //        else
                        //        {
                        //            RuntimeData.RootContainer.Instance.selectedInstance.itemTransform = _event.clickHit;
                        //        }
                        //    }
                        //    // 선택한 객체가 존재하지 않는 역우
                        //    else
                        //    {
                        //        // 마우스 포인터가 UI 객체를 통과하지 않은 경우 ( 빈 공간을 선택한 경우 )
                        //        if(_event.graphicRaycastResults.Count != 0)
                        //        {
                        //            // TODO PJ : UI로 선택된 객체와 선택된 객체 비교
                        //            // 선택된 객체 : selectedObj;
                        //            // UI에서 선택된 객체 찾아야함
                        //            for (int i = 0; i < _event.graphicRaycastResults.Count; i++)
                        //            {
                        //                Debug.Log($"Gr cast hit {i} : {_event.graphicRaycastResults[i].gameObject.name}");
                        //            }
                        //        }
                        //    }
                        //}
                        break;

                    case Manager.ViewSceneStatus.ViewAllDamage:
                        {

                        }
                        break;
                }
            }
        }

        


        #region 휠 드래그 이벤트
        private IEnumerator DragMouseButton(int buttonIndex)
        {
            Vector3 posCache = new Vector3(0, 0, 0);

            // btnIndex : 2
            Transform camTransform;

            while(true)
            {
                yield return new WaitForEndOfFrame();
                if (Input.GetMouseButton(buttonIndex).Equals(false)) break;

                float inputInvert = IsDragInverted ? -1 : 1;
                float mouseDeltaX = inputInvert * Input.GetAxis("Mouse X") / DragSensitivity;
                float mouseDeltaY = inputInvert * Input.GetAxis("Mouse Y") / DragSensitivity;

                //Debug.Log(Input.GetAxis("Mouse X"));
                //Debug.Log(mouseDeltaX);

                if(buttonIndex == 2)
                {
                    camTransform = Manager.MainManager.Instance.MainCamTransform;
                    posCache = camTransform.position;

                    // 메인 카메라 각도 적용
                    camTransform.eulerAngles = Manager.MainManager.Instance.MainCamera.transform.eulerAngles;
                    camTransform.Translate(Vector3.right * mouseDeltaX);
                    camTransform.Translate(Vector3.up * mouseDeltaY);

                    // 경계에서 벗어난 경우 이전 위치값으로 유지
                    if(IsDragVectorOutofBounds(camTransform.position, PanningBounds).Equals(true))
                    {
                        camTransform.position = posCache;
                    }
                }
            }
        }

        private bool IsDragVectorOutofBounds(Vector3 dragVector, Bounds dragBound)
        {
            Vector3 minDragBound = dragBound.min;
            Vector3 maxDragBound = dragBound.max;

            if (dragVector.x < minDragBound.x ||
                dragVector.y < minDragBound.y ||
                dragVector.z < minDragBound.z)
            {
                return true;
            }
            else if (dragVector.x > maxDragBound.x ||
                     dragVector.y > maxDragBound.y ||
                     dragVector.z > maxDragBound.z)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

    }
}
