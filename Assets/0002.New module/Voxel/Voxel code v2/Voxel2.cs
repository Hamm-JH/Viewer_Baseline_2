using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test
{
    public class Voxel2 : MonoBehaviour
    {
        [System.Serializable]
        public class Data
        {
            public Vector3 center;
            public Quaternion rotation;
            public Vector3 scale;
            public int totalDepth;
        }

        /// <summary>
        /// 8개의 꼭지점
        /// </summary>
        public Vector3[] verts;

        /// <summary>
        /// 자식 복셀들
        /// </summary>
        public Voxel2[] voxels;


        [SerializeField] int m_depth;
        /// <summary>
        /// 복셀에 충돌했을때 true
        /// </summary>
        [SerializeField] bool m_bIsCollision;

        /// <summary>
        /// 복셀이 추가생성 비활성화시 true
        /// </summary>
        [SerializeField] bool m_bIsDisabled;

        /// <summary>
        /// 모델에 근접한, 충돌하지 않은 복셀일때 true 
        /// </summary>
        [SerializeField] bool m_bIsNear;

        /// <summary>
        /// 모델에 접한, 충돌한 복셀일때 true
        /// </summary>
        [SerializeField] bool m_bIsNearCollision;

        /// <summary>
        /// 모델이 지리정보 고도 아래에 존재하는가?
        /// </summary>
        [SerializeField] bool m_bIsGround;

        [SerializeField] VoxelController m_controller;
        [SerializeField] Data m_data;

        public int Depth { get => m_depth; set => m_depth = value; }

        /// <summary>
        /// 충돌이 발생했는가?
        /// </summary>
        public bool IsCollision { get => m_bIsCollision; set => m_bIsCollision = value; }

        /// <summary>
        /// 비활성화?
        /// </summary>
        public bool IsDisabled { get => m_bIsDisabled; set => m_bIsDisabled = value; }

        /// <summary>
        /// 근접했는가?
        /// </summary>
        public bool IsNear { get => m_bIsNear; set => m_bIsNear = value; }

        /// <summary>
        /// 근접 충돌했는가?
        /// </summary>
        public bool IsNearCollision { get => m_bIsNearCollision; set => m_bIsNearCollision = value; }

        /// <summary>
        /// 복셀이 지면 아래에 있는가?
        /// </summary>
        public bool IsGround { get => m_bIsGround; set => m_bIsGround = value; }

        public void InitVoxel(Vector3 center, Quaternion rotation, Vector3 scale, int depth, int totalDepth, VoxelController _controller)
        {
            m_controller = new VoxelController(_controller);
            Depth = depth;
            IsCollision = false;

            m_data = new Data();
            m_data.center = center;
            m_data.rotation = rotation;
            m_data.scale = scale;
            m_data.totalDepth = totalDepth;

            var coll = gameObject.AddComponent<BoxCollider>();
            coll.center = center;
            coll.size = scale;

            float groundHeight = DemoCode.Get3D_To_Height(m_data.center);
            if (groundHeight <= 0)
            {
                IsGround = true;
            }
            else
            {
                IsGround = false;
            }

            #region 점 배치
            verts = new Vector3[8];
            Vector3 _deltaVector = new Vector3(
                scale.x / 2,
                scale.y / 2,
                scale.z / 2);

            float angleY = rotation.eulerAngles.y;

            Debug.Log($"cos y : {Mathf.Cos(angleY)}");
            Debug.Log($"sin y : {Mathf.Sin(angleY)}");

            for (int i = 0; i < 8; i++)
            {
                Vector3 _indexVector = new Vector3(
                    i % 2 == 0 ? -1 : 1,
                    i / 4 == 0 ? -1 : 1,
                    (i / 2) % 2 == 0 ? -1 : 1 );

                verts[i] = new Vector3(
                    center.x + _deltaVector.x * _indexVector.x,
                    center.y + _deltaVector.y * _indexVector.y,
                    center.z + _deltaVector.z * _indexVector.z);

                verts[i] = SetRotate(verts[i], m_data.center, angleY);
            }
            #endregion
        }

        public void CreateChildren()
        {
            //if (Depth < m_data.totalDepth)
            //{
            //    voxels = new Voxel2[8];

            //    Vector3 _childScale = m_data.scale / 2;

            //    Vector3 __deltaVector = _childScale / 2;

            //    for (int i = 0; i < 8; i++)
            //    {
            //        Vector3 _indexVector = new Vector3(
            //        i % 2 == 0 ? -1 : 1,
            //        i / 4 == 0 ? -1 : 1,
            //        (i / 2) % 2 == 0 ? -1 : 1);

            //        Vector3 _newCenter = new Vector3(
            //            m_data.center.x + __deltaVector.x * _indexVector.x,
            //            m_data.center.y + __deltaVector.y * _indexVector.y,
            //            m_data.center.z + __deltaVector.z * _indexVector.z
            //            );

            //        GameObject obj = new GameObject($"{this.name}{i}");
            //        Voxel2 _vox = obj.AddComponent<Voxel2>();

            //        voxels[i] = _vox;
            //        _vox.InitVoxel(_newCenter, _childScale, Depth + 1, m_data.totalDepth, m_controller);

            //        obj.transform.SetParent(transform);
            //    }
            //}
            // frame 하나 보내고 충돌상태 확인

            //if (!IsGround) return;

            StartCoroutine(CreateChildrenVoxel());
        }

        public IEnumerator CreateChildrenVoxel()
        {
            if (IsDisabled) yield break;

            if (Depth < m_data.totalDepth)
            {
                voxels = new Voxel2[8];

                Vector3 _childScale = m_data.scale / 2;

                Vector3 __deltaVector = _childScale / 2;

                float angleY = m_data.rotation.eulerAngles.y;

                for (int i = 0; i < 8; i++)
                {
                    Vector3 _indexVector = new Vector3(
                    i % 2 == 0 ? -1 : 1,
                    i / 4 == 0 ? -1 : 1,
                    (i / 2) % 2 == 0 ? -1 : 1);

                    Vector3 _newCenter = new Vector3(
                        m_data.center.x + __deltaVector.x * _indexVector.x,
                        m_data.center.y + __deltaVector.y * _indexVector.y,
                        m_data.center.z + __deltaVector.z * _indexVector.z
                        );

                    _newCenter = SetRotate(_newCenter, m_data.center, angleY);

                    GameObject obj = new GameObject($"{this.name}{i}");
                    Voxel2 _vox = obj.AddComponent<Voxel2>();

                    voxels[i] = _vox;
                    _vox.InitVoxel(_newCenter, m_data.rotation, _childScale, Depth + 1, m_data.totalDepth, m_controller);

                    obj.transform.SetParent(transform);
                }
            }

            yield return new WaitForEndOfFrame();

            if (voxels == null) 
            {
                yield break;
            }

            if (Depth < 2)
            {
                yield break;
            }

            bool bIsAllDetected = true;
            bool bIsSomePartDetected = false;
            // 모든 자식이 충돌한 경우
            // 자식 객체 새 자식생성 disable

            // 일부 자식이 충돌한 경우
            // 충돌하지 않은 객체들에 근접상태 bool값 업데이트

            // 순회하면서 자식 객체의 상태 파악
            for (int i = 0; i < voxels.Length; i++)
            {
                bool isColl = voxels[i].IsCollision;

                bIsAllDetected = bIsAllDetected && isColl;
                bIsSomePartDetected = bIsSomePartDetected || isColl;
            }

            Debug.Log($"all detected : {bIsAllDetected}");
            Debug.Log($"some detected : {bIsSomePartDetected}");
            // 순회하면서 결과 정보에 따라 변수 제어
            for (int i = 0; i < voxels.Length; i++)
            {
                // 모두 검출된 경우
                if (bIsAllDetected)
                {
                    voxels[i].IsDisabled = true;
                }

                // 일부 검출 && 충돌 감지되지 않은 개체의 경우
                if (bIsSomePartDetected && !voxels[i].IsCollision)
                {
                    voxels[i].IsNear = true;
                }
                else if (bIsSomePartDetected && voxels[i].IsCollision)
                {
                    voxels[i].IsNearCollision = true;
                }
            }

            yield break;
        }

        private Vector3 SetRotate(Vector3 target, Vector3 center, float angleY)
        {
            //return target;

            //angleY += 90;
            //angleY = 40;
            //angleY = 3.44f;
            angleY = 90 - angleY;

            //Mathf.Deg2Rad
            Vector3 result = default(Vector3);

            float x = (target.x - center.x) * Mathf.Cos(angleY * Mathf.Deg2Rad)
                    - (target.z - center.z) * Mathf.Sin(angleY * Mathf.Deg2Rad)
                    + center.x;

            float z = (target.x - center.x) * Mathf.Sin(angleY * Mathf.Deg2Rad)
                    + (target.z - center.z) * Mathf.Cos(angleY * Mathf.Deg2Rad)
                    + center.z;

            result = new Vector3(x, target.y, z);

            return result;
        }

        public void UpdateVoxel(VoxelController _controller)
        {
            m_controller.Update(_controller);

            if (voxels == null) return;

            int index = voxels.Length;
            for (int i = 0; i < index; i++)
            {
                voxels[i].UpdateVoxel(_controller);
            }
        }

        private void OnRenderObject()
        {
            if (m_controller == null)
            {
                return;
            }

            if (!m_controller.IsVisualize) return;
            if (!(IsNear ||
                IsCollision ||
                IsNearCollision)) return;

            if (m_controller.VisualizeDepth < Depth) return;
            //if (Depth != 0) return;
            //if (Depth == 1) return;
            //if (Depth == 2) return;

            TestGLCode.CreateLineMaterial();
            // Apply the line material
            TestGLCode.lineMaterial.SetPass(0);

            GL.PushMatrix();
            // Set transformation matrix for drawing to
            // match our transform
            GL.MultMatrix(transform.localToWorldMatrix);

            // Draw lines
            GL.Begin(GL.LINES);
            

            Color colr = Color.white;
            if (m_controller.VisualizeNear && IsNear)
            {
                colr = Color.blue;
            }
            else if (m_controller.VisualizeNearCollision && IsNearCollision)
            {
                colr = Color.red;
            }
            else if (m_controller.VisualizeCollision && IsCollision)
            {
                colr = Color.cyan;
            }
            //if (m_controller.IsVisualize)
            //{
            //}

            GL.Color(colr);

            GL.Vertex(verts[0]); GL.Vertex(verts[1]);
            GL.Vertex(verts[1]); GL.Vertex(verts[3]);
            GL.Vertex(verts[3]); GL.Vertex(verts[2]);
            GL.Vertex(verts[2]); GL.Vertex(verts[0]);

            GL.Vertex(verts[4]); GL.Vertex(verts[5]);
            GL.Vertex(verts[5]); GL.Vertex(verts[7]);
            GL.Vertex(verts[7]); GL.Vertex(verts[6]);
            GL.Vertex(verts[6]); GL.Vertex(verts[4]);

            GL.Vertex(verts[0]); GL.Vertex(verts[4]);
            GL.Vertex(verts[1]); GL.Vertex(verts[5]);
            GL.Vertex(verts[2]); GL.Vertex(verts[6]);
            GL.Vertex(verts[3]); GL.Vertex(verts[7]);

            GL.End();
            GL.PopMatrix();
        }
    }
}
