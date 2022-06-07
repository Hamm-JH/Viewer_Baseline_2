﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.UI
{
    using Definition;
    using Management;
    using View;

    public partial class Element_BottomBar : AElement
    {
        private void Bar_1Camera(Interactable _setter)
        {
            _Children.Toggle(_setter.ChildPanel);
            _Resource.Toggle_Group(0, _setter.gameObject);
        }

        private void Bar_2View(Interactable _setter)
        {
            _Children.Toggle(_setter.ChildPanel);
            _Resource.Toggle_Group(0, _setter.gameObject);
        }

        private void Bar_3OrthoView(Interactable _setter)
        {
            _Children.Toggle(_setter.ChildPanel);
            _Resource.Toggle_Group(0, _setter.gameObject);
        }

        private void Bar_4Show(Interactable _setter)
        {
            _Children.Toggle(_setter.ChildPanel);
            _Resource.Toggle_Group(0, _setter.gameObject);
        }

        private void Bar_5Pan(Interactable _setter)
        {
            _Children.Off();
            _Resource.Toggle_Group(0, _setter.gameObject);

            // 카메라 모드 변경
            if (MainManager.Instance.CurrentCameraMode == Definition.Control.CameraModes.BIM_Panning)
            {
                MainManager.Instance.CurrentCameraMode = Definition.Control.CameraModes.BIM_ISO;
            }
            else
            {
                MainManager.Instance.CurrentCameraMode = Definition.Control.CameraModes.BIM_Panning;
            }
        }

        private void Bar_6Setting(Interactable _setter)
        {
            _Children.Toggle(_setter.ChildPanel);
            _Resource.Toggle_Group(0, _setter.gameObject);
        }

        private void Bar_7Info(Interactable _setter)
        {
            //_Children.Off();
            //_Resource.Toggle_Group(0, _setter.gameObject);

            _setter.ChildPanel.SetActive(!_setter.ChildPanel.activeSelf);
        }

        private void Bar_ZoomIn(Interactable _setter)
        {
            _Children.Off();

            Zoom_in(_setter);
        }

        private void Bar_ZoomOut(Interactable _setter)
        {
            _Children.Off();

            Zoom_out(_setter);
        }
    }
}
