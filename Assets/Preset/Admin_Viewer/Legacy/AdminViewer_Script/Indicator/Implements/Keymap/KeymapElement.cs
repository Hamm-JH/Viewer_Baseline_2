using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Indicator.Element
{
    public class KeymapElement : AElement
    {
        public RectTransform rootPanel;
        public RectTransform subRootPanel;

        public Transform clickedTransform;

        #region Event

        public override void OnPointerEnter(PointerEventData eventData)
        {
            
            //EventSystem.current.
            #region Debug
#if UNITY_EDITOR
            EditDebug.PrintKMEventRoutine($"keymap pointer enter");
#endif
            #endregion

            RuntimeData.RootContainer.Instance.mainCam.isCanControl = false;
            RuntimeData.RootContainer.Instance.subCam.isCanControl = true;

            //Debug.Log($"screen position : {eventData.pointerCurrentRaycast.screenPosition}");
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            #region Debug
#if UNITY_EDITOR
            EditDebug.PrintKMEventRoutine($"keymap pointer exit");
#endif
            #endregion

            RuntimeData.RootContainer.Instance.mainCam.isCanControl = true;
            RuntimeData.RootContainer.Instance.subCam.isCanControl = false;

            //Debug.Log($"screen position : {eventData.pointerCurrentRaycast.screenPosition}");
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            #region Debug
#if UNITY_EDITOR
            EditDebug.PrintKMEventRoutine($"keymap pointer down");
#endif
            #endregion
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            #region Debug
#if UNITY_EDITOR
            EditDebug.PrintKMEventRoutine($"keymap pointer click");
#endif
            #endregion

            Vector2 _clickPosition = eventData.pointerCurrentRaycast.screenPosition;        // 클릭위치

            RectTransform keymapPanel = gameObject.GetComponent<RectTransform>();           // 키맵 패널

            float screenWidth = Screen.width;
            float screenHeight = Screen.height;

            int defScreenWidth = 1920;
            int defScreenHeight = 1080;

            Vector2 panelPivot = keymapPanel.position;         // 패널 중심위치
            Vector2 panelOffsetMin = new Vector2(              // 패널 최소위치
                keymapPanel.offsetMin.x / defScreenWidth * screenWidth,
                keymapPanel.offsetMin.y / defScreenHeight * screenHeight
                );
            Vector2 panelOffsetMax = new Vector2(              // 패널 최대위치
                keymapPanel.offsetMax.x / defScreenWidth * screenWidth,
                keymapPanel.offsetMax.y / defScreenHeight * screenHeight
                );

            Vector2 panelMinPoint = panelPivot + panelOffsetMin;    // 패널 최소점
            Vector2 panelMaxPoint = panelPivot + panelOffsetMax;    // 패널 최대점

            Vector2 _panelClickPosition = _clickPosition - panelMinPoint;   // 패널 클릭위치

            Vector2 panelSize = new Vector2(                        // 패널 크기
                keymapPanel.sizeDelta.x / defScreenWidth * screenWidth,
                keymapPanel.sizeDelta.y / defScreenHeight * screenHeight
                );              

            Vector2 _normalizeClickPosition = _panelClickPosition / panelSize;  // 노말 변환된 패널 클릭위치

            //Debug.Log($"panel pivot : {panelPivot}");               // 패널의 중심위치를 구한다.       // scale O scale 변환에서도 정상으로 뜸

            //Debug.Log($"panel offset min : {panelOffsetMin}");      // 패널의 최소위치를 구한다.       // 수동 변환 필요
            //Debug.Log($"panel offset max : {panelOffsetMax}");      // 패널의 최대위치를 구한다.       // 수동 변환 필요
            //Debug.Log($"screen width : {screenWidth}");
            //Debug.Log($"screen height : {screenHeight}");

            //Debug.Log($"panel min point : {panelMinPoint}");        // 패널의 최소점을 구한다.         // 수동 변환 결과 O
            //Debug.Log($"panel max point : {panelMaxPoint}");        // 패널의 최대점을 구한다.         // 수동 변환 결과 O

            //Debug.Log($"click position : {_clickPosition}");        // 현재 ViewScene 상에 클릭위치를 구한다.   // O
            //Debug.Log($"click panel position : {_panelClickPosition}"); // 패널 클릭위치를 구한다.   // O

            //Debug.Log($"panel size ; {panelSize}");                 // 패널의 크기를 구한다.

            //Debug.Log($"normalize click panel position : {_normalizeClickPosition}");   // 0 ~ 1 사이값으로 변환된 표준위치값을 구한다.

            Camera subCam = RuntimeData.RootContainer.Instance.subCam.GetComponent<Camera>();

            Vector2 cameraViewRect = new Vector2(subCam.pixelWidth, subCam.pixelHeight);
            Vector2 translatedClickCamPosition = _normalizeClickPosition * cameraViewRect;

            //Debug.Log($"pixel Width : {subCam.pixelWidth}");
            //Debug.Log($"pixel Height : {subCam.pixelHeight}");
            //Debug.Log($"scaled pixel width : {subCam.scaledPixelWidth}");
            //Debug.Log($"scaled pixel height : {subCam.scaledPixelHeight}");

            //Debug.Log($"cameraViewRect : {cameraViewRect}");
            //Debug.Log($"translated click camera position : {translatedClickCamPosition}");

            RaycastHit hit;
            Ray ray = subCam.ScreenPointToRay(translatedClickCamPosition);
            //Physics.Raycast(ray, out hit);

            if(Physics.Raycast(ray, out hit))
            {
                //Debug.Log("Hello");
                //Debug.Log(hit.collider.name);

                clickedTransform = hit.collider.transform;

                Manager.EventClassifier.Instance.OnEvent<KeymapElement>(Control.Status.Click, gameObject.GetComponent<KeymapElement>(), eventData);

                //Manager.EventClassifier.Instance.OnEvent<MP2_DmgListElement>(Control.Status.Click, gameObject.GetComponent<MP2_DmgListElement>(), eventData);
            }
            else
            {
                clickedTransform = null;
            }
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            #region Debug
#if UNITY_EDITOR
            EditDebug.PrintKMEventRoutine($"keymap pointer up");
#endif
            #endregion
        }

        #endregion

        public void PanelOff()
        {
            rootPanel.gameObject.SetActive(false);
        }

        public void SwitchPanel()
        {
            rootPanel.gameObject.SetActive(rootPanel.gameObject.activeSelf ? false : true);
        }

		private void Update()
		{
            PointerEventData e_data = new PointerEventData(EventSystem.current);
            e_data.position = Input.mousePosition;

            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(e_data, results);

            // UI 호버링 이벤트에 걸린 요소가 1 이상일때
            if(results.Count > 0)
			{
                var toFind = results.FindAll(x => x.gameObject.name == "KeymapImage");

                // KeymapImage가 호버링 이벤트에 걸렸을 경우
                if(toFind.Count > 0)
				{
                    // 회전 이벤트 활성화
                    RuntimeData.RootContainer.Instance.mainCam.isCanControl = false;
                    RuntimeData.RootContainer.Instance.subCam.isCanControl = true;
                }
                else
				{
                    RuntimeData.RootContainer.Instance.mainCam.isCanControl = true;
                    RuntimeData.RootContainer.Instance.subCam.isCanControl = false;
                }
			}
            else
			{
                RuntimeData.RootContainer.Instance.mainCam.isCanControl = true;
                RuntimeData.RootContainer.Instance.subCam.isCanControl = false;
            }

            return;
            if (EventSystem.current.currentSelectedGameObject != null)
			{
                //Debug.Log(EventSystem.current.currentSelectedGameObject.name);
                //EventSystem.current.currentInputModule
                //Debug.Log(new PointerEventData(EventSystem.current).pointerCurrentRaycast.gameObject);

                //Debug.Log(new PointerEventData(EventSystem.current).hovered[0]);
                //Debug.Log(EventSystem.current.currentInputModule)

                ////PointerEventData e_data = new PointerEventData(EventSystem.current);
                ////e_data.position = Input.mousePosition;
                //
                ////var results = new List<RaycastResult>();
                ////EventSystem.current.RaycastAll(e_data, results);

                //var pEventData = new PointerEventData();
                //new PointerEventData(EventSystem.current).pointerCurrentRaycast
			}
            else
			{
                Debug.Log($"Selected object null");
			}
            return;
            Debug.Log( new PointerEventData(EventSystem.current));
            return;
            var eData = new PointerEventData(EventSystem.current).pointerCurrentRaycast;

            Debug.Log($"***** name : {eData.gameObject.name}");

            if(eData.gameObject.name == "")
			{

			}
        }
	}
}
