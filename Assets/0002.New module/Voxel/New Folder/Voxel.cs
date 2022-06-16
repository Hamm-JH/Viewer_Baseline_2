using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Test.Voxelizer;

namespace Test
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(BoxCollider))]
    public class Voxel : MonoBehaviour
    {
        [System.Serializable]
        public class VoxelCache
        {
            [SerializeField] Mesh m_mesh;
            [SerializeField] Material m_mat;
            [SerializeField] MeshFilter m_filter;
            [SerializeField] MeshRenderer m_renderer;
            [SerializeField] BoxCollider m_boxCollider;
            //[SerializeField] Rigidbody m_rigidbody;

            public Mesh _Mesh { get => m_mesh; set => m_mesh = value; }
            public Material _Mat { get => m_mat; set => m_mat = value; }
            public MeshFilter _Filter { get => m_filter; set => m_filter = value; }
            public MeshRenderer _Renderer { get => m_renderer; set => m_renderer = value; }
            public BoxCollider _BoxCollider { get => m_boxCollider; set => m_boxCollider = value; }
            //public Rigidbody _Rigidbody { get => m_rigidbody; set => m_rigidbody = value; }

            [SerializeField] bool m_isCollision;
            public bool IsCollision 
            { 
                get => m_isCollision; 
                set
                {
                    m_isCollision = value;

                    if (value == true)
                    {
                        _Renderer.material.color = Color.red;
                    }
                    else
                    {
                        Color colr = new Color(0xFB / 255f, 1, 0, 0.1f);
                        _Renderer.material.color = colr;
                    }
                }
            }

            
        }

        // 단일 복셀 처리
        [SerializeField] VoxelCache m_cache;

        [SerializeField] Voxel[] octTree;

        #region Init

        /// <summary>
        /// 복셀을 생성한다.
        /// </summary>
        /// <param name="_name"></param>
        /// <param name="_center"></param>
        /// <param name="_scale"></param>
        /// <param name="_mesh"></param>
        /// <param name="_mat"></param>
        public void InitVoxel(string _name, Vector3 _center, Vector3 _scale, Mesh _mesh, Material _mat, int _depthIndex)
        {
            m_cache = new VoxelCache();

            gameObject.name = _name;
            //m_cache._Rigidbody = gameObject.AddComponent<Rigidbody>();

            m_cache._Mesh = _mesh;
            m_cache._Mat = _mat;

            m_cache._Filter = gameObject.GetComponent<MeshFilter>();
            m_cache._Filter.mesh = _mesh;
            m_cache._Renderer = gameObject.GetComponent<MeshRenderer>();
            m_cache._BoxCollider = gameObject.GetComponent<BoxCollider>();

            SetMesh(m_cache._Filter, _mesh);
            SetMaterial(m_cache._Renderer, _mat);
            SetColliderScale(m_cache._BoxCollider, _scale);
            //SetRigidBody(m_cache._Rigidbody);

            SetCenter(_center);

            m_cache.IsCollision = false;

            if(_depthIndex > 0)
            {
                octTree = new Voxel[8];
            }
        }

        private void SetMesh(MeshFilter _filter, Mesh _mesh)
        {
            _filter.mesh = _mesh;
        }

        private void SetMaterial(MeshRenderer _renderer, Material _mat)
        {
            _renderer.material = _mat;
        }

        private void SetColliderScale(BoxCollider _collider, Vector3 _scale)
        {
            _collider.size = _scale;
        }

        private void SetRigidBody(Rigidbody _rigid)
        {
            _rigid.useGravity = false;
            _rigid.isKinematic = false;
            _rigid.mass = 0;
            _rigid.angularDrag = 0;
            _rigid.constraints = RigidbodyConstraints.FreezeAll;
        }

        private void SetCenter(Vector3 _center)
        {
            transform.position = _center;
        }
        #endregion

        //private void OnCollisionEnter(Collision collision)
        //{
        //    Debug.Log(collision.collider.name);
        //    if (collision.collider.name == "Cube")
        //    {
        //        Debug.Log("Hello");
        //        m_cache.IsCollision = true;
        //    }
        //}

        //private void OnCollisionStay(Collision collision)
        //{
        //    //if(collision.collider.name == "HitData")
        //    //{
        //    //    Debug.Log("Hello");
        //    //    m_cache.IsCollision = true;
        //    //}
        //}

        //private void OnCollisionExit(Collision collision)
        //{
        //    if (collision.collider.name == "Cube")
        //        m_cache.IsCollision = false;
        //}
    }
}
