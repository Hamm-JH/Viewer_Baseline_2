using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Control
{
    public static class ImportManager
    {
        public static System.Type GetControllerComponent(Type controlType)
        {
            //typeof(AContoller);
            switch(controlType)
            {
                case Type.Keyboard_Mouse:   return typeof(Keyboard_Mouse);
                case Type.JoyStick:         return typeof(JoyStick);
                case Type.Touch:            return typeof(Touch);
                default:
                    Debug.Log("not allowed access -> default setting : Keyboard_Mouse");
                    return typeof(Keyboard_Mouse);
            }
            
        }
    }
}
