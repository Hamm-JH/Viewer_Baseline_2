using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.WebAPI
{
	using Data.API;
	using Definition;
	using Definition.Data;
	using Management;
	using System.Data;
	using UnityEngine.Events;
	using UnityEngine.UI;

	/// <summary>
	/// 웹 API 모듈
	/// Front, Back
	/// </summary>
	public partial class Module_WebAPI : AModule
	{
		// _FLAG :: Module - WebAPI
		private void Start()
		{
			OnCreate(ModuleID.WebAPI, FunctionCode.API);
		}

		public override void OnStart()
		{
			//Debug.LogError($"{this.GetType().ToString()} Run");

			InitializeModelIssue();
		}

		public void InitializeModelIssue()
		{
			CoreData data = MainManager.Instance.Data;

			string dmgURI = data.IssueDmgURI;
			string rcvURI = data.IssueRcvURI;

			RequestData(dmgURI, WebType.Issue_Dmg, GetData);
			RequestData(rcvURI, WebType.Issue_Rcv, GetData);

			m_isDmgEnd = false;
			m_isRcvEnd = false;
		}

		private bool m_isDmgEnd;
		private bool m_isRcvEnd;

		/// <summary>
		/// 초기화 완료 확인 // 확인시 관리자에 전달
		/// index ::
		/// 0 : dmg end
		/// 1 : rcv end
		/// </summary>
		/// <param name="_index"></param>
		private void CompleteCheck(int _index)
        {
			switch(_index)
            {
				case 0:	m_isDmgEnd = true;	break;
				case 1: m_isRcvEnd = true;	break;
            }

			// 손상정보, 보수정보 모두 끝났는가?
			if(m_isDmgEnd && m_isRcvEnd)
            {
				// WebAPI 작업 완료
				ContentManager.Instance.CheckInitModuleComplete(ID);
            }
        }

		// Admin Viewer API... ----------------------------------------------------------------------

		public void RequestAddressData(UnityAction<AAPI> action)
		{
			CoreData data = MainManager.Instance.Data;

			string addressURI = data.AddressURI;

			RequestData(addressURI, WebType.Address, action, GetData);
		}

		public void RequestMainPicture(string _imgArgument, UnityAction<Texture2D> _action)
		{
			CoreData data = MainManager.Instance.Data;

			// 이미지 URI
			string imageURL = data.ImageURL;

			string imageURI = $"{imageURL}{_imgArgument}";

			RequestImage(imageURI, WebType.Image_main, _action, GetData);
			
		}

		public void RequestSinglePicture(string _imgArgument, RawImage _rImage, UnityAction<RawImage, Texture2D> _action)
		{
			CoreData data = MainManager.Instance.Data;

			// 이미지 URI
			string imageURL = data.ImageURL;

			string imageURI = $"{imageURL}{_imgArgument}";

			RequestImage(imageURI, WebType.Image_single, _rImage, _action, GetData);
		}

		/// <summary>
		/// history 수집
		/// </summary>
		/// <param name="_action"></param>
		public void RequestHistoryData(UnityAction<DataTable> _action)
		{
			CoreData data = MainManager.Instance.Data;

			string historyURI = data.HistoryURL;

			RequestData(historyURI, WebType.history, _action, GetData);
		}

		public void RequestImageHistoryData(string _imgQuery, UnityAction<string> _action)
		{
			CoreData data = MainManager.Instance.Data;

			string imgHistoryURI = $"{data.HistoryURL}{_imgQuery}";

			RequestData(imgHistoryURI, WebType.imageHistory, _action, GetData);
		}
	}
}
