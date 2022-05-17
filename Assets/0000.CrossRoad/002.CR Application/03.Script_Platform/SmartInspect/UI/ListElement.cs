using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmartInspect
{
    using Module.UI;
    using Management;
    using Management.Content;
    using TMPro;
    using static Platform.Bridge.Bridges;
    using AdminViewer.Tunnel;
    using Platform.Tunnel;
    using Platform.Bridge;

    /// <summary>
    /// SmartInspect의 UI 내부 리스트 요소 클래스
    /// </summary>
    [System.Serializable]
    public partial class ListElement : MonoBehaviour
    {
        /*
         * 리스트 요소는 초기화 지시를 받을 시에
         * 해당 요소가 가지고 있는 데이터를 기반으로 데이터 가공 수행
         */
        public enum Template
        {
            PartCount,
            IssueList,
            Image,
        }

        /// <summary>
        /// 내부 분류용 카테고리
        /// </summary>
        public enum Category
        {
            ALL,
            DMG,
            RCV,
            REIN,
        }

        [System.Serializable]
        public class CountData
        {
            public Michsky.UI.ModernUIPack.ProgressBar m_pBar;
            public TextMeshProUGUI m_pBar_Text;

            public int m_maxIssueCount;
            public BridgeCode m_bCode;
            public TunnelCode m_tCode;

            /// <summary>
            /// 이벤트 발생시 이벤트를 전달하는 목표 리스트 관리 인스턴스
            /// </summary>
            public ListElement m_tgElement;

            /// <summary>
            /// 이벤트 발생시 이 요소가 속한 리스트 관리 인스턴스 
            /// </summary>
            public ListElement m_toggleResourceElement;

            public List<RecordElement> m_elements;
        }

        public Template m_template;
        public Category m_catrgory;
        public UITemplate_SmartInspect m_rootUI;
        public Transform m_contentRoot;

        private TextMeshProUGUI titleText;
        private string titleName;

        [SerializeField] private CountData _countData;

        public CountData _CountData
		{
            get => _countData;
            set => _countData = value;
		}

        public void Init(TextMeshProUGUI _title, string _tName, 
            Michsky.UI.ModernUIPack.ProgressBar _pBar, TextMeshProUGUI _pBarText, ListElement _tgElement)
        {
            _countData = new CountData();
            //ContentManager.Instance._Model.DmgData

            titleText = _title;
            titleName = _tName;
            _countData.m_pBar = _pBar;
            _countData.m_pBar_Text = _pBarText;
            _countData.m_tgElement = _tgElement;
            _countData.m_toggleResourceElement = this;

            // 기본값 할당해두기
            _countData.m_bCode = BridgeCode.ALL;
            _countData.m_tCode = TunnelCode.ALL;

            Debug.Log($"hello list {gameObject.name}");

            ClearElement();

            // 템플릿
            // 1 파트 카운트

            switch(m_catrgory)
            {
                case Category.DMG:
                    DMG_Init();
                    break;

                case Category.RCV:
                    RCV_Init();
                    break;

                case Category.REIN:
                    REIN_Init();
                    break;
            }
        }

        public void ResetList(BridgeCode _bCode)
        {
            _countData.m_bCode = _bCode;

            ClearElement();

            switch (m_catrgory)
            {
                case Category.DMG:
                    DMG_Init();
                    break;

                case Category.RCV:
                    RCV_Init();
                    break;

                case Category.REIN:
                    REIN_Init();
                    break;
            }
        }

        public void ResetList(TunnelCode _tCode)
        {
            _countData.m_tCode = _tCode;

            ClearElement();

            switch (m_catrgory)
            {
                case Category.DMG:
                    DMG_Init();
                    break;

                case Category.RCV:
                    RCV_Init();
                    break;

                case Category.REIN:
                    REIN_Init();
                    break;
            }
        }

        public void Init(Definition._Issue.Issue _issue)
        {
            ClearElement();

            switch(m_template)
            {
                case Template.Image:
                    IMG_Init(_issue);
                    break;
            }
            

        }

        /// <summary>
        /// 기존 요소에 있는 객체들 지우기
        /// </summary>
        private void ClearElement()
        {
            _countData.m_elements = new List<RecordElement>();

            int index = m_contentRoot.childCount;

            for (int i = 0; i < index; i++)
            {
                Destroy(m_contentRoot.GetChild(i).gameObject);
            }
        }

        #region General

        /// <summary>
        /// 교량 :: 카테고리별로 점검정보 리스트를 할당한다.
        /// </summary>
        /// <param name="_category"></param>
        /// <param name="_bCode"></param>
        /// <returns></returns>
        private List<Definition._Issue.Issue> GetIssueList(Category _category, BridgeCode _bCode)
        {
            List<Definition._Issue.Issue> list = new List<Definition._Issue.Issue>();

            if (_category == Category.ALL)
            {
                List<GameObject> objs = SmartInspectManager.Instance._Model.IssueObjs;
                objs.ForEach(x =>
                {
                    Definition._Issue.Issue _issue;
                    if (x.TryGetComponent<Definition._Issue.Issue>(out _issue))
                    {
                        // 이게 목표 이슈인가
                        if (IsTargetIssue(_issue, _bCode))
                        {
                            list.Add(_issue);
                        }
                    }
                });

                if(_bCode == BridgeCode.ALL)
                {
                    _countData.m_maxIssueCount = list.Count;
                }
            }
            else if (_category == Category.DMG)
            {
                List<Definition._Issue.Issue> objLists = SmartInspectManager.Instance._Model.DmgData;
                //list = SmartInspectManager.Instance._Model.DmgData;

                objLists.ForEach(x =>
                {
                    if(IsTargetIssue(x, _bCode))
                    {
                        list.Add(x);
                    }
                });

                if (_bCode == BridgeCode.ALL)
                {
                    _countData.m_maxIssueCount = list.Count;
                }
            }
            else if (_category == Category.RCV)
            {
                List<Definition._Issue.Issue> objLists = SmartInspectManager.Instance._Model.RcvData;
                //list = SmartInspectManager.Instance._Model.RcvData;

                objLists.ForEach(x =>
                {
                    if (IsTargetIssue(x, _bCode))
                    {
                        list.Add(x);
                    }
                });

                if (_bCode == BridgeCode.ALL)
                {
                    _countData.m_maxIssueCount = list.Count;
                }
            }
            else if (_category == Category.REIN)
            {

            }


            return list;
        }

        /// <summary>
        /// 터널 :: 카테고리별로 점검정보 리스트를 할당한다.
        /// </summary>
        /// <param name="_category"></param>
        /// <param name="_tCode"></param>
        /// <returns></returns>
        private List<Definition._Issue.Issue> GetIssueList(Category _category, TunnelCode _tCode)
        {
            List<Definition._Issue.Issue> list = new List<Definition._Issue.Issue>();

            if (_category == Category.ALL)
            {
                List<GameObject> objs = SmartInspectManager.Instance._Model.IssueObjs;
                objs.ForEach(x =>
                {
                    Definition._Issue.Issue _issue;
                    if (x.TryGetComponent<Definition._Issue.Issue>(out _issue))
                    {
                        // 이게 목표 이슈인가
                        if (IsTargetIssue(_issue, _tCode))
                        {
                            list.Add(_issue);
                        }
                    }

                    if (_tCode == TunnelCode.ALL)
                    {
                        _countData.m_maxIssueCount = list.Count;
                    }
                });
            }
            else if (_category == Category.DMG)
            {
                List<Definition._Issue.Issue> objLists = SmartInspectManager.Instance._Model.DmgData;
                //list = SmartInspectManager.Instance._Model.DmgData;

                objLists.ForEach(x =>
                {
                    if (IsTargetIssue(x, _tCode))
                    {
                        list.Add(x);
                    }
                });

                if (_tCode == TunnelCode.ALL)
                {
                    _countData.m_maxIssueCount = list.Count;
                }
            }
            else if (_category == Category.RCV)
            {
                List<Definition._Issue.Issue> objLists = SmartInspectManager.Instance._Model.RcvData;
                //list = SmartInspectManager.Instance._Model.RcvData;

                objLists.ForEach(x =>
                {
                    if(IsTargetIssue(x, _tCode))
                    {
                        list.Add(x);
                    }
                });

                if (_tCode == TunnelCode.ALL)
                {
                    _countData.m_maxIssueCount = list.Count;
                }
            }
            else if (_category == Category.REIN)
            {

            }


            return list;
        }

        /// <summary>
        /// 교량 :: 단일 정보가 목표 정보를 가지고 있는가?
        /// </summary>
        /// <param name="_issue"></param>
        /// <param name="_bCode"></param>
        /// <returns></returns>
        private bool IsTargetIssue(Definition._Issue.Issue _issue, BridgeCode _bCode)
        {
            bool result = false;

            if(_issue.__PartBridgeCode == _bCode)
            {
                result = true;
            }
            else if(_bCode == BridgeCode.ALL)
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// 터널 :: 단일 정보가 목표 정보를 가지고 있는가?
        /// </summary>
        /// <param name="_issue"></param>
        /// <param name="_tCode"></param>
        /// <returns></returns>
        private bool IsTargetIssue(Definition._Issue.Issue _issue, TunnelCode _tCode)
        {
            bool result = false;

            if(_issue.__PartTunnelCode == _tCode)
            {
                result = true;
            }
            else if(_tCode == TunnelCode.ALL)
            {
                result = true;
            }

            return result;
        }

        #endregion
    }
}
