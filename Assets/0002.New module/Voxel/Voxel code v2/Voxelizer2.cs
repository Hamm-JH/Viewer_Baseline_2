using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test
{
    public class Voxelizer2 : MonoBehaviour
    {
        public Vector3 m_firstArea;

        public Vector3 m_firstScale;
        public Vector3 m_firstCenter;
        public Vector3 m_firstMin;
        public Vector3 m_firstMax;

        /// <summary>
        /// 복셀 뎁스
        /// </summary>
        public int depth;

        /// <summary>
        /// 인스턴스 루트
        /// </summary>
        public GameObject m_root;

        /// <summary>
        /// 루트 복셀
        /// </summary>
        public Voxel2 m_rootVoxel;

        /// <summary>
        /// 컨트롤러
        /// </summary>
        public Voxel2.Controller m_controller;

		public void ArrangeVoxels(Bounds _bound)
        {
            m_root = new GameObject("root");

            // 첫 복셀은 이 경계를 기준으로 생성한다.
            m_firstScale = _bound.size;
            m_firstCenter = _bound.center;
            m_firstMin = _bound.min;
            m_firstMax = _bound.max;

            GameObject obj = new GameObject("voxel 1");
            m_rootVoxel = obj.AddComponent<Voxel2>();
            m_rootVoxel.InitVoxel(m_firstCenter, m_firstScale, depth, m_controller);

            obj.transform.SetParent(m_root.transform);
        }

        public void UpdateVoxels()
        {

        }

        private void OnValidate()
        {
            UpdateVoxels();
        }
    }
}
