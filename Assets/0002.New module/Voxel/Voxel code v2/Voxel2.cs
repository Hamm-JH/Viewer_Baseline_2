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
            public float mapHeight;
            public float vCenterHeight;

            public Vector3 center;
            public Vector3 min;
            public Vector3 max;
            

            public Quaternion rotation;
            public Vector3 scale;
            public int totalDepth;
        }

        [System.Serializable]
        public class State
        {
            /// <summary>
            /// ������ �浹������ true
            /// </summary>
            [SerializeField] bool m_bIsCollision;

            /// <summary>
            /// ������ �߰����� ��Ȱ��ȭ�� true
            /// </summary>
            [SerializeField] bool m_bIsDisabled;

            /// <summary>
            /// �𵨿� ������, �浹���� ���� �����϶� true 
            /// </summary>
            [SerializeField] bool m_bIsNear;

            /// <summary>
            /// �𵨿� ����, �浹�� �����϶� true
            /// </summary>
            [SerializeField] bool m_bIsNearCollision;

            /// <summary>
            /// ���� �������� �� �Ʒ��� �����ϴ°�?
            /// </summary>
            [SerializeField] bool m_bIsGround;

            /// <summary>
            /// �浹�� �߻��ߴ°�?
            /// </summary>
            public bool IsCollision { get => m_bIsCollision; set => m_bIsCollision = value; }

            /// <summary>
            /// ��Ȱ��ȭ?
            /// </summary>
            public bool IsDisabled { get => m_bIsDisabled; set => m_bIsDisabled = value; }

            /// <summary>
            /// �����ߴ°�?
            /// </summary>
            public bool IsNear { get => m_bIsNear; set => m_bIsNear = value; }

            /// <summary>
            /// ���� �浹�ߴ°�?
            /// </summary>
            public bool IsNearCollision { get => m_bIsNearCollision; set => m_bIsNearCollision = value; }

            /// <summary>
            /// ������ ���� �Ʒ��� �ִ°�?
            /// </summary>
            public bool IsGround { get => m_bIsGround; set => m_bIsGround = value; }

            public State()
            {
                m_bIsCollision = false;
                m_bIsDisabled = false;
                m_bIsNear = false;
                m_bIsNearCollision = false;
                m_bIsGround = false;
            }
        }

        /// <summary>
        /// 8���� ������
        /// </summary>
        public Vector3[] verts;

        /// <summary>
        /// �ڽ� ������
        /// </summary>
        public Voxel2[] voxels;


        [SerializeField] int m_depth;

        [SerializeField] VoxelController m_controller;
        [SerializeField] Data m_data;
        public State m_state;

        public int Depth { get => m_depth; set => m_depth = value; }

        private bool IsVoxelUnderGround(Vector3 center)
        {
            bool bIsTrue = false;

            float groundHeight = DemoCode.Get3D_To_Height(m_data.center);
            m_data.mapHeight = groundHeight - 4.366352f;
            m_data.vCenterHeight = center.y;

            //GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            //obj.transform.position = new Vector3(
            //    center.x, m_data.mapHeight, center.z
            //    );

            //GameObject obj2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //obj2.transform.position = new Vector3(
            //    center.x, m_data.vCenterHeight, center.z
            //    );

            if (center.y < m_data.mapHeight)
            {
                bIsTrue = true;
            }
            else
            {
                bIsTrue = false;
            }

            return bIsTrue;
        }

        public void InitVoxel(Vector3 center, Quaternion rotation, Vector3 scale, int depth, int totalDepth, VoxelController _controller)
        {
            m_controller = new VoxelController(_controller);
            m_state = new State();

            Depth = depth;

            m_data = new Data();
            m_data.center = center;
            m_data.scale = scale;
            m_data.min = new Vector3(
                center.x - scale.x / 2,
                center.y - scale.y / 2,
                center.z - scale.z / 2);
            
            m_data.max = new Vector3(
                center.x + scale.x / 2,
                center.y + scale.y / 2,
                center.z + scale.z / 2);

            m_data.rotation = rotation;
            m_data.totalDepth = totalDepth;

            //transform.position = m_data.center;

            var coll = gameObject.AddComponent<BoxCollider>();
            coll.center = center;
            coll.size = scale;

            if (IsVoxelUnderGround(m_data.center))
            {
                m_state.IsGround = true;
            }
            else
            {
                m_state.IsGround = false;
            }

            #region �� ��ġ
            verts = new Vector3[8];
            Vector3 _deltaVector = new Vector3(
                scale.x / 2,
                scale.y / 2,
                scale.z / 2);

            float angleY = rotation.eulerAngles.y;

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

            //StartCoroutine(CheckCollision());
        }

        //public IEnumerator CheckCollision()
        //{
        //    yield return new WaitForEndOfFrame();

        //    List<float[]> vertexes = VoxelTestData2.Instance.Vertexes;

        //    int index = vertexes.Count;
        //    for (int i = 0; i < index; i++)
        //    {

        //    }

        //    yield break;
        //}

        //private bool IsIncludeVertex(float[] vertex, Vector3 target)
        //{
        //    bool result = true;



        //    return result;
        //}

        public void CreateChildren()
        {
            StartCoroutine(CreateChildrenVoxel());
        }

        public IEnumerator CreateChildrenVoxel()
        {
            if (m_state.IsDisabled) yield break;

            if (Depth > 0 && m_state.IsGround) yield break;
            //IsNearCollision = false;

            //if (Depth > 3 && IsGround) yield break;

            CreateChildrenVoxels();

            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();

            if (voxels == null) 
            {
                yield break;
            }

            if (Depth < 1)
            {
                yield break;
            }

            m_state.IsNearCollision = false;

            bool bIsAllDetected = true;
            bool bIsSomePartDetected = false;
            // ��� �ڽ��� �浹�� ���
            // �ڽ� ��ü �� �ڽĻ��� disable

            // �Ϻ� �ڽ��� �浹�� ���
            // �浹���� ���� ��ü�鿡 �������� bool�� ������Ʈ

            // ��ȸ�ϸ鼭 �ڽ� ��ü�� ���� �ľ�
            for (int i = 0; i < voxels.Length; i++)
            {
                bool isColl = voxels[i].m_state.IsCollision;

                // ��� �ڽİ�ü�� �浹 ���¸� true�� ��
                bIsAllDetected = bIsAllDetected && isColl;

                // �Ϻ� �ڽİ�ü�� �浹 ���¸� true�� ��
                bIsSomePartDetected = bIsSomePartDetected || isColl;
            }

            //// �Ϻ� �ڽİ�ü�� �浹 ���°� �� ��� 
            //if (bIsSomePartDetected)
            //{
            //    IsNearCollision = false;
            //}

            Debug.Log($"all detected : {bIsAllDetected}");
            Debug.Log($"some detected : {bIsSomePartDetected}");
            // ��ȸ�ϸ鼭 ��� ������ ���� ���� ����
            for (int i = 0; i < voxels.Length; i++)
            {
                // ��� ����� ���
                if (bIsAllDetected)
                {
                    voxels[i].m_state.IsDisabled = true;
                }

                // �Ϻ� ���� && �浹 �������� ���� ��ü�� ���
                if (bIsSomePartDetected && !voxels[i].m_state.IsCollision)
                {
                    voxels[i].m_state.IsNear = true;
                }
                else if (bIsSomePartDetected && voxels[i].m_state.IsCollision)
                {
                    voxels[i].m_state.IsNearCollision = true;
                }
            }

            yield break;
        }

        private void CreateChildrenVoxels()
        {
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

        public void SetVoxelList(ref List<Voxel2> near, ref List<Voxel2> nearCollision)
        {
            if (m_state.IsNear)
            {
                near.Add(this);
                //near[Depth].Add(this);
            }
            
            if (m_state.IsNearCollision)
            {
                nearCollision.Add(this);
            }

            if (voxels == null) return;

            for (int i = 0; i < voxels.Length; i++)
            {
                voxels[i].SetVoxelList(ref near, ref nearCollision);
            }
        }

        private void OnRenderObject()
        {
            if (m_controller == null)
            {
                return;
            }

            //if (Depth >= 4 && IsGround) return;

            if (!m_controller.IsVisualize) return;
            if (!(m_state.IsNear ||
                m_state.IsCollision ||
                m_state.IsNearCollision)) return;

            if (m_controller.Min_vDepth > Depth || m_controller.Max_vDepth < Depth)
            {
                return;
            }

            //if (m_controller.VisualizeDepth < Depth) return;

            TestGLCode.CreateLineMaterial();

            // Apply the line material
            TestGLCode.lineMaterial.SetPass(0);

            GL.PushMatrix();

            // Set transformation matrix for drawing to // match our transform
            GL.MultMatrix(transform.localToWorldMatrix);

            // Draw lines
            GL.Begin(GL.LINES);

            bool bIsRunning = false;

            Color colr = Color.white;
            if (m_controller.VisualizeNear && m_state.IsNear)
            {
                bIsRunning = true;
                colr = Color.blue;
            }
            else if (m_controller.VisualizeNearCollision && m_state.IsNearCollision)
            {
                bIsRunning = true;
                colr = Color.red;
            }
            else if (m_controller.VisualizeCollision && m_state.IsCollision)
            {
                bIsRunning = true;
                colr = Color.cyan;
            }

            if (bIsRunning)
            {
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
            }

            GL.End();
            GL.PopMatrix();
        }
    }
}
