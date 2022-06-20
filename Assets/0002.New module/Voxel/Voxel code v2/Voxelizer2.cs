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
        /// ���� ����
        /// </summary>
        public int depth;

        /// <summary>
        /// �ν��Ͻ� ��Ʈ
        /// </summary>
        public GameObject m_root;

        /// <summary>
        /// ��Ʈ ����
        /// </summary>
        public Voxel2 m_rootVoxel;

        /// <summary>
        /// ��Ʈ�ѷ�
        /// </summary>
        public Voxel2.Controller m_controller;

		public void ArrangeVoxels(Bounds _bound)
        {
            m_root = new GameObject("root");

            // ù ������ �� ��踦 �������� �����Ѵ�.
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
