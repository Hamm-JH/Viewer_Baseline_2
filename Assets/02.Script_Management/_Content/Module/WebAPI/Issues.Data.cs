using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Issues
{
    using Definition._Issue;

    [System.Serializable]
    public class _Data
    {
        [SerializeField] private string issueOrderCode;             // 손상 / 보강 코드
        [SerializeField] private string cdBridge;                   // 교량코드
        [SerializeField] private string cdBridgeParts;              // 선택 객체명
        [SerializeField] private string dcMemberSurface;            // 손상부재면 (6면 : (top, bottom, left, right, front, back) )
        [SerializeField] private int dcLocation;                    // 손상부위 (9면)

        [SerializeField] private Code issueCode;                    // 손상 코드
        [SerializeField] private string issueStatus;                // 손상/보강 상태
        [SerializeField] private string positionVectorString;       // 손상 정보의 위치정보 문자열화한 코드

        [SerializeField] private string ynRecover;                  // 보강 완료 확인
        [SerializeField] private string fgDA001;                    // 

        public string IssueOrderCode
        {
            get => issueOrderCode;
            set => issueOrderCode = value;
        }

        public string CdBridge
        {
            get => cdBridge;
            set => cdBridge = value;
        }

        public string CdBridgeParts
        {
            get => cdBridgeParts;
            set => cdBridgeParts = value;
        }

        public string DcMemberSurface
        {
            get => dcMemberSurface;
            set => dcMemberSurface = value;
        }

        public int DcLocation
        {
            get => dcLocation;
            set => dcLocation = value;
        }

        public Code IssueCode
        {
            get => issueCode;
            set => issueCode = value;
        }

        public string IssueStatus
        {
            get => issueStatus;
            set => issueStatus = value;
        }

        public string PositionVector
        {
            get => positionVectorString;
            set => positionVectorString = value;
        }

        public string YnRecover
        {
            get => ynRecover;
            set => ynRecover = value;
        }

        public string FgDA001
        {
            get => fgDA001;
            set => fgDA001 = value;
        }
    }
}

