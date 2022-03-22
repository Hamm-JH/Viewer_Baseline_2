using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Events
{
    public class Switch : MonoBehaviour
    {
        public GameObject target;

        public void OnSwitch()
        {
            target.SetActive(target.activeSelf ? false : true);
        }
    }
}
