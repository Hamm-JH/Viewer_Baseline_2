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

            _Resource.Toggle_Group(1, _setter.gameObject);

            PlatformCode pCode = MainManager.Instance.Platform;
            if(Platforms.IsSmartInspectPlatform(pCode))
            {
                ((UITemplate_SmartInspect)RootUI).Event_View_Home();
            }
        }

        private void Camera_2Outside(Interactable _setter)
        {
            PlatformCode pCode = MainManager.Instance.Platform;

            if(Platforms.IsTunnelPlatform(pCode))
            {
                GameObject obj = EventManager.Instance._SelectedObject;

                if(obj != null)
                {

                    Cameras.SetCameraDOTweenPosition(MainManager.Instance.MainCamera, obj);
                }

            }

            _Resource.Toggle_Group(1, _setter.gameObject);
        }

        private void Camera_3Inside(Interactable _setter)
        {
            PlatformCode pCode = MainManager.Instance.Platform;

            if(Platforms.IsTunnelPlatform(pCode))
            {
                GameObject obj = EventManager.Instance._SelectedObject;

                if (obj != null)
                {
                    MainManager.Instance.CurrentCameraMode = Definition.Control.CameraModes.OnlyRotate;
                    Cameras.SetCameraDOTweenPosition(MainManager.Instance.MainCamera, obj);
                }

            }


            _Resource.Toggle_Group(1, _setter.gameObject);
        }
    }
}
