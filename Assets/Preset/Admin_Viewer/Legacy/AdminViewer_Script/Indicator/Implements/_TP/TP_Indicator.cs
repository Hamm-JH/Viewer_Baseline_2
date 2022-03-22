using Issue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using MODBS_Library;
using UnityEngine.UI;

namespace Indicator
{
    public class TP_Indicator : AIndicator
    {
        public Image bridgeTypeImage;
        public TMPro.TextMeshProUGUI bridgeNameText;
        public TMPro.TextMeshProUGUI partsNameText;
        public TMPro.TextMeshProUGUI addressText;
        public GameObject partsTab;
        public Image partImage;

        #region Sprite resources

        [SerializeField] private Sprite sprite_PSCI;
        [SerializeField] private Sprite sprite_RCS;
        [SerializeField] private Sprite sprite_RCT;
        [SerializeField] private Sprite sprite_RA;
        [SerializeField] private Sprite sprite_ETC;

        [SerializeField] private Sprite sprite_Arch;
        [SerializeField] private Sprite sprite_Box;

        #endregion

        public override void SetPanelElements(List<AIssue> _issue)
        {
            //Debug.Log("TP panel set element");

            sceneStatus = Manager.MainManager.Instance.SceneStatus;

            SetTitleText();

            Manager.UIManager.Instance.GetRoutineCode(IndicatorType.TP);
        }

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
            // 타이틀 텍스트 세팅
            // 서브텍스트 세팅
            switch (sceneStatus)
            {
                case Manager.ViewSceneStatus.Ready:
                case Manager.ViewSceneStatus.ViewAllDamage:
                    titleText.text = "3종 손상이력관리 시스템";

                    bridgeTypeImage.sprite = SetBridgeIcon(Manager.MainManager.Instance.bridgeType);

                    bridgeNameText.text = string.Format("{0}", RuntimeData.RootContainer.Instance.BridgeName);
                    //bridgeNameText.text = string.Format("{0}({1}:{2})",
                    //    RuntimeData.RootContainer.Instance.BridgeName,
                    //    Manager.MainManager.Instance.bridgeType,
                    //    ChangeBridgeTypeText(Manager.MainManager.Instance.visibleOption.ToString()));

                    partsTab.SetActive(false);
                    addressText.text = $"{RuntimeData.RootContainer.Instance.Address}";

                    partsTab.SetActive(false);
                    break;
                case Manager.ViewSceneStatus.ViewPartDamage:
                case Manager.ViewSceneStatus.ViewPart2R:
                    //titleText.text = "3종 교량 3D 손상정보 관리 시스템";
                    //bridgeNameText.text = $"{RuntimeData.RootContainer.Instance.BridgeName}";
                    if (RuntimeData.RootContainer.Instance.cached3DInstance.itemTransform)
                    {
                        string name = RuntimeData.RootContainer.Instance.cached3DInstance.itemTransform.name;
                        partsTab.SetActive(true);
                        //string partName = BridgeCodeConverter.ConvertCode(name, MODBS_Library.OutOption.AdView_MP2_Indicator);
                        //partName = MasterTemplate.FinalReplace(partName);
                        //partsNameText.text = partName;
                        partsNameText.text = name.Split(',')[3];
                        ChangePartsIcon(name);
                        partsTab.SetActive(true);
                    }
                    else
                    {
                        partsTab.SetActive(false);
                    }
                    break;

                case Manager.ViewSceneStatus.ViewMaintainance:
                    partsTab.SetActive(false);
                    break;
            }
        }

        private Sprite SetBridgeIcon(string bridgeType)
        {
            Sprite _sprite = null;

            //MODBS_Library.CodeLv1 bridgeCode = MODBS_Library.LevelCodeConverter.ConvertLv1Code(bridgeType);

            // Arch
            if(bridgeType == "0101")
			{
                _sprite = sprite_Arch;
			}
            // Box
            else if(bridgeType == "0102")
			{
                _sprite = sprite_Box;
			}

            return _sprite;
        }

