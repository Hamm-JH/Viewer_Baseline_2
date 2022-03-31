using Issue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Indicator
{
    public class Location_Indicator : AIndicator
    {
        public override void SetPanelElements(List<AIssue> _issue)
        {
            //Manager.UIManager.Instance.GetRoutineCode(IndicatorType.Location);
        }

        protected override void ClearElements()
        {
            throw new System.NotImplementedException();
        }

        protected override void SetElements(List<AIssue> _issue)
        {
            throw new System.NotImplementedException();
        }

        protected override void SetTitleText()
        {
            throw new System.NotImplementedException();
        }

        // Start is called before the first frame update
        //void Start()
        //{

        //}

        //// Update is called once per frame
        //void Update()
        //{

        //}

        private void OnMouseOver()
        {
            Debug.Log("Hello");
        }

        private void OnMouseDrag()
        {
            Debug.Log("Drag");
        }

        private void OnMouseEnter()
        {
            Debug.Log("enter");
        }

        private void OnMouseExit()
        {
            Debug.Log("exit");
        }
    }
}
