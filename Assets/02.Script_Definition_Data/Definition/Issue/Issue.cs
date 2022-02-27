using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition._Issue
{
	/// <summary>
	/// 작업 정보 (손상, 보수)
	/// </summary>
	[System.Serializable]
	public class Issue
	{
		[SerializeField] string m_issueOrderCode;
		[SerializeField] string m_cdBridge;
		[SerializeField] string m_cdBridgeParts;
		[SerializeField] string m_dcMemberSurface;
		[SerializeField] string m__dcLocation;
		[SerializeField] string m_issueCode;
		[SerializeField] string m_ynRecover;
		[SerializeField] string m_issueStatus;
		[SerializeField] string m__positionVector;
		
		[SerializeField] int m_dcLocation;
		[SerializeField] Vector3 m_positionVector;

		public string IssueOrderCode { get => m_issueOrderCode; set => m_issueOrderCode=value; }
		public string CdBridge { get => m_cdBridge; set => m_cdBridge=value; }
		public string CdBridgeParts { get => m_cdBridgeParts; set => m_cdBridgeParts=value; }
		public string DcMemberSurface { get => m_dcMemberSurface; set => m_dcMemberSurface=value; }
		public string IssueCode { get => m_issueCode; set => m_issueCode=value; }
		public string YnRecover { get => m_ynRecover; set => m_ynRecover=value; }
		public string IssueStatus { get => m_issueStatus; set => m_issueStatus=value; }
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

		public int DcLocation { get => m_dcLocation; set => m_dcLocation=value; }
		public Vector3 PositionVector { get => m_positionVector; set => m_positionVector=value; }
	}
}