        private void ChangePartsIcon(string name)
        {
            if(name.Contains("Etc"))
			{
                if(name.Contains("T"))
				{
                    partImage.sprite = Resources.Load<Sprite>("Tunnel/ICON/조명");
				}
                else if (name.Contains("Ex"))
				{
                    partImage.sprite = Resources.Load<Sprite>("Tunnel/ICON/피난연결통로");
				}
                else if (name.Contains("_F"))
				{
					partImage.sprite = Resources.Load<Sprite>("Tunnel/ICON/소화전");
				}
                else if (name.Contains("_Ec"))
				{
                    partImage.sprite = Resources.Load<Sprite>("Tunnel/ICON/비상전화");
				}
			}
            else if(name.Contains("S_") || name.Contains("E_"))
			{
                if(name.Contains("_Div"))
				{
                    partImage.sprite = Resources.Load<Sprite>("Tunnel/ICON/중앙분리대");
				}
				else if (name.Contains("C_Ga"))
				{
                    partImage.sprite = Resources.Load<Sprite>("Tunnel/ICON/면벽");
				}
                else if(name.Contains("C_Sl") || name.Contains("_SlIn") || name.Contains("_SlOut"))
				{
                    partImage.sprite = Resources.Load<Sprite>("Tunnel/ICON/평행사면");
                }
                else if(name.Contains("_GaIn") || name.Contains("R_Ga"))
				{
                    partImage.sprite = Resources.Load<Sprite>("Tunnel/ICON/옹벽");
				}
                else if(name.Contains("_rW"))
				{
                    partImage.sprite = Resources.Load<Sprite>("Tunnel/ICON/날개벽");
				}
                else if(name.Contains("_Sl"))
				{
                    partImage.sprite = Resources.Load<Sprite>("Tunnel/ICON/사면");
				}
			}
            else if(name.Contains("M_"))
			{
                if(name.Contains("_Ce"))
				{
                    partImage.sprite = Resources.Load<Sprite>("Tunnel/ICON/천장(도로터널)");
				}
                else if(name.Contains("_Sw"))
				{
                    partImage.sprite = Resources.Load<Sprite>("Tunnel/ICON/벽 내곽");
				}
                else if(name.Contains("_P"))
				{
                    partImage.sprite = Resources.Load<Sprite>("Tunnel/ICON/포장");
                }
                else if (name.Contains("_Co"))
                {
                    partImage.sprite = Resources.Load<Sprite>("Tunnel/ICON/공동구");
                }
                else if (name.Contains("_D"))
                {
                    partImage.sprite = Resources.Load<Sprite>("Tunnel/ICON/배수로");
                }
            }
            else if(name.Contains("L_"))
			{
                partImage.sprite = Resources.Load<Sprite>("Tunnel/ICON/조명");
			}


            //if (name.Contains("AB"))
            //    partImage.sprite = Resources.Load<Sprite>("Icon/Header/ICON_AB");
            //else if (name.Contains("BE"))
            //    partImage.sprite = Resources.Load<Sprite>("Icon/Header/ICON_BE");
            //else if (name.Contains("CB"))
            //    partImage.sprite = Resources.Load<Sprite>("Icon/Header/ICON_CB");
            //else if (name.Contains("DS"))
            //    partImage.sprite = Resources.Load<Sprite>("Icon/Header/ICON_DS");
            //else if (name.Contains("FT"))
            //    partImage.sprite = Resources.Load<Sprite>("Icon/Header/ICON_FT");
            //else if (name.Contains("GD"))
            //    partImage.sprite = Resources.Load<Sprite>("Icon/Header/ICON_GD");
            //else if (name.Contains("GF"))
            //    partImage.sprite = Resources.Load<Sprite>("Icon/Header/ICON_GF");
            //else if (name.Contains("GR"))
            //    partImage.sprite = Resources.Load<Sprite>("Icon/Header/ICON_GR");
            //else if (name.Contains("JI"))
            //    partImage.sprite = Resources.Load<Sprite>("Icon/Header/ICON_JI");
            //else if (name.Contains("PI"))
            //    partImage.sprite = Resources.Load<Sprite>("Icon/Header/ICON_PI");
            //else if (name.Contains("SL"))
            //    partImage.sprite = Resources.Load<Sprite>("Icon/Header/ICON_SL");
        }

        private string ChangeBridgeTypeText(string bridgeType)
        {
            string str = bridgeType;
            if (str == "Skew")
                str = "사교";
            else if (str == "Curve")
                str = "곡교";
            else if (str == "Interactable")
                str = "직교";
            else if (str == "CurveSkew")
                str = "사곡교";
            return str;
        }
    }
}
