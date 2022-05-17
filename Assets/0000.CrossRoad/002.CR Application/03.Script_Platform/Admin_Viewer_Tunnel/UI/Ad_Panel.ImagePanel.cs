using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdminViewer.UI
{
	using UnityEngine.UI;
	using Definition._Issue;
	using Management;
	using Newtonsoft.Json.Linq;
	using Issue;
	using Definition;
	using System;
	using Indicator.Element;
	using System.Linq;

	public partial class Ad_Panel : MonoBehaviour
	{
		public enum ImP_option
		{
			Null,
			All,
			Damage,
			DamageInDay,
			Recover,
			Reinforcement
		}

		[Header("Img panel")]
		[SerializeField] ImgPanel_element img_element;

		private void StartImage()
		{
			//img_element = new ImgPanel_element();
		}

		public void OpenImagePanel(Definition._Issue.Issue _issue)
		{
			Debug.Log($"Hello Image panel");

			Img_ClearElement();
			RequestImages(_issue);
		}

		private void Img_ClearElement()
		{
			img_element.img_enlargePanel.SetActive(false);

			int index = img_element.img_cRoot.transform.childCount;

			for (int i = 0; i < index; i++)
			{
				Destroy(img_element.img_cRoot.transform.GetChild(i).gameObject);
			}
		}

		private void RequestImages(Definition._Issue.Issue _issue)
		{
			string ObjectCode = _issue.CdBridge;
			string partName = _issue.CdBridgeParts;

			ContentManager.Instance._API.RequestImageHistoryData($"&cdTunnelParts={partName}", GetRequestImages);
		}

		#region Get Image index, string datas
		private void GetRequestImages(string _data)
		{
			Debug.Log(_data);

			JObject jObj = JObject.Parse(_data);

			string __data = jObj["data"]["dailyList"].ToString();

			List<Issue> imageIndexList = new List<Issue>();

			JObject dailyList = JObject.Parse(__data);

			foreach(var day in dailyList)
			{
				string date = day.Key;
				// 손상 리스트 있다면
				if(day.Value.SelectToken("damagedList") != null)
				{
					string dmgList = day.Value.SelectToken("damagedList").ToString();

					GetDataIndex(dmgList, date, true, imageIndexList);
				}

				// 보수 리스트 있다면
				if(day.Value.SelectToken("recoverList") != null)
				{
					string rcvList = day.Value.SelectToken("recoverList").ToString();

					GetDataIndex(rcvList, date, false, imageIndexList);
				}
			}



			SetImagePanel(imageIndexList);
		}

		private void GetDataIndex(string _data, string date, bool isDmg, List<Issue> list)
		{
			JObject _inData = JObject.Parse(_data);

			JToken crackToken = _inData.SelectToken("0001");
			JToken bagliToken = _inData.SelectToken("0007");
			JToken baegtaeToken = _inData.SelectToken("0009");
			JToken segulToken = _inData.SelectToken("0013");
			JToken damageToken = _inData.SelectToken("0022");

			List<Issue> ind1 = new List<Issue>();
			List<Issue> ind2 = new List<Issue>();
			List<Issue> ind3 = new List<Issue>();
			List<Issue> ind4 = new List<Issue>();
			List<Issue> ind5 = new List<Issue>();

			if (crackToken != null)
			{
				ind1 = CreateImageIndex(crackToken.ToString(), date, isDmg);
				//_row = SetData_Issue(crackToken.ToString(), $"{prefix}crack", _row, isDmg);
			}

			if (bagliToken != null)
			{
				ind2 = CreateImageIndex(bagliToken.ToString(), date, isDmg);
				//_row = SetData_Issue(bagliToken.ToString(), $"{prefix}bagli", _row, isDmg);
			}

			if (baegtaeToken != null)
			{
				ind3 = CreateImageIndex(baegtaeToken.ToString(), date, isDmg);
				//_row = SetData_Issue(baegtaeToken.ToString(), $"{prefix}baegtae", _row, isDmg);
			}

			if (segulToken != null)
			{
				ind4 = CreateImageIndex(segulToken.ToString(), date, isDmg);
				//_row = SetData_Issue(segulToken.ToString(), $"{prefix}segul", _row, isDmg);
			}

			if (damageToken != null)
			{
				ind5 = CreateImageIndex(damageToken.ToString(), date, isDmg);
				//_row = SetData_Issue(damageToken.ToString(), $"{prefix}damage", _row, isDmg);
			}

			list.AddRange(ind1);
			list.AddRange(ind2);
			list.AddRange(ind3);
			list.AddRange(ind4);
			list.AddRange(ind5);
		}

		private List<Issue> CreateImageIndex(string _data, string date, bool _isDmg)
		{
			List<Issue> list = new List<Issue>();

			JArray jArr = JArray.Parse(_data);

			if(jArr.Count != 0)
			{
				for (int i = 0; i < jArr.Count; i++)
				{
					Issue _img = new Issue();

					if(_isDmg)
					{
						_img.SetDmg(date, jArr[i]);
					}
					else
					{
						_img.SetRcv(date, jArr[i]);
					}

					list.Add(_img);
				}
			}

			return list;
		}
		#endregion

		private void SetImagePanel(List<Issue> _list)
		{
			Debug.Log("On Ready");

			int index = _list.Count;
			for (int i = 0; i < index; i++)
			{
				// 요소 하나 만들고 패널에 넣기

				GameObject _obj = Instantiate<GameObject>(
					Resources.Load<GameObject>("UI/UIElement/ImP_ImgContent"), 
					img_element.img_cRoot.transform);
				Imp_element _element = _obj.GetComponent<Imp_element>();

				_element.largeImgPanel = img_element.img_enlargePanel;

				DateTime currTime =	DateTime.Parse(_list[i].Date);

				_element.yearDate.text = $"{currTime.Year}년";
				_element.date.text = $"{currTime.Month}/{currTime.Day}";
				_element.description.text = $"{_list[i].DmgDescription}";

				if(_list[i].Imgs != null && _list[i].Imgs.Count != 0)
				{
					string imgArgument = string.Format("fid={0}&ftype={1}&fgroup={2}", 
						_list[i].Imgs.First().fid, _list[i].Imgs.First().ftype, _list[i].Fgroup);
					ContentManager.Instance._API.RequestSinglePicture(imgArgument, _element.image, SetSinglePicture);
				}

				// i가 0(시작지점)이 아닐 경우
				if(i != 0)
				{
					DateTime beforeTime = DateTime.Parse(_list[i-1].Date);

					// 현재 연도와 과거 연도가 같을 경우
					if(currTime.Year == beforeTime.Year)
					{
						// 현재 연도 패널을 끈다.
						_element.yearPanel.SetActive(false);
					}
				}
			}
		}

		private void SetSinglePicture(RawImage _rImage, Texture2D _texture2D)
		{
			_rImage.texture = _texture2D;
		}
	}
}
