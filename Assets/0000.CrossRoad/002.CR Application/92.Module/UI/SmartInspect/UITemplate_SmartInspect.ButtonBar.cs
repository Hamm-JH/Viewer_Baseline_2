using System.Collections;
using System.Collections.Generic;

namespace Module.UI
{
    using Definition;
    using DG.Tweening;
    using Management;
    using UnityEngine;
    using UnityEngine.UI;
    using View;

    public partial class UITemplate_SmartInspect : AUI
    {
        private void ButtonBar_OnSelect(Interactable _ui)
        {
            UI_Selectable ui = (UI_Selectable)_ui;

            ButtonBar_OnSelect(ui.Data.m_toggleIndex);
        }

        private void ButtonBar_OnSelect(int index)
        {
            Image img_bg = null;
            Image img_icon = null;

            int lIndex = m_bottomBar.m_btnMenu_bottomBar.Count;
            for (int i = 0; i < lIndex; i++)
            {
                bool isTarget = i == index ? true : false;

                img_bg = m_bottomBar.m_btnMenu_bottomBar[i].btn_menu.GetComponent<Image>();
                img_icon = m_bottomBar.m_btnMenu_bottomBar[i].imgs[0];

                m_uiResources.m_bottomBar.SetColor_btnBar(img_bg, img_icon, i, isTarget);

            }
        }
        
        private void ButtonBar_MoveUI(Interactable _ui)
        {
            UI_Selectable ui = (UI_Selectable)_ui;

            if(ui.Data.m_isMovedDown)
            {
                ui.Data.m_isMovedDown = false;
                ui.ChildPanel.transform.DOMoveY(43, 1);
            }
            else
            {
                ui.Data.m_isMovedDown = true;
                ui.ChildPanel.transform.DOMoveY(-96, 1);
            }
            //Debug.Log(ui.ChildPanel.transform.position);
            // 43 up
            // -96 down
        }

        private void ButtonBar_ZoomIn()
        {
            Debug.Log("111");

            ButtonBar_Zoom(1);
        }

        private void ButtonBar_ZoomOut()
        {
            Debug.Log("222");
            ButtonBar_Zoom(-1);
        }

        private void ButtonBar_Zoom(int index)
        {
            // index :: 1 줌인
            // index ;; -1 줌아웃

            float maxDistance = ContentManager.Instance.generalSettings.MaxCameraDistance;

            GameObject selected = EventManager.Instance._SelectedObject;
            Transform camTransform = MainManager.Instance.MainCamera.transform;
            Vector3 camPos = camTransform.position;
            Vector3 camAngle = camTransform.rotation.eulerAngles;

            Vector3 targetPos = default(Vector3);
            if(selected != null)
            {
                targetPos = selected.transform.position;
            }
            else
            {
                targetPos = default(Vector3);
            }

            //float camToTargetDistance = Vector3.Distance(camPos, targetPos);

            //Vector3 directionNormal = Vector3.Cross(camPos, targetPos);

            Vector3 heading = targetPos - camPos;
            float distance = heading.magnitude;
            Vector3 direction = heading / distance;

            //direction = direction * camAngle;

            // 이동거리 총 길이 / 10
            float range = (float)(maxDistance / 10);

            // 현재 거리에서 근접시 0 아래로 내려가면
            if(distance - (range * index) <= 0)
            {
                range = 0;
            }

            // 거리 값에서 이동거리를 index에 따라 +-
            // 거리값을 최소 최대값에 보간한다.
            //range = Mathf.Clamp(distance + index * range, 0, maxDistance);

            camTransform.Translate(direction * (range * index), Space.World);
        }
    }
}
