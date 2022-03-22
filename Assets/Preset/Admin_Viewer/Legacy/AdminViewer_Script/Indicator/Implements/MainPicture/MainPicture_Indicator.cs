using Issue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Indicator
{
    public class MainPicture_Indicator : AIndicator
    {
        public RawImage image;

        public override void SetPanelElements(List<AIssue> _issue)
        {
            if (image.texture == null)
            {
                SetMainImage();
            }
            
            Manager.UIManager.Instance.GetRoutineCode(IndicatorType.mainPicture);
        }

        public void SetMainImage()
        {
            string _fgroup = RuntimeData.RootContainer.Instance.MainFGroup;
            string _fid = RuntimeData.RootContainer.Instance.MainFid;
            string _ftype = RuntimeData.RootContainer.Instance.MainFType;

            if(_fgroup != null && _fid != null && _ftype != null)
            {
                string argument = string.Format("fid={0}&ftype={1}&fgroup={2}",
                    arg0: _fid,
                    arg1: _ftype,
                    arg2: _fgroup);

                Manager.JSONManager.Instance.LoadImageToJSON(argument, image);
            }
        }

        #region
        protected override void ClearElements()
        {
            throw new System.NotImplementedException();
        }

        protected override void SetElements(List<AIssue> _issue)
        {
            throw new System.NotImplementedException();
        }

        protected override void SetTitleText()
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}
