using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test
{
    public static class RigidBodyUtil
    {
        public static void CollisionEnter(Collision collision)
        {
            Debug.Log($"collision enter target : {collision.collider.name}");

            Voxel2 vox;
            if (collision.collider.TryGetComponent<Voxel2>(out vox))
            {
                if (!vox.m_state.IsCollision)
                {
                    vox.m_state.IsCollision = true;

                    // 자식 노드 시행
                    vox.CreateChildren();
                }
            }
        }

        public static void CollisionExit(Collision collision)
        {
            Debug.Log($"collision exit target : {collision.collider.name}");

            Voxel2 vox;
            if (collision.collider.TryGetComponent<Voxel2>(out vox))
            {
                vox.m_state.IsCollision = false;
            }
        }

        public static void TriggerEnter(Collider other)
        {
            //Debug.Log($"trigger enter target : {other.transform.name}");
            
            Voxel2 vox;
            if (other.transform.TryGetComponent<Voxel2>(out vox))
            {
                if (!vox.m_state.IsCollision)
                {
                    vox.m_state.IsCollision = true;

                    // 자식 노드 시행
                    vox.CreateChildren();
                }
            }
        }

        public static void TriggerExit(Collider other)
        {
            //Debug.Log($"trigger exit target : {other.transform.name}");

            Voxel2 vox;
            if (other.transform.TryGetComponent<Voxel2>(out vox))
            {
                vox.m_state.IsCollision = false;
            }
        }
    }
}
