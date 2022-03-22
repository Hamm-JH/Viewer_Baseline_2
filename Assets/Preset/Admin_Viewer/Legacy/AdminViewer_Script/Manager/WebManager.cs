using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Manager
{
    public class WebManager : MonoBehaviour
    {
        #region Instance 
        private static WebManager instance;

        public static WebManager Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = FindObjectOfType<WebManager>() as WebManager;
                }
                return instance;
            }
        }
        #endregion

        [DllImport("__Internal")]
        public static extern void ViewMap(string argument);

        [DllImport("__Internal")]
        public static extern void InitializeMap(string argument);

        [DllImport("__Internal")]
        public static extern void OnReadyToPrint(string _fName1, string _fName2, string _fName3);
        
        [DllImport("__Internal")]
        public static extern void OnReadyToDrawingPrint(string _f1, string _f2, string _f3, string _f4, string _f5, string _f6, string _f7,
            string _f8, string _f9, string _f10, string _f11, string _f12, string _f13, bool _f14);
    }
}
