using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Indicator.Element
{
    public class BPM2_VP2R_Record : AElement
    {
        public Issue.AIssue issue;
        [SerializeField] private List<TextMeshProUGUI> textList;
        [SerializeField] private List<Image> imageList;
        [SerializeField] private UIEvent imageEvent;

        public void SetElement(Issue.AIssue _issue, int index)
        {
            issue = _issue;

            textList[0].text = index.ToString();
            textList[1].text = Tunnel.TunnelConverter.GetName(issue.BridgePartName); // MODBS_Library.BridgeCodeConverter.ConvertCode(issue.BridgePartName, MODBS_Library.OutOption.AdView_BPMM_VAD);
            textList[2].text = "단면 균열보수"; // TODO : 보수공법
            textList[3].text = ""; //(issue as Issue.ReinforcementIssue).EndDate;

            imageList[0].sprite = UI.IssueConverter.GetRecordImageIcon();

            imageEvent.InitData(
                _issue: issue,
                _panel: PanelPosition.BPM2,
                _eventType: ElementEventType.Image);
        }
    }
}
