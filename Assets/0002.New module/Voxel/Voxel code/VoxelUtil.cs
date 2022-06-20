using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test
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

    public static class VoxelUtil
    {
        public static Mesh InstantiateMesh(Vector3 _scale, PlacePosition _place = PlacePosition.Center)
        {
            Vector3 pPos = new Vector3(
                _place == PlacePosition.Center ? -(_scale.x / 2) : 0,
                _place == PlacePosition.Center ? -(_scale.y / 2) : 0,
                _place == PlacePosition.Center ? -(_scale.z / 2) : 0);

            Mesh mesh = new Mesh();
            List<Vector3> verts = new List<Vector3>();
            verts.Add(new Vector3(pPos.x + 0,           pPos.y + 0,         pPos.z + 0));
            verts.Add(new Vector3(pPos.x + _scale.x,    pPos.y + 0,         pPos.z + 0));
            verts.Add(new Vector3(pPos.x + 0,           pPos.y + 0,         pPos.z + _scale.z));
            verts.Add(new Vector3(pPos.x + _scale.x,    pPos.y + 0,         pPos.z + _scale.z));

            verts.Add(new Vector3(pPos.x + 0,           pPos.y + _scale.y,  pPos.z + 0));
            verts.Add(new Vector3(pPos.x + _scale.x,    pPos.y + _scale.y,  pPos.z + 0));
            verts.Add(new Vector3(pPos.x + 0,           pPos.y + _scale.y,  pPos.z + _scale.z));
            verts.Add(new Vector3(pPos.x + _scale.x,    pPos.y + _scale.y,  pPos.z + _scale.z));
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

        /// <summary>
        /// 메쉬를 만드는 타입의 
        /// </summary>
        /// <param name="_center"> 복셀 중심점 </param>
        /// <param name="_scale"> 크기 중심점 </param>
        /// <param name="_rMesh"> 메쉬 리소스 </param>
        /// <param name="_rMat"> 재질 리소스 </param>
        /// <param name="_depthIndex"> 분할생성 뎁스 인덱스 (octree) </param>
        /// <returns></returns>
        public static GameObject CreateVoxel(string _name, Vector3 _center, Vector3 _scale, Material _rMat, int _depthIndex)
        {
            GameObject obj = new GameObject();

            Mesh mesh = InstantiateMesh(_scale);
            //Mesh mesh = Object.Instantiate(_rMesh);
            Material mat = Object.Instantiate(_rMat);
            mat.color = SetColor(_depthIndex);

            Voxel voxel = obj.AddComponent<Voxel>();

            voxel.InitVoxel(
                _name,
                _center, _scale, mesh, mat, _depthIndex
                );

            return obj;
        }

        private static Color SetColor(int _depthIndex)
        {
            Color colr = default(Color);

            switch(_depthIndex)
            {
                case 0: colr = Color.black; break;

                case 1: colr = Color.blue; break;

                case 2: colr = Color.red; break;
            }

            return colr;
        }
    }


}
