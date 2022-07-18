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
        /// 복셀 경계가 보여야 하는가?
        /// </summary>
        public bool IsVisualize { get => isVisualize; set => isVisualize = value; }

        /// <summary>
        /// 충돌 발생 개체경계 표시해야 하는가?
        /// </summary>
        public bool VisualizeCollision { get => visualizeCollision; set => visualizeCollision = value; }

        /// <summary>
        /// 동일 octree 레벨에서 일부 충돌되지 않은 객체의 경계를 표시해야 하는가?
        /// </summary>
        public bool VisualizeNear { get => visualizeNear; set => visualizeNear = value; }

        /// <summary>
        /// 동일 octree 레벨에서 충돌 발생한 객체의 경계를 표시해야 하는가?
        /// </summary>
        public bool VisualizeNearCollision { get => visualizeNearCollision; set => visualizeNearCollision = value; }

        /// <summary>
        /// 최소 복셀 표시 레벨
        /// </summary>
        public int Min_vDepth { get => min_vDepth; set => min_vDepth = value; }

        /// <summary>
        /// 최대 복셀 표시 레벨
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
        /// 복셀의 회전 각도
        /// </summary>
        public Quaternion m_rotation;

        /// <summary>
        /// 컨트롤러
        /// </summary>
        public VoxelController m_controller;

        /// <summary>
        /// 복셀 테스트 데이터
        /// </summary>
        public VoxelTestData2 m_data;

        /// <summary>
        /// 복셀 배치 시작 전에 복셀 배치에 필요한 초기 변수를 설정한다.
        /// </summary>
        /// <param name="depth">복셀 배치의 최대 Depth</param>
        /// <param name="minVDepth">복셀 표현의 최소 Depth</param>
        /// <param name="maxVDepth">복셀 표현의 최대 Depth</param>
        public void Prepare(int depth, int minVDepth, int maxVDepth)
        {
            m_controller = new VoxelController();
            m_controller.Min_vDepth = minVDepth;
            m_controller.Max_vDepth = maxVDepth;
            m_depth = depth;
            
        }

        /// <summary>
        /// Case 1 :: 시설물을 중심으로 root octree 하나를 만드는 방법
        /// </summary>
        /// <param name="_bound"></param>
		public void ArrangeVoxels(Bounds _bound, Vector3 center, Quaternion _rotation, VoxelTestData2 _data)
        {
            m_data = _data;

            m_root = new GameObject("root");

            // 첫 복셀은 이 경계를 기준으로 생성한다.
            // 중앙 위치
            m_center = center;
            //m_center = _bound.center;

            m_rotation = _rotation;

            // 루트 크기
            m_scale = _bound.size;

            float max = _bound.size.x;
            max = max < _bound.size.y ? _bound.size.y : max;
            max = max < _bound.size.z ? _bound.size.z : max;

            // 경계 크기 정육면체로 변경
            m_scale = new Vector3(max, max, max);

            ArrangeVoxels();

            //StartCoroutine(Routine_collSwitch(m_data));
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

        /// <summary>
        /// 복셀 배치 시작
        /// </summary>
        private void ArrangeVoxels()
        {
            GameObject obj = new GameObject("voxel 0");
            m_rootVoxel = obj.AddComponent<Voxel2>();
            m_rootVoxel.InitVoxel(m_center, m_rotation, m_scale, 0, m_depth, m_controller);

            obj.transform.SetParent(m_root.transform);
        }

        /// <summary>
        /// Inspector창 변수값 변경시 실행
        /// </summary>
        private void OnValidate()
        {
            UpdateVoxels();
        }

        /// <summary>
        /// Inspector 변수값 변경시 복셀 내부 데이터 업데이트
        /// </summary>
        public void UpdateVoxels()
        {
            if (m_rootVoxel == null) return;

            m_rootVoxel.UpdateVoxel(m_controller);
        }
    }
}
