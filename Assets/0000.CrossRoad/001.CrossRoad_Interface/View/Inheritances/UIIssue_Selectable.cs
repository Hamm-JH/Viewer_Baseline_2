using Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace View
{
    /// <summary>
    /// Waypoint_Indicator에서 생성된 인스턴스가 중개자로서 이 인스턴스를 할당한다.
    /// </summary>
    public partial class UIIssue_Selectable : Interactable, IPointerEnterHandler, IPointerExitHandler
    {
        /// <summary>
        /// Waypoint_Indicator에서 생성된 ui중에서 main 인스턴스가 이 변수를 할당받는다.
        /// </summary>
        [SerializeField] Issue_Selectable m_issueSelectable;
        [SerializeField] Item_waypoint._IssueWayPoint m_hoveringItem;
        public Issue_Selectable IssueSelectable { get => m_issueSelectable; set => m_issueSelectable = value; }
        public Item_waypoint._IssueWayPoint HoveringItem { get => m_hoveringItem; set => m_hoveringItem = value; }

        #region Implement
        public override GameObject Target => gameObject;

        public override List<GameObject> Targets => throw new System.NotImplementedException();

        /// <summary>
        /// 값 변경 대응
        /// </summary>
        /// <param name="_value"></param>
        public override void OnChangeValue(float _value)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 선택 해제시 실행
        /// </summary>
        public override void OnDeselect()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 선택 해제
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        public override void OnDeselect<T1, T2>(T1 t1, T2 t2)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 선택 해제
        /// </summary>
        public override void OnSelect()
        {
            throw new System.NotImplementedException();
        }
        #endregion

        /// <summary>
        /// 상호작용 되능 손상정보 객체 수집
        /// </summary>
        /// <param name="_selectable"></param>
        /// <param name="_hoveringItem"></param>
        public void GetIssueSelectable(Issue_Selectable _selectable, Item_waypoint._IssueWayPoint _hoveringItem)
        {
            IssueSelectable = _selectable;
            HoveringItem = _hoveringItem;
        }

        // Hover start
        /// <summary>
        /// 호버링 - 포인터 진입
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerEnter(PointerEventData eventData)
        {
            //Debug.Log($"{this.name} OnPointerEnter");
            if(IssueSelectable != null)
            {
                HoveringItem.OnHover(IssueSelectable.Issue.IsDmg);
            }
        }

        // Hover end
        /// <summary>
        /// 호버링 - 호버링 종료
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerExit(PointerEventData eventData)
        {
            if(IssueSelectable != null)
            {
                //Debug.Log($"{this.name} OnPointerExit");
                HoveringItem.OffHover(IssueSelectable.Issue.IsDmg);
            }
        }

        private void OnEnable()
        {
            if(HoveringItem != null)
            {
                HoveringItem.OffHover(IssueSelectable.Issue.IsDmg);
            }
        }

    }
}
