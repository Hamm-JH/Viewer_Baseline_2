using Issue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Indicator
{
    public class BP_Indicator : AIndicator
    {
        #region Values

        Manager.ViewSceneStatus _sceneStatus;

        [Header("value")]
        [SerializeField] private List<RectTransform> stateInterfaces;

        private int statusIndex;
        #endregion

        #region Overrides

        public override void SetPanelElements(List<AIssue> _issue)
        {
            //Debug.Log("BP panel set element");

            _sceneStatus = Manager.MainManager.Instance.SceneStatus;
            statusIndex = SetStatusIndex(_sceneStatus);

            ClearElements();
            SetElements(_issue);
        }

        protected override void ClearElements()
        {
            SetStatePanels(statusIndex);
        }

        protected override void SetElements(List<AIssue> _issue)
        {
            Manager.UIManager.Instance.GetRoutineCode(IndicatorType.BP);
        }

        protected override void SetTitleText() { }

        #endregion

        private void Awake()
        {
            statusIndex = default(int);
        }

        private int SetStatusIndex(Manager.ViewSceneStatus _status)
        {
            int result = -1;

            if(_status == Manager.ViewSceneStatus.Ready)
            {
                result = 0;
            }
            else if(_status == Manager.ViewSceneStatus.ViewAllDamage)
            {
                result = 1;
            }
            else if(_status == Manager.ViewSceneStatus.ViewPartDamage)
            {
                result = 2;
            }
            else if(_status == Manager.ViewSceneStatus.ViewPart2R)
            {
                result = 3;
            }
            else if(_status == Manager.ViewSceneStatus.ViewMaintainance)
            {
                result = 4;
            }

            return result;
        }

        private void SetStatePanels(int _statusIndex)
        {
            if (_statusIndex < 0 || _statusIndex >= stateInterfaces.Count) return;

            int index = stateInterfaces.Count;
            for (int i = 0; i < index; i++)
            {
                stateInterfaces[i].gameObject.SetActive( (_statusIndex == i) );
            }
        }

    }
}
