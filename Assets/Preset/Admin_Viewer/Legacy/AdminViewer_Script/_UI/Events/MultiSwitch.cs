using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Events
{
    public class MultiSwitch : MonoBehaviour
    {
        public List<GameObject> targets;

        public void OnMultiSwitch()
        {
            bool result = false;

            foreach (GameObject obj in targets)
            {
                result = result | obj.activeSelf;
            }

            foreach (GameObject obj in targets)
            {
                obj.SetActive(!result);
            }
        }
    }
}
