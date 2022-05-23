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

        public override void OnChangeValue(float _value)
        {
            throw new System.NotImplementedException();
        }

        public override void OnDeselect()
        {
            throw new System.NotImplementedException();
        }

        public override void OnDeselect<T1, T2>(T1 t1, T2 t2)
        {
            throw new System.NotImplementedException();
        }

        public override void OnSelect()
        {
            throw new System.NotImplementedException();
        }
        #endregion

        public void GetIssueSelectable(Issue_Selectable _selectable, Item_waypoint._IssueWayPoint _hoveringItem)
        {
            IssueSelectable = _selectable;
            HoveringItem = _hoveringItem;
        }

        // Hover start
        public void OnPointerEnter(PointerEventData eventData)
        {
            //Debug.Log($"{this.name} OnPointerEnter");
            if(IssueSelectable != null)
            {
                HoveringItem.OnHover(IssueSelectable.Issue.IsDmg);
            }
        }

        // Hover end
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
