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

            public Color mainColor;

            /// <summary>
            /// 주 아이콘이 켜져있어야 하는가
            /// </summary>
            public bool isMain;

            /// <summary>
            /// 효과 아이콘이 켜져있어야 하는가
            /// </summary>
            public bool isEffect;

            /// <summary>
            /// 이 아이콘은 가려져있는가
            /// </summary>
            private bool isLookup;

            /// <summary>
            /// 이 개체가 카메라에서 바로 보이는 개체인가?
            /// </summary>
            public bool IsLookup { get => isLookup; set => isLookup = value; }

            public bool IsNotNull()
            {
                bool result = false;

                if (mainIcon_waypoint != null && effect_waypoint != null) result = true;

                return result;
            }

            public void SetColor(Color _color)
            {
                mainColor = _color;
                mainIcon_waypoint.onScreenSpriteColor       = mainColor;
                mainIcon_waypoint.offScreenSpriteColor      = mainColor;
                effect_waypoint.onScreenGameObjectColor     = mainColor;
                effect_waypoint.offScreenGameObjectColor    = mainColor;
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
                

                //if(mainIcon_waypoint != null && mainIcon_waypoint.gameObject != null)
                //{
                //    mainIcon_waypoint.gameObject.SetActive(_isCanSee);
                //}
            }

            public void SetIssueSelectable(Issue_Selectable _selectable)
            {
                // MainIcon에만 Issue_Selectable을 할당한다.
                // 호버링을 위해 이 객체를 UIIssue_Selectable로 보낸다.
                mainIcon_waypoint.AddOnIssueSelectable(_selectable, this);
            }

            public void OnHover(bool _isDmg)
            {
                if(IsLookup)
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
            }

            public void OffHover(bool _isDmg)
            {
                if(IsLookup)
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
            }

            public void Lookup(bool _isLookup)
            {
                // 보이지 않고있을 경우
                if(!_isLookup)
                {
                    mainIcon_waypoint.onScreenSpriteColor = Color.white;
                }
                else
                {
                    mainIcon_waypoint.onScreenSpriteColor = mainColor;
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
            if (!IsObjectActiveSelf()) return;

            bool isCanLookup = IsCameraCanLookUp();
            bool isObjectActive = IsObjectActiveSelf();

            // 보이는 상태에서 - 객체가 켜져있으면 켜기
            // 보이는 상태에서 - 객체가 꺼져있으면 끄기
            // 보이지 않는 상태에서 - 객체가 켜져있으면 켜기
            // 보이지 않는 상태에서 - 객체가 꺼져있으면 끄기

            if (IsCameraCanLookUp())
            {
                IssueWayPoint.IsLookup = true;
                IssueWayPoint.ToggleIcon(true, gameObject.name);
                IssueWayPoint.Lookup(true);
            }
            else
            {
                IssueWayPoint.IsLookup = false;
                IssueWayPoint.ToggleIcon(false, gameObject.name);
                IssueWayPoint.Lookup(false);
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
            // true로 시작함
            // 레이캐스팅의 결과중에 대상 객체 하나만 있을 경우 자동으로 true로 넘어감
            // 레이캐스팅 결과중에 불투명 개체가 하나라도 있으면 false로 변경되고 지속됨.
            bool result = true;
            bool isTransparency = true;

            RaycastHit[] hits;
            //RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(cam.WorldToScreenPoint(transform.position));
            MeshRenderer render;

            float maxDistance = Vector3.Distance(cam.transform.position, transform.position);
            hits = Physics.RaycastAll(ray, maxDistance);
            //Debug.DrawRay(cam.transform.position, transform.position, Color.black);
            Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.black);

            // 

            for (int i = 0; i < hits.Length; i++)
            {
                // 검출된 객체가 이 객체인 경우 -> 0526 이 조건은 이제 상수 :: 다른 일을 하지 않는다.
                if (hits[i].transform.gameObject == gameObject)
                {
                    //result = true;
                    //break;
                }
                else
                {
                    if (hits[i].transform.TryGetComponent<MeshRenderer>(out render))
                    {
                        // 알파값 1이면 불투명
                        if (render.material.color.a == 1)
                        {
                            isTransparency = isTransparency && false;
                            //break;
                        }
                        // 알파값 1 미만이면 반투명
                        else 
                        {
                            isTransparency = isTransparency && true;
                        }
                    }
                }
            }
            
            if(isTransparency)
            {
                if(hits.Length != 0)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            else
            {
                if(hits.Length != 0)
                {
                    if(hits.Length == 2)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
                else
                {
                    result = true;
                }
            }

            //// 모두 반투명이고, ray에 걸린게 1개 이상인가?
            //if (isTransparency && hits.Length != 0)
            //{
            //    // 중간의 객체들이 모두 반투명이면
            //    result = true;
            //}
            //// 중간에 불투명이 걸렸고, ray에 걸린게 1개 이상인가?
            //else if (!isTransparency && hits.Length != 0)
            //{
            //    // 2개 이상이지만, 해당 객체가 검출되지 않은 경우
            //    if(!result)
            //    {
            //        result = false;
            //    }
            //    // 2개 이상이지만, 해당 객체가 검출된 경우
            //    else
            //    {
            //        result = true;
            //    }
            //}

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
