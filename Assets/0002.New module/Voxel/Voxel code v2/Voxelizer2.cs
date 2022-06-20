using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test
{
    [System.Serializable]
    public class VoxelController
    {
        [SerializeField] int visualizeDepth;

        public int VisualizeDepth { get => visualizeDepth; set => visualizeDepth = value; }

        public VoxelController()
        {
            VisualizeDepth = 0;
        }

        public VoxelController(VoxelController controller)
        {
            VisualizeDepth = controller.VisualizeDepth;
        }

        public void Update(VoxelController controller)
        {
            VisualizeDepth = controller.VisualizeDepth;
        }
    }

    public class Voxelizer2 : MonoBehaviour
    {
        /// <summary>
        /// �ν��Ͻ� ��Ʈ
        /// </summary>
        public GameObject m_root;

        /// <summary>
        /// ��Ʈ ����
        /// </summary>
        public Voxel2 m_rootVoxel;

        /// <summary>
        /// �ְ� ���� ������
        /// </summary>
        public int m_depth;

        /// <summary>
        /// ������ ��Ʈ ũ��
        /// </summary>
        public Vector3 m_scale;

        /// <summary>
        /// ������ �ʱ� �߽���
        /// </summary>
        public Vector3 m_center;

        /// <summary>
        /// ��Ʈ�ѷ�
        /// </summary>
        public VoxelController m_controller;

        /// <summary>
        /// Case 1 :: �ü����� �߽����� root octree �ϳ��� ����� ���
        /// </summary>
        /// <param name="_bound"></param>
		public void ArrangeVoxels(Bounds _bound)
        {
            m_root = new GameObject("root");

            // ù ������ �� ��踦 �������� �����Ѵ�.
            m_center = _bound.center;
            m_scale = _bound.size;

            ArrangeVoxels();
        }

        /// <summary>
        /// Ư�� ������ �߽����� octree �ϳ��� ���� (Manager �������� ���� octree�� ����)
        /// </summary>
        /// <param name="_base"></param>
        /// <param name="_scale"></param>
        /// <param name="_depth"></param>
        public void ArrangeVoxels(Transform _base, Vector3 _scale, int _depth)
        {
            m_controller = new VoxelController();

            m_root = new GameObject("root");

            m_center = _base.position;
            m_scale = _scale;

            m_depth = _depth;

            ArrangeVoxels();
        }

        private void ArrangeVoxels()
        {
            GameObject obj = new GameObject("voxel 1");
            m_rootVoxel = obj.AddComponent<Voxel2>();
            m_rootVoxel.InitVoxel(m_center, m_scale, m_depth, m_depth, m_controller);

            obj.transform.SetParent(m_root.transform);
        }



        private void OnValidate()
        {
            UpdateVoxels();
        }

        public void UpdateVoxels()
        {
            if (m_rootVoxel == null) return;

            m_rootVoxel.UpdateVoxel(m_controller);
        }
    }
}
