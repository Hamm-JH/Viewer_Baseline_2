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
        /// 인스턴스 루트
        /// </summary>
        public GameObject m_root;

        /// <summary>
        /// 루트 복셀
        /// </summary>
        public Voxel2 m_rootVoxel;

        /// <summary>
        /// 최고 뎁스 진입점
        /// </summary>
        public int m_depth;

        /// <summary>
        /// 복셀의 루트 크기
        /// </summary>
        public Vector3 m_scale;

        /// <summary>
        /// 복셀의 초기 중심점
        /// </summary>
        public Vector3 m_center;

        /// <summary>
        /// 컨트롤러
        /// </summary>
        public VoxelController m_controller;

        /// <summary>
        /// Case 1 :: 시설물을 중심으로 root octree 하나를 만드는 방법
        /// </summary>
        /// <param name="_bound"></param>
		public void ArrangeVoxels(Bounds _bound)
        {
            m_root = new GameObject("root");

            // 첫 복셀은 이 경계를 기준으로 생성한다.
            m_center = _bound.center;
            m_scale = _bound.size;

            ArrangeVoxels();
        }

        /// <summary>
        /// 특정 지점을 중심으로 octree 하나를 만듬 (Manager 레벨에서 여러 octree를 만듬)
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
