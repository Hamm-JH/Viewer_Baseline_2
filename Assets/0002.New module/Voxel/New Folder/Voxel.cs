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
            [SerializeField] int m_depthIndex;

            [SerializeField] Mesh m_mesh;
            [SerializeField] Material m_mat;
            [SerializeField] MeshFilter m_filter;
            [SerializeField] MeshRenderer m_renderer;
            [SerializeField] BoxCollider m_boxCollider;

            public int DepthIndex { get => m_depthIndex; set => m_depthIndex = value; }
            public Mesh _Mesh { get => m_mesh; set => m_mesh = value; }
            public Material _Mat { get => m_mat; set => m_mat = value; }
            public MeshFilter _Filter { get => m_filter; set => m_filter = value; }
            public MeshRenderer _Renderer { get => m_renderer; set => m_renderer = value; }
            public BoxCollider _BoxCollider { get => m_boxCollider; set => m_boxCollider = value; }

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
        /// (Loop) 복셀을 생성한다.
        /// </summary>
        /// <param name="_name"> 인스턴스 이름 </param>
        /// <param name="_center"> 중심점 </param>
        /// <param name="_scale"> 복셀 크기 </param>
        /// <param name="_mesh"> 메쉬 </param>
        /// <param name="_mat"> 재질 </param>
        /// <param name="_depthIndex"> 뎁스 인덱스 
        ///     * 뎁스 
        /// </param>
        public void InitVoxel(string _name, Vector3 _center, Vector3 _scale, Mesh _mesh, Material _mat, int _depthIndex)
        {
            m_cache = new VoxelCache();

            gameObject.name = _name;
            //m_cache._Rigidbody = gameObject.AddComponent<Rigidbody>();

            m_cache._Mesh = _mesh;
            m_cache._Mat = _mat;

            m_cache._Filter = gameObject.GetComponent<MeshFilter>();
            m_cache._Renderer = gameObject.GetComponent<MeshRenderer>();
            m_cache._BoxCollider = gameObject.GetComponent<BoxCollider>();

            SetMesh(m_cache._Filter, _mesh);
            SetMaterial(m_cache._Renderer, _mat);
            SetColliderScale(m_cache._BoxCollider, _scale);

            SetCenter(_center);

            m_cache.IsCollision = false;

            if(_depthIndex > 0)
            {
                octTree = new Voxel[8];

                for (int i = 0; i < 8; i++)
                {
                    GameObject obj = SetOctChild(_name, _center, _scale, _mesh, _mat, _depthIndex - 1, i);
                    octTree[i] = obj.GetComponent<Voxel>();
                }
            }
        }

        /// <summary>
        /// 이 복셀 리소스의 OctTree 자식 객체를 생성한다.
        /// </summary>
        /// <param name="_name"> 현재 복셀의 이름 </param>
        /// <param name="_center"> 현재 복셀의 중심점 </param>
        /// <param name="_scale"> 현재 복셀의 규모 </param>
        /// <param name="_mesh"> 메쉬 </param>
        /// <param name="_mat"> 재질 </param>
        /// <param name="_depthIndex"> octree 뎁스 인덱스 </param>
        /// <param name="octIndex"> 현재 복셀의 자식 복셀요소 인덱스 </param>
        private GameObject SetOctChild(string _name, Vector3 _center, Vector3 _scale, Mesh _mesh, Material _mat, int _depthIndex, int octIndex)
        {
            // 변경필요
            // 1 중심점
            // 2 변경된 규모

            // oct index 이동방향벡터
            Vector3 octIndexVector = new Vector3(
                octIndex % 2 == 0 ? -1 : 1,
                octIndex / 4 == 0 ? -1 : 1,
                (octIndex / 2) % 2 == 0 ? -1 : 1);

            // oct 중심점 이동규모벡터
            Vector3 octScaleVector = new Vector3(
                _scale.x / 4,
                _scale.y / 4,
                _scale.z / 4 );


            // oct 인덱스 중심점
            Vector3 octMovedCenter = new Vector3(
                _center.x + octScaleVector.x * octIndexVector.x,
                _center.y + octScaleVector.y * octIndexVector.y,
                _center.z + octScaleVector.z * octIndexVector.z );

            // oct 변경 규모
            Vector3 octScale = new Vector3(
                _scale.x / 2,
                _scale.y / 2,
                _scale.z / 2 );

            GameObject obj = VoxelUtil.CreateVoxel($"{_name} {_depthIndex}",
                octMovedCenter, octScale, _mat, _depthIndex);

            return obj;
        }

        #region Setting Method

        /// <summary>
        /// 메쉬 할당
        /// </summary>
        /// <param name="_filter"></param>
        /// <param name="_mesh"></param>
        private void SetMesh(MeshFilter _filter, Mesh _mesh)
        {
            _filter.mesh = _mesh;
        }

        /// <summary>
        /// 재질 할당
        /// </summary>
        /// <param name="_renderer"></param>
        /// <param name="_mat"></param>
        private void SetMaterial(MeshRenderer _renderer, Material _mat)
        {
            _renderer.material = _mat;
        }

        /// <summary>
        /// 충돌체 크기변경 (Box)
        /// </summary>
        /// <param name="_collider"></param>
        /// <param name="_scale"></param>
        private void SetColliderScale(BoxCollider _collider, Vector3 _scale)
        {
            _collider.size = _scale;
        }

        /// <summary>
        /// 복셀 위치 할당
        /// </summary>
        /// <param name="_center"></param>
        private void SetCenter(Vector3 _center)
        {
            transform.position = _center;
        }

        #endregion

        #endregion
    }
}
