using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test
{
    public enum PlacePosition
    {
        /// <summary>
        /// ������ ��ġ�������� ��� ���
        /// </summary>
        Original,

        /// <summary>
        /// �߽����� ��ġ�������� ��� ���
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
        /// �޽��� ����� Ÿ���� 
        /// </summary>
        /// <param name="_center"> ���� �߽��� </param>
        /// <param name="_scale"> ũ�� �߽��� </param>
        /// <param name="_rMesh"> �޽� ���ҽ� </param>
        /// <param name="_rMat"> ���� ���ҽ� </param>
        /// <param name="_depthIndex"> ���һ��� ���� �ε��� (octree) </param>
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
