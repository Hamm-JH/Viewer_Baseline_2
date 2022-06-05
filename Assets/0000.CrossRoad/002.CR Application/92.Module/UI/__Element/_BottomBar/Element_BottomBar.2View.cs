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
        private void View_1ISO(Interactable _setter)
        {
            MainManager.Instance.UpdateCameraMode(UIEventType.Viewport_ViewMode_ISO);
            ContentManager.Instance.SetCameraCenterPosition(UIEventType.Viewport_ViewMode_ISO);

            _Resource.Toggle_Group(2, _setter.gameObject);
        }

        private void View_2Top(Interactable _setter)
        {
            MainManager.Instance.UpdateCameraMode(UIEventType.Viewport_ViewMode_TOP);
            ContentManager.Instance.SetCameraCenterPosition(UIEventType.Viewport_ViewMode_TOP);
            
            _Resource.Toggle_Group(2, _setter.gameObject);
        }

        private void View_3Side(Interactable _setter)
        {
            MainManager.Instance.UpdateCameraMode(UIEventType.Viewport_ViewMode_SIDE);
            ContentManager.Instance.SetCameraCenterPosition(UIEventType.Viewport_ViewMode_SIDE);

            _Resource.Toggle_Group(2, _setter.gameObject);
        }

        private void View_4Bottom(Interactable _setter)
        {
            MainManager.Instance.UpdateCameraMode(UIEventType.Viewport_ViewMode_BOTTOM);
            ContentManager.Instance.SetCameraCenterPosition(UIEventType.Viewport_ViewMode_BOTTOM);

            _Resource.Toggle_Group(2, _setter.gameObject);
        }
    }
}
