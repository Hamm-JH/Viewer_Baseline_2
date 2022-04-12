using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.UI
{
    using Data.API;
    using Definition;
    using Management;
    using View;


    public partial class UITemplate_SmartInspect : AUI
    {
        /// <summary>
        /// 모듈 세팅이 완료된 시점에서 초기화를 수행한다.
        /// </summary>
        public void Initialize_AfterModuleInitialize()
        {
            ContentManager.Instance._API.RequestAddressData(API_GetAddress);
        }

        /// <summary>
        /// API에서 주소값을 받아오면 주소 데이터 기반으로 초기화를 수행한다.
        /// </summary>
        /// <param name="_data"></param>
        private void Initialize_API_GetAddress(AAPI _data)
        {
            // 다시 언박싱
            DAddress data = (DAddress)_data;

            //TitData.oName.text = data.nmTunnel;
            //BotData.adrName.text = data.nmAddress;

            Header_SetObjectName(data.nmTunnel);    // 시설물명
            //SetTitleText(data.nmTunnel);      // 시설물명
            //SetAddressText(data.nmAddress);   // 주소 할당

            //string _fid = data.mp_fid;
            //string _ftype = data.mp_ftype;
            //string _fgroup = data.mp_filename;

            //string argument = string.Format("fid={0}&ftype={1}&fgroup={2}", _fid, _ftype, _fgroup);

            API_RequestTexture(data.mp_fid, data.mp_ftype, data.mp_fgroup, API_getMainTexture);     // 텍스트 가져오기
        }
    }
}
