using Definition;
using Module.Interaction;
using Module.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using View;

namespace Management.Events.Inputs
{
    public class Event_Hover : EventData_Input
    {
        private Vector3 m_mousePosition;
        
        /// <summary>
        /// ȣ�� �̺�Ʈ ������
        /// </summary>
        /// <param name="_eType">�̺�Ʈ �з�</param>
        /// <param name="_mousePos">���콺 ��ġ</param>
        /// <param name="_camera">ī�޶�</param>
        /// <param name="_grRaycaster">�׷��� ����ĳ����</param>
        public Event_Hover(InputEventType _eType,
            Vector3 _mousePos,
            Camera _camera, GraphicRaycaster _grRaycaster
            /*UnityEvent<GameObject> _event*/)
        {
            StatusCode = Status.Ready;

            EventType = _eType;
            m_camera = _camera;
            m_grRaycaster = _grRaycaster;
            m_mousePosition = _mousePos;
        }

        /// <summary>
        /// �̺�Ʈ ��ó��
        /// </summary>
        /// <param name="_mList">��� ����Ʈ</param>
        public override void OnProcess(List<ModuleCode> _mList)
        {
            PlatformCode pCode = MainManager.Instance.Platform;

            Status success = Status.Update;

            Elements = new List<View.IInteractable>();
            m_selected3D = null;
            m_hit = default(RaycastHit);
            m_results = new List<RaycastResult>();

            Get_Collect3DObject(m_mousePosition, out m_selected3D, out m_hit, out m_results);

            if(Selected3D != null)
            {
                IInteractable interactable;
                if(Selected3D.TryGetComponent<IInteractable>(out interactable))
                {
                    Elements.Add(interactable);
                    StatusCode = success;

                    return;
                }
            }

            StatusCode = Status.Update;
        }

        /// <summary>
        /// �̺�Ʈ ��ó��
        /// </summary>
        /// <param name="_sEvents">���� �̺�Ʈ ����Ʈ</param>
        /// <exception cref="Definition.Exceptions.PlatformNotDefinedException">���ǵ��� ���� �÷��� �ڵ� ����</exception>
        public override void DoEvent(Dictionary<InputEventType, AEventData> _sEvents)
        {
            //Debug.Log("Hello hover");

            Module_Interaction interaction = ContentManager.Instance.Module<Module_Interaction>();
            UITemplate_Tunnel _uiDemo;

            PlatformCode pCode = MainManager.Instance.Platform;
            if(Platforms.IsDemoWebViewer(pCode))
            {
                UITemplate_WebViewer _ui = interaction.UiInstances[0].gameObject.GetComponent<UITemplate_WebViewer>();
                UIIssue_Selectable uiIssue;
                m_results.ForEach(x =>
                {
                    if (m_results.Count == 1)
                    {
                        if (x.gameObject.transform.parent.TryGetComponent<UIIssue_Selectable>(out uiIssue))
                        {
                            if (uiIssue.HoveringItem.IsLookup)
                            {
                                //Debug.Log(x.gameObject.transform.parent.name);
                                // MainIcon�� ��� - mainIcon�� IssueSelectable�� �������ִ�.
                                if (uiIssue.IssueSelectable != null)
                                {
                                    //m_camera.WorldToScreenPoint(m_mousePosition);
                                    _ui.GetUIEventPacket(Hover_EventType.OnHover,
                                        new Definition.Data.Packet_HoverEvent(
                                            ContentManager.Instance._Canvas,
                                            m_mousePosition,
                                            uiIssue.IssueSelectable.Issue)); //.HoverPanel_OnHover(ContentManager.Instance._Canvas, m_mousePosition, uiIssue.IssueSelectable.Issue);
                                    return;
                                }
                            }
                        }
                    }
                });

                if (m_results.Count == 0)
                {
                    _ui.GetUIEventPacket(Hover_EventType.OffHover,
                                        new Definition.Data.Packet_HoverEvent(
                                            ContentManager.Instance._Canvas,
                                            m_mousePosition,
                                            null));
                }
            }
            else if(Platforms.IsSmartInspectPlatform(pCode))
            {
                UITemplate_SmartInspect _ui = (UITemplate_SmartInspect)interaction.UiInstances[0];

                UIIssue_Selectable uiIssue;
                m_results.ForEach(x =>
                {
                    //Debug.Log($"ray result : {x.gameObject.name}");

                    if (m_results.Count == 1)
                    {
                        if (x.gameObject.transform.parent.TryGetComponent<UIIssue_Selectable>(out uiIssue))
                        {
                            //Debug.Log($"count : {m_results.Count}");
                            //Debug.Log($"object name : {x.gameObject.name}");

                            // �� ��ü�� ī�޶� �ٷ� ���̰� �ִ°�?
                            if (uiIssue.HoveringItem.IsLookup)
                            {
                                //Debug.Log(x.gameObject.transform.parent.name);
                                // MainIcon�� ��� - mainIcon�� IssueSelectable�� �������ִ�.
                                if (uiIssue.IssueSelectable != null)
                                {
                                    _ui.GetUIEventPacket(Hover_EventType.OnHover,
                                        new Definition.Data.Packet_HoverEvent(
                                            ContentManager.Instance._Canvas,
                                            m_mousePosition,
                                            uiIssue.IssueSelectable.Issue));
                                    return;
                                }
                            }
                        }
                    }
                    else
                    {
                        _ui.GetUIEventPacket(Hover_EventType.OffHover,
                                        new Definition.Data.Packet_HoverEvent(
                                            ContentManager.Instance._Canvas,
                                            m_mousePosition,
                                            null));
                    }
                    
                });

                if (m_results.Count == 0)
                {
                    _ui.GetUIEventPacket(Hover_EventType.OffHover,
                                        new Definition.Data.Packet_HoverEvent(
                                            ContentManager.Instance._Canvas,
                                            m_mousePosition,
                                            null));
                }
            }
            else
            {
                throw new Definition.Exceptions.PlatformNotDefinedException(pCode);
            }

        }
    }
}
