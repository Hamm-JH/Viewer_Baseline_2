using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platform.Tunnel
{
	/// <summary>
	/// �ͳ� �ڵ�
	/// </summary>
	public enum TunnelCode
	{
		ALL = -2,
		Null = -1,
		// ��Ÿ

		/// <summary>
		/// �����ȭ
		/// </summary>
		ETC_EmergencyCall = 7,

		/// <summary>
		/// ��ȭ��
		/// </summary>
		ETC_fireplug = 9,

		/// <summary>
		/// ���
		/// </summary>
		ETC_EmergencyExit = 16,
		ETC_IndicatorLight1,
		ETC_IndicatorLight2,

		/// <summary>
		/// �����
		/// </summary>
		MAIN_Drain = 5,

		/// <summary>
		/// ����
		/// </summary>
		MAIN_Cover,

		/// <summary>
		/// ����
		/// </summary>
		MAIN_Paving = 15,

		/// <summary>
		/// ����
		/// </summary>
		MAIN_SideWall = 20,

		/// <summary>
		/// õ��
		/// </summary>
		MAIN_Ceiling = 13,

		/// <summary>
		/// �߾Ӻи���
		/// </summary>
		MAIN_CorrugatedSteel,

		/// <summary>
		/// ����
		/// </summary>
		LIGHT = 11,

		/// <summary>
		/// ��
		/// </summary>
		JET = 4,

		/// <summary>
		/// �߾Ӻи���
		/// </summary>
		WALL_CentralReservation = 12,

		/// <summary>
		/// �˺�
		/// </summary>
		WALL_Gate = 10,

		/// <summary>
		/// ���
		/// </summary>
		WALL_Slope = 8

	}

}
//public class Tunnel_Definition : MonoBehaviour
//{
//    // Start is called before the first frame update
//    void Start()
//    {
        
//    }

//    // Update is called once per frame
//    void Update()
//    {
        
//    }
//}
