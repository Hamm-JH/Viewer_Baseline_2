using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Indicator.Element
{
    public class BPMM_ViewAllDamage_Element : AElement, IDragHandler, IScrollHandler
    {
        public RectTransform rootElementPanel;
        [SerializeField] private Scrollbar scrollBar;

        private List<Issue.AIssue> issueList;

        private UI.DragCall dragCache;


        private void Awake()
        {
            dragCache = new UI.DragCall();
        }

        public override void SetElement(params object[] arg)
        {
            if (arg.Length >= 1 && arg[0].GetType().Equals(typeof(List<Issue.AIssue>)))
            {
                issueList = arg[0] as List<Issue.AIssue>;
            }

            ClearRecords();
            CreateRecords(issueList);
            SetScrollBar();
        }

        private void ClearRecords()
        {
            int index = rootElementPanel.childCount;
            for (int i = 0; i < index; i++)
            {
                Destroy(rootElementPanel.GetChild(i).gameObject);
            }
        }

        private void CreateRecords(List<Issue.AIssue> _issues)
        {
            int internalIndex = 0;

            int index = _issues.Count;
            for (int i = 0; i < index; i++)
            {
                if (_issues[i].GetType().Equals(typeof(Issue.DamagedIssue)))
                {
                    internalIndex++;

                    //Debug.Log("damaged issue");

                    GameObject obj = Instantiate<GameObject>(Resources.Load<GameObject>("Indicator/BPMM_RecordElement"));
                    obj.transform.SetParent(rootElementPanel);

                    BPMM_VAD_Record record = obj.GetComponent<BPMM_VAD_Record>();
                    record.SetElement(_issues[i], internalIndex);
                }
                //else if(_issues[i].GetType().Equals(typeof(Issue.RecoveredIssue)))
                //{
                //    Debug.Log("recovered issue");
                //}
            }
        }

        private void SetScrollBar()
        {
            scrollBar.value = 0;

            dragCache.Set(
                _direction: UI.DragDirection.Y,
                _vector: UI.DragVector.TopToBottom,
                _method: UI.DragMethod.ScrollBar,
                _target: rootElementPanel
                );

            UI.Drag.InitializeScrollBar_barSize(
                scrollBar: scrollBar,
                dragCall: dragCache);
        }

        #region Events

        public void OnDrag(PointerEventData eventData)
        {
            float velocity = Input.GetAxis("Mouse Y");

            dragCache.Set(
                _direction: UI.DragDirection.Y,
                _vector: UI.DragVector.TopToBottom,
                _method: UI.DragMethod.MouseWheel,
                _moveVelocity: velocity,
                _target: rootElementPanel);

            UI.Drag.OnControl(dragCache);

            UI.Drag.UpdateScrollBar_barValue(scrollBar, dragCache);
        }

        public void OnScroll(PointerEventData eventData)
        {
            RuntimeData.RootContainer.Instance.IsScrollInPanel = true;

            float velocity = -Input.GetAxis("Mouse ScrollWheel");

            dragCache.Set(
                _direction: UI.DragDirection.Y,
                _vector: UI.DragVector.TopToBottom,
                _moveVelocity: velocity,
                _method: UI.DragMethod.MouseWheel,
                _target: rootElementPanel,
                _dragResist: 1f
                );

            UI.Drag.OnControl(dragCache);

            // 휠 스크롤시 스크롤바 상태 갱신
            UI.Drag.UpdateScrollBar_barValue(scrollBar, dragCache);
        }

        public void VerticalScrollValueChange()
        {
            float scrollBarValue = scrollBar.value;

            dragCache.Set(
                _direction: UI.DragDirection.Y,
                _vector: UI.DragVector.TopToBottom,
                _moveVelocity: scrollBarValue,
                _method: UI.DragMethod.ScrollBar,
                _target: rootElementPanel
                );

            //UI.Drag.OnChangeScrollPosition(scrollBarValue, dragCache);

            UI.Drag.OnControl(dragCache);
        }

        #endregion

        //public void ScrollLayoutGroup(float velocity)
        //{
        //    dragCache.Set(
        //        _direction: UI.DragDirection.Y,
        //        _vector: UI.DragVector.TopToBottom,
        //        _moveVelocity: -velocity,
        //        _target: rootElementPanel,
        //        _dragResist: 1f
        //        );

        //    CallDragTransform(dragCache);
        //}
    }
}
