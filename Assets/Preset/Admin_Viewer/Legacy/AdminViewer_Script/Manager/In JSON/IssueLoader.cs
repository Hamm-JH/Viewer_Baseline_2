using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
//using SimpleJSON;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Net;
//using Newtonsoft.Json;
using Object = System.Object;
using UnityEngine.UI;

namespace Manager
{
    public class IssueLoader : MonoBehaviour
    {
        //[SerializeField] private string AddressURL;
        //[SerializeField] private string DamagedURL;
        //[SerializeField] private string RecoveredURL;

        private Dictionary<JSONLoadType, string> loadURL;

        private Dictionary<JSONLoadType, string> loadKeys;

        private bool isAdrEnd;
        private bool isDmgEnd;
        private bool isRcvEnd;
        private bool isHistoryEnd;

        //private void Awake()
        //{
        //    //JSONManager.Instance.LoadImageToJSON("추가할 인자값", MainManager.Instance.BridgeCode);

        //    //List<Issue.AIssue> issues = RuntimeData.RootContainer.Instance.IssueObjectList;

        //    //int index = RuntimeData.RootContainer.Instance.IssueObjectList.Count;
        //    //for (int i = 0; i < index; i++)
        //    //{
        //    //    // Ex) 1번 [손상 정보]의 [fgroup]
        //    //    string fgroup = RuntimeData.RootContainer.Instance.IssueObjectList[0].FGroup;

        //    //    // 손상 정보가 DamagedIssue일 경우
        //    //    if(RuntimeData.RootContainer.Instance.IssueObjectList[i].GetType() == typeof(Issue.DamagedIssue))
        //    //    {
        //    //        int inIndex = (RuntimeData.RootContainer.Instance.IssueObjectList[0] as Issue.DamagedIssue).ImgIndexes.Count;
        //    //        for (int ii = 0; ii < inIndex; ii++)
        //    //        {
        //    //            // Ex) i번 [손상 정보]의 ii번 [이미지]의 [fid]
        //    //            string fid = (RuntimeData.RootContainer.Instance.IssueObjectList[i] as Issue.DamagedIssue).ImgIndexes[ii][Issue.KeyOfDamages.fid];

        //    //            // Ex) i번 [손상 정보]의 ii번 [이미지]의 [ftype]
        //    //            string ftype=(RuntimeData.RootContainer.Instance.IssueObjectList[i] as Issue.DamagedIssue).ImgIndexes[ii][Issue.KeyOfDamages.ftype];
        //    //        }
        //    //    }
        //    //    // 손상 정보가 RecoverIssue일 경우
        //    //    else if(RuntimeData.RootContainer.Instance.IssueObjectList[i].GetType() == typeof(Issue.RecoveredIssue))
        //    //    {
        //    //        int inIndex = (RuntimeData.RootContainer.Instance.IssueObjectList[0] as Issue.RecoveredIssue).ImgIndexes.Count;
        //    //        for (int ii = 0; ii < inIndex; ii++)
        //    //        {
        //    //            // Ex) i번 [보수 정보]의 ii번 [이미지]의 [fid]
        //    //            string fid = (RuntimeData.RootContainer.Instance.IssueObjectList[i] as Issue.RecoveredIssue).ImgIndexes[ii][Issue.KeyOfRecovers.fid];

        //    //            // Ex) i번 [보수 정보]의 ii번 [이미지]의 [ftype]
        //    //            string ftype = (RuntimeData.RootContainer.Instance.IssueObjectList[i] as Issue.RecoveredIssue).ImgIndexes[ii][Issue.KeyOfRecovers.ftype];
        //    //        }
        //    //    }
        //    //}

        //}

        private void Start()
        {
            //StartCoroutine(LoadJSON(JSONLoadType.Damaged, "20200512-00000001"));

            StartCoroutine(WaitLoadingIssue());
        }

        /// <summary>
        /// 주소/손상/보수 할당완료 대기
        /// </summary>
        /// <returns></returns>
        private IEnumerator WaitLoadingIssue()
        {
            yield return new WaitUntil(() => isAdrEnd.Equals(true));
            yield return new WaitUntil(() => isDmgEnd.Equals(true));
            yield return new WaitUntil(() => isRcvEnd.Equals(true));

            RuntimeData.RootContainer.Instance.isIssueRoutineEnd = true;
            //MainManager.Instance.InitializeRoutineCheck();
        }

        //public void Initialize(Manager.UrlSet urlSet)
        //{
        //    //AddressURL = "http://wesmart.synology.me:45000/bridge/search?cdBridge=";
        //    //DamagedURL = "http://wesmart.synology.me:45000/bridge/damage/state?cdBridge=";
        //    //RecoveredURL = "http://wesmart.synology.me:45000/bridge/recover/state?cdBridge=";

        //    loadURL = new Dictionary<JSONLoadType, string>();
        //    loadURL.Add(JSONLoadType.BridgeList, urlSet.bridgeListURL);
        //    loadURL.Add(JSONLoadType.Address, urlSet.addressURL);
        //    loadURL.Add(JSONLoadType.Damaged, urlSet.damagedURL);
        //    loadURL.Add(JSONLoadType.Recovered, urlSet.recoverURL);
        //    loadURL.Add(JSONLoadType.Image, urlSet.imageURL);
        //    loadURL.Add(JSONLoadType.History, urlSet.historyURL);
        //    loadURL.Add(JSONLoadType.IssueHistory, urlSet.historyURL);
        //    loadURL.Add(JSONLoadType.TotalHistory, urlSet.historyURL);
        //    loadURL.Add(JSONLoadType.HistoryImage, urlSet.historyURL);
        //    loadURL.Add(JSONLoadType.Report, urlSet.reportURL);
        //    //loadURL.Add(JSONLoadType.Image, u)

        //    loadKeys = new Dictionary<JSONLoadType, string>();
        //    loadKeys.Add(JSONLoadType.Damaged, "damagedList");
        //    loadKeys.Add(JSONLoadType.Recovered, "recoverList");

        //    isAdrEnd = false;
        //    isDmgEnd = false;
        //    isRcvEnd = false;
        //}

        #region 손상/보수 신규할당

