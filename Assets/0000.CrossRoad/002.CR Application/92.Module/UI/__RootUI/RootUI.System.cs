using System.Collections;
using System.Collections.Generic;

namespace Module.UI
{
    using Data.API;
    using Definition;
    using UnityEngine;
    using View;

    public abstract partial class RootUI : AUI
    {
        public override void API_GetAddress(AAPI _data)
        {
            //throw new System.NotImplementedException();
            Debug.Log("API_GetAddress");
        }

        public override void OnModuleComplete()
        {
            //throw new System.NotImplementedException();
            Debug.Log("OnModuleComplete");
        }

        public override void OnStart()
        {
            //throw new System.NotImplementedException();
            Debug.Log("OnStart");
        }

        public override void ReInvokeEvent()
        {
            //throw new System.NotImplementedException();
            Debug.Log("ReInvokeEvent");
        }

        public override void SetObjectData_Tunnel(GameObject selected)
        {
            //throw new System.NotImplementedException();
            Debug.Log("SetObjectData_Tunnel");
        }

        public override void TogglePanelList(int _index, GameObject _exclusive)
        {
            //throw new System.NotImplementedException();
            Debug.Log("TogglePanelList");
        }
    }
}
