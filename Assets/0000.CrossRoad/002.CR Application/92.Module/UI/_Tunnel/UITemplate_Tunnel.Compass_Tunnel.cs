using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.UI
{
	using Definition;
    using Management;
    using Module.Model;
    using View;

    public partial class UITemplate_Tunnel : AUI
	{
		public void Compass_SetPosition(Compass_EventType _type, Interactable _interactable)
        {
            Module_Model model = ContentManager.Instance.Module<Module_Model>();
            List<Transform> tnPoses = model.Trs_tunnel;

            Transform target = null;

			if(_type == Compass_EventType.Compass_Prev)
            {
                target = tnPoses[0];
            }
			else if(_type == Compass_EventType.Compass_Next)
            {
                target = tnPoses[1];
            }

            if (target == null) return;

            Cameras.SetCameraDOTweenPosition_Compass(MainManager.Instance.MainCamera, target.gameObject);
            //Cameras.SetCameraDOTweenPosition(MainManager.Instance.MainCamera, target.gameObject);

        }
	}
}
