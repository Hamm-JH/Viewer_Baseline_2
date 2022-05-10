using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management
{
    using Definition;
    using DG.Tweening;
    using Module.Interaction;
    using Module.UI;
    using System.Linq;
    using View;

	public partial class ContentManager : IManager<ContentManager>
	{
        public void ButtonBar_Toggle(bool isOn)
        {
            PlatformCode pCode = MainManager.Instance.Platform;
            if(Platforms.IsBridgePlatform(pCode))
            {
                float toMoveY = isOn ? 64 : -96;

                UITemplate_Tunnel uiTunnel = (UITemplate_Tunnel)(Module<Module_Interaction>().UiInstances.First());

                ButtonBar_MoveY(uiTunnel.m_buttonBar, toMoveY);
            }
            else if(Platforms.IsTunnelPlatform(pCode))
            {
                // 이동 위치 할당
                float toMoveY = isOn ? 64 : -96;

                UITemplate_Tunnel uiTunnel = (UITemplate_Tunnel)(Module<Module_Interaction>().UiInstances.First());

                ButtonBar_MoveY(uiTunnel.m_buttonBar, toMoveY);
            }
        }

        private void ButtonBar_MoveY(GameObject _obj, float _toMove)
        {
            _obj.transform.DOMoveY(_toMove, 1);
        }

        private void ButtonBar_MoveUI(Interactable _ui)
        {
            UI_Selectable ui = (UI_Selectable)_ui;

            if (ui.Data.m_isMovedDown)
            {
                ui.Data.m_isMovedDown = false;
                ui.ChildPanel.transform.DOMoveY(43, 1);
            }
            else
            {
                ui.Data.m_isMovedDown = true;
                ui.ChildPanel.transform.DOMoveY(-96, 1);
            }
            //Debug.Log(ui.ChildPanel.transform.position);
            // 43 up
            // -96 down
        }
    }
}
