using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Indicator.Element
{
    public enum PanelPosition
    {
        BPM1,
        BPM2,

        BPMM,

        ImP,

        St5_Dmg,
        St5_Rcv,
        St5_Rein,
    }

    public enum ElementEventType
    {
        Image,
        Exit,
        Tag,
    }

    public class UIEvent : AElement
    {
        #region Event Values
        [Header("Optional event arguments")]
        
        // Image : 이미지 패널 생성용 손상정보
        public Issue.AIssue issue;

        // Exit : 패널 종료용 transform
        public RectTransform panelTransform;

        // Tag : 패널 정보 스위칭용 표시기 객체
        public AIndicator indicator;
        public ImP_option option;

        #endregion

        #region required values
        [Header("event parameter")]
        public PanelPosition panel;
        public ElementEventType eventType;
        public Manager.ViewSceneStatus sceneStatus;
        #endregion

        #region initialize
        public void InitData(Issue.AIssue _issue, 
            PanelPosition _panel, ElementEventType _eventType)
        {
            issue = _issue;
            panel = _panel;
            eventType = _eventType;
            sceneStatus = Manager.MainManager.Instance.SceneStatus;
        }

        public void InitData(RectTransform _panelTransform,
            PanelPosition _panel, ElementEventType _eventType)
        {
            panelTransform = _panelTransform;
            panel = _panel;
            eventType = _eventType;
            sceneStatus = Manager.MainManager.Instance.SceneStatus;
        }
        #endregion

        public override void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("event click");

            Debug.Log(panel.ToString());
            Debug.Log(eventType.ToString());

            if(eventType.Equals(ElementEventType.Image))
            {
                InvokeOpenImagePanel();
                return;
            }
            if(eventType.Equals(ElementEventType.Exit))
            {
                InvokeExitPanel();
                return;
            }
            if(eventType.Equals(ElementEventType.Tag))
            {
                InvokeTagData();
                return;
            }
        }

        #region Image event
        /// <summary>
        /// 이미지 패널 생성 시작단계
        /// </summary>
        private void InvokeOpenImagePanel()
        {
            Indicator.ImP_option option = GetImagePanelOption();

            if (!option.Equals(Indicator.ImP_option.Null))
            {
                ImP_Indicator imagePanelIndicator = (Manager.UIManager.Instance.indicatorDic[IndicatorType.ImP] as ImP_Indicator);

                imagePanelIndicator.gameObject.SetActive(true);

                imagePanelIndicator.SetPanelElements(
                    _issue: issue,
                    initOption: option
                    );
            }
        }

        private Indicator.ImP_option GetImagePanelOption()
        {
            Indicator.ImP_option _option = ImP_option.Null;

            switch(sceneStatus)
            {
                case Manager.ViewSceneStatus.ViewAllDamage:

                    break;

                case Manager.ViewSceneStatus.ViewPartDamage:
                    switch(panel)
                    {
                        case PanelPosition.BPM1:
                            _option = ImP_option.Damage;
                            break;

                        case PanelPosition.BPM2:
                            _option = ImP_option.DamageInDay;
                            break;
                    }
                    break;

                case Manager.ViewSceneStatus.ViewPart2R:
                    switch(panel)
                    {
                        case PanelPosition.BPM1:
                            _option = ImP_option.Recover;
                            break;

                        case PanelPosition.BPM2:
                            _option = ImP_option.Reinforcement;
                            break;
                    }
                    break;

                case Manager.ViewSceneStatus.ViewMaintainance:
                    switch(panel)
                    {
                        //case PanelPosition.BPM1:
                        //    _option = ImP_option.Damage;
                        //    break;

                        //case PanelPosition.BPM2:
                        //    if(issue.GetType().Equals(typeof(Issue.RecoveredIssue)))
                        //    {
                        //        _option = ImP_option.Recover;
                        //    }
                        //    else if(issue.GetType().Equals(typeof(Issue.ReinforcementIssue)))
                        //    {
                        //        _option = ImP_option.Reinforcement;
                        //    }
                        //    break;

                        case PanelPosition.St5_Dmg:
                            _option = ImP_option.Damage;
                            break;

                        case PanelPosition.St5_Rcv:
                            _option = ImP_option.Recover;
                            break;

                        case PanelPosition.St5_Rein:
                            _option = ImP_option.Reinforcement;
                            break;
                    }
                    break;
            }

            return _option;
        }
        #endregion

        #region Exit event

        private void InvokeExitPanel()
        {
            if(panelTransform != null)
            {
                panelTransform.gameObject.SetActive(false);
            }
        }

        #endregion

        #region Tag event

        private void InvokeTagData()
        {
            
            if(indicator != null)
            {
                if(panel.Equals(PanelPosition.ImP))
                {
                    ImP_Indicator imagePanelIndicator = (Manager.UIManager.Instance.indicatorDic[IndicatorType.ImP] as ImP_Indicator);

                    imagePanelIndicator.TagData(option);
                }
                //else if(panel.Equals(PanelPosition.BPM2))
                //{
                //    BPM2_Indicator bpm2Indicator = (Manager.UIManager.Instance.indicatorDic[IndicatorType.BPM2] as BPM2_Indicator);

                //    if(option.Equals(ImP_option.Recover))
                //    {
                //        bpm2Indicator.ChangeTag(1);
                //    }
                //    else if(option.Equals(ImP_option.Reinforcement))
                //    {
                //        bpm2Indicator.ChangeTag(2);
                //    }
                //}
            }
        }

        #endregion
    }
}
