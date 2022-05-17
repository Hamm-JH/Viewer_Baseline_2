using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public class Item_waypoint : MonoBehaviour
    {
        [System.Serializable]
        public class _IssueWayPoint
        {
            public Waypoint_Indicator mainIcon_waypoint;
            public Waypoint_Indicator effect_waypoint;

            public bool IsNotNull()
            {
                bool result = false;

                if (mainIcon_waypoint != null && effect_waypoint != null) result = true;

                return result;
            }

            public void SetColor(Color _color)
            {
                mainIcon_waypoint.onScreenSpriteColor = _color;
                mainIcon_waypoint.offScreenSpriteColor = _color;
                effect_waypoint.onScreenGameObjectColor = _color;
                effect_waypoint.offScreenGameObjectColor = _color;
            }
        }

        [SerializeField]
        private _IssueWayPoint m_issueWayPoint;

        public _IssueWayPoint IssueWayPoint { get => m_issueWayPoint; set => m_issueWayPoint = value; }

        //private void Start()
        //{
        //    if (IssueWayPoint != null)
        //    {
        //        IssueWayPoint.SetColor(Color.white);
        //    }
        //}

        public void SetColor(Color _color)
        {
            if(IssueWayPoint.IsNotNull())
            {
                IssueWayPoint.SetColor(_color);
            }
        }
    }
}
