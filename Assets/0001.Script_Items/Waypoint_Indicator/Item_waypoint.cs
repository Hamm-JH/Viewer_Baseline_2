using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    using View;

    public class Item_waypoint : MonoBehaviour
    {
        [System.Serializable]
        public class _IssueWayPoint
        {
            public Waypoint_Indicator mainIcon_waypoint;
            public Waypoint_Indicator effect_waypoint;
            public Issue_Selectable issue_Selectable;

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
                if(mainIcon_waypoint == null)
                {
                    Debug.LogError("mainIcon_waypoint is null");
                    return;
                }

                mainIcon_waypoint.gameObject.SetActive(_isCanSee);
            }

            public void SetIssueSelectable(Issue_Selectable _selectable)
            {
                // MainIcon에만 Issue_Selectable을 할당한다.
                // 호버링을 위해 이 객체를 UIIssue_Selectable로 보낸다.
                mainIcon_waypoint.AddOnIssueSelectable(_selectable, this);
            }

            public void OnHover(bool _isDmg)
            {
                // 손상일 경우
                if(_isDmg)
                {
                    mainIcon_waypoint.onScreenSpriteColor = new Color(0xec / 255f, 0x3a / 255f, 0x0c / 255f, 1);
                }
                // 보수일 경우
                else
                {
                    mainIcon_waypoint.onScreenSpriteColor = new Color(0x1c / 255f, 0xb5 / 255f, 0xd6 / 255f, 1);
                }
            }

            public void OffHover(bool _isDmg)
            {
                // 손상일 경우
                if (_isDmg)
                {
                    mainIcon_waypoint.onScreenSpriteColor = new Color(0xee / 255f, 0x57 / 255f, 0x30 / 255f, 1);
                }
                // 보수일 경우
                else
                {
                    mainIcon_waypoint.onScreenSpriteColor = new Color(0x3e / 255f, 0xb0 / 255f, 0xc9 / 255f, 1);
                }
            }

            public void ToggleMain(bool _isOn)
            {
                mainIcon_waypoint.gameObject.SetActive(_isOn);
            }

            public void ToggleFx(bool _isOn)
            {
                effect_waypoint.gameObject.SetActive(_isOn);
            }
        }

        [SerializeField]
        private _IssueWayPoint m_issueWayPoint;

        public _IssueWayPoint IssueWayPoint { get => m_issueWayPoint; set => m_issueWayPoint = value; }

        [SerializeField] Camera cam;
        [SerializeField] GameObject m_targetIssueObject;

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
