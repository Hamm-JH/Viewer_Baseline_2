using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.WebAPI
{
	using Data.API;
	using Definition;
	using Definition._Issue;
	using Management;
	using Newtonsoft.Json.Linq;
	using System.Data;
	using UnityEngine.Events;
	using UnityEngine.UI;

	public partial class Module_WebAPI : AModule
	{
		#region GetData
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

		private void GetData(string _data, WebType _webT, UnityAction<string> _callback)
		{
			switch(_webT)
			{
				case WebType.imageHistory:
					_callback.Invoke(_data);
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
			}
		}

		private void GetData(RawImage _rImage, Texture2D _texture, WebType _webT, UnityAction<RawImage, Texture2D> _callback)
		{
			switch(_webT)
			{
				case WebType.Image_single:
					_callback.Invoke(_rImage, _texture);
					break;
			}
		}

		private void GetData(string _data, WebType _webT, UnityAction<DataTable> _callback)
		{
			switch(_webT)
			{
				case WebType.history:
					GetData_history(_data, _callback);
					break;
			}
		}
		
		#endregion

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
					string fgUF001 = "";
					string fgLM001 = "";
					string nmAddress = "";
					string nmTunnel = "";
					string mp_fgroup = "";
					string mp_fid = "";
					string mp_ftype = "";

					PlatformCode pCode = MainManager.Instance.Platform;
					if(Platforms.IsTunnelPlatform(pCode))
					{
						fgUF001 = "fgUF001";
						fgLM001 = "fgLM001";
						nmAddress = "nmAddress";
						nmTunnel = "nmTunnel";
						mp_fgroup = "fgroup";
						mp_fid = "fid";
						mp_ftype = "ftype";
					}
					else if(Platforms.IsBridgePlatform(pCode))
					{
						fgUF001 = "fgUS001";
						fgLM001 = "fgPO001";
						nmAddress = "nmAddress";
						nmTunnel = "nmBridge";
						mp_fgroup = "fgroup";
						mp_fid = "fid";
						mp_ftype = "ftype";
					}

					data.fgUF001 = jArr[0].SelectToken(fgUF001).ToString();		// 터널 형태
					data.fgLM001 = jArr[0].SelectToken(fgLM001).ToString();
					data.nmAddress = jArr[0].SelectToken(nmAddress).ToString();
					data.nmTunnel = jArr[0].SelectToken(nmTunnel).ToString();

					if(jArr[0].SelectToken("files").ToString() != "")
					{
						JArray mp = JArray.Parse(jArr[0].SelectToken("files").ToString());

						data.mp_fgroup = jArr[0].SelectToken(mp_fgroup).ToString();

						if(mp.Count > 0)
						{
							data.mp_fid = mp[0].SelectToken(mp_fid).ToString();
							data.mp_ftype = mp[0].SelectToken(mp_ftype).ToString();
							//data.mp_fgroup = mp[0].SelectToken("filename").ToString();
						}
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

			JObject dmgListObj = JObject.Parse(data);
			string dmgListString = dmgListObj["damagedList"].ToString();

			JArray jArr = JArray.Parse(dmgListString);

			if (jArr.Count != 0)
			{
				for (int i = 0; i < jArr.Count; i++)
				{
					Issue _v = new Issue();

					_v.SetDmg(jArr[i]);

					dmgDatas.Add(_v);
				}
			}

			OnComplete(WebType.Issue_Dmg, dmgDatas);	// 손상정보 (Legacy) 완료
			// 손상정보 완료가 되지 않았을때 (초기화 시점에만 동작)
			if(!m_isDmgEnd)
            {
				CompleteCheck(0);	// 손상정보 완료
            }
		}

		private void GetData_Rcv(string _data)
		{
			List<Issue> rcvDatas = new List<Issue>();
			//Debug.Log("rcv set data");

			JObject jObj = JObject.Parse(_data);

			string result = jObj["result"].ToString();
			string data = jObj["data"].ToString(); // JSON Object "data" to JSON Token Array

			JObject rcvListObj = JObject.Parse(data);
			string rcvListString = rcvListObj["recoverList"].ToString();

			JArray jArr = JArray.Parse(rcvListString);

			if (jArr.Count != 0)
			{
				for (int i = 0; i < jArr.Count; i++)
				{
					Issue _v = new Issue();

					_v.SetRcv(jArr[i]);

					rcvDatas.Add(_v);
				}
			}

			OnComplete(WebType.Issue_Rcv, rcvDatas);	// 보수정보 (Legacy) 완료
			// 보수정보 완료가 되지 않았을때 (초기화 시점에만 동작)
			if(!m_isRcvEnd)
            {
				CompleteCheck(1);	// 보수정보 완료
            }
		}

		private void GetImage_main(Texture2D _texture, UnityAction<Texture2D> _callback)
		{
			_callback.Invoke(_texture);
		}

		private void GetData_history(string _data, UnityAction<DataTable> _callback)
		{
			DataTable result = new DataTable();

			result.Columns.Add(new DataColumn("date", typeof(string)));
			result.Columns.Add(new DataColumn("Dcrack", typeof(List<Issue>)));
			result.Columns.Add(new DataColumn("Dbagli", typeof(List<Issue>)));
			result.Columns.Add(new DataColumn("Dbaegtae", typeof(List<Issue>)));
			result.Columns.Add(new DataColumn("Dsegul", typeof(List<Issue>)));
			result.Columns.Add(new DataColumn("Ddamage", typeof(List<Issue>)));
			result.Columns.Add(new DataColumn("Rcrack", typeof(List<Issue>)));
			result.Columns.Add(new DataColumn("Rbagli", typeof(List<Issue>)));
			result.Columns.Add(new DataColumn("Rbaegtae", typeof(List<Issue>)));
			result.Columns.Add(new DataColumn("Rsegul", typeof(List<Issue>)));
			result.Columns.Add(new DataColumn("Rdamage", typeof(List<Issue>)));

			JObject jObj = JObject.Parse(_data);

			string __data = jObj["data"]["dailyList"].ToString();

			JObject dailyList = JObject.Parse(__data);

			//Debug.Log(_jObj.First.ToString());
			////JArray jArr = JArray.Parse(__data);
			//Debug.Log($"*********{_jObj.First}");

			foreach(var day in dailyList)
			{
				//Debug.Log(day.Key);
				//Debug.Log(day.Value.ToString());

				DataRow row = result.NewRow();
				row["date"] = day.Key;

				// 손상정보가 이 날짜에 있다면?
				if(day.Value.SelectToken("damagedList") != null)
				{
					//Debug.Log($"DMG : {day.Value.SelectToken("damagedList").ToString()}");
					row = GetDataRow(day.Value.SelectToken("damagedList").ToString(), row, true);
				}
				else
				{
					//Debug.LogError("damagedList not defined");
				}

				// 보수정보가 이 날짜에 있다면?
				if (day.Value.SelectToken("recoverList") != null)
				{
					//Debug.Log(day.Value.SelectToken("recoverList").ToString());
					row = GetDataRow(day.Value.SelectToken("recoverList").ToString(), row, false);
				}
				else
				{
					//Debug.LogError("recoverList not defined");
				}

				result.Rows.Add(row);
			}

			_callback.Invoke(result);
		}

		/// <summary>
		/// 행 단위로 데이터 추가
		/// </summary>
		/// <param name="_data"> 분류된 데이터 </param>
		/// <param name="_row"> 할당받아야 하는 행 변수 </param>
		/// <param name="isDmg"> 손상정보일 경우 : true </param>
		/// <returns></returns>
		private DataRow GetDataRow(string _data, DataRow _row, bool isDmg)
		{
			//Debug.Log($"DMG : {_dmgData}");
			JObject _inData = JObject.Parse(_data);

			JToken crackToken = _inData.SelectToken("0001");
			JToken bagliToken = _inData.SelectToken("0007");
			JToken baegtaeToken = _inData.SelectToken("0009");
			JToken segulToken = _inData.SelectToken("0013");
			JToken damageToken = _inData.SelectToken("0022");

			string prefix = isDmg ? "D" : "R";

			if(crackToken != null)
			{
				_row = SetData_Issue(crackToken.ToString(), $"{prefix}crack", _row, isDmg);
			}

			if (bagliToken != null)
			{
				_row = SetData_Issue(bagliToken.ToString(), $"{prefix}bagli", _row, isDmg);
			}

			if (baegtaeToken != null)
			{
				_row = SetData_Issue(baegtaeToken.ToString(), $"{prefix}baegtae", _row, isDmg);
			}

			if (segulToken != null)
			{
				_row = SetData_Issue(segulToken.ToString(), $"{prefix}segul", _row, isDmg);
			}

			if (damageToken != null)
			{
				_row = SetData_Issue(damageToken.ToString(), $"{prefix}damage", _row, isDmg);
			}

			return _row;
		}

		/// <summary>
		/// 단일 테이블 요소에 데이터 추가
		/// </summary>
		/// <param name="_data"></param>
		/// <param name="_rowKey"></param>
		/// <param name="_row"></param>
		/// <returns></returns>
		private DataRow SetData_Issue(string _data, string _rowKey, DataRow _row, bool isDmg)
		{
			List<Issue> list = new List<Issue>();

			//Debug.Log($"{_rowKey} : {_dmgData}");

			JArray jArr = JArray.Parse(_data);

			if(jArr.Count != 0)
			{
				for (int i = 0; i < jArr.Count; i++)
				{
					//Debug.Log(jArr[i].ToString());

					Issue _v = new Issue();

					if(isDmg)
					{
						_v.SetDmg(jArr[i]);
					}
					else
					{
						_v.SetRcv(jArr[i]);
					}

					list.Add(_v);
				}
				_row[_rowKey] = list;
			}

			return _row;
		}
	}
}
