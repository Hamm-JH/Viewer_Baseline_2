using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test
{
    public class Voxel2 : MonoBehaviour
    {
        [System.Serializable]
        public class Controller
        {
            [SerializeField] int visualizeDepth;
            private Controller controller;


            public int VisualizeDepth { get => visualizeDepth; set => visualizeDepth = value; }

            public Controller(Controller controller)
            {
                this.controller = controller;
            }

        }

        public Vector3[] verts;

        public Voxel2[] voxels;

        [SerializeField] int m_depth;
        [SerializeField] Controller m_controller;

        public int Depth { get => m_depth; set => m_depth = value; }

        public void InitVoxel(Vector3 _center, Vector3 _scale, int depth, Controller _controller)
        {
            Depth = depth;
            m_controller = new Controller(_controller);
            
            verts = new Vector3[8];

            Vector3 _deltaVector = new Vector3(
                _scale.x / 2,
                _scale.y / 2,
                _scale.z / 2);


            for (int i = 0; i < 8; i++)
            {
                Vector3 _indexVector = new Vector3(
                    i % 2 == 0 ? -1 : 1,
                    i / 4 == 0 ? -1 : 1,
                    (i / 2) % 2 == 0 ? -1 : 1 );

                verts[i] = new Vector3(
                    _center.x + _deltaVector.x * _indexVector.x,
                    _center.y + _deltaVector.y * _indexVector.y,
                    _center.z + _deltaVector.z * _indexVector.z);
            }

            if (depth >= 0)
            {
                voxels = new Voxel2[8];

                Vector3 _childScale = _scale / 2;

                Vector3 __deltaVector = _childScale / 2;

                for (int i = 0; i < 8; i++)
                {
                    Vector3 _indexVector = new Vector3(
                    i % 2 == 0 ? -1 : 1,
                    i / 4 == 0 ? -1 : 1,
                    (i / 2) % 2 == 0 ? -1 : 1);

                    Vector3 _newCenter = new Vector3(
                        _center.x + __deltaVector.x * _indexVector.x,
                        _center.y + __deltaVector.y * _indexVector.y,
                        _center.z + __deltaVector.z * _indexVector.z
                        );

                    GameObject obj = new GameObject($"voxel {i}");
                    Voxel2 _vox = obj.AddComponent<Voxel2>();

                    voxels[i] = _vox;
                    _vox.InitVoxel(_newCenter, _childScale, depth - 1, _controller);

                    obj.transform.SetParent(transform);
                }
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
