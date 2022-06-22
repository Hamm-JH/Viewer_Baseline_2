using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test
{
    public class RigidBodyTester : MonoBehaviour
    {


        private void OnCollisionEnter(Collision collision)
        {
            RigidBodyUtil.CollisionEnter(collision);
        }

        private void OnCollisionExit(Collision collision)
        {
            RigidBodyUtil.CollisionExit(collision);
        }

        private void OnTriggerEnter(Collider other)
        {
            RigidBodyUtil.TriggerEnter(other);
        }

        private void OnTriggerExit(Collider other)
        {
            RigidBodyUtil.TriggerExit(other);
        }
    }
}
