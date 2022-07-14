using Items;
using Management;
using Newtonsoft.Json.Linq;
using Platform.Tunnel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition._Issue
{
	/// <summary>
	/// 손상 코드분류
	/// </summary>
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
		public Issue()
		{
			m_imgs = new List<ImgIndex>();
		}

		[System.Serializable]
		public class ImgIndex
		{
			public string fid;
			public string ftype;
		}
		#region field
		[SerializeField] bool m_isDmg;
		[SerializeField] string m_date;

		[SerializeField] string m_issueOrderCode;
		[SerializeField] string m_cdBridge;
		[SerializeField] string m_cdBridgeParts;
		[SerializeField] string m_nmUser;
		[SerializeField] string m_dcMemberSurface;
		[SerializeField] string m__dcLocation;
		[SerializeField] string m__issueCode;
		[SerializeField] string m_ynRecover;
		[SerializeField] string m_issueStatus;
		[SerializeField] string m__positionVector;
		[SerializeField] string m__rotationVector;

		[SerializeField] string m_width;
		[SerializeField] string m_height;
		[SerializeField] string m_depth;
		[SerializeField] string m_dcRemark;
		[SerializeField] string m_dcRecover;

		[SerializeField] string m_dateDmg;
		[SerializeField] string m_dateRcvStart;
		[SerializeField] string m_dateRcvEnd;

		[SerializeField] string m_fgroup;
		[SerializeField] List<ImgIndex> m_imgs;

		[SerializeField] IssueCodes m_issueCode;
		[SerializeField] int m_dcLocation;
		[SerializeField] Vector3 m_positionVector;
		[SerializeField] Vector3 m_rotationVector;

		[SerializeField] Item_waypoint m_waypoint;

		/// <summary>
		/// 이 issue가 손상이면 true 보수면 false
		/// </summary>
		public bool IsDmg { get => m_isDmg; set => m_isDmg = value; }

		/// <summary>
		/// 손상정보의 발생 일시
		/// </summary>
		public string Date { get => m_date; set => m_date=value; }

		/// <summary>
		/// 손상 정보 코드
		/// </summary>
		public string IssueOrderCode { get => m_issueOrderCode; set => m_issueOrderCode=value; }

		/// <summary>
		/// 시설물 코드
		/// </summary>
		public string CdBridge { get => m_cdBridge; set => m_cdBridge=value; }

		/// <summary>
		/// 손상 발생 부재코드
		/// </summary>
		public string CdBridgeParts { get => m_cdBridgeParts; set => m_cdBridgeParts=value; }

		/// <summary>
		/// 등록자 이름
		/// </summary>
		public string NmUser { get => m_nmUser; set => m_nmUser=value; }

		/// <summary>
		/// 손상 6면 : 부재에서 손상이 위치한 6면중 하나 (상,하,좌,우,전,후)
		/// </summary>
		public string DcMemberSurface { get => m_dcMemberSurface; set => m_dcMemberSurface=value; }

		/// <summary>
		/// 보수 유무 Y : 보수됨, N : 보수 안됨
		/// </summary>
		public string YnRecover { get => m_ynRecover; set => m_ynRecover=value; }

		/// <summary>
		/// 손상정보 상태
		/// </summary>
		public string IssueStatus { get => m_issueStatus; set => m_issueStatus=value; }

		/// <summary>
		/// 문자열 : 손상코드 -> 문자열 코드를 받으면 실제 손상코드로 보간한다.
		/// </summary>
		public string _IssueCode 
		{ 
			get => m__issueCode; 
			set
			{
				//Debug.Log($"issue code : {value}");
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

		/// <summary>
		/// 손상 6면의 특정 면에서 손상이 위치한 특정 위치 (1 ~ 9 // 왼쪽 위에서 오른쪽 아래로 좌측 상단, 중앙 상단, 우측 상단, 좌측 중앙, 중앙 중단, 우측 우단, 좌측 하단, 중앙 하단, 우측 하단 순)
		/// </summary>
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

		/// <summary>
		/// 문자열 : 손상정보의 위치 -> 문자열 코드를 받으면 실제 벡터정보로 보간한다.
		/// </summary>
		public string _PositionVector 
		{ 
			get => m__positionVector; 
			set
			{
				m__positionVector=value;
				string[] vStr = m__positionVector.Split(',');

				//Debug.Log($"pVector : {m__positionVector}");
				Vector3 pos = new Vector3(
					float.Parse(vStr[0]),
					float.Parse(vStr[1]),
					float.Parse(vStr[2])
					);

				m_positionVector = pos;
			}
		}

		/// <summary>
		/// 문자열 : 손상정보의 각도 -> 문자열 코드를 받으면 실제 오일러 각도정보로 보간한다.
		/// </summary>
		public string _RotationVector
        {
			get => m__rotationVector;
			set
            {
				m__rotationVector = value;
				string[] vStr = m__rotationVector.Split(',');
				if(vStr.Length != 3)
                {
					m__rotationVector = "45,45,45";
					vStr = m__rotationVector.Split(',');
                }
				

				Vector3 rot = new Vector3(
					float.Parse(vStr[0]),
					float.Parse(vStr[1]),
					float.Parse(vStr[2])
					);

				m_rotationVector = rot;

            }
        }

		/// <summary>
		/// 손상 정보의 가로폭
		/// </summary>
		public string Width { get => m_width; set => m_width=value; }

		/// <summary>
		/// 손상 정보의 세로폭
		/// </summary>
		public string Height { get => m_height; set => m_height=value; }

		/// <summary>
		/// 손상 정보의 깊이
		/// </summary>
		public string Depth { get => m_depth; set => m_depth=value; }

		/// <summary>
		/// 손상 상태의 손상 세부설명
		/// </summary>
		public string DmgDescription { get => m_dcRemark; set => m_dcRemark=value; }

		/// <summary>
		/// 보수 상태의 손상 세부설명
		/// </summary>
		public string RcvDescription { get => m_dcRecover; set => m_dcRecover=value; }

		/// <summary>
		/// 손상 발생일
		/// </summary>
		public string DateDmg { get => m_dateDmg; set => m_dateDmg=value; }

		/// <summary>
		/// 보수정보 시작일
		/// </summary>
		public string DateRcvStart { get => m_dateRcvStart; set => m_dateRcvStart=value; }

		/// <summary>
		/// 보수정보 완료일
		/// </summary>
		public string DateRcvEnd { get => m_dateRcvEnd; set => m_dateRcvEnd=value; }

		/// <summary>
		/// 파일 그룹
		/// </summary>
		public string Fgroup { get => m_fgroup; set => m_fgroup=value; }

		/// <summary>
		/// 이미지 리스트 클래스 정본
		/// </summary>
		public List<ImgIndex> Imgs { get => m_imgs; set => m_imgs=value; }

		/// <summary>
		/// 손상 6면의 특정 면에서 손상이 위치한 특정 위치 (1 ~ 9 // 왼쪽 위에서 오른쪽 아래로 좌측 상단, 중앙 상단, 우측 상단, 좌측 중앙, 중앙 중단, 우측 우단, 좌측 하단, 중앙 하단, 우측 하단 순)
		/// </summary>
		public int DcLocation { get => m_dcLocation; set => m_dcLocation=value; }

		/// <summary>
		/// 손상 정보의 위치 벡터
		/// </summary>
		public Vector3 PositionVector { get => m_positionVector; set => m_positionVector = value; }

		/// <summary>
		/// 손상 정보의 오일러 각도
		/// </summary>
		public Vector3 RotationVector { get => m_rotationVector; set => m_rotationVector = value; }

		/// <summary>
		/// 손상 정보의 손상 코드분륜
		/// </summary>
		public IssueCodes IssueCode { get => m_issueCode; set => m_issueCode=value; }

		/// <summary>
		/// waypoint 셋업을 위해 Issue_Selectable 생성시에 초기 할당하는 item변수
		/// </summary>
		public Item_waypoint Waypoint { get => m_waypoint; set => m_waypoint = value; }
		#endregion

		/// <summary>
		/// 손상부재의 이름
		/// </summary>
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
						result = Platform.Bridge.Bridges.GetName(partCode);
					}
				}

				return result;
			}
		}

		/// <summary>
		/// 손상정보의 이름
		/// </summary>
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

		/// <summary>
		/// 손상정보의 위치
		/// </summary>
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

		public string __TunnelPartCode
		{
			get
			{
				return "";
			}
		}

		/// <summary>
		/// 교량 :: 부재명에서 코드찾기
		/// </summary>
		public Platform.Bridge.BridgeCode __PartBridgeCode
        {
			get
            {
				//string pName = __PartName;
				string pName = CdBridgeParts;

				return Platform.Bridge.Bridges.GetPartCode(pName);
            }
        }

		/// <summary>
		/// 터널 :: 부재명에서 코드찾기
		/// </summary>
		public TunnelCode __PartTunnelCode
        {
			get
            {
				//string pName = __PartName;
				string pName = CdBridgeParts;

				//Debug.Log(pName);
				TunnelCode code = Tunnels.GetPartCode(pName);
				return code;
            }
        }

        
		/// <summary>
		/// 발생된 정보를 손상정보로 변환한다.
		/// </summary>
		/// <param name="_date">손상정보 발생일</param>
		/// <param name="_token">변환 대상 문자열 토큰정보</param>
        public void SetDmg(string _date, JToken _token)
		{
			Date = _date;

			SetDmg(_token);
		}

		/// <summary>
		/// 발생된 정보를 손상정보로 변환한다.
		/// </summary>
		/// <param name="_token">변환 대상 문자열 토큰정보</param>
		public void SetDmg(JToken _token)
		{
			string kIssueOrderCode = "";
			string kCdBridge			= "";
			string kCdBridgeParts		= "";
			string kNmUser				= "";
			string kDcMemberSurface		= "";
			string k_DcLocation			= "";
			string k_IssueCode			= "";

			string kIssueStatus			= "";
			string k_PositionVector		= "";
			string k_RotationVector		= "";
			string kDateDmg				= "";
			string kWidth				= "";
			string kHeight				= "";
			string kDepth				= "";
			string kDmgDescription		= "";
			string kFgroup				= "";
			string kFid					= "";
			string kFtype				= "";

			PlatformCode pCode = MainManager.Instance.Platform;
			if(Platforms.IsTunnelPlatform(pCode))
			{
				kIssueOrderCode     = "cdTunnelDamaged";
				kCdBridge			= "cdTunnel";
				kCdBridgeParts		= "cdTunnelParts";
				kNmUser				= "nmUser";
				kDcMemberSurface	= "dcDamageMemberSurface";
				k_DcLocation		= "dcLocation";
				k_IssueCode			= "fgDA001";
               
				kIssueStatus = "dcGrade";
				k_PositionVector	= "dcPinLocation";
				k_RotationVector	= "dcRotation";
				kDateDmg			= "dtCheck";
				kWidth				= "noDamageWidth";
				kHeight				= "noDamageHeight";
				kDepth				= "noDamageDepth";
				kFgroup				= "fgroup";
				kFid				= "fid";
				kFtype				= "ftype";
				
				kDmgDescription		= "dcRemark";
			}
			else if(Platforms.IsBridgePlatform(pCode))
			{
				kIssueOrderCode     = "cdBridgeDamaged";
				kCdBridge           = "cdBridge";
				kCdBridgeParts      = "cdBridgeParts";
				kNmUser             = "nmUser";
				kDcMemberSurface    = "dcDamageMemberSurface";
				k_DcLocation        = "dcLocation";
				k_IssueCode         = "fgDA001";
               
				kIssueStatus = "dcGrade";
				k_PositionVector    = "dcPinLocation";
				k_RotationVector	= "dcRotation";
				kDateDmg            = "dtCheck";
				kWidth              = "noDamageWidth";
				kHeight             = "noDamageHeight";
				kDepth              = "noDamageDepth";
				kFgroup             = "fgroup";
				kFid                = "fid";
				kFtype              = "ftype";
				
				kDmgDescription     = "dcRemark";
			}

			IsDmg = true;
			IssueOrderCode       =  _token.SelectToken(kIssueOrderCode).ToString();
			CdBridge             =  _token.SelectToken(kCdBridge).ToString();
			CdBridgeParts        =  _token.SelectToken(kCdBridgeParts).ToString();
			NmUser               =   _token.SelectToken(kNmUser).ToString();
			DcMemberSurface      =   _token.SelectToken(kDcMemberSurface).ToString();
			_DcLocation          =   _token.SelectToken(k_DcLocation).ToString();
			_IssueCode           =   _token.SelectToken(k_IssueCode).ToString();
			//DcMemberSurface	= parseString(ParseCode.Surface, _token.SelectToken(JSON.IssueKey.dcDamageMemberSurface.ToString()).ToString());
			//DcLocation			=      int.Parse(_token.SelectToken(JSON.IssueKey.dcLocation.ToString()).ToString());
			//IssueCode			=       parseIssueCode(_token.SelectToken(JSON.IssueKey.fgDA001.ToString()).ToString());
			YnRecover            = "";
			IssueStatus          =   _token.SelectToken(kIssueStatus).ToString();
			_PositionVector      =  _token.SelectToken(k_PositionVector).ToString();
			_RotationVector		 = _token.SelectToken(k_RotationVector).ToString();
			DateDmg          = _token.SelectToken(kDateDmg).ToString();
			Width                =   _token.SelectToken(kWidth).ToString();
			Height               =   _token.SelectToken(kHeight).ToString();
			Depth                =   _token.SelectToken(kDepth).ToString();
			Fgroup              =   _token.SelectToken(kFgroup).ToString();
			DmgDescription          =   _token.SelectToken(kDmgDescription).ToString();

			if(_token.SelectToken("files") != null && _token.SelectToken("files").ToString() != "")
			{
				JArray jImgArr = JArray.Parse(_token.SelectToken("files").ToString());
				foreach(var arg in jImgArr)
				{
					ImgIndex _index = new ImgIndex();
					_index.fid = arg.SelectToken(kFid).ToString();
					_index.ftype = arg.SelectToken(kFtype).ToString();
					Imgs.Add(_index);
				}
			}
			try
            {
            }
			catch (Exception e)
            {
				Debug.Log($"error {e.ToString()}");
            }
			//if(JArray.Parse(_token.SelectToken("files").ToString()) != null)
			//{
			//}
		}

		/// <summary>
		/// 발생된 정보를 보수정보로 변환한다.
		/// </summary>
		/// <param name="_date">보수정보 발생일</param>
		/// <param name="_token">변환 대상 문자열 토큰정보</param>
		public void SetRcv(string _date, JToken _token)
		{
			Date = _date;

			SetRcv(_token);
		}

		/// <summary>
		/// 발생된 정보를 보수정보로 변환한다.
		/// </summary>
		/// <param name="_token">변환 대상 문자열 토큰정보</param>
		public void SetRcv(JToken _token)
		{
			string kIssueOrderCode = "";
			string kCdBridge		= "";
			string kCdBridgeParts	= "";
			string kNmUser			= "";
			string kDcMemberSurface	= "";
			string k_DcLocation		= "";
			string k_IssueCode		= "";
			
			string kYnRecover		= "";
			string k_PositionVector	= "";

			string kDateRcvStart	= "";
			string kDateRcvEnd		= "";
			string kWidth			= "";
			string kHeight			= "";
			string kDepth			= "";
			string kFgroup = "";
			string kFid = "";
			string kFtype = "";

			string kDmgDescription	= "";
			string kRcvDescription	= "";

			PlatformCode pCode = MainManager.Instance.Platform;
			if (Platforms.IsTunnelPlatform(pCode))
			{
				kIssueOrderCode		= "cdTunnelRecover";
				kCdBridge			= "cdTunnel";
				kCdBridgeParts		= "cdTunnelParts";
				kNmUser				= "nmUser";
				kDcMemberSurface	= "dcDamageMemberSurface";
				k_DcLocation		= "dcLocation";
				k_IssueCode			= "fgDA001";
									
				kYnRecover			= "ynRecover";
				k_PositionVector	= "dcPinLocation";
               
				kDateRcvStart = "dtStart";
				kDateRcvEnd			= "dtEnd";
				kWidth				= "noDamageWidth";
				kHeight				= "noDamageHeight";
				kDepth				= "noDamageDepth";
				kFgroup             = "fgroup";
				kFid                = "fid";
				kFtype              = "ftype";

				kDmgDescription		= "dcRemark";
				kRcvDescription     = "dcRecover";
			}
			else if (Platforms.IsBridgePlatform(pCode))
			{
				kIssueOrderCode     = "cdBridgeRecover";
				kCdBridge           = "cdBridge";
				kCdBridgeParts      = "cdBridgeParts";
				kNmUser             = "nmUser";
				kDcMemberSurface    = "dcDamageMemberSurface";
				k_DcLocation        = "dcLocation";
				k_IssueCode         = "fgDA001";

				kYnRecover          = "ynRecover";
				k_PositionVector    = "dcPinLocation";
                
				kDateRcvStart = "dtStart";
				kDateRcvEnd         = "dtEnd";
				kWidth              = "noDamageWidth";
				kHeight             = "noDamageHeight";
				kDepth              = "noDamageDepth";
				kFgroup             = "fgroup";
				kFid                = "fid";
				kFtype              = "ftype";

				kDmgDescription     = "dcRemark";
				kRcvDescription     = "dcRecover";
			}

			IsDmg = false;
			IssueOrderCode   = _token.SelectToken(kIssueOrderCode).ToString();
			CdBridge         = _token.SelectToken(kCdBridge).ToString();
			CdBridgeParts    = _token.SelectToken(kCdBridgeParts).ToString();
			NmUser           = _token.SelectToken(kNmUser).ToString();
			DcMemberSurface  = _token.SelectToken(kDcMemberSurface).ToString();
			_DcLocation      = _token.SelectToken(k_DcLocation).ToString();
			_IssueCode       = _token.SelectToken(k_IssueCode).ToString();
			//DcMemberSurface  = parseString(ParseCode.Surface, _token.SelectToken(JSON.IssueKey.dcDamageMemberSurface.ToString()).ToString());
			//DcLocation       = int.Parse(_token.SelectToken(JSON.IssueKey.dcLocation.ToString()).ToString());
			//IssueCode        = parseIssueCode(_token.SelectToken(JSON.IssueKey.fgDA001.ToString()).ToString());
			YnRecover        = _token.SelectToken(kYnRecover).ToString();
			_PositionVector  = _token.SelectToken(k_PositionVector).ToString();
			IssueStatus      = "";
			DateRcvStart     = _token.SelectToken(kDateRcvStart).ToString();
			DateRcvEnd       = _token.SelectToken(kDateRcvEnd).ToString();

			Width                =   _token.SelectToken(kWidth).ToString();
			Height               =   _token.SelectToken(kHeight).ToString();
			Depth                =   _token.SelectToken(kDepth).ToString();
			Fgroup              = _token.SelectToken(kFgroup).ToString();
			DmgDescription          =   _token.SelectToken(kDmgDescription).ToString();
			RcvDescription       =   _token.SelectToken(kRcvDescription).ToString();

			if (_token.SelectToken("files") != null && _token.SelectToken("files").ToString() != "")
			{
				JArray jImgArr = JArray.Parse(_token.SelectToken("files").ToString());
				foreach (var arg in jImgArr)
				{
					ImgIndex _index = new ImgIndex();
					_index.fid = arg.SelectToken(kFid).ToString();
					_index.ftype = arg.SelectToken(kFtype).ToString();
					Imgs.Add(_index);
				}
			}
			//if (JArray.Parse(_token.SelectToken("files").ToString()) != null)
			//{
			//}
			try
            {
            }
			catch (System.Exception e)
            {
				Debug.Log($"error {e.ToString()}");
            }
		}
	}
}
