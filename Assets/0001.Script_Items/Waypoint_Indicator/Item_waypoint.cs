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

            /// <summary>
            /// 해당 객체가 카메라에 보일 경우 On
            /// 해당 객체가 카메라에 안보일 경우 Off
            /// </summary>
            /// <param name="_isCanSee"></param>
            public void ToggleIcon(bool _isCanSee)
            {
                mainIcon_waypoint.gameObject.SetActive(_isCanSee);
            }
        }

        [SerializeField]
        private _IssueWayPoint m_issueWayPoint;

        public _IssueWayPoint IssueWayPoint { get => m_issueWayPoint; set => m_issueWayPoint = value; }

        [SerializeField] Camera cam;

        private void Start()
        {
            cam = Camera.main;
        }

        private void Update()
        {
            if (!CheckObjectsInCameraFrustum()) return;

            if (IsCameraCanLookUp())
            {
                IssueWayPoint.ToggleIcon(true);
            }
            else
            {
                IssueWayPoint.ToggleIcon(false);
            }
        }

        public void SetColor(Color _color)
        {
            if(IssueWayPoint.IsNotNull())
            {
                IssueWayPoint.SetColor(_color);
            }
        }

        private bool CheckObjectsInCameraFrustum()
        {
            Vector3 screenPoint = cam.WorldToViewportPoint(gameObject.transform.position);
            bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;

            return onScreen;
        }

        private bool IsCameraCanLookUp()
        {
            bool result = false;

            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(cam.WorldToScreenPoint(transform.position));
            if (Physics.Raycast(ray, out hit))
            {

                if (hit.transform.gameObject == gameObject)
                {
                    result = true;
                }
            }

            return result;
        }
    }
}
