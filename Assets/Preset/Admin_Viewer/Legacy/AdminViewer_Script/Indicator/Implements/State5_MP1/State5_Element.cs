using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Indicator.Element
{
    public class State5_Element : AElement
    {
        public Issue.AIssue issue;
        public int index;

        //public TableElement root;
        public List<TextMeshProUGUI> textmeshes;
        public List<Image> images;

        //public void SetElement<T>(Issue.AIssue _issue, int _index, TableElement _root) where T : Issue.AIssue
        //{
        //    issue = _issue;
        //    index = _index + 1;
        //    root = _root;

        //    if(typeof(T) == typeof(Issue.DamagedIssue))
        //    {
        //        SetDmgIssueData(_issue as Issue.DamagedIssue, index);
        //    }
        //    else if(typeof(T) == typeof(Issue.RecoveredIssue))
        //    {
        //        SetRcvIssueData(_issue as Issue.RecoveredIssue, index);
        //    }
        //    else if(typeof(T) == typeof(Issue.ReinforcementIssue))
        //    {
        //        SetReinIssueData(_issue as Issue.ReinforcementIssue, index);
        //    }
        //}

        private void SetDmgIssueData(Issue.DamagedIssue _issue, int index)
        {
            //textmeshes[0] 숫자
            textmeshes[0].text = index.ToString();

            //textmeshes[1] 손상정보 dmg name
            textmeshes[1].text = _issue.ConvertIssueCode(_issue.IssueCodes);

            //textmeshes[2] 손상부재 part name
            textmeshes[2].text = Tunnel.TunnelConverter.GetName(issue.BridgePartName);// MODBS_Library.BridgeCodeConverter.ConvertCode(issue.BridgePartName, MODBS_Library.OutOption.AdView_BPMM_VAD);

            //textmeshes[3] 손상위치 dmg name
            textmeshes[3].text = _issue.ConvertPosition();

            //textmeshes[4] 확인날짜 date
            //textmeshes[4].text = (_issue as Issue.DamagedIssue).CheckDate;
            textmeshes[4].text = _issue.CheckDate;

            //image[0] 9면 이미지
            images[0].sprite = _issue.GetPositionSprite(issue.Issue9Location);

            // img[1] 사진 이미지
            images[1].sprite = UI.IssueConverter.GetRecordImageIcon();

            //images[1].GetComponent<UIEvent>().issue = _issue;
        }

        private void SetRcvIssueData(Issue.RecoveredIssue _issue, int index)
        {
            //textmeshes[0] 숫자
            textmeshes[0].text = index.ToString();

            //txt[1] 손상정보 dmg name
            textmeshes[1].text = _issue.ConvertIssueCode(_issue.IssueCodes);

            //textmeshes[1] 보수부재 part name
            textmeshes[2].text = Tunnel.TunnelConverter.GetName(issue.BridgePartName); //MODBS_Library.BridgeCodeConverter.ConvertCode(issue.BridgePartName, MODBS_Library.OutOption.AdView_BPMM_VAD);

            //textmeshes[2] 보수공법 rcv method
            textmeshes[3].text = "단면 균열보수";

            //textmeshes[3] 확인날짜 date
            //textmeshes[3].text = (issue as Issue.RecoveredIssue).EndDate;
            textmeshes[4].text = _issue.EndDate;

            //image[0] 사진 이미지
            images[0].sprite = UI.IssueConverter.GetRecordImageIcon();

            //images[0].GetComponent<UIEvent>().issue = _issue;

            //textList[0].text = index.ToString();
            //textList[1].text = MODBS_Library.BridgeCodeConverter.ConvertCode(issue.BridgePartName, MODBS_Library.OutOption.AdView_BPMM_VAD);
            //textList[2].text = "단면 균열보수"; // TODO : 보수공법
            //textList[3].text = (issue as Issue.RecoveredIssue).EndDate;

            //imageList[0].sprite = UI.IssueConverter.GetRecordImageIcon();
        }

        private void SetReinIssueData(Issue.ReinforcementIssue _issue, int index)
        {
            //textmeshes[0] 숫자
            textmeshes[0].text = index.ToString();

            //txt[1] 손상정보 dmg name
            textmeshes[1].text = _issue.ConvertIssueCode(_issue.IssueCodes);

            //textmeshes[1] 보강부재 part name
            textmeshes[2].text = Tunnel.TunnelConverter.GetName(issue.BridgePartName); // MODBS_Library.BridgeCodeConverter.ConvertCode(issue.BridgePartName, MODBS_Library.OutOption.AdView_BPMM_VAD);

            //textmeshes[2] 보강공법 rein method
            textmeshes[3].text = "단면 균열보강";

            //textmeshes[3] 확인날짜 date
            textmeshes[4].text = _issue.DTEnd;

            //image[0] 사진 이미지
            images[0].sprite = UI.IssueConverter.GetRecordImageIcon();

            //images[0].GetComponent<UIEvent>().issue = _issue;
        }
    }
}
