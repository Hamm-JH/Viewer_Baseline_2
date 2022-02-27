using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.WebAPI
{
	using Definition;
	using Definition._Issue;
	using Newtonsoft.Json.Linq;

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
					_v.IssueCode         =	jArr[i].SelectToken("fgDA001").ToString();
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
					_v.IssueCode		= jArr[i].SelectToken("fgDA001").ToString();
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
	}
}
