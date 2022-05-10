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
		
		public abstract void API_GetAddress(AAPI _data);
		//      {
		//	// 다시 언박싱
		//	DAddress data = (DAddress)_data;

		//	//TitData.oName.text = data.nmTunnel;
		//	//BotData.adrName.text = data.nmAddress;
		//	SetTitleText(data.nmTunnel);
		//	SetAddressText(data.nmAddress);

		//	//string _fid = data.mp_fid;
		//	//string _ftype = data.mp_ftype;
		//	//string _fgroup = data.mp_filename;

		//	//string argument = string.Format("fid={0}&ftype={1}&fgroup={2}", _fid, _ftype, _fgroup);
		//	GetTexture(data.mp_fid, data.mp_ftype, data.mp_fgroup, GetMainPicture);
		//}

		public void API_RequestTexture(string _fid, string _ftype, string _fgroup, UnityAction<Texture2D> _callback)
		{
			string argument = string.Format("fid={0}&ftype={1}&fgroup={2}", _fid, _ftype, _fgroup);

			ContentManager.Instance._API.RequestMainPicture(argument, _callback);
		}

		public void API_requestHistoryData(UnityAction<DataTable> _action)
        {
			//Module_Model model = ContentManager.Instance.Module<Module_Model>();
			ContentManager.Instance.Module<Module_WebAPI>().RequestHistoryData(_action);
        }
	}
}
