using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.UI
{
    using Data.API;
    using Management;
    using Module.Model;
    using Module.WebAPI;
    using System.Data;
    using UnityEngine.Events;

    public abstract partial class AUI : AModule
    {
		
		/// <summary>
		/// 주소정보 가져오기
		/// </summary>
		/// <param name="_data">가져온 데이터</param>
		public abstract void API_GetAddress(AAPI _data);

		/// <summary>
		/// 텍스처 요청 API
		/// </summary>
		/// <param name="_fid">요청 키</param>
		/// <param name="_ftype">요청 키</param>
		/// <param name="_fgroup">요청 그룹</param>
		/// <param name="_callback">콜백 이벤트</param>
		public void API_RequestTexture(string _fid, string _ftype, string _fgroup, UnityAction<Texture2D> _callback)
		{
			string argument = string.Format("fid={0}&ftype={1}&fgroup={2}", _fid, _ftype, _fgroup);

			ContentManager.Instance._API.RequestMainPicture(argument, _callback);
		}

		/// <summary>
		/// 이력정보 요청
		/// </summary>
		/// <param name="_action">콜백</param>
		public void API_requestHistoryData(UnityAction<DataTable> _action)
        {
			//Module_Model model = ContentManager.Instance.Module<Module_Model>();
			ContentManager.Instance.Module<Module_WebAPI>().RequestHistoryData(_action);
        }
	}
}
