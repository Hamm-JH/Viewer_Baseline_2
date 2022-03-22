using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Indicator.Element
{
    public class IssueElement : AElement
    {
        public Issue.AIssue issue;
        public Issue.DamagedIssue dIssue;
        public Issue.RecoveredIssue rIssue;

        [SerializeField] private TextMeshProUGUI PartName;
        [SerializeField] private TextMeshProUGUI WidthCountName;
        [SerializeField] private TextMeshProUGUI VerticalCountName;
        [SerializeField] private TextMeshProUGUI DepthCountName;
        [SerializeField] private TextMeshProUGUI DescriptionName;

        [SerializeField] private RawImage image1;
        [SerializeField] private RawImage image2;

        [SerializeField] private RectTransform moreImgPanel;
        [SerializeField] private UIEvent moreImgEvent;
        
        #region Singleton

        private static IssueElement _instance;

        public static IssueElement Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<IssueElement>();
                }
                return _instance;
            }
        }

        #endregion
        private void OnDisable()
        {
            image1.transform.GetChild(0).gameObject.SetActive(true);
            image2.transform.GetChild(0).gameObject.SetActive(true);
        }
        public void GetIssueData(Issue.AIssue _issue)
        {
            issue = _issue;

            PartName.text = GetPartTitleName(issue);
            WidthCountName.text = issue.IssueWidth.ToString();
            VerticalCountName.text = issue.IssueHeight.ToString();
            DepthCountName.text = issue.IssueDepth.ToString();
            DescriptionName.text = issue.Description;

            if (_issue.GetType().Equals(typeof(Issue.DamagedIssue)))
            {
                dIssue = _issue as Issue.DamagedIssue;
                SetImage(dIssue);
            }
        }

        private string GetPartTitleName(Issue.AIssue _issue)
        {
            string result = "";

            string _6surface = _issue.Issue6Surfaces.ToString();
            string _9Location = _issue.Convert9Location(_issue.Issue9Location);
            string _issueString = _issue.ConvertIssueCode(_issue.IssueCodes);

            result = $"{_6surface}_{_9Location}_{_issueString}";

            return result;
        }

        private void SetImage(Issue.DamagedIssue _issue)
        {
            initImage();
            moreImgEvent.InitData(
                _issue: _issue,
                _panel: PanelPosition.BPM2,
                _eventType: ElementEventType.Image);

            int index = 0;
            if(_issue.ImgIndexes != null)
            {
                index = _issue.ImgIndexes.Count;
            }

            string fgroup = _issue.FGroup;
            string argument = "";
            if(index >= 1)
            {
                moreImgPanel.gameObject.SetActive(true);

                argument = string.Format("fid={0}&ftype={1}&fgroup={2}",
                     arg0: _issue.ImgIndexes[0][Issue.KeyOfDamages.fid],
                     arg1: _issue.ImgIndexes[0][Issue.KeyOfDamages.ftype],
                     arg2: fgroup);

                //image1.enabled = true;
                Manager.JSONManager.Instance.LoadImageToJSON(argument, image1);
                image1.transform.GetChild(0).gameObject.SetActive(false);
            }
            if(index >= 2)
            {
                argument = string.Format("fid={0}&ftype={1}&fgroup={2}",
                     arg0: _issue.ImgIndexes[1][Issue.KeyOfDamages.fid],
                     arg1: _issue.ImgIndexes[1][Issue.KeyOfDamages.ftype],
                     arg2: fgroup);

                //image2.enabled = true;
                Manager.JSONManager.Instance.LoadImageToJSON(argument, image2);
                image2.transform.GetChild(0).gameObject.SetActive(false);
            }

            //gameObject.name = $"{imgIndex.fgroup}/{imgIndex.fid}/{imgIndex.ftype}";

            //isInteractable = false;
            //string argument = string.Format("fid={0}&ftype={1}&fgroup={2}",
            //    imgIndex.fid, imgIndex.ftype, imgIndex.fgroup);
        }

        private void SetImage(Issue.RecoveredIssue _issue)
        {
            initImage();
            moreImgEvent.InitData(
                _issue: _issue,
                _panel: PanelPosition.BPM2,
                _eventType: ElementEventType.Image);

            int index = 0;
            if (_issue.ImgIndexes != null)
            {
                index = _issue.ImgIndexes.Count;
            }

            string fgroup = _issue.FGroup;
            string argument = "";
            if (index >= 1)
            {
                moreImgPanel.gameObject.SetActive(true);

                argument = string.Format("fid={0}&ftype={1}&fgroup={2}",
                     arg0: _issue.ImgIndexes[0][Issue.KeyOfRecovers.fid],
                     arg1: _issue.ImgIndexes[0][Issue.KeyOfRecovers.ftype],
                     arg2: fgroup);

                //image1.enabled = true;
                Manager.JSONManager.Instance.LoadImageToJSON(argument, image1);
            }
            if (index >= 2)
            {
                argument = string.Format("fid={0}&ftype={1}&fgroup={2}",
                     arg0: _issue.ImgIndexes[1][Issue.KeyOfRecovers.fid],
                     arg1: _issue.ImgIndexes[1][Issue.KeyOfRecovers.ftype],
                     arg2: fgroup);

                //image2.enabled = true;
                Manager.JSONManager.Instance.LoadImageToJSON(argument, image2);
            }

            //gameObject.name = $"{imgIndex.fgroup}/{imgIndex.fid}/{imgIndex.ftype}";

            //isInteractable = false;
            //string argument = string.Format("fid={0}&ftype={1}&fgroup={2}",
            //    imgIndex.fid, imgIndex.ftype, imgIndex.fgroup);
        }

        private void initImage()
        {
            image1.enabled = false;
            image2.enabled = false;

            moreImgPanel.gameObject.SetActive(false);
        }
        
    }
}
