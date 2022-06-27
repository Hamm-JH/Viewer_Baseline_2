using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test
{
    using Management;
    using Mapbox.Map;
    using Mapbox.Unity.Map;
    using Mapbox.Unity.Utilities;
    using Mapbox.Utils;

    //[System.Serializable]
    //public class Phase0_Resource
    //{
    //    public GameObject prefab;
    //    public GameObject instanceObject;
    //    public VoxelTestData2 instance_data;
    //}

    [System.Serializable]
    public class Phase1_2Position
    {
        [SerializeField] private List<GameObject> points;
        [SerializeField] private float distance;

        public List<GameObject> Points { get => points; set => points = value; }
        public float Distance { get => distance; set => distance = value; }
    }

    [System.Serializable]
    public class Phase2_CenterRotation
    {
        [SerializeField] private GameObject centerPoint;
        [SerializeField] private Quaternion rotation;

        public GameObject CenterPoint { get => centerPoint; set => centerPoint = value; }
        public Quaternion Rotation { get => rotation; set => rotation = value; }
    }

    [System.Serializable]
    public class Phase3_Object
    {
        public GameObject prefab;
        public GameObject instanceObject;
        public VoxelTestData2 instance_data;
    }
    
    [System.Serializable]
    public class Phase4_Voxel
    {
        public Voxelizer2 voxelizer2;
    }

    /// <summary>
    /// ��/�浵���� ���� �̵� ��ġ�� �����ϴ� Ŭ����
    /// </summary>
    [System.Serializable]
    public class UnitMovementPosition
    {
        [Header("�߽� ���浵 ��")]
        public Vector2d centerLatLon;
        [Header("���� �Ÿ���ŭ ������ ���浵 ��")]
        public Vector2d targetLatLon;

        [Header("���� �Ÿ��� (1km)")]
        public Vector2d targetKM_xy;
        [Header("1km �Ÿ��� ���浵 ��")]
        public Vector2d targetKM_xy_LatLon;
    }

    public class DemoCode : MonoBehaviour
    {
        public AbstractMap absMap;

        public GameObject test;

        [Header("")]
        [SerializeField] private double lantitude;
        [SerializeField] private double longitude;
        [SerializeField] private float zoom;

        public double Lantitude { get => lantitude; set => lantitude = value; }
        public double Longitude { get => longitude; set => longitude = value; }
        public float Zoom { get => zoom; set => zoom = value; }

        public Phase1_2Position phase1;
        public Phase2_CenterRotation phase2;
        public Phase3_Object phase3;
        public Phase4_Voxel phase4;

        public UnitMovementPosition uMPos_x;
        public UnitMovementPosition uMPos_y;

        private void Update()
        {

            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                //Debug.Log(Vector2d.one);
                
                Vector2d start = new Vector2d(37.845823, 126.767088);
                Vector2d end = new Vector2d(37.843780, 126.767954);

                Vector2d unitVector = new Vector2d(start.x + 0.01f, start.y);

                //Vector2d start = Mapbox.Unity.Utilities.Conversions.LatLonToMeters(new Vector2d(37.845823, 126.767088));
                //Vector2d end = Mapbox.Unity.Utilities.Conversions.LatLonToMeters(new Vector2d(37.843780, 126.767954));

                Vector2d diff = start - end;
                Vector2d diff2 = start - unitVector;

                //Debug.Log(start);
                //Debug.Log(end);
                //Debug.Log(diff);
                //Debug.Log(diff2);

                Vector2d oneKilometer = Vector2d.one * 1000;
                Vector2d oneKilometerToLatLon = Mapbox.Unity.Utilities.Conversions.MetersToLatLon(oneKilometer);

                Debug.Log($"v1 : {oneKilometer}");
                Debug.Log($"v1 to lalo : {oneKilometerToLatLon}");

                Debug.Log($"meter x : {oneKilometer.x}");
                Debug.Log($"meter y : {oneKilometer.y}");

                Debug.Log($"lat x : {oneKilometerToLatLon.x}");
                Debug.Log($"lon y : {oneKilometerToLatLon.y}");

                //Debug.Log(Mapbox.Unity.Utilities.Conversions.MetersToLatLon(Vector2d.one));

                // ���� ���浵���� �󸶳� �Ÿ��� �����°�?
                //Debug.Log(Mapbox.Unity.Utilities.Conversions.LatLonToMeters(diff2));

                // ���浵���� ���� ����
                //Debug.Log(Mapbox.Unity.Utilities.Conversions.LatLonToMeters(diff));
                //Debug.Log(Mapbox.Unity.Utilities.Conversions.LatLonToMeters(diff.x, 0));
                //Debug.Log(Mapbox.Unity.Utilities.Conversions.LatLonToMeters(0, diff.y));

                // ���� ����
                //Debug.Log(Vector2d.Distance(Vector2d.zero, diff));
                //Debug.Log(Vector2d.Distance(start, end));

                //Mapbox.Unity.Utilities.VectorExtensions.
            }
            else if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                GetUnitDistance_Lon_X();
            }
            else if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                GetUnitDistance_Lat_Y();
            }

            // Ư�� ��ġ�� �̵� (����/����)
            if (Input.GetKeyDown(KeyCode.A))
            {
                absMap.UpdateMap(new Vector2d(37.845823, 126.767088), 15);
            }
            // Ư�� ��ġ�� �̵� (����/����)
            if (Input.GetKeyDown(KeyCode.S))
            {
                absMap.UpdateMap(new Vector2d(37.843780, 126.767954), 15);
            }
            // �� ��(������)�� 3D ��ġ Ȯ��
            else if (Input.GetKeyDown(KeyCode.G))
            {
                SetPhase1_SetStartEndPosition();
            }
            // ��ġ �߽���, ���� ����
            else if (Input.GetKeyDown(KeyCode.H))
            {
                SetPhase2_SetCenterPositionRotation();
            }
            // �ü��� ��ġ, ����� ���� ����
            else if (Input.GetKeyDown(KeyCode.J))
            {
                SetPhase3_CreateResource();
                SetPhase3_AfterPhase2_SetResource();
                SetPhase3_SetDemoObject();
            }
            // �ü����� ���� ��ġ
            else if (Input.GetKeyDown(KeyCode.K))
            {
                SetPhase4_SetVoxel();
            }
            else if (Input.GetKeyDown(KeyCode.L))
            {
                SetStartEndDistance();
            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                // pos1, pos2
                Vector3 pos1 = Mapbox.Unity.Utilities.Conversions.GeoToWorldGlobePosition(37.556711, 126.893982, 1);

                Debug.Log($"pos1 : {pos1}");

                test.transform.position = pos1;
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                // pos1, pos2
                Vector3 pos2 = Mapbox.Unity.Utilities.Conversions.GeoToWorldGlobePosition(37.547711, 126.888532, 1);

                Debug.Log($"pos2 : {pos2}");

                test.transform.position = pos2;
            }


        }

        private void __review()
        {
            //if (Input.GetKeyDown(KeyCode.D))
            //{
            //    // ������ -> 3D ��ġ����
            //    Vector3 pos = Mapbox.Unity.Utilities.Conversions.GeoToWorldGlobePosition(Lantitude, Longitude, 1);
            //    Debug.Log($"base pos : {pos}");

            //    // pos1, pos2
            //    Vector3 pos1 = Mapbox.Unity.Utilities.Conversions.GeoToWorldGlobePosition(37.556711, 126.893982, 1);
            //    Vector3 pos2 = Mapbox.Unity.Utilities.Conversions.GeoToWorldGlobePosition(37.547711, 126.888532, 1);



            //    Debug.Log($"pos1 : {pos1}");
            //    Debug.Log($"pos2 : {pos2}");

            //    test.transform.position = pos;
            //}
            //else if (Input.GetKeyDown(KeyCode.F))
            //{
            //    double xTile = Mapbox.Unity.Utilities.Conversions.TileXToNWLongitude(37, 1);

            //    Debug.Log($"xTile : {xTile}");
            //}
            //else if (Input.GetKeyDown(KeyCode.G))
            //{
            //    Vector2d llpos = new Vector2d(Lantitude, Longitude);
            //    Vector2d pos = Mapbox.Unity.Utilities.Conversions.GeoToWorldPosition(llpos, new Vector2d(37.556911, 126.894852), 1);
            //    var gg = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            //    gg.transform.position = new Vector3((float)pos.x, 0, (float)pos.y);
            //}
        }

        /// <summary>
        /// �浵 �������� ���� �Ÿ���ŭ ������ ���浵�� ���Ѵ�.
        /// </summary>
        private void GetUnitDistance_Lon_X()
        {
            if (uMPos_x == null)
            {
                uMPos_x = new UnitMovementPosition();
            }

            // �߽� ���浵 ���� ���Ѵ�.
            uMPos_x.centerLatLon = absMap.CenterLatitudeLongitude;

            // �߽ɿ��� ���� �Ÿ��� 1km�� ���ϱ� ���� x�� 1km ���� �����Ѵ�.
            uMPos_x.targetKM_xy = new Vector2d(1000, 0);
            // 1km���� ��/�浵�� ��ȯ�Ѵ�.
            uMPos_x.targetKM_xy_LatLon = Mapbox.Unity.Utilities.Conversions.MetersToLatLon(uMPos_x.targetKM_xy);

            // ���� ��/�浵��ŭ �̵��� �Ÿ��� �� ���浵�� �Ҵ��Ѵ�.
            uMPos_x.targetLatLon = uMPos_x.centerLatLon + uMPos_x.targetKM_xy_LatLon;

            //Debug.Log($"km x : {targetKM_x.x}");    // 1000
            //Debug.Log($"km y : {targetKM_x.y}");    // 0

            //Debug.Log($"lalo x : {targetKM_x_LatLon.x}");   // 0
            //Debug.Log($"lalo y : {targetKM_x_LatLon.y}");   // 0.00898315284119521

            GameObject obj = SetScenePosition(uMPos_x.targetLatLon.x, uMPos_x.targetLatLon.y, absMap, "posX");
        }

        /// <summary>
        /// ���� �������� ���� �Ÿ���ŭ ������ ���浵�� ���Ѵ�.
        /// </summary>
        private void GetUnitDistance_Lat_Y()
        {
            if (uMPos_y == null)
            {
                uMPos_y = new UnitMovementPosition();
            }

            // �߽� ���浵 ���� ���Ѵ�.
            uMPos_y.centerLatLon = absMap.CenterLatitudeLongitude;

            // �߽ɿ��� ���� �Ÿ��� 1km�� ���ϱ� ���� y�� 1km ���� �����Ѵ�.
            uMPos_y.targetKM_xy = new Vector2d(0, 1000);
            // 1km���� ��/�浵�� ��ȯ�Ѵ�.
            uMPos_y.targetKM_xy_LatLon = Mapbox.Unity.Utilities.Conversions.MetersToLatLon(uMPos_y.targetKM_xy);

            // ���� ��/�浵��ŭ �̵��� �Ÿ��� �� ���浵�� �Ҵ��Ѵ�.
            uMPos_y.targetLatLon = uMPos_y.centerLatLon + uMPos_y.targetKM_xy_LatLon;

            //Debug.Log($"km x : {targetKM_x.x}");    // 0
            //Debug.Log($"km y : {targetKM_x.y}");    // 1000

            //Debug.Log($"lalo x : {targetKM_x_LatLon.x}");   // 0.008983152804392
            //Debug.Log($"lalo y : {targetKM_x_LatLon.y}");   // 0

            GameObject obj = SetScenePosition(uMPos_y.targetLatLon.x, uMPos_y.targetLatLon.y, absMap, "posY");
        }

        private Vector3 GetLatLonToXZ(Vector2d _value )
        {
            Vector3 result = default(Vector3);

            result = Mapbox.Unity.Utilities.Conversions.GeoToWorldPosition(_value.x, _value.y, absMap.CenterMercator, absMap.WorldRelativeScale).ToVector3xz();

            return result;
        }

        #region phase1 : �������� ���浵 -> 3D ��ġ ���Ѵ�.

        private void SetPhase1_SetStartEndPosition()
        {
            phase1.Points = new List<GameObject>();
            //phase1.Points.Add(SetScenePosition(37.556711, 126.893982, absMap, "object 1"));
            //phase1.Points.Add(SetScenePosition(37.547711, 126.888532, absMap, "object 2"));
            phase1.Points.Add(SetScenePosition(37.845823, 126.767088, absMap, "object 1"));
            phase1.Points.Add(SetScenePosition(37.843780, 126.767954, absMap, "object 2"));
        }

        private GameObject SetScenePosition(double lan, double lon, AbstractMap _map, string _name)
        {
            GameObject obj;

            Vector2d llpos = new Vector2d(lan, lon);
            Vector3 pos = GetLatLonToXZ(llpos);
            //Vector3 pos = Mapbox.Unity.Utilities.Conversions.GeoToWorldPosition(llpos.x, llpos.y, absMap.CenterMercator, absMap.WorldRelativeScale).ToVector3xz();
            /*+ new Vector3(0, 3, 0)*/
            obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            obj.transform.position = pos;
            obj.name = _name;

            return obj;
        }

        #endregion

        #region phase2 : ������ ������ ������ �߽� ��ġ // ��ġ ������ ���Ѵ�.

        private void SetPhase2_SetCenterPositionRotation()
        {
            GameObject start = phase1.Points[0];
            GameObject end = phase1.Points[1];

            // �߽��� ���
            Vector3 centerPos = (start.transform.position + end.transform.position) / 2;

            GameObject center = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            center.name = "center";

            center.transform.position = centerPos;
            // ���� ���
            center.transform.LookAt(start.transform);

            // �߽���, ���� �Ҵ�
            phase2.CenterPoint = center;
            phase2.Rotation = center.transform.rotation;
        }

        #endregion

        #region phase3 : �ü��� ��ġ

        private void SetPhase3_CreateResource()
        {
            phase3.instanceObject = Instantiate(phase3.prefab);
            phase3.instance_data = phase3.instanceObject.GetComponent<VoxelTestData2>();
        }

        private void SetPhase3_AfterPhase2_SetResource()
        {
            phase3.instance_data.SetResource();
        }

        private void SetPhase3_SetDemoObject()
        {
            phase1.Distance = Vector3.Distance(phase1.Points[0].transform.position, phase1.Points[1].transform.position);
            //float dist = Vector3.Distance(phase1.Points[0].transform.position, phase1.Points[1].transform.position);

            phase3.instanceObject.transform.position = phase1.Points[0].transform.position;
            phase3.instanceObject.transform.rotation = phase2.Rotation;
            phase3.instanceObject.transform.Rotate(new Vector3(0, 90, 0));
        }
        #endregion

        #region phase4 : octree ���� ��ġ

        private void SetPhase4_SetVoxel()
        {
            GameObject obj = new GameObject("voxelizer2");

            //phase3.instanceObject.transform.rotation;

            phase4.voxelizer2 = obj.AddComponent<Voxelizer2>();

            phase4.voxelizer2.Prepare();
            phase4.voxelizer2.ArrangeVoxels(phase3.instance_data.Bound, phase2.CenterPoint.transform.position, 
                phase3.instanceObject.transform.rotation, phase3.instance_data);
        }

        #endregion

        #region phse5 : �������� �Ÿ� ���ϰ� �ð�ȭ

        private void SetStartEndDistance()
        {
            Transform sTr = phase3.instance_data.start;
            Transform eTr = phase3.instance_data.end;

            // �������� ���浵 ��ȯ
            Vector2d startLaLo = sTr.GetGeoPosition(absMap.CenterMercator, absMap.WorldRelativeScale);
            Vector2d endLaLo = eTr.GetGeoPosition(absMap.CenterMercator, absMap.WorldRelativeScale);
            //Vector2d sGP = Mapbox.Unity.Utilities.Conversions.

            Vector2d diff = startLaLo - endLaLo;

            // ���浵 ����� �Ÿ����
            Vector2d diffMeter = Mapbox.Unity.Utilities.Conversions.LatLonToMeters(diff);

            //Debug.Log($"diff meter : {diffMeter}");

            // ���浵 ��� �Ÿ����� ���� �Ÿ� ���
            double diffDistance = Mathd.Sqrt(diffMeter.x * diffMeter.x + diffMeter.y * diffMeter.y);

            //Debug.Log($"diff distance : {diffDistance}");

            // ������ ������ ���浵�� ���Ѵ�.
            // �� ���浵���� ���Ѵ�.
            // ���浵���� x,y �Ÿ����� ���Ѵ�.
            // mathf.sqrt(x*x + y*y) ������ �����Ѵ�.

            GameObject lineObj = new GameObject("simpleLine");
            SimpleLiner sLiner = lineObj.AddComponent<SimpleLiner>();
            sLiner.SetLine(sTr, eTr, absMap, diffDistance);
            
        }

        #endregion
    }
}
