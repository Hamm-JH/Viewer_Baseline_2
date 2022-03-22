using Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Events
{
    public class Close : MonoBehaviour
    {
        public GameObject target;

        public void OnClose()
        {
            if (target != null)
            {
                target.SetActive(false);
#if UNITY_EDITOR
#else
                if(target.name == "LocationPanel")
                {
                    WebManager.ViewMap("F");
                }
#endif
            }
        }
    }
}
