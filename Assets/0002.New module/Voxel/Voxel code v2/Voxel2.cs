using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test
{
    public class Voxel2 : MonoBehaviour
    {
        /// <summary>
        /// 8개의 꼭지점
        /// </summary>
        public Vector3[] verts;

        /// <summary>
        /// 자식 복셀들
        /// </summary>
        public Voxel2[] voxels;


        [SerializeField] int m_depth;
        [SerializeField] VoxelController m_controller;

        public int Depth { get => m_depth; set => m_depth = value; }

        public void InitVoxel(Vector3 center, Vector3 scale, int depth, int totalDepth, VoxelController _controller)
        {
            m_controller = new VoxelController(_controller);
            Depth = depth;
            
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

            if (Depth > 0)
            {
                voxels = new Voxel2[8];

                Vector3 _childScale = scale / 2;

                Vector3 __deltaVector = _childScale / 2;

                for (int i = 0; i < 8; i++)
                {
                    Vector3 _indexVector = new Vector3(
                    i % 2 == 0 ? -1 : 1,
                    i / 4 == 0 ? -1 : 1,
                    (i / 2) % 2 == 0 ? -1 : 1);

                    Vector3 _newCenter = new Vector3(
                        center.x + __deltaVector.x * _indexVector.x,
                        center.y + __deltaVector.y * _indexVector.y,
                        center.z + __deltaVector.z * _indexVector.z
                        );

                    GameObject obj = new GameObject($"voxel {i}");
                    Voxel2 _vox = obj.AddComponent<Voxel2>();

                    voxels[i] = _vox;
                    _vox.InitVoxel(_newCenter, _childScale, Depth - 1, totalDepth, _controller);

                    obj.transform.SetParent(transform);
                }
            }
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

            if (Depth < m_controller.VisualizeDepth)
            {
                return;
            }

            TestGLCode.CreateLineMaterial();
            // Apply the line material
            TestGLCode.lineMaterial.SetPass(0);

            GL.PushMatrix();
            // Set transformation matrix for drawing to
            // match our transform
            GL.MultMatrix(transform.localToWorldMatrix);

            // Draw lines
            GL.Begin(GL.LINES);

            GL.Color(Color.green);

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
