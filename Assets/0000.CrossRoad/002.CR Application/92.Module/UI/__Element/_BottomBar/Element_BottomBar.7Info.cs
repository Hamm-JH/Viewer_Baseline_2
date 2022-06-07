using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.UI
{
    using Definition;
    using View;
    using Management;
    using Definition.Data;
    using TMPro;

    public partial class Element_BottomBar : AElement
    {
        // 정보창 켜서 나오는 프로퍼티 창 처리
        private void Info_Update(APacket value)
        {
            Packet_ObjectInfo oInfo = (Packet_ObjectInfo)value;

            TextMeshProUGUI text1 = resource.m_info.m_infoText1;
            TextMeshProUGUI text2 = resource.m_info.m_infoText2;
            TextMeshProUGUI text3 = resource.m_info.m_infoText3;
            TextMeshProUGUI text4 = resource.m_info.m_infoText4;
            TextMeshProUGUI text5 = resource.m_info.m_infoText5;

            text1.text = "- 시설물 등록자 정보-";
            text2.text = "- 시설물 분류정보-";
            text3.text = oInfo._Object.name;
            text4.text = "- 등급 정보-";
            text5.text = "- 등록 일자-";
            
        }

        private void Info_Delete(APacket value)
        {
            Packet_ObjectInfo oInfo = (Packet_ObjectInfo)value;

            TextMeshProUGUI text1 = resource.m_info.m_infoText1;
            TextMeshProUGUI text2 = resource.m_info.m_infoText2;
            TextMeshProUGUI text3 = resource.m_info.m_infoText3;
            TextMeshProUGUI text4 = resource.m_info.m_infoText4;
            TextMeshProUGUI text5 = resource.m_info.m_infoText5;

            text1.text = "";
            text2.text = "";
            text3.text = "";
            text4.text = "";
            text5.text = "";
        }
    }
}
