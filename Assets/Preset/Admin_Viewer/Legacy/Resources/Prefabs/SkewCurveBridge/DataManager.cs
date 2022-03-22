using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class DataManager : MonoBehaviour
{
    #region 변수 선언부
    //public JSONManager JSONManager;
    //public PositionManager manager;

    public RectTransform backTextRectTransform;

    public List<GameObject> managementParts;
    public int bridgeNum;

    //public Image iconImage;
    public GameObject linearImage;
    public GameObject slashImage;
    public GameObject curveImage;

    List<string> bridgeParts = new List<string>();
    List<int> bridgeDamagedCnt = new List<int>();
    List<int> bridgeRecoverCnt = new List<int>();

    int GDDamagedCnt = 0;
    int GDRecoverCnt = 0;
    int CBDamagedCnt = 0;
    int CBRecoverCnt = 0;
    int SLDamagedCnt = 0;
    int SLRecoverCnt = 0;
    int DSDamagedCnt = 0;
    int DSRecoverCnt = 0;
    int GRDamagedCnt = 0;
    int GRRecoverCnt = 0;
    int GFDamagedCnt = 0;
    int GFRecoverCnt = 0;
    int CUDamagedCnt = 0;
    int CURecoverCnt = 0;
    int ABDamagedCnt = 0;
    int ABRecoverCnt = 0;
    int PIDamagedCnt = 0;
    int PIRecoverCnt = 0;
    int BEDamagedCnt = 0;
    int BERecoverCnt = 0;
    int JIDamagedCnt = 0;
    int JIRecoverCnt = 0;
    int FTDamagedCnt = 0;
    int FTRecoverCnt = 0;
    #endregion

    #region 화면 회전 시 손상/보수 아이콘 위치, 회전값 Update
    //public void Update()
    //{
    //    manager.UpdatePosition(bridgeNum);
    //}
    #endregion

    // Start is called before the first frame update
    public void InitialSet()
    {
        //addressText.text =  JSONManager.bridgeAddress + " > " + JSONManager.bridgeName;
        //addressTextRectTransform.sizeDelta = new Vector2(addressText.preferredWidth, addressText.preferredHeight);
        //backTextRectTransform.sizeDelta = new Vector2(addressText.preferredWidth + 40, 80);

        //switch (JSONManager.bridgePlanShape)
        //{
        //    case "linear":
        //        {
        //            bridgeNum = 0;
        //            linearImage.SetActive(true);
        //            //iconImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Materials/IconLinear") as Sprite;
        //        }
        //        break;
        //    case "slash":
        //        {
        //            bridgeNum = 1;
        //            //iconImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Materials/IconSlash") as Sprite;
        //            slashImage.SetActive(true);
        //        }
        //        break;
        //    case "curve":
        //        {
        //            bridgeNum = 2;
        //            //iconImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Materials/IconCurve") as Sprite;
        //            curveImage.SetActive(true);
        //        }
        //        break;
        //}
        //manager.StartBridgeType(bridgeNum);
        
        //for(int i = 0; i < JSONManager.cdBridgeParts.Count; i++)
        //{
        //    bridgeParts.Add(JSONManager.cdBridgeParts[i]);
        //    bridgeDamagedCnt.Add(JSONManager.damagedCnt[i]);
        //    bridgeRecoverCnt.Add(JSONManager.recoverCnt[i]);
        //}
        
        for(int i = 0; i < bridgeParts.Count; i++)
        {
            if (bridgeParts[i].Length == 6)
            {
                if (bridgeParts[i].Contains("GD"))
                {
                    GDDamagedCnt += bridgeDamagedCnt[i];
                    GDRecoverCnt += bridgeRecoverCnt[i];
                }
                if (bridgeParts[i].Contains("CB"))
                {
                    CBDamagedCnt += bridgeDamagedCnt[i];
                    CBRecoverCnt += bridgeRecoverCnt[i];
                }
                if (bridgeParts[i].Contains("SL"))
                {
                    SLDamagedCnt += bridgeDamagedCnt[i];
                    SLRecoverCnt += bridgeRecoverCnt[i];
                }
                if (bridgeParts[i].Contains("DS"))
                {
                    DSDamagedCnt += bridgeDamagedCnt[i];
                    DSRecoverCnt += bridgeRecoverCnt[i];
                }
                if (bridgeParts[i].Contains("CR") || bridgeParts[i].Contains("CL"))
                {
                    CUDamagedCnt += bridgeDamagedCnt[i];
                    CURecoverCnt += bridgeRecoverCnt[i];
                }
                if (bridgeParts[i].Contains("AB"))
                {
                    ABDamagedCnt += bridgeDamagedCnt[i];
                    ABRecoverCnt += bridgeRecoverCnt[i];
                }
                if (bridgeParts[i].Contains("PI"))
                {
                    PIDamagedCnt += bridgeDamagedCnt[i];
                    PIRecoverCnt += bridgeRecoverCnt[i];
                }
                if (bridgeParts[i].Contains("BE"))
                {
                    BEDamagedCnt += bridgeDamagedCnt[i];
                    BERecoverCnt += bridgeRecoverCnt[i];
                }
                if (bridgeParts[i].Contains("JI"))
                {
                    JIDamagedCnt += bridgeDamagedCnt[i];
                    JIRecoverCnt += bridgeRecoverCnt[i];
                }
                if (bridgeParts[i].Contains("FT"))
                {
                    FTDamagedCnt += bridgeDamagedCnt[i];
                    FTRecoverCnt += bridgeRecoverCnt[i];
                }
            }
            else if (bridgeParts[i].Length > 6)
            {
                if (bridgeParts[i].Contains("GL") || bridgeParts[i].Contains("GR"))
                {
                    if (bridgeParts[i].Contains("GF"))
                    {
                        GFDamagedCnt += bridgeDamagedCnt[i];
                        GFRecoverCnt += bridgeRecoverCnt[i];
                    }
                    else
                    {
                        GRDamagedCnt += bridgeDamagedCnt[i];
                        GRRecoverCnt += bridgeRecoverCnt[i];
                    }
                }
            }
            else if (bridgeParts[i].Length < 6)
                return;
        }

        bridgeParts = bridgeParts.Distinct().ToList();
        for(int i = 0; i < bridgeParts.Count; i++)
        {
            if (bridgeParts[i].Contains("GD"))
            {
                if (GDDamagedCnt != 0)
                {
                    managementParts[0].transform.GetChild(0).GetChild(1).GetComponent<Text>().text = GDDamagedCnt.ToString();
                    managementParts[0].SetActive(true);
                }
                if (GDRecoverCnt != 0)
                {
                    managementParts[12].transform.GetChild(0).GetChild(1).GetComponent<Text>().text = GDRecoverCnt.ToString();
                    managementParts[12].SetActive(true);
                }
            }
            else if (bridgeParts[i].Contains("CB"))
            {
                if (CBDamagedCnt != 0)
                {
                    managementParts[1].transform.GetChild(0).GetChild(1).GetComponent<Text>().text = CBDamagedCnt.ToString();
                    managementParts[1].SetActive(true);
                }
                if (CBRecoverCnt != 0)
                {
                    managementParts[13].transform.GetChild(0).GetChild(1).GetComponent<Text>().text = CBRecoverCnt.ToString();
                    managementParts[13].SetActive(true);
                }
            }
            else if (bridgeParts[i].Contains("SL"))
            {
                if (SLDamagedCnt != 0)
                {
                    managementParts[2].transform.GetChild(0).GetChild(1).GetComponent<Text>().text = SLDamagedCnt.ToString();
                    managementParts[2].SetActive(true);
                }
                if (SLRecoverCnt != 0)
                {
                    managementParts[14].transform.GetChild(0).GetChild(1).GetComponent<Text>().text = SLRecoverCnt.ToString();
                    managementParts[14].SetActive(true);
                }
            }
            else if (bridgeParts[i].Contains("DS"))
            {
                if (DSDamagedCnt != 0)
                {
                    managementParts[3].transform.GetChild(0).GetChild(1).GetComponent<Text>().text = DSDamagedCnt.ToString();
                    managementParts[3].SetActive(true);
                }
                if (DSRecoverCnt != 0)
                {
                    managementParts[15].transform.GetChild(0).GetChild(1).GetComponent<Text>().text = DSRecoverCnt.ToString();
                    managementParts[15].SetActive(true);
                }
            }
            else if(bridgeParts[i].Contains("GR") || bridgeParts[i].Contains("GL"))
            {
                if (bridgeParts[i].Contains("GF"))
                {
                    if (GFDamagedCnt != 0)
                    {
                        managementParts[5].transform.GetChild(0).GetChild(1).GetComponent<Text>().text = GFDamagedCnt.ToString();
                        managementParts[5].SetActive(true);
                    }
                    if (GFRecoverCnt != 0)
                    {
                        managementParts[17].transform.GetChild(0).GetChild(1).GetComponent<Text>().text = GFRecoverCnt.ToString();
                        managementParts[17].SetActive(true);
                    }
                }
                else
                {
                    if (GFDamagedCnt != 0)
                    {
                        managementParts[5].transform.GetChild(0).GetChild(1).GetComponent<Text>().text = GFDamagedCnt.ToString();
                        managementParts[5].SetActive(true);
                    }
                    if (GFRecoverCnt != 0)
                    {
                        managementParts[17].transform.GetChild(0).GetChild(1).GetComponent<Text>().text = GFRecoverCnt.ToString();
                        managementParts[17].SetActive(true);
                    }
                }
            }
            else if(bridgeParts[i].Contains("CR") || bridgeParts[i].Contains("CL"))
            {
                if (CUDamagedCnt != 0)
                {
                    managementParts[6].transform.GetChild(0).GetChild(1).GetComponent<Text>().text = CUDamagedCnt.ToString();
                    managementParts[6].SetActive(true);
                }
                if (CURecoverCnt != 0)
                {
                    managementParts[18].transform.GetChild(0).GetChild(1).GetComponent<Text>().text = CURecoverCnt.ToString();
                    managementParts[18].SetActive(true);
                }
            }
            else if (bridgeParts[i].Contains("AB"))
            {
                if (ABDamagedCnt != 0)
                {
                    managementParts[7].transform.GetChild(0).GetChild(1).GetComponent<Text>().text = ABDamagedCnt.ToString();
                    managementParts[7].SetActive(true);
                }
                if (ABRecoverCnt != 0)
                {
                    managementParts[19].transform.GetChild(0).GetChild(1).GetComponent<Text>().text = ABRecoverCnt.ToString();
                    managementParts[19].SetActive(true);
                }
            }
            else if (bridgeParts[i].Contains("PI"))
            {
                if (PIDamagedCnt != 0)
                {
                    managementParts[8].transform.GetChild(0).GetChild(1).GetComponent<Text>().text = PIDamagedCnt.ToString();
                    managementParts[8].SetActive(true);
                }
                if (PIRecoverCnt != 0)
                {
                    managementParts[20].transform.GetChild(0).GetChild(1).GetComponent<Text>().text = PIRecoverCnt.ToString();
                    managementParts[20].SetActive(true);
                }
            }
            else if (bridgeParts[i].Contains("BE"))
            {
                if (BEDamagedCnt != 0)
                {
                    managementParts[9].transform.GetChild(0).GetChild(1).GetComponent<Text>().text = BEDamagedCnt.ToString();
                    managementParts[9].SetActive(true);
                }
                if (BERecoverCnt != 0)
                {
                    managementParts[21].transform.GetChild(0).GetChild(1).GetComponent<Text>().text = BERecoverCnt.ToString();
                    managementParts[21].SetActive(true);
                }
            }
            else if (bridgeParts[i].Contains("JI"))
            {
                if (JIDamagedCnt != 0)
                {
                    managementParts[10].transform.GetChild(0).GetChild(1).GetComponent<Text>().text = JIDamagedCnt.ToString();
                    managementParts[10].SetActive(true);
                }
                if (JIRecoverCnt != 0)
                {
                    managementParts[22].transform.GetChild(0).GetChild(1).GetComponent<Text>().text = JIRecoverCnt.ToString();
                    managementParts[22].SetActive(true);
                }
            }
            else if (bridgeParts[i].Contains("FT"))
            {
                if (FTDamagedCnt != 0)
                {
                    managementParts[11].transform.GetChild(0).GetChild(1).GetComponent<Text>().text = FTDamagedCnt.ToString();
                    managementParts[11].SetActive(true);
                }
                if (FTRecoverCnt != 0)
                {
                    managementParts[23].transform.GetChild(0).GetChild(1).GetComponent<Text>().text = FTRecoverCnt.ToString();
                    managementParts[23].SetActive(true);
                }
            }
        }
    }
}
