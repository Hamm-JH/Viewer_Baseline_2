using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Issues
{
    using Definition._Issue;

    [System.Serializable]
    public class _Data
    {
        [SerializeField] private string issueOrderCode;             // �ջ� / ���� �ڵ�
        [SerializeField] private string cdBridge;                   // �����ڵ�
        [SerializeField] private string cdBridgeParts;              // ���� ��ü��
        [SerializeField] private string dcMemberSurface;            // �ջ����� (6�� : (top, bottom, left, right, front, back) )
        [SerializeField] private int dcLocation;                    // �ջ���� (9��)

        [SerializeField] private Code issueCode;                    // �ջ� �ڵ�
        [SerializeField] private string issueStatus;                // �ջ�/���� ����
        [SerializeField] private string positionVectorString;       // �ջ� ������ ��ġ���� ���ڿ�ȭ�� �ڵ�

        [SerializeField] private string ynRecover;                  // ���� �Ϸ� Ȯ��
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

