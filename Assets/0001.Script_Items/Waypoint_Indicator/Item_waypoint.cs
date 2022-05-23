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
            /// �ش� ��ü�� ī�޶� ���� ��� On
            /// �ش� ��ü�� ī�޶� �Ⱥ��� ��� Off
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
                // MainIcon���� Issue_Selectable�� �Ҵ��Ѵ�.
                // ȣ������ ���� �� ��ü�� UIIssue_Selectable�� ������.
                mainIcon_waypoint.AddOnIssueSelectable(_selectable, this);
            }

            public void OnHover(bool _isDmg)
            {
                // �ջ��� ���
                if(_isDmg)
                {
                    mainIcon_waypoint.onScreenSpriteColor = new Color(0xec / 255f, 0x3a / 255f, 0x0c / 255f, 1);
                }
                // ������ ���
                else
                {
                    mainIcon_waypoint.onScreenSpriteColor = new Color(0x1c / 255f, 0xb5 / 255f, 0xd6 / 255f, 1);
                }
            }

            public void OffHover(bool _isDmg)
            {
                // �ջ��� ���
                if (_isDmg)
                {
                    mainIcon_waypoint.onScreenSpriteColor = new Color(0xee / 255f, 0x57 / 255f, 0x30 / 255f, 1);
                }
                // ������ ���
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
            for (int i = 0; i < hits.Length; i++)
            {
                // ����� ��ü�� �� ��ü�� ���
                if (hits[i].transform.gameObject == gameObject)
                {
                    result = true;
                    break;
                }
                else
                {
                    if (hits[i].transform.TryGetComponent<MeshRenderer>(out render))
                    {
                        // ���İ� 1�̸� ������
                        if (render.material.color.a == 1)
                        {
                            isTransparency = false;
                            break;
                        }
                        // ���İ� 1 �̸��̸� ������
                        else { }
                    }
                }
            }

            // ��� �������̰�, ray�� �ɸ��� 1�� �̻��ΰ�?
            if (isTransparency && hits.Length != 0)
            {
                // �߰��� ��ü���� ��� �������̸�
                result = true;
            }
            // �߰��� �������� �ɷȰ�, ray�� �ɸ��� 1�� �̻��ΰ�?
            else if (!isTransparency && hits.Length != 0)
            {
                // 2�� �̻�������, �ش� ��ü�� ������� ���� ���
                if(!result)
                {
                    result = false;
                }
                // 2�� �̻�������, �ش� ��ü�� ����� ���
                else
                {
                    result = true;
                }
            }

            // �ݺ� :: ���� ��ü�� �� ��ü -> �ٷ� result = true, break
            // �ݺ� :: ���� ��ü�� �� ��ü �ƴ� -> ������ : true, ������ : false

            //if (Physics.Raycast(ray, out hit))
            //{
            //    // ����� ��ü�� �� ��ü�ΰ�
            //    if (hit.transform.gameObject == gameObject)
            //    {
            //        result = true;
            //    }
            //    // ����� ��ü�� �ٸ� ��ü�ΰ�
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
