using Module.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SmartInspect
{
    using AdminViewer.Tunnel;
    using Definition;
    using Management;
    using Module.WebAPI;
    using Platform.Bridge;
    using Platform.Tunnel;
    using TMPro;
    using View;
    using static Platform.Bridge.Bridges;
    using static SmartInspect.ListElement;

    /// <summary>
    /// 레코드 패킷 인스턴스
    /// </summary>
    public class Packet_Record
    {
        public int m_requestIndex;
        public Definition._Issue.Issue m_issue;
        public UITemplate_SmartInspect m_rootUI;
        public GameObject m_element;

        #region Part Count

        /// <summary>
        /// 이벤트 발생을 전달하는 리스트 인스턴스
        /// </summary>
        public ListElement m_listElement;

        /// <summary>
        /// record element가 속한 ui 표시를 변경할 리스트 인스턴스
        /// </summary>
        public ListElement m_resourceElement;

        public BridgeCode m_bPartCode;
        public TunnelCode m_tPartCode;

        /// <summary>
        /// part count 카운트
        /// </summary>
        public int m_listedNumber;

        /// <summary>
        /// part name
        /// </summary>
        public string m_pc_name;

        #endregion

        #region DMG - List

        /// <summary>
        /// 요소의 순서 번호
        /// </summary>
        public int m_elementNumber;

        #endregion

        #region RCV - List



        #endregion

        #region IMG - List

        public string m_fgroup;
        public string m_fid;
        public string m_ftype;

        #endregion

        public int packetDebugger;

        /// <summary>
        /// DMG, RCV, REIN - 교량 part count
        /// </summary>
        /// <param name="_rIndex"></param>
        /// <param name="_lNumber"></param>
        /// <param name="_partName"></param>
        /// <param name="_rootUI"></param>
        public Packet_Record(int _rIndex, int _lNumber, string _partName,
            BridgeCode _bPartCode, ListElement _element, ListElement _resourceElement, UITemplate_SmartInspect _rootUI)
        {
            m_requestIndex = _rIndex;
            m_listedNumber = _lNumber;
            m_pc_name = _partName;

            m_bPartCode = _bPartCode;
            m_listElement = _element;
            m_resourceElement = _resourceElement;

            m_rootUI = _rootUI;

            packetDebugger = 1;
        }

        /// <summary>
        /// DMG, RCV, REIN - 터널 part count
        /// </summary>
        /// <param name="_rIndex"></param>
        /// <param name="_lNumber"></param>
        /// <param name="_partName"></param>
        /// <param name="_rootUI"></param>
        public Packet_Record(int _rIndex, int _lNumber, string _partName,
            TunnelCode _tPartCode, ListElement _element, ListElement _resourceElement, UITemplate_SmartInspect _rootUI)
        {
            m_requestIndex = _rIndex;
            m_listedNumber = _lNumber;
            m_pc_name = _partName;

            m_tPartCode = _tPartCode;
            m_listElement = _element;
            m_resourceElement = _resourceElement;

            m_rootUI = _rootUI;

            packetDebugger = 1;
        }

        /// <summary>
        /// ALL - IMG
        /// </summary>
        /// <param name="_rIndex"></param>
        /// <param name="_issue"></param>
        /// <param name="_rootUI"></param>
        public Packet_Record(int _rIndex, Definition._Issue.Issue _issue, UITemplate_SmartInspect _rootUI)
        {
            m_requestIndex = _rIndex;
            m_issue = _issue;
            m_rootUI = _rootUI;

            packetDebugger = 2;
        }

        public Packet_Record(int _rIndex, string _fgroup, string _fid, string _ftype, Definition._Issue.Issue _issue,
            UITemplate_SmartInspect _rootUI)
        {
            m_requestIndex = _rIndex;
            m_fgroup = _fgroup;
            m_fid = _fid;
            m_ftype = _ftype;
            m_issue = _issue;
            m_rootUI = _rootUI;

            packetDebugger = 3;
        }

        /// <summary>
        /// DMG, RCV, REIN - part list
        /// </summary>
        /// <param name="_rIndex"></param>
        /// <param name="_number"></param>
        /// <param name="_issue"></param>
        /// <param name="_rootUI"></param>
        public Packet_Record(int _rIndex, int _number, Definition._Issue.Issue _issue, 
            ListElement _element, ListElement _resourceElement, UITemplate_SmartInspect _rootUI)
        {
            m_requestIndex = _rIndex;
            m_elementNumber = _number;
            m_issue = _issue;
            m_rootUI = _rootUI;

            m_listElement = _element;
            m_resourceElement = _resourceElement;

            packetDebugger = 4;
        }
    }

    /// <summary>
    /// 리스트의 개별 요소
    /// </summary>
    [System.Serializable]
    public partial class RecordElement : MonoBehaviour
    {
        /// <summary>
        /// 이 요소가 어떤 요소인지 알려주는 변수
        /// 0 : part count
        /// 1 : dmg work
        /// 2 : rcv work
        /// 3 : img work
        /// </summary>
        [SerializeField] int m_eIndex = -1;
        public BridgeCode m_codeBridge;
        //public 

        public Definition._Issue.Issue m_issue;

        public Data m_data;

        public void Init(Packet_Record _packet)
        {
            m_eIndex = _packet.m_requestIndex;
            m_issue = _packet.m_issue;

            _packet.m_element = gameObject;

            m_data.Init(_packet);
        }

        // 1 요소 생성시 어떤 요소인지 파악해 아닌 요소를 지우는 코드 
        // m_data.Select_Element
    }

    [System.Serializable]
    public class Data
    {
        // PartCount
        [Header("부재별 카운트 요소")]
        public PartCount m_partCount;

        // DMG WorkList
        [Header("손상 - 작업정보")]
        public DMG_WorkElement m_dmgWork;

        // RCV WorkList
        [Header("보수보강 - 작업정보")]
        public RCV_WorkElement m_rcvWork;

        [Header("점검정보 - 이미지 리스트")]
        public IMG_WorkElement m_imgWork;

        public void Init(Packet_Record _packet)
        {
            Select_Element(_packet.m_requestIndex);

            m_partCount.Init(_packet);
            m_dmgWork.Init(_packet);
            m_rcvWork.Init(_packet);
            m_imgWork.Init(_packet);
        }

        public void Select_Element(int _index)
        {
            bool isPCount = false;
            bool isDWork = false;
            bool isRWork = false;
            bool isImgWork = false;
            
            switch(_index)
            {
                case 0: isPCount = true;    break;
                case 1: isDWork = true;    break;
                case 2: isRWork = true;    break;
                case 3: isImgWork = true;   break;
            }

            if(isPCount)
            {
                m_partCount.root.SetActive(true);
            }
            else
            {
                GameObject.Destroy(m_partCount.root);
                m_partCount.root = null;
            }

            if(isDWork)
            {
                m_dmgWork.root.SetActive(true);
            }
            else
            {
                GameObject.Destroy(m_dmgWork.root);
                m_dmgWork.root = null;
            }

            if(isRWork)
            {
                m_rcvWork.root.SetActive(true);
            }
            else
            {
                GameObject.Destroy(m_rcvWork.root);
                m_rcvWork.root = null;
            }

            if(isImgWork)
            {
                m_imgWork.root.SetActive(true);
            }
            else
            {
                GameObject.Destroy(m_imgWork.root);
                m_imgWork.root = null;
            }
        }
    }

    /// <summary>
    /// 부재분류별 손상개수
    /// </summary>
    [System.Serializable]
    public class PartCount
    {
        public GameObject root;

        public Image background;
        public TextMeshProUGUI tx_Count;
        public TextMeshProUGUI tx_Title;

        public List<UI_Selectable> btns_detail;

        public void Init(Packet_Record _packet)
        {
            if (root == null) return;

            //tx_Title.text = "";   // 타이틀 텍스트 (지간, 경간 같은거)
            tx_Count.text = _packet.m_listedNumber.ToString();
            tx_Title.text = _packet.m_pc_name;


            btns_detail.ForEach(x => Set_UIInstance(x, _packet));
        }

        private void Set_UIInstance(UI_Selectable _resource, Packet_Record _packet)
        {
            _resource.RootUI = _packet.m_rootUI;
            _resource.EventType = UIEventType.Ins_Panel_OnSelectCount;
            _resource.Data.m_issueListElement = _packet.m_listElement;
            _resource.Data.m_resourceListElement = _packet.m_resourceElement;

            PlatformCode pCode = MainManager.Instance.Platform;
            if (Platforms.IsBridgePlatform(pCode))
            {
                _resource.Data.m_bridgeIssueCode = _packet.m_bPartCode;
            }
            else if (Platforms.IsTunnelPlatform(pCode))
            {
                _resource.Data.m_tunnelCode = _packet.m_tPartCode;
            }
            else
            {
                throw new Definition.Exceptions.PlatformNotDefinedException(pCode);
            }
        }
    }

    [System.Serializable]
    public class DMG_WorkElement
    {
        public GameObject root;
        public TextMeshProUGUI tx_number;
        public TextMeshProUGUI tx_issueName;
        public TextMeshProUGUI tx_partName;
        public TextMeshProUGUI tx_locName;
        public TextMeshProUGUI tx_date;

        public List<UI_Selectable> btns_ui; 

        public void Init(Packet_Record _packet)
        {
            if (root == null) return;

            tx_number.text = _packet.m_elementNumber.ToString();
            tx_issueName.text = _packet.m_issue.__IssueName;
            tx_partName.text = _packet.m_issue.__PartName;
            tx_locName.text = _packet.m_issue.__LocationName;
            tx_date.text = _packet.m_issue.DateDmg;

            btns_ui.ForEach(x =>
            {
                Set_UIInstance(x, _packet);
            });
        }

        private void Set_UIInstance(UI_Selectable _resource, Packet_Record _packet)
        {
            _resource.Data.m_issueListElement = _packet.m_listElement;
            _resource.Data.m_resourceListElement = _packet.m_resourceElement;
            _resource.RootUI = _packet.m_rootUI;
            _resource.ChildPanel = _packet.m_element;
        }
    }

    [System.Serializable]
    public class RCV_WorkElement
    {
        public GameObject root;
        public TextMeshProUGUI tx_number;
        public TextMeshProUGUI tx_partName;
        public TextMeshProUGUI tx_repairName;
        public TextMeshProUGUI tx_date;

        public List<UI_Selectable> btns_ui;

        public void Init(Packet_Record _packet)
        {
            if (root == null) return;

            tx_number.text = _packet.m_elementNumber.ToString();
            tx_partName.text = _packet.m_issue.__PartName;
            tx_repairName.text = $"{_packet.m_issue.__IssueName}보수";
            //tx_repairName.text = _packet.m_issue.RcvDescription;

            tx_date.text = _packet.m_issue.DateRcvEnd;

            btns_ui.ForEach(x =>
            {
                Set_UIInstance(x, _packet);
            });
        }

        private void Set_UIInstance(UI_Selectable _resource, Packet_Record _packet)
        {
            _resource.Data.m_issueListElement = _packet.m_listElement;
            _resource.Data.m_resourceListElement = _packet.m_resourceElement;
            _resource.RootUI = _packet.m_rootUI;
            _resource.ChildPanel = _packet.m_element;
        }
    }

    [System.Serializable]
    public class IMG_WorkElement
    {
        public GameObject root;
        public RawImage m_rImage;
        public TextMeshProUGUI m_workerName;
        public TextMeshProUGUI m_description;

        public List<UI_Selectable> btns_ui;

        public void Init(Packet_Record _packet)
        {
            if (root == null) return;

            //m_rImage; // root에서 이미지 할당 요청
            string fgroup = _packet.m_fgroup;
            string fid = _packet.m_fid;
            string ftype = _packet.m_ftype;
            string imgArgument = string.Format("fid={0}&ftype={1}&fgroup={2}",  fid, ftype, fgroup);
            Management.Content.SmartInspectManager.Instance.Module<Module_WebAPI>(Definition.ModuleID.WebAPI)
                .RequestSinglePicture(imgArgument, m_rImage, SetSinglePicture);

            m_workerName.text = _packet.m_issue.NmUser;
            m_description.text = _packet.m_issue.DmgDescription;

            btns_ui.ForEach(x =>
            {
                Set_UIInstance(x, _packet);
            });

        }

        private void SetSinglePicture(RawImage _rImage, Texture2D _texture2D)
        {
            _rImage.texture = _texture2D;
        }

        private void Set_UIInstance(UI_Selectable _resource, Packet_Record _packet)
        {
            _resource.RootUI = _packet.m_rootUI;
        }
    }
}
