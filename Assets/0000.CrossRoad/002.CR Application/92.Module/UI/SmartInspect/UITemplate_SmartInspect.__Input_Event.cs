using Definition;
using Management;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View;

namespace Module.UI
{
    public partial class UITemplate_SmartInspect : RootUI
    {

        /// <summary>
        /// 3D 객체 선택 이벤트 받음
        /// </summary>
        /// <param name="_obj"></param>
        public void Input_Select3DObject(GameObject _obj)
        {
            // 선택한 3D 객체를 Header 텍스트에 할당한다.
            PlatformCode pCode = MainManager.Instance.Platform;

            //if(Platforms.IsTunnelPlatform())
            if(_obj != null)
            {
                m_basePanel.m_header.partBackground.SetActive(true);
                m_basePanel.m_header.partName.gameObject.SetActive(true);
                m_basePanel.m_header.partName.text = Definition.Projects.Parts.GetPartName(_obj.name);
            }
            else
            {
                m_basePanel.m_header.partBackground.SetActive(false);
                m_basePanel.m_header.partName.gameObject.SetActive(false);
            }
        }
    }
}
