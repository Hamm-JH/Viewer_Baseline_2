﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

namespace Indicator.Element
{
    public class BPM1_VP2R_Record : AElement
    {
        public Issue.AIssue issue;
        [SerializeField] private TableElement root;
        [SerializeField] private List<TextMeshProUGUI> textList;
        [SerializeField] private List<Image> imageList;
        [SerializeField] private UIEvent imageEvent;

        public void SetElement(Issue.AIssue _issue, int index, TableElement _root)
        {
            issue = _issue;
            root = _root;

            textList[0].text = index.ToString();
            textList[1].text = Tunnel.TunnelConverter.GetName(issue.BridgePartName); // MODBS_Library.BridgeCodeConverter.ConvertCode(issue.BridgePartName, MODBS_Library.OutOption.AdView_BPMM_VAD);
            textList[2].text = "단면 균열보수"; // TODO : 보수공법
            textList[3].text = (issue as Issue.RecoveredIssue).EndDate;

            imageList[0].sprite = UI.IssueConverter.GetRecordImageIcon();

            imageEvent.InitData(
                _issue: issue,
                _panel: PanelPosition.BPM1,
                _eventType: ElementEventType.Image);
        }
        public override void OnPointerClick(PointerEventData eventData)
        {
            #region Debug
#if UNITY_EDITOR
            EditDebug.PrintBP1EventRoutine($"BPM1 pointer click");
#endif
            #endregion

            Debug.Log(this.issue);

            SetIssueColors();

            // root로 클릭 이벤트와, 자기 자신을 보내서 리스트 안의 모든 객체들의 시각화 상태를 변경한다.
            root.ChangeSelectionChildState(transform.GetComponent<BPM1_VP2R_Record>());
            
            //List<Issue.AIssue> _issueList = RuntimeData.RootContainer.Instance.IssueObjectList;

            //for (int i = 0; i < _issueList.Count; i++)
            //{
            //    if (_issueList[i] == this.issue)
            //    {
            //        _issueList[i].GetComponent<MeshRenderer>().material.SetColor("Color01", new Color(252 / 255f, 190 / 255f, 20 / 255f));
            //    }
            //    else
            //    {
            //        if (_issueList[i].GetType().Equals(typeof(Issue.DamagedIssue)))
            //            _issueList[i].GetComponent<MeshRenderer>().material.SetColor("Color01", new Color(255 / 255f, 45 / 255f, 45 / 255f));
            //        else if (_issueList[i].GetType().Equals(typeof(Issue.RecoveredIssue)))
            //            _issueList[i].GetComponent<MeshRenderer>().material.SetColor("Color01", new Color(45 / 255f, 94 / 255f, 255 / 255f));
            //    }
            //}
        }

        /// <summary>
        /// 손상 정보의 색상을 변경한다.
        /// </summary>
        private void SetIssueColors()
        {
            //Debug.Log(this.issue);
            List<Issue.AIssue> _issueList = RuntimeData.RootContainer.Instance.IssueObjectList;

            for (int i = 0; i < _issueList.Count; i++)
            {
                if (_issueList[i] == this.issue)
                {
                    _issueList[i].GetComponent<MeshRenderer>().material.SetColor("Color01", new Color(252 / 255f, 190 / 255f, 20 / 255f));
                }
                else
                {
                    if (_issueList[i].GetType().Equals(typeof(Issue.DamagedIssue)))
                        _issueList[i].GetComponent<MeshRenderer>().material.SetColor("Color01", new Color(255 / 255f, 45 / 255f, 45 / 255f));
                    else if (_issueList[i].GetType().Equals(typeof(Issue.RecoveredIssue)))
                        _issueList[i].GetComponent<MeshRenderer>().material.SetColor("Color01", new Color(45 / 255f, 94 / 255f, 255 / 255f));
                }
            }
        }
    }
}
