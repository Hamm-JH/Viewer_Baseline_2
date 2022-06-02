using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.UI
{
    using Definition;
    using View;
    using Management;

    public partial class Element_BottomBar : AElement
    {
        private void Camera_1Home(Interactable _setter)
        {
            ContentManager.Instance.SetCameraCenterPosition();
            //_Settings.ResetSlider_transparency();
            //_Settings.ResetSlider_scale();
            MainManager.Instance.UpdateCameraMode(Definition.Control.CameraModes.BIM_ISO);

            Show_All(_setter);
        }

        private void Camera_2Outside(Interactable _setter)
        {
            PlatformCode pCode = MainManager.Instance.Platform;

            if(Platforms.IsTunnelPlatform(pCode))
            {

            }
        }

        private void Camera_3Inside(Interactable _setter)
        {
            PlatformCode pCode = MainManager.Instance.Platform;

            if(Platforms.IsTunnelPlatform(pCode))
            {

            }
        }
    }
}
