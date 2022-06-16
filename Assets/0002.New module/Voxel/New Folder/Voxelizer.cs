using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test
{
    public class Voxelizer : MonoBehaviour
    {
        public enum PlacePosition
        {
            /// <summary>
            /// 원점을 배치기준으로 잡는 방법
            /// </summary>
            Original,

            /// <summary>
            /// 중심점을 배치기준으로 잡는 방법
            /// </summary>
            Center,
        }

        /// <summary>
        /// 배치형태
        /// </summary>
        public PlacePosition _PlacePosition;

        /// <summary>
        /// 루트 복셀 크기
        /// </summary>
        public Vector3 _rootScale;

        /// <summary>
        /// 베이스 Material
        /// </summary>
        public Material m_mat;

        /// <summary>
        /// 베이스 메쉬
        /// </summary>
        public Mesh m_mesh;

        /// <summary>
        /// 뎁스 인덱스
        /// </summary>
        public int m_depthIndex;

        public GameObject m_root;

        private void Start()
        {
            m_mesh = InstantiateMesh(_rootScale, _PlacePosition);

            m_root = new GameObject("voxel root");

            for (int x = 0; x < 12; x++)
            {
                for (int y = 0; y < 12; y++)
                {
                    for (int z = 0; z < 12; z++)
                    {
                        GameObject voxel = CreateVoxel(new int[3] { x, y, z }, _rootScale, m_mesh, m_mat);
                        voxel.transform.SetParent(m_root.transform);
                    }
                }
            }
        }

        public GameObject CreateVoxel(int[] _indexes, Vector3 _scale, Mesh _instantiatedMesh, Material _mat)
        {
            Vector3 center = new Vector3(
                _scale.x * _indexes[0],
                _scale.y * _indexes[1],
                _scale.z * _indexes[2]
                );

            GameObject obj = new GameObject();

            Mesh mesh = Instantiate<Mesh>(_instantiatedMesh);
            Voxel voxel = obj.AddComponent<Voxel>();

            voxel.InitVoxel(
                $"Voxel {_indexes[0]},{_indexes[1]},{_indexes[2]}",
                center, _scale, mesh, _mat, m_depthIndex);

            return obj;
        }

        // 복셀 배치
        public Mesh InstantiateMesh(Vector3 _scale, PlacePosition _place)
        {
            Vector3 pPos = new Vector3(
                _place == PlacePosition.Center ? -(_scale.x / 2) : 0,
                _place == PlacePosition.Center ? -(_scale.y / 2) : 0,
                _place == PlacePosition.Center ? -(_scale.z / 2) : 0);

            Mesh mesh = new Mesh();
            List<Vector3> verts = new List<Vector3>();
            verts.Add(new Vector3(pPos.x + 0        , pPos.y + 0        , pPos.z + 0));
            verts.Add(new Vector3(pPos.x + _scale.x , pPos.y + 0        , pPos.z + 0));
            verts.Add(new Vector3(pPos.x + 0        , pPos.y + 0        , pPos.z + _scale.z));
            verts.Add(new Vector3(pPos.x + _scale.x , pPos.y + 0        , pPos.z + _scale.z));

            verts.Add(new Vector3(pPos.x + 0        , pPos.y + _scale.y , pPos.z + 0));
            verts.Add(new Vector3(pPos.x +_scale.x  , pPos.y + _scale.y , pPos.z + 0));
            verts.Add(new Vector3(pPos.x + 0        , pPos.y + _scale.y , pPos.z + _scale.z));
            verts.Add(new Vector3(pPos.x +_scale.x  , pPos.y + _scale.y , pPos.z + _scale.z));
            mesh.vertices = verts.ToArray();

            List<int> tris = new List<int>();
            tris.AddRange(new List<int>() { 0, 1, 2 });
            tris.AddRange(new List<int>() { 3, 2, 1 });

            tris.AddRange(new List<int>() { 0, 4, 1 });
            tris.AddRange(new List<int>() { 5, 1, 4 });

            tris.AddRange(new List<int>() { 1, 5, 3 });
            tris.AddRange(new List<int>() { 7, 3, 5 });

            tris.AddRange(new List<int>() { 7, 6, 3 });
            tris.AddRange(new List<int>() { 2, 3, 6 });

            tris.AddRange(new List<int>() { 6, 4, 2 });
            tris.AddRange(new List<int>() { 0, 2, 4 });

            tris.AddRange(new List<int>() { 4, 6, 5 });
            tris.AddRange(new List<int>() { 7, 5, 6 });
            mesh.triangles = tris.ToArray();

            return mesh;
        }
    }
}
