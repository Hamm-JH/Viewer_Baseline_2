using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test
{
    public class Voxelizer : MonoBehaviour
    {
        /// <summary>
        /// 배치형태
        /// </summary>
        public PlacePosition _PlacePosition;

        /// <summary>
        /// 루트 복셀 크기
        /// </summary>
        public Vector3 _rootScale;


        public Vector3 _InitializeIndex;

        /// <summary>
        /// 베이스 Material
        /// </summary>
        public Material m_mat;

        /// <summary>
        /// 뎁스 인덱스
        /// </summary>
        public int m_depthIndex;

        public GameObject m_root;

        private void Start()
        {
            ArrangeVoxels();
        }

        public void ArrangeVoxels()
        {
            m_root = new GameObject("voxel root");

            for (int x = 0; x < _InitializeIndex.x; x++)
            {
                for (int y = 0; y < _InitializeIndex.y; y++)
                {
                    for (int z = 0; z < _InitializeIndex.z; z++)
                    {
                        GameObject voxel = CreateVoxel(new int[3] { x, y, z }, _rootScale, m_mat, m_depthIndex);
                        voxel.transform.SetParent(m_root.transform);
                    }
                }
            }
        }

        /// <summary>
        /// 1. Init :: 복셀을 생성한다.
        /// </summary>
        /// <param name="_indexes"> 배열 인덱스 번호 </param>
        /// <param name="_scale"> 복셀 기본 크기 </param>
        /// <param name="_iMesh"> 생성된 메쉬 </param>
        /// <param name="_mat"> 기본 재질 </param>
        /// <param name="_depthIndex"> 뎁스 </param>
        /// <returns></returns>
        public GameObject CreateVoxel(int[] _indexes, Vector3 _scale, Material _mat, int _depthIndex)
        {
            Vector3 center = new Vector3(
                _scale.x * _indexes[0],
                _scale.y * _indexes[1],
                _scale.z * _indexes[2]
                );

            GameObject _obj = VoxelUtil.CreateVoxel($"Voxel {_indexes[0]},{_indexes[1]},{_indexes[2]} // Depth {_depthIndex}",
                center, _scale, _mat, _depthIndex);

            return _obj;
        }
    }
}
