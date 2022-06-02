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
        }

        private void View_2Top(Interactable _setter)
        {
            MainManager.Instance.UpdateCameraMode(UIEventType.Viewport_ViewMode_TOP);
            ContentManager.Instance.SetCameraCenterPosition(UIEventType.Viewport_ViewMode_TOP);
        }

        private void View_3Side(Interactable _setter)
        {
            MainManager.Instance.UpdateCameraMode(UIEventType.Viewport_ViewMode_SIDE);
            ContentManager.Instance.SetCameraCenterPosition(UIEventType.Viewport_ViewMode_SIDE);
        }

        private void View_4Bottom(Interactable _setter)
        {
            MainManager.Instance.UpdateCameraMode(UIEventType.Viewport_ViewMode_BOTTOM);
            ContentManager.Instance.SetCameraCenterPosition(UIEventType.Viewport_ViewMode_BOTTOM);
        }
    }
}
