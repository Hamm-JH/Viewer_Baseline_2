using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition
{

	public enum WebType
	{
		Issue_Dmg = 0x11,
		Issue_Rcv = 0x12,

	}
    public enum SendRequestCode
    {
        Null,
        SelectObject,   // 3D ��ü ����
        SelectIssue,    // �ջ�/���� ����
        SelectObject6Shape, // 6�� ��ü ����
        SelectSurfaceLocation,  // 9�� ����

        InitializeRegisterMode, // �ջ� ��� ���
        SetPinVector,           // ��ġ���� �Ҵ�

    }

    public enum ReceiveRequestCode
    {
        Null,

        ResetIssue,         // �ջ�/���� ����

        SelectObject,       // 3D ��ü ����
        SelectIssue,        // �ջ�/���� ����
        SelectObject6Shape, // 6�� ��ü ����
        SelectSurfaceLocation,  // 9�� ����
        InformationWidthChange, // ����â �� ���� ����

        //SetIssueStatus,         // �ջ� / ���� ���� ����
        ChangePinMode,          // PinMode ���� ����
        InitializeRegisterMode, // ��� ��� ����
        FinishRegisterMode,     // ��� �ܰ� ����

    }
}
