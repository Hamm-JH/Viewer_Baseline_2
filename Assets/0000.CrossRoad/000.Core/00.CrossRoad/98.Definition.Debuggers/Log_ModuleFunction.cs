using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Definition.Debuggers
{
    public class Log_ModuleFunction : Debuggers
    {
        public static void Log(ModuleID _id, FunctionCode _code)
        {
            StringBuilder str = new StringBuilder();

            str.AppendLine($"ModuleID : {_id.ToString()}");
            str.AppendLine($"FunctionCode : {_code.ToString()}");
            Log(str.ToString());
        }

        private static void Log(string message)
        {
#if UNITY_EDITOR
            Debug.Log(message);
#endif
        }
    }

}
