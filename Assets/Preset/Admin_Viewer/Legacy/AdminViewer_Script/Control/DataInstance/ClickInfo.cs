using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Control.Data
{
    public class ClickInfo
    {
        public Manager.ViewSceneStatus sceneStatus;
        public MouseEventType mouseEvent;
        public InvokedEventType eventType;

        public Transform clickHit;      // 물리 레이캐스트 결과
        public RaycastHit singleHit;    // 물리 레이캐스트 히트

        public List<RaycastResult> graphicRaycastResults;   // UI 레이캐스트 결과

        public ClickInfo(Manager.ViewSceneStatus _sceneStatus, MouseEventType _mouseEvent, InvokedEventType _invokedType)
        {
            sceneStatus = _sceneStatus;
            mouseEvent = _mouseEvent;
            eventType = _invokedType;
            clickHit = null;
            graphicRaycastResults = null;
        }
    }
}
