using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Issue
{
    public enum KeyOfRecords
    {
        cdTunnelDamaged = 0,    // 손상정보     //cdBridgeDamaged = 0,    // 손상정보
        fgDA001 = 1,            // 손상코드

        cdTunnelParts = 2,      // 부재정보
        dcDamageMemberSurface = 3,  // 6면 정보
        dcLocation = 4,         // 9면 정보

        dcPinLocation = 5,      // 위치 정보
        dcRemark = 6,           // 설명

        dcGrade = 7,            // 등급
        noDamageWidth = 8,      // 폭
        noDamageHeight = 9,     // 높이
        noDamageDepth = 10,     // 깊이

        fgroup = 11,            // 파일 그룹
        files = 12,
        fid = 13,               // 파일 ID
        ftype = 14,             // 파일 형태

        dtCheck = 15,           //손상 날짜
        dtEnd = 16,             //보수 종료 날짜
        nmUser = 17,
    }

    public enum KeyOfDamages
    {
        cdTunnel = 0,       // cdBridge = 0,
        fgDA001 = 1,
        nmUser = 2,
        cdTunnelParts = 3,      // cdBridgeParts = 3,
        dcDamageMemberSurface = 4,
        dcLocation = 5,
        dcPinLocation = 6,

        dtCheck = 7,
        cdTunnelDamaged = 8,    // cdBridgeDamaged = 8,

        fgroup = 9,
        fid = 10,
        ftype = 11,

        // 손상정보 
        dcGrade = 12,        // 손상등급
        noDamageWidth = 13,  // 손상 너비
        noDamageHeight = 14, // 손상 높이
        noDamageDepth = 15,  // 손상 깊이
        dcRemark = 16,       // 설명

    }

    public enum KeyOfRecovers
    {
        cdTunnel = 0,       // cdBridge = 0,
        fgDA001 = 1,
        nmUser = 2,
        cdTunnelParts = 3,  // cdBridgeParts = 3,
        dcDamageMemberSurface = 4,
        dcLocation = 5,
        dcPinLocation = 6,

        cdTunnelRecover = 7,    // cdBridgeRecover = 7,
        dtStart = 8,
        dtEnd = 9,

        fgroup = 10,
        fid = 11,
        ftype = 12,

        // 손상정보
        dcGrade = 13,       // 손상등급
        noDamageWidth = 14, // 손상 너비
        noDamageHeight = 15,// 손상 높이
        noDamageDepth = 16, // 손상 깊이
        dcRemark = 17,      // 설명
    }

    public enum IssueCode
    {
        Null = -1,
        Crack = 0,          // 균열
        Efflorescense = 1,  // 박리박락
        Spalling = 2,       // 백태
        Breakage = 3,       // 파손
        Scour_Erosion = 4,  // 세굴, 침식
    }

    public enum Issue6Surface
    {
        Null = -1,
        Top,
        Bottom,
        Front,
        Back,
        Left,
        Right,
    }

    [System.Serializable]
    public abstract class AIssue : MonoBehaviour, IIssue
    {
        #region 변수
        [SerializeField] protected string cdBridge;
        [SerializeField] protected string issueOrderCode;  // cdBridgeDamaged, cdBridgeRecover
        [SerializeField] protected IssueCode fgDA001;
        [SerializeField] protected string nmUser;
        [SerializeField] protected string cdBridgeParts;

        [SerializeField] protected Issue6Surface dcDamageMemberSurface;
        [SerializeField] protected int dcLocation;
        [SerializeField] protected Vector3 dcPinLocation;

        [SerializeField] protected string fgroup;

        [SerializeField] protected string dcGrade;
        [SerializeField] protected int issueWidth;
        [SerializeField] protected int issueHeight;
        [SerializeField] protected int issueDepth;
        [SerializeField] protected string description;
        [SerializeField] protected string issueKinds;
        [SerializeField] public string dtCheck;
        [SerializeField] protected string dtEnd;
        [SerializeField] protected List<Dictionary<KeyOfRecords, string>> imgIndexes;
        #endregion

        #region 속성
        public string BridgeName
        {
            get => cdBridge;
            set => cdBridge = value;
        } // 교량 이름

        public string IssueOrderCode
        {
            get => issueOrderCode;
            set => issueOrderCode = value;
        } // 손상/보수 인식코드

        public IssueCode IssueCodes
        {
            get => fgDA001;
            set => fgDA001 = value;
        } // 손상/보수 코드

        public string NmUser
        {
            get => nmUser;
            set => nmUser = value;
        }

        public string BridgePartName
        {
            get => cdBridgeParts;
            set => cdBridgeParts = value;
        } // 손상/보수 위치한 부재명

        public Issue6Surface Issue6Surfaces
        {
            get => dcDamageMemberSurface;
            set => dcDamageMemberSurface = value;
        } // 손상/보수 위치 6면

        public int Issue9Location
        {
            get => dcLocation;
            set => dcLocation = value;
        } // 손상/보수 9면 위치

        public Vector3 PinVector
        {
            get => dcPinLocation;
            set => dcPinLocation = value;
        } // 손상/보수 위치

        public string FGroup
        {
            get => fgroup;
            set => fgroup = value;
        } // 이미지 파일그룹 식별자

        public string IssueGrade
        {
            get => dcGrade;
            set => dcGrade = value;
        }

        public int IssueWidth
        {
            get => issueWidth;
            set => issueWidth = value;
        }

        public int IssueHeight
        {
            get => issueHeight;
            set => issueHeight = value;
        }

        public int IssueDepth
        {
            get => issueDepth;
            set => issueDepth = value;
        }

        public string Description
        {
            get => description;
            set => description = value;
        }

        public string IssueKinds
        {
            get => issueKinds;
            set => issueKinds = value;
        }

        public string DTCheck
        {
            get => dtCheck;
            set => dtCheck = value;
        }

        public string DTEnd
        {
            get => dtEnd;
            set => dtEnd = value;
        }
        // RecordIssue가 DamagedIssue와 RecoveredIssue의 ImgIndexes를 관리하는 방법이 달라서 임의로 AIssue에 선언
        public List<Dictionary<KeyOfRecords, string>> ImgIndexes
        {
            get => imgIndexes;
            set => imgIndexes = value;
        }
        #endregion

        private void Awake()
        {
            BridgeName = "";
            IssueOrderCode = "";
            IssueCodes = IssueCode.Null;
            NmUser = "";
            BridgePartName = "";
            Issue6Surfaces = Issue6Surface.Null;
            Issue9Location = -1;
            PinVector = Vector3.zero;
            FGroup = "";

            IssueGrade = "";
            IssueWidth = 0;
            IssueHeight = 0;
            IssueDepth = 0;
            Description = "";
            IssueKinds = "";
            DTCheck = "";
            DTEnd = "";
            ImgIndexes = null;


            init();
        }

        protected virtual void init()
        {

        }

        public string ConvertIssueCodeToString(IssueCode code)
        {
            switch (code)
            {
                case IssueCode.Crack: return "0001";
                case IssueCode.Efflorescense: return "0007";
                case IssueCode.Spalling: return "0009";
                case IssueCode.Scour_Erosion: return "0013";
                case IssueCode.Breakage: return "0022";
                default: return null;
            }
        }

        public string ConvertIssueCode(IssueCode code)
        {
            switch (code)
            {
                case IssueCode.Crack: return "균열";
                case IssueCode.Efflorescense: return "박리박락";
                case IssueCode.Spalling: return "백태";
                case IssueCode.Scour_Erosion: return "세굴_침식";
                case IssueCode.Breakage: return "파손";
                default: return null;
            }
        }

        public string ConvertPartName(string partName)
        {
            //MODBS_Library.CodeLv4 lv4 = MODBS_Library.CodeLv4.Null;

            //string[] args = partName.Split('_');
            //if (args.Length > 2)
            //{
            //    if (Enum.TryParse<MODBS_Library.CodeLv4>(args[1], out lv4))
            //    {
            //        if (!lv4.Equals(MODBS_Library.CodeLv4.Null))
            //        {
            //            string name = MODBS_Library.LevelCodeConverter.ConvertLv4String(lv4);
            //            Debug.Log(name);
            //            return name;
            //        }
            //    }
            //}

            return null;
        }

        #region 위치정보 표시 (BPMM_VAD)
        public string ConvertPosition()
        {
            string _6surfaceString = Issue6Surfaces.ToString();
            string _9Location = Convert9Location(Issue9Location);

            return string.Format($"{_6surfaceString} {_9Location}");
        }

        public string Convert9Location(int locationIndex)
        {
            string result = "";
            switch (locationIndex)
            {
                case 1: return "상단좌측";
                case 2: return "상단중앙";
                case 3: return "상단우측";

                case 4: return "중단좌측";
                case 5: return "중단중앙";
                case 6: return "중단우측";

                case 7: return "하단좌측";
                case 8: return "하단중앙";
                case 9: return "하단우측";
                default: return "중단중앙";
            }
        }

        public Sprite GetPositionSprite(int locationIndex)
        {
            string baseResourceLocation = "Icon/New/6Surface/";
            Sprite result = Resources.Load<Sprite>($"{baseResourceLocation}{locationIndex}");

            return result;
        }

        #endregion

        public void Set(AIssue _issue)
        {
            BridgeName = _issue.BridgeName;
            IssueOrderCode = _issue.IssueOrderCode;
            IssueCodes = _issue.IssueCodes;
            NmUser = _issue.NmUser;
            BridgePartName = _issue.BridgePartName;
            Issue6Surfaces = _issue.Issue6Surfaces;
            Issue9Location = _issue.Issue9Location;
            PinVector = _issue.PinVector;
            FGroup = _issue.FGroup;

            IssueGrade = _issue.IssueGrade;
            IssueWidth = _issue.IssueWidth;
            IssueHeight = _issue.IssueHeight;
            IssueDepth = _issue.IssueDepth;
            Description = _issue.Description;
            IssueKinds = _issue.IssueKinds;
            DTCheck = _issue.DTCheck;
            DTEnd = _issue.DTEnd;
            ImgIndexes = _issue.ImgIndexes;


        }

        public abstract void SetObject<T>(T _data);
    }
}
