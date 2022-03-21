using Management;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition._Issue
{
	public enum IssueCodes
	{
		Null = -1,          // 손상 없음
		Crack = 1,          // 균열
		Scour_Erosion = 4,      // 세굴, 침식
		Spalling = 7,       // 박리박락
		Efflorescence = 9,  // 백태
		breakage = 22,           // 파손
	}

	/// <summary>
	/// 작업 정보 (손상, 보수)
	/// </summary>
	[System.Serializable]
	public class Issue
	{
		#region field
		[SerializeField] string m_issueOrderCode;
		[SerializeField] string m_cdBridge;
		[SerializeField] string m_cdBridgeParts;
		[SerializeField] string m_dcMemberSurface;
		[SerializeField] string m__dcLocation;
		[SerializeField] string m__issueCode;
		[SerializeField] string m_ynRecover;
		[SerializeField] string m_issueStatus;
		[SerializeField] string m__positionVector;

		[SerializeField] string m_dateDmg;
		[SerializeField] string m_dateRcvStart;
		[SerializeField] string m_dateRcvEnd;

		[SerializeField] IssueCodes m_issueCode;
		[SerializeField] int m_dcLocation;
		[SerializeField] Vector3 m_positionVector;


		public string IssueOrderCode { get => m_issueOrderCode; set => m_issueOrderCode=value; }
		public string CdBridge { get => m_cdBridge; set => m_cdBridge=value; }
		public string CdBridgeParts { get => m_cdBridgeParts; set => m_cdBridgeParts=value; }
		public string DcMemberSurface { get => m_dcMemberSurface; set => m_dcMemberSurface=value; }
		public string YnRecover { get => m_ynRecover; set => m_ynRecover=value; }
		public string IssueStatus { get => m_issueStatus; set => m_issueStatus=value; }
		public string _IssueCode 
		{ 
			get => m__issueCode; 
			set
			{
				Debug.Log($"issue code : {value}");
				m__issueCode=value;
				IssueCodes code;
				if(Enum.TryParse<IssueCodes>(m__issueCode, out code))
				{
					m_issueCode = code;
				}
				else
				{
					Debug.LogError("m__issuecode assign error");
				}
			}
		}
		public string _DcLocation 
		{ 
			get => m__dcLocation; 
			set
			{
				m__dcLocation=value;
				if(!int.TryParse(m__dcLocation, out m_dcLocation))
				{
					Debug.LogError("m__dcLocation assign error");
				}
			}
		}
		public string _PositionVector 
		{ 
			get => m__positionVector; 
			set
			{
				m__positionVector=value;
				string[] vStr = m__positionVector.Split(',');

				Vector3 pos = new Vector3(
					float.Parse(vStr[0]),
					float.Parse(vStr[1]),
					float.Parse(vStr[2])
					);

				m_positionVector = pos;
			}
		}

		public string DateDmg { get => m_dateDmg; set => m_dateDmg=value; }
		public string DateRcvStart { get => m_dateRcvStart; set => m_dateRcvStart=value; }
		public string DateRcvEnd { get => m_dateRcvEnd; set => m_dateRcvEnd=value; }


		public int DcLocation { get => m_dcLocation; set => m_dcLocation=value; }
		public Vector3 PositionVector { get => m_positionVector; set => m_positionVector=value; }
		public IssueCodes IssueCode { get => m_issueCode; set => m_issueCode=value; }

		#endregion

		public string __PartName
		{
			get
			{
				string result = "";

				string partCode = CdBridgeParts;

				if(MainManager.Instance)
				{
					PlatformCode pCode = MainManager.Instance.Platform;

					if(Platforms.IsTunnelPlatform(pCode))
					{
						result = Platform.Tunnel.Tunnels.GetName(partCode);
					}
					else if(Platforms.IsBridgePlatform(pCode))
					{
						Debug.LogError("Bridge Code to name Set");
					}
				}

				return result;
			}
		}

		public string __IssueName
		{
			get
			{
				string result = "";

				IssueCodes iCode = IssueCode;

				switch(iCode)
				{
					case IssueCodes.Crack:
						result = "균열";
						break;

					case IssueCodes.Spalling:
						result = "박리박락";
						break;

					case IssueCodes.Efflorescence:
						result = "백태";
						break;

					case IssueCodes.breakage:
						result = "파손";
						break;
				}

				return result;
			}
		}

		public string __LocationName
		{
			get
			{
				string result = "";

				int index = DcLocation;

				index = index-1;

				int horizontalIndex = index / 3;
				int verticalIndex = index % 3;

				string hName = "";
				string vName = "";
				
				switch(horizontalIndex)
				{
					case 0: hName = "상부"; break;
					case 1: hName = "중심"; break;
					case 2: hName = "하부"; break;
				}

				switch(verticalIndex)
				{
					case 0: vName = "좌측"; break;
					case 1: vName = "중앙"; break;
					case 2: vName = "우측"; break;
				}

				result = $"{hName} {vName}";

				return result;
			}
		}
	}
}
