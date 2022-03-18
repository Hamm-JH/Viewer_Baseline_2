using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.WebAPI
{
	using Data.API;
	using Definition;
	using Definition._Issue;
	using Newtonsoft.Json.Linq;
	using UnityEngine.Events;

	public partial class Module_WebAPI : AModule
	{
		private void GetData(string _data, WebType _webT)
		{
			switch (_webT)
			{
				case WebType.Issue_Dmg:
					GetData_Dmg(_data);
					break;

				case WebType.Issue_Rcv:
					GetData_Rcv(_data);
					break;
			}
		}

		private void GetData(string _data, WebType _webT, UnityAction<AAPI> _callback)
		{
			switch(_webT)
			{
				case WebType.Address:
					GetData_Address(_data, _callback);
					break;
			}
		}

		private void GetData(Texture2D _texture, WebType _webT, UnityAction<Texture2D> _callback)
		{
			switch(_webT)
			{
				case WebType.Image_main:
					GetImage_main(_texture, _callback);
					break;

				case WebType.Image_single:

					break;
			}
		}

		private void GetData_Address(string _data, UnityAction<AAPI> _callback)
		{
			// 데이터 세팅 인스턴스
			DAddress data = new DAddress();
			Debug.Log($"Hello Address");

			{
				JObject jObj = JObject.Parse(_data);

				string iData = jObj["data"].ToString();
				//iData = iData[0].ToString();

				JArray jArr = JArray.Parse(iData);

				if(jArr.Count > 0)
				{
					data.fgUF001 = jArr[0].SelectToken("fgUF001").ToString();
					data.fgLM001 = jArr[0].SelectToken("fgLM001").ToString();
					data.nmAddress = jArr[0].SelectToken("nmAddress").ToString();
					data.nmTunnel = jArr[0].SelectToken("nmTunnel").ToString();

					JArray mp = JArray.Parse(jArr[0].SelectToken("files").ToString());

					data.mp_fgroup = jArr[0].SelectToken("fgroup").ToString();

					if(mp.Count > 0)
					{
						data.mp_fid = mp[0].SelectToken("fid").ToString();
						data.mp_ftype = mp[0].SelectToken("ftype").ToString();
						//data.mp_fgroup = mp[0].SelectToken("filename").ToString();
					}
				}
			}

			_callback.Invoke((AAPI)data);
		}

		private void GetData_Dmg(string _data)
		{
			List<Issue> dmgDatas = new List<Issue>();

			JObject jObj = JObject.Parse(_data);

			string result = jObj["result"].ToString();
			string data = jObj["data"].ToString(); // JSON Object "data" to JSON Token Array

			//Debug.Log(_data);
			//Debug.Log(result);
			//Debug.Log(data);

			JObject dmgListObj = JObject.Parse(data);
			string dmgListString = dmgListObj["damagedList"].ToString();

			JArray jArr = JArray.Parse(dmgListString);

			if (jArr.Count != 0)
			{
				for (int i = 0; i < jArr.Count; i++)
				{
					Issue _v = new Issue();

					_v.IssueOrderCode	 =  jArr[i].SelectToken("cdTunnelDamaged").ToString();
					_v.CdBridge			 =  jArr[i].SelectToken("cdTunnel").ToString();
					_v.CdBridgeParts	 =  jArr[i].SelectToken("cdTunnelParts").ToString();
					_v.DcMemberSurface   =	jArr[i].SelectToken("dcDamageMemberSurface").ToString();
					_v._DcLocation        =	jArr[i].SelectToken("dcLocation").ToString();
					_v._IssueCode         =	jArr[i].SelectToken("fgDA001").ToString();
					//_v.DcMemberSurface   = parseString(ParseCode.Surface, jArr[i].SelectToken(JSON.IssueKey.dcDamageMemberSurface.ToString()).ToString());
					//_v.DcLocation        =      int.Parse(jArr[i].SelectToken(JSON.IssueKey.dcLocation.ToString()).ToString());
					//_v.IssueCode         =       parseIssueCode(jArr[i].SelectToken(JSON.IssueKey.fgDA001.ToString()).ToString());
					_v.YnRecover         = "";
					_v.IssueStatus       =	jArr[i].SelectToken("dcGrade").ToString();
					_v._PositionVector    =  jArr[i].SelectToken("dcPinLocation").ToString();

					dmgDatas.Add(_v);
				}
			}

			OnComplete(WebType.Issue_Dmg, dmgDatas);
		}

		private void GetData_Rcv(string _data)
		{
			List<Issue> rcvDatas = new List<Issue>();
			//Debug.Log("rcv set data");

			JObject jObj = JObject.Parse(_data);

			string result = jObj["result"].ToString();
			string data = jObj["data"].ToString(); // JSON Object "data" to JSON Token Array

			//Debug.Log(_data);
			//Debug.Log(result);
			//Debug.Log(data);

			JObject rcvListObj = JObject.Parse(data);
			string rcvListString = rcvListObj["recoverList"].ToString();

			JArray jArr = JArray.Parse(rcvListString);

			if (jArr.Count != 0)
			{
				for (int i = 0; i < jArr.Count; i++)
				{
					Issue _v = new Issue();

					_v.IssueOrderCode	= jArr[i].SelectToken("cdTunnelRecover").ToString();
					_v.CdBridge			= jArr[i].SelectToken("cdTunnel").ToString();
					_v.CdBridgeParts	= jArr[i].SelectToken("cdTunnelParts").ToString();
					_v.DcMemberSurface	= jArr[i].SelectToken("dcDamageMemberSurface").ToString();
					_v._DcLocation		= jArr[i].SelectToken("dcLocation").ToString();
					_v._IssueCode		= jArr[i].SelectToken("fgDA001").ToString();
					//_v.DcMemberSurface  = parseString(ParseCode.Surface, jArr[i].SelectToken(JSON.IssueKey.dcDamageMemberSurface.ToString()).ToString());
					//_v.DcLocation       = int.Parse(jArr[i].SelectToken(JSON.IssueKey.dcLocation.ToString()).ToString());
					//_v.IssueCode        = parseIssueCode(jArr[i].SelectToken(JSON.IssueKey.fgDA001.ToString()).ToString());
					_v.YnRecover		= jArr[i].SelectToken("ynRecover").ToString();
					_v._PositionVector	= jArr[i].SelectToken("dcPinLocation").ToString();
					_v.IssueStatus		= "";

					rcvDatas.Add(_v);
				}
			}

			OnComplete(WebType.Issue_Rcv, rcvDatas);
		}

		private void GetImage_main(Texture2D _texture, UnityAction<Texture2D> _callback)
		{
			//Texture2D tex = new Texture2D(100, 100);

			_callback.Invoke(_texture);
		}
	}
}
