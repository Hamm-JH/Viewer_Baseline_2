using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;
//using Newtonsoft.Json.Linq;
//using UniJSON;
using System.Linq;
using System;
using UnityEngine.UI;

namespace Manager
{
    public class JSONManager : MonoBehaviour
    {
        #region Singleton 

        private static JSONManager _instance;

        public static JSONManager Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = FindObjectOfType<JSONManager>();
                }
                return _instance;
            }
        }

        #endregion

        #region 변수
        //[SerializeField] private TestLoader gltfLoader;
        //[SerializeField] private TestGui testGUI;
        [SerializeField] private IssueLoader issueLoader;
        #endregion

        #region 초기화

        /// <summary>
        /// IssueLoader 내부 변수 초기화
        /// </summary>
        /// <param name="urlSet"></param>
        public void Initialize(Manager.UrlSet urlSet)
        {
            issueLoader.Initialize(urlSet);
        }

        #endregion

        #region 객체 생성

        public void LoadObjectList(string URI)
        {
            //Debug.Log("LoadObjectList");
            StartCoroutine(issueLoader.LoadJSON(JSONLoadType.BridgeList, ""));
        }

        /// <summary>
        /// GLTF 객체 생성
        /// </summary>
        /// <param name="URI"></param>
        public void LoadObjectToJSON(string URI)
        {
            //gltfLoader.LoadUrl(URI);
        }

        /// <summary>
        /// 손상/보수정보 할당
        /// </summary>
        /// <param name="bridgeCode"></param>
        public void LoadIssueToJSON(string bridgeCode)
        {
            StartCoroutine(issueLoader.LoadJSON(JSONLoadType.Address, bridgeCode));
            StartCoroutine(issueLoader.LoadJSON(JSONLoadType.Damaged, bridgeCode));
            StartCoroutine(issueLoader.LoadJSON(JSONLoadType.Recovered, bridgeCode));
        }

        public void LoadImageToJSON(string arguments, RawImage image)
        {
            StartCoroutine(issueLoader.LoadJSONCallback(JSONLoadType.Image, arguments, image));
        }

        

        public void LoadImageToJSON(string arguments, Indicator.Element.Imp_element callback)
        {
            StartCoroutine(issueLoader.LoadJSONCallback(JSONLoadType.Image, arguments, callback));
        }

        #region history
        public void LoadHistory(JSONLoadType loadType, string bridgeCode, string query, string restrict = "")
        {
            StartCoroutine(issueLoader.LoadHistoryJSON(loadType, bridgeCode, query, restrict));
        }

        public void LoadHistory(JSONLoadType loadType, string bridgeCode, string query, Issue.AIssue issue, Indicator.ImP_option option)
        {
            StartCoroutine(issueLoader.LoadHistoryJSON(loadType, bridgeCode, query, issue, option));
        }
        #endregion

        #endregion
    }
}
