using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Test
{
    public class VoxelTestData2 : MonoBehaviour
    {
        private static VoxelTestData2 instance;
        
        public static VoxelTestData2 Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<VoxelTestData2>() as VoxelTestData2;
                }

                return instance;
            }
        }

        public Transform start;
        public Transform end;

        [SerializeField] Vector3 center;
        [SerializeField] Bounds bound;
        [SerializeField] GameObject bridge;
        [SerializeField] float boundOffset;
        [SerializeField] List<Collider> colliders;
        //[SerializeField] List<float[]> vertexes;

        [SerializeField] Voxelizer2 voxelizer2;

        public Vector3 Center { get => center; set => center = value; }
        public Bounds Bound { get => bound; set => bound = value; }
        public float BoundOffset { get => boundOffset; set => boundOffset = value; }

        public List<Collider> Colliders { get => colliders; set => colliders = value; }
        //public List<float[]> Vertexes { get => vertexes; set => vertexes = value; }

        private void Start()
        {
            //GetCenter();
            //GetBound();
            //voxelizer2.ArrangeVoxels(Bound);
        }

        public void SetResource()
        {
            GetCenter();
            GetBound();
        }

        public void OnArrange()
        {
            voxelizer2.ArrangeVoxels(Bound, bound.center, Quaternion.identity, this);
        }

        public void OnTest()
        {
            GetCenter();
            GetBound();
            voxelizer2.ArrangeVoxels(Bound, bound.center, Quaternion.identity, this);
        }

        private void GetCenter()
        {
            Center = (start.transform.position + end.transform.position) / 2;
            //bridge.transform.position = -Center;
        }

        public void SetCenter()
        {
            bridge.transform.position = -Center;
        }

        private void GetBound()
        {
            Transform[] trs = transform.GetComponentsInChildren<Transform>();
            //Vertexes = new List<float[]>();
            List<Transform> _trs = trs.ToList().FindAll(t => t.GetComponent<MeshRenderer>() != null);


            Vector3 min = default(Vector3);
            Vector3 max = default(Vector3);

            int index = _trs.Count;
            for (int i = 0; i < index; i++)
            {
                //MeshFilter ft;
                //if (trs[i].TryGetComponent<MeshFilter>(out ft))
                //{
                //    Vertexes.AddRange(GetVertices(ft.sharedMesh));
                //}

                MeshRenderer render;
                if (trs[i].TryGetComponent<MeshRenderer>(out render))
                {
                    if (i == 0)
                    {
                        min = render.bounds.min;
                        max = render.bounds.max;
                    }
                    else
                    {
                        Bounds _b = render.bounds;
                        min = new Vector3(
                            min.x < _b.min.x ? min.x : _b.min.x,
                            min.y < _b.min.y ? min.y : _b.min.y,
                            min.z < _b.min.z ? min.z : _b.min.z
                            );

                        max = new Vector3(
                            max.x > _b.max.x ? max.x : _b.max.x,
                            max.y > _b.max.y ? max.y : _b.max.y,
                            max.z > _b.max.z ? max.z : _b.max.z
                            );
                    }
                }
            }

            //Debug.Log($"mesh vertices : {Vertexes.Count}");

            Bounds bound = new Bounds();
            bound.center = (min + max) / 2;
            bound.size = max - min + new Vector3(BoundOffset, BoundOffset, BoundOffset);
            Bound = bound;

            //GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //obj.transform.position = bound.center;
            //obj.transform.localScale = bound.size;

            //voxelizer2.ArrangeVoxels(Bound);
        }

        private List<float[]> GetVertices(Mesh mesh)
        {
            List<float[]> vertices = new List<float[]>();

            foreach(Vector3 vc in mesh.vertices)
            {
                vertices.Add(new float[] { vc.x, vc.y, vc.z });
            }

            return vertices;
        }
    }
}
