using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test
{
    public class VoxelTestManager : MonoBehaviour
    {
        public int testCase;

        /// <summary>
        /// �ü����� root��� voxel octree�� �����ϴ� ���
        /// </summary>
        [System.Serializable]
        public class TestCase1
        {
            public VoxelTestData2 testData;
        }

        /// <summary>
        /// �� ���� ������ root��� voxel octree�� �����ϴ� ���
        /// </summary>
        [System.Serializable]
        public class TestCase2
        {
            [System.Serializable]
            public class Data
            {
                public Transform _base;
                public Vector3 _scale;
                public int _depth;
            }

            public List<Data> targetList;

            public List<Voxelizer2> voxelizerList;

            public void OnTest()
            {
                voxelizerList = new List<Voxelizer2>();

                int index = targetList.Count;
                for (int i = 0; i < index; i++)
                {
                    Data data = targetList[i];

                    Voxelizer2 _vox = VoxelTestManager.CreateVoxelizer($"voxelizer {i}");
                    _vox.ArrangeVoxels(data._base, data._scale, data._depth);

                    voxelizerList.Add(_vox);
                }
            }
        }

        [Header("Case 1 :: �ü����� �߽����� octree�� �����ϴ� ���")]
        public TestCase1 testCase1;


        [Header("Case 2 :: Ư�� �������� �߽����� octree ����Ʈ�� �����ϴ� ���")]
        public TestCase2 testCase2;

        // Start is called before the first frame update
        void Start()
        {
            switch(testCase)
            {
                case 0:
                    testCase1.testData.OnTest();
                    break;

                case 1:
                    testCase2.OnTest();
                    break;
            }
        }

        public static Voxelizer2 CreateVoxelizer(string name)
        {
            GameObject obj = new GameObject(name);
            Voxelizer2 voxelizer = obj.AddComponent<Voxelizer2>();
            return voxelizer;
        }
    }
}
