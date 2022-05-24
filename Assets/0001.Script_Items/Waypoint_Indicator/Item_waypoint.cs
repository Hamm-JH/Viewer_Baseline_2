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

            public bool isMain;
            public bool isEffect;

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
            public void ToggleIcon(bool _isCanSee, string name )
            {
                if(mainIcon_waypoint == null || mainIcon_waypoint.gameObject == null)
                {
                    Debug.LogError("mainIcon_waypoint is null");
                    return;
                }
                //Debug.Log(name);
                if(mainIcon_waypoint != null && mainIcon_waypoint.gameObject != null)
                {
                    mainIcon_waypoint.gameObject.SetActive(_isCanSee);
                }
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
                isMain = _isOn;
            }

            public void ToggleFx(bool _isOn)
            {
                effect_waypoint.gameObject.SetActive(_isOn);
                isEffect = _isOn;
            }

            public void SetScale(float _value)
            {
                mainIcon_waypoint.onScreenSpriteSize = _value * 0.14f;
                effect_waypoint.onScreenCenteredPrefabSize = _value * 0.5f;
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
            //if (!CheckObjectsInCameraFrustum()) return;
            ////if (!IsObjectActiveSelf()) return;

            if (IsCameraCanLookUp())
            {
                IssueWayPoint.ToggleIcon(true, gameObject.name);
            }
            else
            {
                IssueWayPoint.ToggleIcon(false, gameObject.name);
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
            bool isTransparency = true;

            RaycastHit[] hits;
            //RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(cam.WorldToScreenPoint(transform.position));
            MeshRenderer render;

            float maxDistance = Vector3.Distance(cam.transform.position, transform.position);
            hits = Physics.RaycastAll(ray, maxDistance);
            //Debug.DrawRay(cam.transform.position, transform.position, Color.black);
            Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.black);
            for (int i = 0; i < hits.Length; i++)
            {
                // 검출된 객체가 이 객체인 경우
                if (hits[i].transform.gameObject == gameObject)
                {
                    result = true;
                    break;
                }
                else
                {
                    if (hits[i].transform.TryGetComponent<MeshRenderer>(out render))
                    {
                        // 알파값 1이면 불투명
                        if (render.material.color.a == 1)
                        {
                            isTransparency = false;
                            break;
                        }
                        // 알파값 1 미만이면 반투명
                        else { }
                    }
                }
            }

            // 모두 반투명이고, ray에 걸린게 1개 이상인가?
            if (isTransparency && hits.Length != 0)
            {
                // 중간의 객체들이 모두 반투명이면
                result = true;
            }
            // 중간에 불투명이 걸렸고, ray에 걸린게 1개 이상인가?
            else if (!isTransparency && hits.Length != 0)
            {
                // 2개 이상이지만, 해당 객체가 검출되지 않은 경우
                if(!result)
                {
                    result = false;
                }
                // 2개 이상이지만, 해당 객체가 검출된 경우
                else
                {
                    result = true;
                }
            }

            // 반복 :: 검출 객체가 이 객체 -> 바로 result = true, break
            // 반복 :: 검출 객체가 이 객체 아님 -> 반투명 : true, 불투명 : false

            //if (Physics.Raycast(ray, out hit))
            //{
            //    // 검출된 객체는 이 객체인가
            //    if (hit.transform.gameObject == gameObject)
            //    {
            //        result = true;
            //    }
            //    // 검출된 객체는 다른 객체인가
            //    else
            //    {

            //    }
            //}

            return result;
        }

        private bool IsObjectActiveSelf()
        {
            bool result = false;

            if(IssueWayPoint.isMain)
            {
                result = true;
            }
            //if(IssueWayPoint.mainIcon_waypoint.gameObject.activeSelf)
            //{
            //    result = true;
            //}

            return result;
        }
    }
}
