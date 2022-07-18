using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test
{
    [System.Serializable]
    public class VoxelController
    {
        [SerializeField] int min_vDepth;
        [SerializeField] int max_vDepth;

        [SerializeField] bool isVisualize;
        [SerializeField] bool visualizeCollision;
        [SerializeField] bool visualizeNear;
        [SerializeField] bool visualizeNearCollision;

        /// <summary>
        /// ���� ��谡 ������ �ϴ°�?
        /// </summary>
        public bool IsVisualize { get => isVisualize; set => isVisualize = value; }

        /// <summary>
        /// �浹 �߻� ��ü��� ǥ���ؾ� �ϴ°�?
        /// </summary>
        public bool VisualizeCollision { get => visualizeCollision; set => visualizeCollision = value; }

        /// <summary>
        /// ���� octree �������� �Ϻ� �浹���� ���� ��ü�� ��踦 ǥ���ؾ� �ϴ°�?
        /// </summary>
        public bool VisualizeNear { get => visualizeNear; set => visualizeNear = value; }

        /// <summary>
        /// ���� octree �������� �浹 �߻��� ��ü�� ��踦 ǥ���ؾ� �ϴ°�?
        /// </summary>
        public bool VisualizeNearCollision { get => visualizeNearCollision; set => visualizeNearCollision = value; }

        /// <summary>
        /// �ּ� ���� ǥ�� ����
        /// </summary>
        public int Min_vDepth { get => min_vDepth; set => min_vDepth = value; }

        /// <summary>
        /// �ִ� ���� ǥ�� ����
        /// </summary>
        public int Max_vDepth { get => max_vDepth; set => max_vDepth = value; }

        public VoxelController()
        {
            IsVisualize = true;
            VisualizeCollision = true;
            VisualizeNear = true;
            VisualizeNearCollision = true;

            Min_vDepth = 0;
            Max_vDepth = 5;
        }

        public VoxelController(VoxelController controller)
        {
            IsVisualize = controller.IsVisualize;
            VisualizeCollision = controller.VisualizeCollision;
            VisualizeNear = controller.VisualizeNear;
            VisualizeNearCollision = controller.VisualizeNearCollision;

            Min_vDepth = controller.Min_vDepth;
            Max_vDepth = controller.Max_vDepth;
        }

        public void Update(VoxelController controller)
        {
            IsVisualize = controller.IsVisualize;
            VisualizeCollision = controller.VisualizeCollision;
            VisualizeNear = controller.VisualizeNear;
            VisualizeNearCollision = controller.VisualizeNearCollision;

            Min_vDepth = controller.Min_vDepth;
            Max_vDepth = controller.Max_vDepth;
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
        /// ������ ȸ�� ����
        /// </summary>
        public Quaternion m_rotation;

        /// <summary>
        /// ��Ʈ�ѷ�
        /// </summary>
        public VoxelController m_controller;

        /// <summary>
        /// ���� �׽�Ʈ ������
        /// </summary>
        public VoxelTestData2 m_data;

        /// <summary>
        /// ���� ��ġ ���� ���� ���� ��ġ�� �ʿ��� �ʱ� ������ �����Ѵ�.
        /// </summary>
        /// <param name="depth">���� ��ġ�� �ִ� Depth</param>
        /// <param name="minVDepth">���� ǥ���� �ּ� Depth</param>
        /// <param name="maxVDepth">���� ǥ���� �ִ� Depth</param>
        public void Prepare(int depth, int minVDepth, int maxVDepth)
        {
            m_controller = new VoxelController();
            m_controller.Min_vDepth = minVDepth;
            m_controller.Max_vDepth = maxVDepth;
            m_depth = depth;
            
        }

        /// <summary>
        /// Case 1 :: �ü����� �߽����� root octree �ϳ��� ����� ���
        /// </summary>
        /// <param name="_bound"></param>
		public void ArrangeVoxels(Bounds _bound, Vector3 center, Quaternion _rotation, VoxelTestData2 _data)
        {
            m_data = _data;

            m_root = new GameObject("root");

            // ù ������ �� ��踦 �������� �����Ѵ�.
            // �߾� ��ġ
            m_center = center;
            //m_center = _bound.center;

            m_rotation = _rotation;

            // ��Ʈ ũ��
            m_scale = _bound.size;

            float max = _bound.size.x;
            max = max < _bound.size.y ? _bound.size.y : max;
            max = max < _bound.size.z ? _bound.size.z : max;

            // ��� ũ�� ������ü�� ����
            m_scale = new Vector3(max, max, max);

            ArrangeVoxels();

            //StartCoroutine(Routine_collSwitch(m_data));
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

        /// <summary>
        /// ���� ��ġ ����
        /// </summary>
        private void ArrangeVoxels()
        {
            GameObject obj = new GameObject("voxel 0");
            m_rootVoxel = obj.AddComponent<Voxel2>();
            m_rootVoxel.InitVoxel(m_center, m_rotation, m_scale, 0, m_depth, m_controller);

            obj.transform.SetParent(m_root.transform);
        }

        /// <summary>
        /// Inspectorâ ������ ����� ����
        /// </summary>
        private void OnValidate()
        {
            UpdateVoxels();
        }

        /// <summary>
        /// Inspector ������ ����� ���� ���� ������ ������Ʈ
        /// </summary>
        public void UpdateVoxels()
        {
            if (m_rootVoxel == null) return;

            m_rootVoxel.UpdateVoxel(m_controller);
        }
    }
}
