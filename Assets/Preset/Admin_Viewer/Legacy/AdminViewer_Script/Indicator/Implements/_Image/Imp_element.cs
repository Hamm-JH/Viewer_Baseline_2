using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Indicator.Element
{
    public class Imp_element : AElement
    {
        //public Issue.AIssue reference;
        private Indicator.ImageIndex imgIndex;

        public ImP_option DataOption
        {
            get
            {
                return imgIndex.imgOption;
            }
        }

        [Header("data")]
        public TextMeshProUGUI yearDate;
        public TextMeshProUGUI date;
        public RawImage image;
        public TextMeshProUGUI description;

        [Header("panel")]
        public GameObject yearPanel;
        public GameObject datePanel;
        public GameObject descriptionPanel;

        public void SetElement(Indicator.ImageIndex _imgIndex, bool isFirstDate, bool isFirstYear)
        {
            //reference = _reference;
            imgIndex = _imgIndex;
            RequestImage(imgIndex);

            if (isFirstYear) { yearDate.text = $"{imgIndex.date.Split('-')[0]}년"; }
            else { yearPanel.SetActive(false); }

            if (isFirstDate)
            {
                date.text = imgIndex.date;
                description.text = imgIndex.description;
            }
            else
            {
                datePanel.SetActive(false);
                description.text = "";
            }

            //if (isFirstDate)
            //{
            //    imgIndex = _imgIndex;

            //    yearDate.text = $"{imgIndex.date.Split('-')[0]}년";
            //    date.text = imgIndex.date;
            //    description.text = imgIndex.description;
            //    RequestImage(imgIndex);
            //}
            //else
            //{
            //    imgIndex = _imgIndex;

            //    yearPanel.SetActive(false);
            //    datePanel.SetActive(false);
            //    description.text = "";
            //    RequestImage(imgIndex);
            //}
        }

        public void RequestImage(Indicator.ImageIndex _imgIndex)
        {
            int requestIndex = _imgIndex.requestIndex;
            if(requestIndex != -1 && requestIndex < _imgIndex.imgList.Count)
            {
                string argument = string.Format("fid={0}&ftype={1}&fgroup={2}",
                    arg0: _imgIndex.imgList[requestIndex]["fid"],
                    arg1: _imgIndex.imgList[requestIndex]["ftype"],
                    arg2: _imgIndex.fgroup);

                Manager.JSONManager.Instance.LoadImageToJSON(argument, gameObject.GetComponent<Imp_element>());
            }
        }

        public void SetDescriptionLength()
        {
            if(Indicator.ImP_Indicator.Instance.overDescriptionHeight > 0)
            {
                float length = this.description.transform.parent.GetComponent<RectTransform>().sizeDelta.y + Indicator.ImP_Indicator.Instance.overDescriptionHeight;
                this.description.transform.parent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, length);
                this.description.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, length);

            }
        }
        public void GetTexture(Texture2D _texture)
        {
            try
            {
                image.texture = _texture;
            }
            catch (System.Exception e) 
            {

            }
        }

        public void EnlargePhotoButton()
        {
            Indicator.ImP_Indicator.Instance.enlargeImage.gameObject.SetActive(true);
            Indicator.ImP_Indicator.Instance.enlargeImage.transform.GetChild(1).GetComponent<RawImage>().texture = this.image.texture;
        }
    }
}
