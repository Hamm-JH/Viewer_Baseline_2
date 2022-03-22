using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Indicator.Element
{
    public class BPMM_VAD_Record : AElement
    {
        public Issue.AIssue issue;
        [SerializeField] private TextMeshProUGUI textIndex;
        [SerializeField] private TextMeshProUGUI textIssueCode;
        [SerializeField] private TextMeshProUGUI textPart;
        [SerializeField] private TextMeshProUGUI textPosition;
        [SerializeField] private Image IconPosition;
        [SerializeField] private TextMeshProUGUI textDate;


        public void SetElement(Issue.AIssue _issue, int index)
        {
            issue = _issue;

            textIndex.text = index.ToString();
            textIssueCode.text = issue.ConvertIssueCode(issue.IssueCodes);
            textPart.text = Tunnel.TunnelConverter.GetName(issue.BridgePartName); // MODBS_Library.BridgeCodeConverter.ConvertCode(issue.BridgePartName, MODBS_Library.OutOption.AdView_BPMM_VAD); /*issue.ConvertPartName(issue.BridgePartName);*/
            textPosition.text = issue.ConvertPosition();
            IconPosition.sprite = issue.GetPositionSprite(issue.Issue9Location);
            textDate.text = (issue as Issue.DamagedIssue).CheckDate;
        }
    }
}