        public IEnumerator LoadJSON(JSONLoadType loadType, string bridgeCode)
        {
            string URI = "";

            URI = loadURL[loadType] + bridgeCode;

#if UNITY_EDITOR
            EditDebug.PrintINITJSONRoutine($"Load Type : {loadType.ToString()} / URI : {URI}");
#endif

            using (UnityWebRequest webRequest = UnityWebRequest.Get(URI))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.isNetworkError)
                {
                    Debug.Log(loadType.ToString());
                    Debug.Log(string.Format($"Error : {webRequest.error}"));
                }
                else
                {

                    SetJSONData(loadType, webRequest.downloadHandler.text);
                }
            }
        }

        public IEnumerator LoadHistoryJSON(JSONLoadType loadType, string bridgeCode, string query, string restrict = "")
        {
            string URI = "";

            URI = string.Format("{0}cdTunnel={1}{2}", loadURL[loadType], bridgeCode, query);
            //URI = string.Format("{0}cdBridge={1}{2}", loadURL[loadType], bridgeCode, query);

            Debug.Log(URI);

            //URI = string.Format("{0}cdBridge={1}&cdBridgeParts={2}", loadURL[loadType], bridgeCode, bridgeParts);

#if UNITY_EDITOR
            EditDebug.PrintINITJSONRoutine(URI);
#endif

            using (UnityWebRequest webRequest = UnityWebRequest.Get(URI))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.isNetworkError)
                {
                    Debug.Log(string.Format($"Error : {webRequest.error}"));
                }
                else
                {
                    yield return new WaitForSeconds(0.5f);
                    SetJSONData(loadType, webRequest.downloadHandler.text, restrict);
                }
            }
        }

//        public IEnumerator LoadHistoryJSON(JSONLoadType loadType, string bridgeCode, string query, Issue.AIssue issue, Indicator.ImP_option option)
//        {
//            string URI = "";

//            URI = string.Format("{0}cdTunnel={1}{2}", loadURL[loadType], bridgeCode, query);
//            //URI = string.Format("{0}cdBridge={1}{2}", loadURL[loadType], bridgeCode, query);

//            Debug.Log(URI);

//            //URI = string.Format("{0}cdBridge={1}&cdBridgeParts={2}", loadURL[loadType], bridgeCode, bridgeParts);

//#if UNITY_EDITOR
//            EditDebug.PrintINITJSONRoutine(URI);
//#endif

//            using (UnityWebRequest webRequest = UnityWebRequest.Get(URI))
//            {
//                yield return webRequest.SendWebRequest();

