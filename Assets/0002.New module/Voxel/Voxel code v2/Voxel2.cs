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

        [SerializeField] VoxelController m_controller;
        [SerializeField] Data m_data;

        public int Depth { get => m_depth; set => m_depth = value; }
        public bool IsCollision { get => m_bIsCollision; set => m_bIsCollision = value; }
        public bool IsDisabled { get => m_bIsDisabled; set => m_bIsDisabled = value; }
        public bool IsNear { get => m_bIsNear; set => m_bIsNear = value; }
        public bool IsNearCollision { get => m_bIsNearCollision; set => m_bIsNearCollision = value; }

        public void InitVoxel(Vector3 center, Vector3 scale, int depth, int totalDepth, VoxelController _controller)
        {
            m_controller = new VoxelController(_controller);
            Depth = depth;
            IsCollision = false;

            m_data = new Data();
            m_data.center = center;
            m_data.scale = scale;
            m_data.totalDepth = totalDepth;

            var coll = gameObject.AddComponent<BoxCollider>();
            coll.center = center;
            coll.size = scale;

            #region 점 배치
            verts = new Vector3[8];
            Vector3 _deltaVector = new Vector3(
                scale.x / 2,
                scale.y / 2,
                scale.z / 2);

            
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
            }
            #endregion

            //#region 자식 복셀 배치
            //if (Depth > 0)
            //{
            //    voxels = new Voxel2[8];

            //    Vector3 _childScale = scale / 2;

            //    Vector3 __deltaVector = _childScale / 2;

            //    for (int i = 0; i < 8; i++)
            //    {
            //        Vector3 _indexVector = new Vector3(
            //        i % 2 == 0 ? -1 : 1,
            //        i / 4 == 0 ? -1 : 1,
            //        (i / 2) % 2 == 0 ? -1 : 1);

            //        Vector3 _newCenter = new Vector3(
            //            center.x + __deltaVector.x * _indexVector.x,
            //            center.y + __deltaVector.y * _indexVector.y,
            //            center.z + __deltaVector.z * _indexVector.z
            //            );

            //        GameObject obj = new GameObject($"{this.name}{i}");
            //        Voxel2 _vox = obj.AddComponent<Voxel2>();

            //        voxels[i] = _vox;
            //        _vox.InitVoxel(_newCenter, _childScale, Depth - 1, totalDepth, _controller);

            //        obj.transform.SetParent(transform);
            //    }
            //}
            //#endregion
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

                    GameObject obj = new GameObject($"{this.name}{i}");
                    Voxel2 _vox = obj.AddComponent<Voxel2>();

                    voxels[i] = _vox;
                    _vox.InitVoxel(_newCenter, _childScale, Depth + 1, m_data.totalDepth, m_controller);

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
