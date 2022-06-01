using Definition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View;

namespace Module.UI
{
    public partial class Element_BottomBar : AElement
    {
        #region Button event
        public override void GetUIEvent(UIEventType _uType, Interactable _setter)
        {
        }

        public override void GetUIEvent(Inspect_EventType _uType, Interactable _setter)
        {
        }

        public override void GetUIEvent(BottomBar_EventType _type, Interactable _setter)
        {
            Debug.Log(_type.ToString());
            switch(_type)
            {
                case BottomBar_EventType.Btn1_CameraMode:

                    break;

                case BottomBar_EventType.Btn2_ViewPosition:

                    break;

                case BottomBar_EventType.Btn3_OrthoMode:

                    break;

                case BottomBar_EventType.Btn4_ShowMode:

                    break;

                case BottomBar_EventType.Btn5_Pan:

                    break;

                case BottomBar_EventType.Btn6_Setting:

                    break;

                case BottomBar_EventType.Btn7_Info:

                    break;

                case BottomBar_EventType.ZoomIn:

                    break;

                case BottomBar_EventType.ZoomOut:

                    break;
            }
        }
        #endregion

        #region Slider event
        public override void GetUIEvent(float _value, UIEventType _uType, Interactable _setter)
        {
        }

        public override void GetUIEvent(float _value, Inspect_EventType _uType, Interactable _setter)
        {
        }

        public override void GetUIEvent(float _value, BottomBar_EventType _type, Interactable _setter)
        {
        }
        #endregion
    }
}
