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
            //Debug.Log(_type.ToString());
            switch(_type)
            {
                #region BottomBar
                case BottomBar_EventType.Btn1_CameraMode:
                    Bar_1Camera(_setter);
                    break;

                case BottomBar_EventType.Btn2_ViewPosition:
                    Bar_2View(_setter);
                    break;

                case BottomBar_EventType.Btn3_OrthoMode:
                    Bar_3OrthoView(_setter);
                    break;

                case BottomBar_EventType.Btn4_ShowMode:
                    Bar_4Show(_setter);
                    break;

                case BottomBar_EventType.Btn5_Pan:
                    Bar_5Pan(_setter);
                    break;

                case BottomBar_EventType.Btn6_Setting:
                    Bar_6Setting(_setter);
                    break;

                case BottomBar_EventType.Btn7_Info:
                    Bar_7Info(_setter);
                    break;

                case BottomBar_EventType.ZoomIn:
                    Bar_ZoomIn(_setter);
                    break;

                case BottomBar_EventType.ZoomOut:
                    Bar_ZoomOut(_setter);
                    break;
                #endregion

                #region 1 CameraMode
                case BottomBar_EventType.CameraMode_1Home:
                    Camera_1Home(_setter);
                    break;

                case BottomBar_EventType.CameraMode_2Outside:
                    Camera_2Outside(_setter);
                    break;

                case BottomBar_EventType.CameraMode_3Inside:
                    Camera_3Inside(_setter);
                    break;
                #endregion

                #region 2 View
                case BottomBar_EventType.View_ISO:
                    View_1ISO(_setter);
                    break;

                case BottomBar_EventType.View_Top:
                    View_2Top(_setter);
                    break;

                case BottomBar_EventType.View_Side:
                    View_3Side(_setter);
                    break;

                case BottomBar_EventType.View_Bottom:
                    View_4Bottom(_setter);
                    break;
                #endregion

                #region 3 Orthogonal
                case BottomBar_EventType.Ortho_Orthogonal:
                    Ortho_1Orthogonal(_setter);
                    break;

                case BottomBar_EventType.Ortho_Perspective:
                    Ortho_2Perspective(_setter);
                    break;
                #endregion

                #region 4 Show
                case BottomBar_EventType.Show_ShowAll:
                    Show_All(_setter);
                    break;

                case BottomBar_EventType.Show_Hide:
                    Show_Hide(_setter);
                    break;

                case BottomBar_EventType.Show_Isolate:
                    Show_Isolate(_setter);
                    break;
                #endregion

                #region Function
                case BottomBar_EventType.Function_ElementOff:
                    OffElement(_setter);
                    break;

                case BottomBar_EventType.Function_ElementOn:
                    OnElement(_setter);
                    break;

                case BottomBar_EventType.Function_Toggle:
                    ToggleElement(_setter);
                    break;
                #endregion
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
            //Debug.Log(_type.ToString());
            switch(_type)
            {
                case BottomBar_EventType.Setting_IconSize:
                    Setting_IconSize(_value, _setter);
                    break;

                case BottomBar_EventType.Setting_ModelTransparency:
                    Setting_ModelTransparency(_value, _setter);
                    break;

                case BottomBar_EventType.Setting_ZoomSensitivity:
                    Setting_ZoomSensitivity(_value, _setter);
                    break;

                case BottomBar_EventType.Setting_FontSize:
                    Setting_FontSize(_value, _setter);
                    break;
            }
        }
        #endregion
    }
}