//                if (webRequest.isNetworkError)
//                {
//                    Debug.Log(string.Format($"Error : {webRequest.error}"));
//                }
//                else
//                {
//                    yield return new WaitForSeconds(0.5f);
//                    SetJSONData(loadType, webRequest.downloadHandler.text, issue, option);
//                }
//            }
//        }

        //private void SetJSONData(JSONLoadType loadType, string _JSONData, Issue.AIssue issue, Indicator.ImP_option option)
        //{
        //    //string _data = "";
        //    //JSONArray _issueArray;

        //    //switch (loadType)
        //    //{
        //    //    case JSONLoadType.HistoryImage:
        //    //        {
        //    //            _data = JSON.Parse(_JSONData)["data"].ToString();

        //    //            string _dailyList = JSON.Parse(_data)["dailyList"].ToString();
        //    //            JSONNode dailyList = JSON.Parse(_data)["dailyList"];

        //    //            string date = "";
        //    //            List<Indicator.ImageIndex> imageIndexList = new List<Indicator.ImageIndex>();

        //    //            //Debug.Log("restrict : " + restrict);

        //    //            int count = 0;
        //    //            foreach (var dates in dailyList)
        //    //            {
        //    //                count++;
        //    //                //Debug.Log($"{count} {dates}");

        //    //                date = dates.Key;   // 날짜
        //    //                if (dates.Value["damagedList"].Count > 0 && (option == Indicator.ImP_option.Damage || option == Indicator.ImP_option.DamageInDay))
        //    //                {
        //    //                    Issue.IssueCode issueCode = Issue.IssueCode.Null;

        //    //                    foreach (var issues in dates.Value["damagedList"])
        //    //                    {
        //    //                        issueCode = ParseIssueCode(issues.Key);
        //    //                        foreach (var node in issues.Value)
        //    //                        {
        //    //                            JSONNode _node = node;

        //    //                            Indicator.ImP_option imgOption = Indicator.ImP_option.Damage;
        //    //                            string _issueCode = _node["cdTunnelDamaged"].Value;

        //    //                            string description = _node["dcRemark"].Value;
        //    //                            string fgroup = _node["fgroup"].Value;

        //    //                            List<Dictionary<string, string>> imgIndexes = new List<Dictionary<string, string>>();

        //    //                            JSONArray files = _node["files"].AsArray;
        //    //                            if (files != null)
        //    //                            {
        //    //                                int index = files.Count;
        //    //                                for (int i = 0; i < index; i++)
        //    //                                {
        //    //                                    Dictionary<string, string> inDic = new Dictionary<string, string>();
        //    //                                    inDic.Add("fid", files[i]["fid"].Value);
        //    //                                    inDic.Add("ftype", files[i]["ftype"].Value);
        //    //                                    imgIndexes.Add(inDic);
        //    //                                }

        //    //                                if (option == Indicator.ImP_option.DamageInDay)
        //    //                                {
        //    //                                    Issue.DamagedIssue _issue = issue as Issue.DamagedIssue;
        //    //                                    if ((_issue.CheckDate == _node["dtCheck"].Value) && (_issue.IssueOrderCode == _issueCode))
        //    //                                    {
        //    //                                        Indicator.ImageIndex imgIndex = new Indicator.ImageIndex(
        //    //                                        _date: date,
        //    //                                        _issueCode: issueCode,
        //    //                                        _description: description,
        //    //                                        _imgOption: imgOption,
        //    //                                        _issueOrderCode: _issueCode,
        //    //                                        _fgroup: fgroup,
        //    //                                        _imgList: imgIndexes
        //    //                                        );
        //    //                                        imageIndexList.Add(imgIndex);
        //    //                                    }
        //    //                                }
        //    //                                else if (option == Indicator.ImP_option.Damage)
        //    //                                {
        //    //                                    Issue.DamagedIssue _issue = issue as Issue.DamagedIssue;
        //    //                                    if (_issue.IssueOrderCode == _issueCode)
        //    //                                    {
        //    //                                        Indicator.ImageIndex imgIndex = new Indicator.ImageIndex(
        //    //                                            _date: date,
        //    //                                            _issueCode: issueCode,
        //    //                                            _description: description,
        //    //                                            _imgOption: imgOption,
        //    //                                            _issueOrderCode: _issueCode,
        //    //                                            _fgroup: fgroup,
        //    //                                            _imgList: imgIndexes
        //    //                                            );
        //    //                                        imageIndexList.Add(imgIndex);
        //    //                                    }
        //    //                                }
        //    //                            }

        //    //                        }
        //    //                    }
        //    //                }
        //    //                if (dates.Value["recoverList"].Count > 0 && option == Indicator.ImP_option.Recover)
        //    //                {
        //    //                    Issue.IssueCode issueCode = Issue.IssueCode.Null;

        //    //                    foreach (var issues in dates.Value["recoverList"])
        //    //                    {
        //    //                        issueCode = ParseIssueCode(issues.Key);
        //    //                        foreach (var node in issues.Value)
        //    //                        {
        //    //                            JSONNode _node = node;

        //    //                            Indicator.ImP_option imgOption = Indicator.ImP_option.Recover;
        //    //                            string _issueCode = _node["cdTunnelRecover"].Value;

        //    //                            List<Dictionary<string, string>> imgIndexes = new List<Dictionary<string, string>>();

        //    //                            string description = _node["dcRecover"].Value;
        //    //                            string fgroup = _node["fgroup"].Value;

        //    //                            JSONArray files = _node["files"].AsArray;
        //    //                            if (files != null)
        //    //                            {
        //    //                                int index = files.Count;
        //    //                                for (int i = 0; i < index; i++)
        //    //                                {
        //    //                                    Dictionary<string, string> inDic = new Dictionary<string, string>();
        //    //                                    inDic.Add("fid", files[i]["fid"].Value);
        //    //                                    inDic.Add("ftype", files[i]["ftype"].Value);
        //    //                                    imgIndexes.Add(inDic);
        //    //                                }

        //    //                                Issue.RecoveredIssue _issue = issue as Issue.RecoveredIssue;
        //    //                                if(_issue.IssueOrderCode == _issueCode)
        //    //                                {
        //    //                                    Indicator.ImageIndex imgIndex = new Indicator.ImageIndex(
        //    //                                        _date: date,
        //    //                                        _issueCode: issueCode,
        //    //                                        _description: description,
        //    //                                        _imgOption: imgOption,
        //    //                                        _issueOrderCode: _issueCode,
        //    //                                        _fgroup: fgroup,
        //    //                                        _imgList: imgIndexes
        //    //                                        );
        //    //                                    imageIndexList.Add(imgIndex);
        //    //                                }
        //    //                            }
        //    //                        }
        //    //                    }


        //    //                }
        //    //            }

        //    //            (Manager.UIManager.Instance.indicatorDic[Indicator.IndicatorType.ImP] as Indicator.ImP_Indicator).SetImages(imageIndexList);
        //    //        }
        //    //        break;
        //    //}
        //}

        private void SetJSONData(JSONLoadType loadType, string _JSONData, string restrict = "")
        {
//            string _data = "";
//            JSONArray _issueArray;

//            switch (loadType)
//            {
//                case JSONLoadType.BridgeList:       // Editor routine
//                    {
//                        _data = JSON.Parse(_JSONData)["data"].ToString();

//                        List<RuntimeData.BridgeList> bridgeList = new List<RuntimeData.BridgeList>();

//                        _issueArray = JSON.Parse(_data).AsArray;
//                        int index = _issueArray.Count;
//                        for (int i = 0; i < index; i++)
//                        {
//                            RuntimeData.BridgeList _bridge = new RuntimeData.BridgeList();

//                            _bridge.bridgeCode = _issueArray[i]["cdBridge"].Value;
//                            _bridge.address = _issueArray[i]["nmAddress"].Value;

//                            bridgeList.Add(_bridge);
//                        }

//                        RuntimeData.RootContainer.Instance.bridgeCodeList = bridgeList;

//                        MainManager.Instance.EditorInitialize();

//                    }
//                    break;

//                case JSONLoadType.Address:
//                    {
//                        #region Debug
//#if UNITY_EDITOR
//                        EditDebug.PrintLoadJSONRoutine("Issue Load : Address");
//#endif
//                        #endregion

//                        //Debug.LogError("O :: 1122 description data blocked");

//                        //return;

//                        _data = JSON.Parse(_JSONData)["data"].ToString();
//                        _data = JSON.Parse(_data)[0].ToString();

//                        string _type = JSON.Parse(_data)["fgUF001"].Value;
//                        string liningMaterial = JSON.Parse(_data)["fgLM001"].Value;

//                        if(liningMaterial == "0001")
//						{
//							MainManager.Instance.liningMaterial = "콘크리트";
//                        }
//                        else if(liningMaterial == "0002")
//						{
//                            MainManager.Instance.liningMaterial = "파형강판";
//						}

//                        GameObject obj = null;
//                        if(_type == "0101")
//						{
//                            obj = Instantiate<GameObject>(Resources.Load<GameObject>("FBX/아치형 1130 Dimension"));
//                        }
//                        else
//                        {
//                            // 박스형
//                            obj = Instantiate<GameObject>(Resources.Load<GameObject>("FBX/박스형 1130 Dimension"));
//                        }

//                        if(obj == null)
//						{
//                            Debug.LogError("치수선 관리객체 생성 실패");
//						}
//                        else
//						{
//                            MainManager.Instance.dimcode = obj.GetComponent<Dim.DimScript>();
//						}

//                        string __data = JSON.Parse(_data)["nmAddress"].Value;

//                        string _skew = JSON.Parse(_data)["ynRectangular"].Value;
//                        string _curve = JSON.Parse(_data)["ynCurve"].Value;

//                        if (_skew == "n" && _curve == "n")   // 직교
//                        {
//                            MainManager.Instance.visibleOption = InVisibleOption.Interactable;
//                        }
//                        else if (_skew == "n" && _curve == "y") // 곡교
//                        {
//                            MainManager.Instance.visibleOption = InVisibleOption.Curve;
//                        }
//                        else if (_skew == "y" && _curve == "n") // 사교
//                        {
//                            MainManager.Instance.visibleOption = InVisibleOption.Skew;
//                        }
//                        else if (_skew == "y" && _curve == "y") // 사곡교
//                        {
//                            MainManager.Instance.visibleOption = InVisibleOption.CurveSkew;
//                        }

//                        MainManager.Instance.bridgeType = JSON.Parse(_data)["fgUF001"].Value;
//                        //MainManager.Instance.bridgeType = JSON.Parse(_data)["fgUS001"].Value;



//                        RuntimeData.RootContainer.Instance.MainFGroup = JSON.Parse(_data)["fgroup"].Value;

//                        JSONArray _mainPictureArray = JSON.Parse(_data)["files"].AsArray;

//                        if(_mainPictureArray != null)
//                        {
//                            if (_mainPictureArray.Count > 0)
//                            {
//                                RuntimeData.RootContainer.Instance.MainFid = _mainPictureArray[0]["fid"].Value;
//                                RuntimeData.RootContainer.Instance.MainFType = _mainPictureArray[0]["ftype"].Value;
//                            }
//                            else
//                            {
//                                RuntimeData.RootContainer.Instance.MainFid = null;
//                                RuntimeData.RootContainer.Instance.MainFType = null;
//                            }
//                        }

//                        RuntimeData.RootContainer.Instance.Address = __data;

//                        isAdrEnd = true;
//                    }
//                    break;

//                case JSONLoadType.Damaged:
//                    {
//                        #region Debug
//#if UNITY_EDITOR
//                        EditDebug.PrintLoadJSONRoutine("Issue Load : damaged");
//#endif
//                        #endregion

//                        Dictionary<Issue.KeyOfDamages, string> _dics = new Dictionary<Issue.KeyOfDamages, string>();

//                        //Debug.LogError("1122 Issue dmg blocked");

//                        //return;

//                        _data = JSON.Parse(_JSONData)["data"].ToString();

//                        //Debug.Log("이름 : " +JSON.Parse(_data)["nmBridge"].ToString());
//                        RuntimeData.RootContainer.Instance.BridgeName = JSON.Parse(_data)["nmTunnel"].Value;
//                        //RuntimeData.RootContainer.Instance.BridgeName = JSON.Parse(_data)["nmBridge"].Value;


//                        _issueArray = JSON.Parse(_data)[loadKeys[loadType]].AsArray;
//                        for (int i = 0; i < _issueArray.Count; i++)
//                        {
//                            GameObject dmgObject = Instantiate(Resources.Load<GameObject>("Cache/cacheIssue"));
//                            dmgObject.AddComponent<Issue.DamagedIssue>();
//                            dmgObject.layer = LayerMask.NameToLayer("Issue");

//                            string __data = _issueArray[i].ToString();

//                            _dics.Add(Issue.KeyOfDamages.cdTunnel, JSON.Parse(__data)[Issue.KeyOfDamages.cdTunnel.ToString()].Value);
//                            _dics.Add(Issue.KeyOfDamages.cdTunnelDamaged, JSON.Parse(__data)[Issue.KeyOfDamages.cdTunnelDamaged.ToString()].Value);
//                            _dics.Add(Issue.KeyOfDamages.fgDA001, JSON.Parse(__data)[Issue.KeyOfDamages.fgDA001.ToString()].Value);
//                            _dics.Add(Issue.KeyOfDamages.nmUser, JSON.Parse(__data)[Issue.KeyOfDamages.nmUser.ToString()].Value);

//                            _dics.Add(Issue.KeyOfDamages.cdTunnelParts, JSON.Parse(__data)[Issue.KeyOfDamages.cdTunnelParts.ToString()].Value);
//                            _dics.Add(Issue.KeyOfDamages.dcDamageMemberSurface, JSON.Parse(__data)[Issue.KeyOfDamages.dcDamageMemberSurface.ToString()].Value);
//                            _dics.Add(Issue.KeyOfDamages.dcLocation, JSON.Parse(__data)[Issue.KeyOfDamages.dcLocation.ToString()].Value);
//                            _dics.Add(Issue.KeyOfDamages.dcPinLocation, JSON.Parse(__data)[Issue.KeyOfDamages.dcPinLocation.ToString()].Value);

//                            _dics.Add(Issue.KeyOfDamages.dtCheck, JSON.Parse(__data)[Issue.KeyOfDamages.dtCheck.ToString()].Value);

//                            _dics.Add(Issue.KeyOfDamages.dcGrade, JSON.Parse(__data)[Issue.KeyOfDamages.dcGrade.ToString()].Value);

//                            _dics.Add(Issue.KeyOfDamages.noDamageWidth, JSON.Parse(__data)[Issue.KeyOfDamages.noDamageWidth.ToString()].Value);
//                            _dics.Add(Issue.KeyOfDamages.noDamageHeight, JSON.Parse(__data)[Issue.KeyOfDamages.noDamageHeight.ToString()].Value);
//                            _dics.Add(Issue.KeyOfDamages.noDamageDepth, JSON.Parse(__data)[Issue.KeyOfDamages.noDamageDepth.ToString()].Value);
//                            _dics.Add(Issue.KeyOfDamages.dcRemark, JSON.Parse(__data)[Issue.KeyOfDamages.dcRemark.ToString()].Value);

//                            //====================================================================================

//                            string fGroup = SetFGroup(JSON.Parse(__data)[Issue.KeyOfDamages.fgroup.ToString()].Value);

//                            _dics.Add(Issue.KeyOfDamages.fgroup, fGroup);

//                            List<Dictionary<Issue.KeyOfDamages, string>> imgIndexes = new List<Dictionary<Issue.KeyOfDamages, string>>();

//                            if (_dics[Issue.KeyOfDamages.fgroup] != null)
//                            {
//                                JSONArray imageArray = JSON.Parse(__data)["files"].AsArray;
//                                if (imageArray != null)
//                                {
//                                    Dictionary<Issue.KeyOfDamages, string> inDic;

//                                    for (int ii = 0; ii < imageArray.Count; ii++)
//                                    {
//                                        inDic = new Dictionary<Issue.KeyOfDamages, string>();

//                                        string __imgData = imageArray[ii].ToString();

//#if UNITY_EDITOR
//                                        EditDebug.PrintINITJSONIMGRoutine($"Issue image file {ii} : {__imgData}");
//#endif

//                                        inDic.Add(Issue.KeyOfDamages.fid, JSON.Parse(__imgData)[Issue.KeyOfDamages.fid.ToString()].Value);
//                                        inDic.Add(Issue.KeyOfDamages.ftype, JSON.Parse(__imgData)[Issue.KeyOfDamages.ftype.ToString()].Value);

//                                        //Debug.Log(inDic[Issue.KeyOfDamages.fid]);
//                                        //Debug.Log(inDic[Issue.KeyOfDamages.ftype]);

//                                        imgIndexes.Add(inDic);

//                                        //Debug.Log($"fid : {inDic[Issue.KeyOfDamages.fid]}");
//                                        //Debug.Log($"ftype : {inDic[Issue.KeyOfDamages.ftype]}");
//                                    }
//                                }
//                            }
//                            //_dics.Add(Issue.KeyOfDamages.fgroup, JSON.Parse(__data)[Issue.KeyOfDamages.fgroup.ToString()].Value);

//                            //Debug.Log($"{JSON.Parse(__data)[Issue.KeyOfDamages.fgroup.ToString()].Value}");

//                            //string fGroup = JSON.Parse(__data)["fgroup"].Value; // 이미지 받아오기용 fgroup 변수 할당
//                            //Debug.Log($"fGroup : {fGroup}");



//                            //Debug.Log($"Data {i} : {__data}");

//                            //Debug.Log("BridgeName : " + JSON.Parse(__data)["cdBridge"].ToString());
//                            //Debug.Log("BridgeDamagedCode : " + JSON.Parse(__data)["cdBridgeDamaged"].ToString());       // 다름
//                            //Debug.Log("issueCode : " + JSON.Parse(__data)["fgDA001"].ToString());
//                            //Debug.Log("Bridge part : " + JSON.Parse(__data)["cdBridgeParts"].ToString());
//                            //Debug.Log("Issue6Surface : " + JSON.Parse(__data)["dcDamageMemberSurface"].ToString());
//                            //Debug.Log("Issue9Pos : " + JSON.Parse(__data)["dcLocation"].ToString());
//                            //Debug.Log("Issue Position : " + JSON.Parse(__data)["dcPinLocation"].ToString());

//                            //Debug.Log("date start : " + JSON.Parse(__data)["dtCheck"].ToString());                      // 다름

//                            dmgObject.GetComponent<Issue.DamagedIssue>().SetObject<Dictionary<Issue.KeyOfDamages, string>>(_dics);
//                            dmgObject.GetComponent<Issue.DamagedIssue>().SetObject<List<Dictionary<Issue.KeyOfDamages, string>>>(imgIndexes);

//                            dmgObject.name = dmgObject.GetComponent<Issue.AIssue>().IssueOrderCode;
//                            dmgObject.transform.position = dmgObject.GetComponent<Issue.AIssue>().PinVector;
//                            dmgObject.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/Damage/damagedIssue");

//                            Issue.IssueCode _code = dmgObject.GetComponent<Issue.DamagedIssue>().IssueCodes;
//                            dmgObject.GetComponent<MeshRenderer>().material.SetTexture("_Texture", Resources.Load<Texture>($"Materials/Damage/Textures/{_code.ToString()}"));

//                            RuntimeData.RootContainer.Instance.IssueObjectList.Add(dmgObject.GetComponent<Issue.AIssue>());
//                            dmgObject.transform.SetParent(RuntimeData.RootContainer.Instance.RootIssueObject);

//                            _dics.Clear();
//                        }

//                        isDmgEnd = true;
//                    }
//                    break;

//                case JSONLoadType.Recovered:
//                    {
//                        #region Debug
//#if UNITY_EDITOR
//                        EditDebug.PrintLoadJSONRoutine("Issue Load : recovered");
//#endif
//                        #endregion

//                        //Debug.LogError("1122 Issue rcv blocked");

//                        //return;

//                        Dictionary<Issue.KeyOfRecovers, string> _dics = new Dictionary<Issue.KeyOfRecovers, string>();

//                        _data = JSON.Parse(_JSONData)["data"].ToString();
//                        _issueArray = JSON.Parse(_data)[loadKeys[loadType]].AsArray;
//                        for (int i = 0; i < _issueArray.Count; i++)
//                        {
//                            //GameObject rcvObject = new GameObject("rcvObject");
//                            GameObject rcvObject = Instantiate(Resources.Load<GameObject>("Cache/cacheIssue"));
//                            rcvObject.AddComponent<Issue.RecoveredIssue>();
//                            rcvObject.layer = LayerMask.NameToLayer("Issue");

//                            string __data = _issueArray[i].ToString();

//                            _dics.Add(Issue.KeyOfRecovers.cdTunnel, JSON.Parse(__data)[Issue.KeyOfRecovers.cdTunnel.ToString()].Value);
//                            _dics.Add(Issue.KeyOfRecovers.cdTunnelRecover, JSON.Parse(__data)[Issue.KeyOfRecovers.cdTunnelRecover.ToString()].Value);
//                            _dics.Add(Issue.KeyOfRecovers.fgDA001, JSON.Parse(__data)[Issue.KeyOfRecovers.fgDA001.ToString()].Value);
//                            _dics.Add(Issue.KeyOfRecovers.nmUser, JSON.Parse(__data)[Issue.KeyOfRecovers.nmUser.ToString()].Value);

//                            _dics.Add(Issue.KeyOfRecovers.cdTunnelParts, JSON.Parse(__data)[Issue.KeyOfRecovers.cdTunnelParts.ToString()].Value);
//                            _dics.Add(Issue.KeyOfRecovers.dcDamageMemberSurface, JSON.Parse(__data)[Issue.KeyOfRecovers.dcDamageMemberSurface.ToString()].Value);
//                            _dics.Add(Issue.KeyOfRecovers.dcLocation, JSON.Parse(__data)[Issue.KeyOfRecovers.dcLocation.ToString()].Value);
//                            _dics.Add(Issue.KeyOfRecovers.dcPinLocation, JSON.Parse(__data)[Issue.KeyOfRecovers.dcPinLocation.ToString()].Value);

//                            _dics.Add(Issue.KeyOfRecovers.dtStart, JSON.Parse(__data)[Issue.KeyOfRecovers.dtStart.ToString()].Value);
//                            _dics.Add(Issue.KeyOfRecovers.dtEnd, JSON.Parse(__data)[Issue.KeyOfRecovers.dtEnd.ToString()].Value);

//                            _dics.Add(Issue.KeyOfRecovers.dcGrade, JSON.Parse(__data)[Issue.KeyOfRecovers.dcGrade.ToString()].Value);
//                            _dics.Add(Issue.KeyOfRecovers.noDamageWidth, JSON.Parse(__data)[Issue.KeyOfRecovers.noDamageWidth.ToString()].Value);
//                            _dics.Add(Issue.KeyOfRecovers.noDamageHeight, JSON.Parse(__data)[Issue.KeyOfRecovers.noDamageHeight.ToString()].Value);
//                            _dics.Add(Issue.KeyOfRecovers.noDamageDepth, JSON.Parse(__data)[Issue.KeyOfRecovers.noDamageDepth.ToString()].Value);
//                            _dics.Add(Issue.KeyOfRecovers.dcRemark, JSON.Parse(__data)[Issue.KeyOfRecovers.dcRemark.ToString()].Value);
//                            //Debug.Log($"Recover Data {i} : {__data}");

//                            //====================================================================================

//                            string fGroup = SetFGroup(JSON.Parse(__data)[Issue.KeyOfRecovers.fgroup.ToString()].Value);

//                            _dics.Add(Issue.KeyOfRecovers.fgroup, fGroup);

//                            List<Dictionary<Issue.KeyOfRecovers, string>> imgIndexes = new List<Dictionary<Issue.KeyOfRecovers, string>>();

//                            if (_dics[Issue.KeyOfRecovers.fgroup] != null)
//                            {
//                                JSONArray imageArray = JSON.Parse(__data)["files"].AsArray;
//                                Dictionary<Issue.KeyOfRecovers, string> inDic;
//                                if (imageArray != null)
//                                {
//                                    for (int ii = 0; ii < imageArray.Count; ii++)
//                                    {
//                                        inDic = new Dictionary<Issue.KeyOfRecovers, string>();

//                                        string __imgData = imageArray[ii].ToString();

//#if UNITY_EDITOR
//                                        EditDebug.PrintINITJSONIMGRoutine($"Issue image file {ii} : {__imgData}");
//#endif

//                                        inDic.Add(Issue.KeyOfRecovers.fid, JSON.Parse(__imgData)[Issue.KeyOfRecovers.fid.ToString()].Value);
//                                        inDic.Add(Issue.KeyOfRecovers.ftype, JSON.Parse(__imgData)[Issue.KeyOfRecovers.ftype.ToString()].Value);

//                                        imgIndexes.Add(inDic);
//                                    }
//                                    //Dictionary<Issue.kor>
//                                }

//                                imageArray = JSON.Parse(__data)["filesDamaged"].AsArray;
//                                if (imageArray != null)
//                                {
//                                    for (int ii = 0; ii < imageArray.Count; ii++)
//                                    {
//                                        inDic = new Dictionary<Issue.KeyOfRecovers, string>();

//                                        string __imgData = imageArray[ii].ToString();

//#if UNITY_EDITOR
//                                        EditDebug.PrintINITJSONIMGRoutine($"Issue image file {ii} : {__imgData}");
//#endif

//                                        inDic.Add(Issue.KeyOfRecovers.fid, JSON.Parse(__imgData)[Issue.KeyOfRecovers.fid.ToString()].Value);
//                                        inDic.Add(Issue.KeyOfRecovers.ftype, JSON.Parse(__imgData)[Issue.KeyOfRecovers.ftype.ToString()].Value);

//                                        imgIndexes.Add(inDic);
//                                    }
//                                }
//                            }

//                            rcvObject.GetComponent<Issue.RecoveredIssue>().SetObject<Dictionary<Issue.KeyOfRecovers, string>>(_dics);
//                            rcvObject.GetComponent<Issue.RecoveredIssue>().SetObject<List<Dictionary<Issue.KeyOfRecovers, string>>>(imgIndexes);

//                            rcvObject.name = rcvObject.GetComponent<Issue.AIssue>().IssueOrderCode;
//                            rcvObject.transform.position = rcvObject.GetComponent<Issue.AIssue>().PinVector;
//                            rcvObject.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/Damage/recoveredIssue");

//                            Issue.IssueCode _code = rcvObject.GetComponent<Issue.RecoveredIssue>().IssueCodes;
//                            rcvObject.GetComponent<MeshRenderer>().material.SetTexture("_Texture", Resources.Load<Texture>($"Materials/Damage/Textures/{_code.ToString()}"));

//                            RuntimeData.RootContainer.Instance.IssueObjectList.Add(rcvObject.GetComponent<Issue.AIssue>());
//                            rcvObject.transform.SetParent(RuntimeData.RootContainer.Instance.RootIssueObject);

//                            _dics.Clear();
//                        }

//                        isRcvEnd = true;
//                    }
//                    break;

//                case JSONLoadType.History:
//                case JSONLoadType.IssueHistory:
//                case JSONLoadType.TotalHistory:
//                    {
//                        //table 정의
//                        DataTable _historyTable = new DataTable();

//                        //column 정의
//                        _historyTable.Columns.Add(new DataColumn("date", typeof(string)));
//                        _historyTable.Columns.Add(new DataColumn("Dcrack", typeof(List<Issue.RecordIssue>)));
//                        _historyTable.Columns.Add(new DataColumn("Dbagli", typeof(List<Issue.RecordIssue>)));
//                        _historyTable.Columns.Add(new DataColumn("Dbaegtae", typeof(List<Issue.RecordIssue>)));
//                        _historyTable.Columns.Add(new DataColumn("Dsegul", typeof(List<Issue.RecordIssue>)));
//                        _historyTable.Columns.Add(new DataColumn("Ddamage", typeof(List<Issue.RecordIssue>)));
//                        _historyTable.Columns.Add(new DataColumn("Rcrack", typeof(List<Issue.RecordIssue>)));
//                        _historyTable.Columns.Add(new DataColumn("Rbagli", typeof(List<Issue.RecordIssue>)));
//                        _historyTable.Columns.Add(new DataColumn("Rbaegtae", typeof(List<Issue.RecordIssue>)));
//                        _historyTable.Columns.Add(new DataColumn("Rsegul", typeof(List<Issue.RecordIssue>)));
//                        _historyTable.Columns.Add(new DataColumn("Rdamage", typeof(List<Issue.RecordIssue>)));

//                        //Row 생성
//                        JSONNode dailyList = JSON.Parse(JSON.Parse(_JSONData)["data"].ToString())["dailyList"];

//                        int _index = 0;
//                        foreach (var dailykeys in dailyList)
//                        {
//                            DataRow row = _historyTable.NewRow();
//                            row["date"] = dailykeys.Key;
//                            if (dailykeys.Value["damagedList"].Count > 0)
//                            {
//                                foreach (var dType in dailykeys.Value["damagedList"])
//                                {
//                                    string code = string.Empty;
//                                    switch (dType.Key)
//                                    {
//                                        case "0001":
//                                            {
//                                                List<Issue.RecordIssue> issueList = new List<Issue.RecordIssue>();

//                                                foreach (var node in dType.Value)
//                                                {
//                                                    Issue.RecordIssue _recordIssue = SetRecordInstance(node);
//                                                    issueList.Add(_recordIssue);
//                                                }
//                                                row["Dcrack"] = issueList;
//                                            }
//                                            break;

//                                        case "0007":
//                                            {
//                                                List<Issue.RecordIssue> issueList = new List<Issue.RecordIssue>();
//                                                foreach (var node in dType.Value)
//                                                {
//                                                    Issue.RecordIssue _recordIssue = SetRecordInstance(node);
//                                                    issueList.Add(_recordIssue);
//                                                }
//                                                row["Dbagli"] = issueList;
//                                            }
//                                            break;

//                                        case "0009":
//                                            {
//                                                List<Issue.RecordIssue> issueList = new List<Issue.RecordIssue>();
//                                                foreach (var node in dType.Value)
//                                                {
//                                                    Issue.RecordIssue _recordIssue = SetRecordInstance(node);
//                                                    issueList.Add(_recordIssue);
//                                                }
//                                                row["Dbaegtae"] = issueList;
//                                            }
//                                            break;

//                                        case "0013":
//                                            {
//                                                List<Issue.RecordIssue> issueList = new List<Issue.RecordIssue>();
//                                                foreach (var node in dType.Value)
//                                                {
//                                                    Issue.RecordIssue _recordIssue = SetRecordInstance(node);
//                                                    issueList.Add(_recordIssue);
//                                                }
//                                                row["Dsegul"] = issueList;
//                                            }
//                                            break;

//                                        case "0022":
//                                            {
//                                                List<Issue.RecordIssue> issueList = new List<Issue.RecordIssue>();
//                                                foreach (var node in dType.Value)
//                                                {
//                                                    Issue.RecordIssue _recordIssue = SetRecordInstance(node);
//                                                    issueList.Add(_recordIssue);
//                                                }
//                                                row["Ddamage"] = issueList;
//                                            }
//                                            break;
//                                    }
//                                }
//                            }
//                            if (dailykeys.Value["recoverList"].Count > 0)
//                            {
//                                foreach (var dType in dailykeys.Value["recoverList"])
//                                {
//                                    string code = string.Empty;
//                                    switch (dType.Key)
//                                    {
//                                        case "0001":
//                                            {
//                                                List<Issue.RecordIssue> issueList = new List<Issue.RecordIssue>();
//                                                foreach (var node in dType.Value)
//                                                {
//                                                    Issue.RecordIssue _recordIssue = SetRecordInstance(node);
//                                                    issueList.Add(_recordIssue);
//                                                }
//                                                row["Rcrack"] = issueList;
//                                            }
//                                            break;

//                                        case "0007":
//                                            {
//                                                List<Issue.RecordIssue> issueList = new List<Issue.RecordIssue>();
//                                                foreach (var node in dType.Value)
//                                                {
//                                                    Issue.RecordIssue _recordIssue = SetRecordInstance(node);
//                                                    issueList.Add(_recordIssue);
//                                                }
//                                                row["Rbagli"] = issueList;
//                                            }
//                                            break;

//                                        case "0009":
//                                            {
//                                                List<Issue.RecordIssue> issueList = new List<Issue.RecordIssue>();
//                                                foreach (var node in dType.Value)
//                                                {
//                                                    Issue.RecordIssue _recordIssue = SetRecordInstance(node);
//                                                    issueList.Add(_recordIssue);
//                                                }
//                                                row["Rbaegtae"] = issueList;
//                                            }
//                                            break;

//                                        case "0013":
//                                            {
//                                                List<Issue.RecordIssue> issueList = new List<Issue.RecordIssue>();
//                                                foreach (var node in dType.Value)
//                                                {
//                                                    Issue.RecordIssue _recordIssue = SetRecordInstance(node);
//                                                    issueList.Add(_recordIssue);
//                                                }
//                                                row["Rsegul"] = issueList;
//                                            }
//                                            break;

//                                        case "0022":
//                                            {
//                                                List<Issue.RecordIssue> issueList = new List<Issue.RecordIssue>();
//                                                foreach (var node in dType.Value)
//                                                {
//                                                    Issue.RecordIssue _recordIssue = SetRecordInstance(node);
//                                                    issueList.Add(_recordIssue);
//                                                }
//                                                row["Rdamage"] = issueList;
//                                            }
//                                            break;
//                                    }
//                                }
//                            }
//                            _historyTable.Rows.Add(row);
//                        }
//                        // 20201127 DataTable 생성 완료 후 date 내림차순으로 다시 설정
//                        //DataView dv = new DataView(_historyTable);  // _historyTable로 DataView 형태의 dv 생성
//                        //dv.Sort = "date DESC";  // dv를 date(날짜) 내림차순으로 정렬
//                        //_historyTable = dv.ToTable();   // 내림차순으로 정렬된 dv를 _historyTable에 할당
//                        //DataRow[] _rows = _historyTable.Select("","date DESC");
//                        //Debug.Log("현재 dataview 정렬 확인 중");

//                        //DataTable tableTemp = _historyTable.Clone();
//                        //for (int i = 0; i < _rows.Length; i++)
//                        //    tableTemp.ImportRow(_rows[i]);
//                        //값 전달
//                        ObjectManager.Instance.historyTable = _historyTable;
//                        //if (loadType == JSONLoadType.History)
//                        //{
//                        //    (UIManager.Instance.indicatorDic[Indicator.IndicatorType.MPM2] as Indicator.MPM2_Indicator)
//                        //        .GetViewPart2RDataTable(_historyTable);
//                        //}
//                        //else if (loadType == JSONLoadType.IssueHistory)
//                        //{
//                        //    (UIManager.Instance.indicatorDic[Indicator.IndicatorType.MPM2] as Indicator.MPM2_Indicator)
//                        //        .GetViewMaintainanceDataTable(_historyTable);
//                        //}
//                        if (loadType == JSONLoadType.TotalHistory)
//                        {
//                            (UIManager.Instance.indicatorDic[Indicator.IndicatorType.State5_BP2] as Indicator.State5_BP2_Indicator)
//                                .GetHistoryTable(_historyTable);
//                        }

//                        isHistoryEnd = true;
//                    }
//                    break;

//                case JSONLoadType.HistoryImage:
//                    {
//                        _data = JSON.Parse(_JSONData)["data"].ToString();

//                        string _dailyList = JSON.Parse(_data)["dailyList"].ToString();
//                        JSONNode dailyList = JSON.Parse(_data)["dailyList"];

//                        string date = "";
//                        List<Indicator.ImageIndex> imageIndexList = new List<Indicator.ImageIndex>();

//                        //Debug.Log("restrict : " + restrict);

//                        int count = 0;
//                        foreach (var dates in dailyList)
//                        {
//                            count++;
//                            //Debug.Log($"{count} {dates}");

//                            date = dates.Key;   // 날짜
//                            if (dates.Value["damagedList"].Count > 0)
//                            {
//                                Issue.IssueCode issueCode = Issue.IssueCode.Null;

//                                foreach (var issues in dates.Value["damagedList"])
//                                {
//                                    issueCode = ParseIssueCode(issues.Key);
//                                    foreach (var node in issues.Value)
//                                    {
//                                        JSONNode _node = node;

//                                        Indicator.ImP_option imgOption = Indicator.ImP_option.Damage;
//                                        string _issueCode = _node["cdTunnelDamaged"].Value;

//                                        string description = _node["dcRemark"].Value;
//                                        string fgroup = _node["fgroup"].Value;

//                                        List<Dictionary<string, string>> imgIndexes = new List<Dictionary<string, string>>();

//                                        JSONArray files = _node["files"].AsArray;
//                                        if (files != null)
//                                        {
//                                            int index = files.Count;
//                                            for (int i = 0; i < index; i++)
//                                            {
//                                                Dictionary<string, string> inDic = new Dictionary<string, string>();
//                                                inDic.Add("fid", files[i]["fid"].Value);
//                                                inDic.Add("ftype", files[i]["ftype"].Value);
//                                                imgIndexes.Add(inDic);
//                                            }
//                                        }

//                                        Indicator.ImageIndex imgIndex = new Indicator.ImageIndex(
//                                            _date: date,
//                                            _issueCode: issueCode,
//                                            _description: description,
//                                            _imgOption: imgOption,
//                                            _issueOrderCode: _issueCode,
//                                            _fgroup: fgroup,
//                                            _imgList: imgIndexes
//                                            );
//                                        imageIndexList.Add(imgIndex);
//                                    }
//                                }
//                            }
//                            if (dates.Value["recoverList"].Count > 0)
//                            {
//                                Issue.IssueCode issueCode = Issue.IssueCode.Null;

//                                foreach (var issues in dates.Value["recoverList"])
//                                {
//                                    issueCode = ParseIssueCode(issues.Key);
//                                    foreach (var node in issues.Value)
//                                    {
//                                        JSONNode _node = node;

//                                        Indicator.ImP_option imgOption = Indicator.ImP_option.Recover;
//                                        string _issueCode = _node["cdTunnelRecover"].Value;

//                                        List<Dictionary<string, string>> imgIndexes = new List<Dictionary<string, string>>();

//                                        string description = _node["dcRecover"].Value;
//                                        string fgroup = _node["fgroup"].Value;

//                                        JSONArray files = _node["files"].AsArray;
//                                        if (files != null)
//                                        {
//                                            int index = files.Count;
//                                            for (int i = 0; i < index; i++)
//                                            {
//                                                Dictionary<string, string> inDic = new Dictionary<string, string>();
//                                                inDic.Add("fid", files[i]["fid"].Value);
//                                                inDic.Add("ftype", files[i]["ftype"].Value);
//                                                imgIndexes.Add(inDic);
//                                            }

//                                            Indicator.ImageIndex imgIndex = new Indicator.ImageIndex(
//                                                _date: date,
//                                                _issueCode: issueCode,
//                                                _description: description,
//                                                _imgOption: imgOption,
//                                                _issueOrderCode: _issueCode,
//                                                _fgroup: fgroup,
//                                                _imgList: imgIndexes
//                                                );
//                                            imageIndexList.Add(imgIndex);
//                                        }
//                                    }
//                                }


//                            }
//                        }

//                        (Manager.UIManager.Instance.indicatorDic[Indicator.IndicatorType.ImP] as Indicator.ImP_Indicator).SetImages(imageIndexList);

//                    }
//                    break;
//            }
        }

        //private Issue.RecordIssue SetRecordInstance(JSONNode _jsonNode)
        //{
        //    Issue.RecordIssue _issue = new Issue.RecordIssue();

        //    _issue.SetObject<JSONNode>(_jsonNode);

        //    return _issue;
        //}

        private string SetFGroup(string arg)
        {
            string[] result = arg.Split('-');

            if (result.Length == 2 && arg.Length == 17)
            {
                return arg;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region 이미지 받아오기

        //        public IEnumerator LoadJSONCallback(JSONLoadType loadType, string arguments, Indicator.Element.BP3_DmgImgElement callback)
        //        {
        //            string URI = "";

        //            URI = loadURL[loadType] + arguments;

        //            //Debug.Log($"Load Image : \n" +
        //            //    $"{URI}");

        //#if UNITY_EDITOR
        //            EditDebug.PrintINITJSONRoutine(URI);
        //#endif

        //            using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(URI))
        //            {
        //                yield return webRequest.SendWebRequest();

        //                if (webRequest.isNetworkError || webRequest.isHttpError)
        //                {
        //                    Debug.Log(string.Format($"Error : {webRequest.error}"));
        //                }
        //                else
        //                {
        //                    //Debug.Log("======================================");
        //                    //Debug.Log($"URI : {URI}");
        //                    //Debug.Log($"arguments : {arguments}");
        //                    //Debug.Log("======================================");

        //                    //Texture2D _texture = new Texture2D(100, 100);
        //                    //Texture2D texture = Instantiate<Texture2D>( ((DownloadHandlerTexture)webRequest.downloadHandler).texture );

        //                    //Debug.Log(((DownloadHandlerTexture)webRequest.downloadHandler).texture);

        //                    //Debug.Log(((DownloadHandlerTexture)webRequest.downloadHandler).data.Length);



        //                    Texture2D texture = new Texture2D(100, 100);
        //                    //callback.GetTexture(texture);
        //                    callback.GetTexture(((DownloadHandlerTexture)webRequest.downloadHandler).texture);


        //                }
        //            }

        //            yield break;
        //        }

        public IEnumerator LoadJSONCallback(JSONLoadType loadType, string arguments, RawImage callback)
        {
            string URI = "";

            URI = loadURL[loadType] + arguments;

            //Debug.Log($"Load Image : \n" +
            //    $"{URI}");

#if UNITY_EDITOR
            EditDebug.PrintINITJSONRoutine(URI);
#endif

            using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(URI))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.isNetworkError || webRequest.isHttpError)
                {
                    Debug.Log(string.Format($"Error : {webRequest.error}"));
                }
                else
                {
                    //Debug.Log("======================================");
                    //Debug.Log($"URI : {URI}");
                    //Debug.Log($"arguments : {arguments}");
                    //Debug.Log("======================================");

                    //Texture2D _texture = new Texture2D(100, 100);
                    //Texture2D texture = Instantiate<Texture2D>( ((DownloadHandlerTexture)webRequest.downloadHandler).texture );

                    //Debug.Log(((DownloadHandlerTexture)webRequest.downloadHandler).texture);

                    //Debug.Log(((DownloadHandlerTexture)webRequest.downloadHandler).data.Length);

                    callback.enabled = true;

                    Texture2D texture = new Texture2D(100, 100);
                    //callback.GetTexture(texture);

                    texture = ((DownloadHandlerTexture)webRequest.downloadHandler).texture;

                    callback.texture = texture;
                    //callback.GetTexture(((DownloadHandlerTexture)webRequest.downloadHandler).texture);
                }
            }

            yield break;
        }

        public IEnumerator LoadJSONCallback(JSONLoadType loadType, string arguments, Indicator.Element.Imp_element callback)
        {
            string URI = "";

            URI = loadURL[loadType] + arguments;

            //Debug.Log($"Load Image : \n" +
            //    $"{URI}");

#if UNITY_EDITOR
            EditDebug.PrintINITJSONRoutine(URI);
#endif

            using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(URI))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.isNetworkError || webRequest.isHttpError)
                {
                    Debug.Log(string.Format($"Error : {webRequest.error}"));
                }
                else
                {
                    //Debug.Log("======================================");
                    //Debug.Log($"URI : {URI}");
                    //Debug.Log($"arguments : {arguments}");
                    //Debug.Log("======================================");

                    //Texture2D _texture = new Texture2D(100, 100);
                    //Texture2D texture = Instantiate<Texture2D>( ((DownloadHandlerTexture)webRequest.downloadHandler).texture );

                    //Debug.Log(((DownloadHandlerTexture)webRequest.downloadHandler).texture);

                    //Debug.Log(((DownloadHandlerTexture)webRequest.downloadHandler).data.Length);



                    Texture2D texture = new Texture2D(100, 100);
                    //callback.GetTexture(texture);
                    callback.GetTexture(((DownloadHandlerTexture)webRequest.downloadHandler).texture);


                }
            }

            yield break;
        }

        #endregion

        private Issue.IssueCode ParseIssueCode(string code)
        {
            switch (code)
            {
                case "0001": return Issue.IssueCode.Crack;
                case "0007": return Issue.IssueCode.Efflorescense;
                case "0009": return Issue.IssueCode.Spalling;
                case "0013": return Issue.IssueCode.Scour_Erosion;
                case "0022": return Issue.IssueCode.Breakage;
                default: return Issue.IssueCode.Null;
            }
        }
    }
}
