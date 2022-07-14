using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Definition
{
	public static class ExternalAPI
	{
        /// <summary>
        /// ���Ž� �� ��� : ���� On/Off ��û
        /// </summary>
        /// <param name="argument">
        /// Y : ���� �ѱ�
        /// N : ���� ����
        /// </param>
        [DllImport("__Internal")]
        public static extern void ViewMap(string argument);

        /// <summary>
        /// ���Ž� �� ��� : ���� ���� ���� ��û
        /// </summary>
        /// <param name="argument">Y : ���� �ѱ�</param>
        [DllImport("__Internal")]
        public static extern void InitializeMap(string argument);

        /// <summary>
        /// ���Ž� ������ ��� : ���� ���� ��½� �ʿ� �̹��� ����
        /// </summary>
        /// <param name="_fName1">Base64 ���·� ���޵� �̹��� 1�� (�ü��� ����)</param>
        /// <param name="_fName2">Base64 ���·� ���޵� �̹��� 2�� (�׷��� 1��)</param>
        /// <param name="_fName3">Base64 ���·� ���޵� �̹��� 3�� (�׷��� 2��)</param>
        [DllImport("__Internal")]
        public static extern void OnReadyToPrint(string _fName1, string _fName2, string _fName3);

        /// <summary>
        /// ���Ž� ������ ��� : �ü��� ������ ��,��,��,��,��,�� �Ϲ� �ܸ�, �ܰ�/ġ���� ���� �ܸ� ����
        /// </summary>
        /// <param name="_f1">��� �Ϲ� �ܸ�</param>
        /// <param name="_f2">��� �ܰ�/ġ�� ���� �ܸ�</param>
        /// <param name="_f3">�Ϻ� �Ϲ� �ܸ�</param>
        /// <param name="_f4">�Ϻ� �ܰ�/ġ�� ���� �ܸ�</param>
        /// <param name="_f5">���� �Ϲ� �ܸ�</param>
        /// <param name="_f6">���� �ܰ�/ġ�� ���� �ܸ�</param>
        /// <param name="_f7">���� �Ϲ� �ܸ�</param>
        /// <param name="_f8">���� �ܰ�/ġ�� ���� �ܸ�</param>
        /// <param name="_f9">���� �Ϲ� �ܸ�</param>
        /// <param name="_f10">���� �ܰ�/ġ�� ���� �ܸ�</param>
        /// <param name="_f11">�ĸ� �Ϲ� �ܸ�</param>
        /// <param name="_f12">�ĸ� �ܰ�/ġ�� ���� �ܸ�</param>
        /// <param name="_f13">�ü�����</param>
        /// <param name="_f14">���� ��� ���� �ջ��߰�, ������ ��� ���� �����߰�</param>
        [DllImport("__Internal")]
        public static extern void OnReadyToDrawingPrint(string _f1, string _f2, string _f3, string _f4, string _f5, string _f6, string _f7,
            string _f8, string _f9, string _f10, string _f11, string _f12, string _f13, bool _f14);
    }
}
