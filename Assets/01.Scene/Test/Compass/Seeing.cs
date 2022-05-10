using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test
{
    public class Seeing : MonoBehaviour
    {
        public Transform target;

        // Update is called once per frame
        void Update()
        {
            transform.LookAt(target);
        }
    }
}
