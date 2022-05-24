using Definition;
using Management;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View;

namespace Module.UI
{
    public partial class UITemplate_SmartInspect : AUI
    {
        /// <summary>
        /// UI 템플릿 초기화시 실행
        /// </summary>
        public override void OnStart()
        {
            Debug.Log("UITemplate_SmartInspect Start method");
        }

        

        

        public override void OnModuleComplete()
        {
            throw new System.NotImplementedException();
        }


        public override void ReInvokeEvent()
        {
            throw new System.NotImplementedException();
        }

        public override void SetObjectData_Tunnel(GameObject selected)
        {
            throw new System.NotImplementedException();
        }

        public override void TogglePanelList(int _index, GameObject _exclusive)
        {
			m_bottomBar.m_subPanels.ForEach(x => x.SetActive((x != _exclusive) ? false : x.activeSelf));
        }
    }
}
