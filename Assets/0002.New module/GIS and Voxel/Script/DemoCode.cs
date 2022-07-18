using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test
{
    using Management;
    using Mapbox.Map;
    using Mapbox.Unity.Map;
    using Mapbox.Unity.MeshGeneration.Data;
    using Mapbox.Unity.Utilities;
    using Mapbox.Utils;

    /// <summary>
    /// �ü����� ��ġ�� ����, ������ �Ҵ��Ѵ�.
    /// </summary>
    [System.Serializable]
    public class Phase1_2Position
    {
        /// <summary>
        /// ����, ���� �Ҵ� ����Ʈ
        /// </summary>
        [SerializeField] private List<GameObject> points;

        /// <summary>
        /// ��,������ �Ÿ�
        /// </summary>
        [SerializeField] private float distance;

        public List<GameObject> Points { get => points; set => points = value; }
        public float Distance { get => distance; set => distance = value; }
    }

    /// <summary>
    /// ��,���� ������ �߽� ��ġ�� ���Ѵ�
    /// </summary>
    [System.Serializable]
    public class Phase2_CenterRotation
    {
        /// <summary>
        /// �߽� ��ġ ��ü
        /// </summary>
        [SerializeField] private GameObject centerPoint;

        /// <summary>
        /// �������� �������� ȸ������
        /// </summary>
        [SerializeField] private Quaternion rotation;

        public GameObject CenterPoint { get => centerPoint; set => centerPoint = value; }
        public Quaternion Rotation { get => rotation; set => rotation = value; }
    }

    /// <summary>
    /// ��ġ�� ������ ��ü�� ������ ��ü�� ������ Ŭ����
    /// </summary>
    [System.Serializable]
    public class Phase3_Object
    {
        /// <summary>
        /// �ü��� ������
        /// </summary>
        public GameObject prefab;

        /// <summary>
        /// ���������� ������ ��ü
        /// </summary>
        public GameObject instanceObject;

        /// <summary>
        /// ���� �׽�Ʈ ������
        /// </summary>
        public VoxelTestData2 instance_data;
    }
    
    /// <summary>
    /// ���� Ŭ���� �Ҵ�
    /// </summary>
    [System.Serializable]
    public class Phase4_Voxel
    {
        public Voxelizer2 voxelizer2;
    }

    /// <summary>
    /// ���� ����Ʈ ����
    /// </summary>
    [System.Serializable]
    public class Phase7_VoxelList
    {
        /// <summary>
        /// ���� ����
        /// </summary>
        [SerializeField]
        public List<Voxel2> nearVoxels;

        /// <summary>
        /// �浹 ����
        /// </summary>
        [SerializeField]
        public List<Voxel2> nearCollisionVoxels;

        /// <summary>
        /// ���� ����Ʈ ����, �Ҵ�
        /// </summary>
        /// <param name="_voxelizer"></param>
        public void CreateList(Voxelizer2 _voxelizer)
        {
            int depth = _voxelizer.m_depth;

            nearVoxels = new List<Voxel2>(depth); //new List<Voxel2>[depth];
            nearCollisionVoxels = new List<Voxel2>();

            //for (int i = 0; i < depth; i++)
            //{
            //    //nearVoxels[i] = new List<Voxel2>();
            //    //nearCollisionVoxels[i] = new List<Voxel2>();
            //}
        }
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
        #region Instance
        private static DemoCode instance;

        public static DemoCode Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<DemoCode>() as DemoCode;
                }
                return instance;
            }
        }
        #endregion

        /// <summary>
        /// Mapbox Ŭ����
        /// </summary>
        public AbstractMap absMap;

        /// <summary>
        /// ������ ��ü
        /// </summary>
        public GameObject test;

        [Header("��,�浵, �� ����")]
        [SerializeField] private double lantitude;
        [SerializeField] private double longitude;
        [SerializeField] private float zoom;

        public double Lantitude { get => lantitude; set => lantitude = value; }
        public double Longitude { get => longitude; set => longitude = value; }
        public float Zoom { get => zoom; set => zoom = value; }

        [Header("������ ���� ��ġ �ܰ躰 ������ Ŭ����")]
        public Phase1_2Position phase1;
        public Phase2_CenterRotation phase2;
        public Phase3_Object phase3;
        public Phase4_Voxel phase4;
        public Phase7_VoxelList phase7;

        public UnitMovementPosition uMPos_x;
        public UnitMovementPosition uMPos_y;

        private void Update()
        {
            // Ŭ���� ��ġ�� ��/�浵 ��ǥ�� �����´�.
            if(Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    Debug.Log($"hit point : {hit.point}");

                    Vector2d latlon = Get3DToLatLon(hit.point);

                    Debug.Log($"lat lon : {latlon.x}, {latlon.y}");
                }
            }

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
            }

            {
                // 0 : ��ġ�� �ü����� ��ó Ư�� ��ġ�� �̵� (����/����)
                if (Input.GetKeyDown(KeyCode.T))
                {
                    absMap.UpdateMap(new Vector2d(37.845823, 126.767088), 15);
                }
                // 0 : ��ġ�� �ü����� Ư�� ��ġ�� �̵� (����/����)
                if (Input.GetKeyDown(KeyCode.Y))
                {
                    absMap.UpdateMap(new Vector2d(37.843780, 126.767954), 15);
                }

                // 1 :: �� ��(������)�� 3D ��ġ Ȯ��
                if (Input.GetKeyDown(KeyCode.G))
                {
                    SetPhase1_SetStartEndPosition();
                }
                // 2 :: ��ġ �߽���, ���� ����
                else if (Input.GetKeyDown(KeyCode.H))
                {
                    SetPhase2_SetCenterPositionRotation();
                }
                // 3 :: �ü��� ��ġ, ����� ���� ����
                else if (Input.GetKeyDown(KeyCode.J))
                {
                    SetPhase3_CreateResource();
                    SetPhase3_AfterPhase2_SetResource();
                    SetPhase3_SetDemoObject();
                }
                // 4 :: �ü����� ���� ��ġ
                else if (Input.GetKeyDown(KeyCode.K))
                {
                    SetPhase4_SetVoxel();
                }


                // :: �������� �Ÿ����
                else if (Input.GetKeyDown(KeyCode.L))
                {
                    SetStartEndDistance();
                }
                // :: Ư�� ������ �� ���ϱ�
                else if (Input.GetKeyDown(KeyCode.Z))
                {
                    SetPhase6_GetHeightFromLatLon();
                }
                // :: ��� �̵� ������ ���� ���ϱ�
                else if (Input.GetKeyDown(KeyCode.X))
                {
                    SetPhase7_SetVoxelList();
                }
            }


        }

        // ==========

        #region Debugging

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

        #endregion

        // ==========

        #region Core Functions

        // 3D - ���浵�� ��ȯ

        #region LatLon -> 3D

        /// <summary>
        /// ���浵���� 3D ��ǥ ���
        /// </summary>
        /// <param name="_value">���浵</param>
        /// <returns>3D ��ǥ</returns>
        public static Vector3 GetLatLonTo3D(Vector2d _value)
        {
            Vector3 result = default(Vector3);

            result = Mapbox.Unity.Utilities.Conversions.GeoToWorldPosition(_value.x, _value.y, Instance.absMap.CenterMercator, Instance.absMap.WorldRelativeScale).ToVector3xz();

            return result;
        }

        #endregion

        #region 3D -> LatLon 

        /// <summary>
        /// 3D���� ���浵 ��ǥ ���
        /// </summary>
        /// <param name="point">3D ��ǥ</param>
        /// <returns>���浵</returns>
        public static Vector2d Get3DToLatLon(Vector3 point)
        {
            Vector2d v2d = point.GetGeoPosition(Instance.absMap.CenterMercator, Instance.absMap.WorldRelativeScale);

            return v2d;
        }

        #endregion

        // ���浵 ���� Ÿ�� ����

        #region LatLon -> UnityTile

        /// <summary>
        /// ���浵 ��ġ���� Ÿ�� ID ����
        /// </summary>
        public static UnwrappedTileId GetLatLonToTileId(double latitude, double longitude)
        {
            var tileIDWnWrapped = TileCover.CoordinateToTileId(new Vector2d(latitude, longitude), (int)Instance.absMap.Zoom);

            return tileIDWnWrapped;
        }

        /// <summary>
        /// Ÿ�� ID ������� ����Ƽ Ÿ�� ����
        /// </summary>
        public static UnityTile GetTileIdToUnityTile(UnwrappedTileId tileID)
        {
            UnityTile tile = Instance.absMap.MapVisualizer.GetUnityTileFromUnwrappedTileId(tileID);

            return tile;
        }

        /// <summary>
        /// ���浵 ��ġ�� Ÿ�� ����
        /// </summary>
        public static UnityTile GetLatLon_To_UnityTile(double latitude, double longitude)
        {
            UnwrappedTileId tileId = GetLatLonToTileId(latitude, longitude);

            UnityTile tile = GetTileIdToUnityTile(tileId);

            return tile;
        }

        #endregion

        #region LatLon -> tileIndex

        /// <summary>
        /// latlon -> meter ��ȯ
        /// </summary>
        public static Vector2d GetLatLonToMeters(double latitude, double longitude)
        {
            Vector2d latlon = new Vector2d(latitude, longitude);

            Vector2d meter = Conversions.LatLonToMeters(latlon);

            return meter;
        }

        /// <summary>
        /// Ÿ�� �߾� �޾ƿ���
        /// </summary>
        public static Vector2d GetTileCenter(UnityTile tile)
        {
            Vector2d center = tile.Rect.Center;

            return center;
        }

        /// <summary>
        /// Ÿ�� ��ġ�� �⺻ ��ġ ���ϱ�
        /// </summary>
        public static Vector2d GetTileBasePoint(UnityTile tile)
        {
            Vector2d _base = GetTileCenter(tile);

            _base = _base - new Vector2d(tile.Rect.Size.x / 2, tile.Rect.Size.y / 2);

            return _base;
        }

        /// <summary>
        /// Ÿ�� Ư�� ��ġ�� ��� ��ġ ���ϱ�
        /// </summary>
        /// <returns> Ÿ�� Ư�� ��ġ�� ��� ��ġ </returns>
        public static Vector2d GetTile_To_DiffPosition(double latitude, double longitude, UnityTile tile)
        {
            // lat lon to meters because the tiles rect is also in meters
            Vector2d v2d = GetLatLonToMeters(latitude, longitude);

            // get the origin of the tile in meters
            Vector2d v2dBase = GetTileBasePoint(tile);

            // offset between the tile origin and the lat lon point
            Vector2d diff = v2d - v2dBase;

            return diff;
        }

        /// <summary>
        /// Ÿ�� Ư�� ��ġ�� �ε��� ���ϱ�
        /// </summary>
        /// <returns> Ÿ�� Ư�� ��ġ�� �ε��� </returns>
        public static Vector2 GetTile_To_DiffIndex(double latitude, double longitude, UnityTile tile)
        {
            Vector2 index = default(Vector2);

            Vector2d diff = GetTile_To_DiffPosition(latitude, longitude, tile);

            // mapping the differences to (0-1)
            index = new Vector2(
                (float)(diff.x / tile.Rect.Size.x),
                (float)(diff.y / tile.Rect.Size.y)
                );

            return index;
        }

        #endregion

        #region LatLon -> Height

        /// <summary>
        /// Ư�� Ÿ���� Ÿ�� �ε������� ���� ���
        /// </summary>
        /// <param name="diffIndex">Ÿ�� �ε���</param>
        /// <param name="tile">GIS Ÿ��</param>
        /// <returns>��</returns>
        public static float GetDiffIndex_To_Height(Vector2 diffIndex, UnityTile tile)
        {
            float h = tile.QueryHeightData(diffIndex.x, diffIndex.y);

            return h;
        }

        /// <summary>
        /// Ư�� ���浵���� ���� ���
        /// </summary>
        /// <param name="latitude">����</param>
        /// <param name="longitude">�浵</param>
        /// <returns>��</returns>
        public static float GetLatLon_To_Height(double latitude, double longitude)
        {
            float height = 0f;

            // get tile id
            var tileIDUnWrapped = GetLatLonToTileId(latitude, longitude);

            // get tile
            UnityTile tile = GetTileIdToUnityTile(tileIDUnWrapped);

            Vector2 diffIndex = GetTile_To_DiffIndex(latitude, longitude, tile);

            height = GetDiffIndex_To_Height(diffIndex, tile);

            return height;
        }

        /// <summary>
        /// 3D ��ǥ���� ������ �� ���
        /// </summary>
        /// <param name="point">3D ��ǥ</param>
        /// <returns></returns>
        public static float Get3D_To_Height(Vector3 point)
        {
            Vector2d latlon = Get3DToLatLon(point);

            float height = GetLatLon_To_Height(latlon.x, latlon.y);

            return height;
        }

        /// <summary>
        /// ���浵���� �� ���� 3D ��ġ ���ϱ�
        /// </summary>
        /// <param name="latitude">����</param>
        /// <param name="longitude">�浵</param>
        /// <param name="map">Mapbox �ν��Ͻ�</param>
        /// <returns>��ġ���� (������)</returns>
        public static Vector3 GetLatLon_To_Location(double latitude, double longitude, AbstractMap map)
        {
            Vector3 location = default(Vector3);

            var tileIDUnWrapped = GetLatLonToTileId(latitude, longitude);

            // get tile
            UnityTile tile = GetTileIdToUnityTile(tileIDUnWrapped);

            Vector2 diffIndex = GetTile_To_DiffIndex(latitude, longitude, tile);

            float height = GetDiffIndex_To_Height(diffIndex, tile);

            location = Conversions.GeoToWorldPosition(latitude, longitude, map.CenterMercator, map.WorldRelativeScale).ToVector3xz();

            location = new Vector3(location.x, -height, location.z);

            return location;
        }

        #endregion

        #endregion

        // ==========

        #region Voxel With GIS Scenario 1

        #region phase1 : �������� ���浵 -> 3D ��ġ ���Ѵ�.

        /// <summary>
        /// �������� ���浵 -> 3D ��ġ�� ���ϰ� ����� ��ü ��ġ
        /// </summary>
        private void SetPhase1_SetStartEndPosition()
        {
            phase1.Points = new List<GameObject>();
            //phase1.Points.Add(SetScenePosition(37.556711, 126.893982, absMap, "object 1"));
            //phase1.Points.Add(SetScenePosition(37.547711, 126.888532, absMap, "object 2"));
            phase1.Points.Add(SetScenePosition(37.8455847, 126.7671465, absMap, "object 1"));
            phase1.Points.Add(SetScenePosition(37.843780, 126.767954, absMap, "object 2"));
        }

        /// <summary>
        /// ������ ���浵 ������� ��ȯ�� 3D ��ǥ�� ��ü ����
        /// </summary>
        /// <param name="lan">����</param>
        /// <param name="lon">�浵</param>
        /// <param name="_map">Mapbox ����</param>
        /// <param name="_name">��ü��</param>
        /// <returns>��ü</returns>
        private GameObject SetScenePosition(double lan, double lon, AbstractMap _map, string _name)
        {
            GameObject obj;

            Vector2d llpos = new Vector2d(lan, lon);
            Vector3 pos = GetLatLonTo3D(llpos);

            obj = new GameObject();// GameObject.CreatePrimitive(PrimitiveType.Sphere);
            obj.transform.position = new Vector3(pos.x, -2.4f, pos.z);
            obj.name = _name;

            return obj;
        }

        #endregion

        #region phase2 : ������ ������ ������ �߽� ��ġ // ��ġ ������ ���Ѵ�.

        /// <summary>
        /// ������ ������ ������ �߽� ��ġ // ��ġ ������ ���Ѵ�.
        /// </summary>
        private void SetPhase2_SetCenterPositionRotation()
        {
            GameObject start = phase1.Points[0];
            GameObject end = phase1.Points[1];

            // �߽��� ���
            Vector3 centerPos = (start.transform.position + end.transform.position) / 2;

            GameObject center = new GameObject();// GameObject.CreatePrimitive(PrimitiveType.Sphere);
            center.name = "center";

            center.transform.position = new Vector3(centerPos.x, -2.4f, centerPos.z);
            // ���� ���
            center.transform.LookAt(start.transform);

            // �߽���, ���� �Ҵ�
            phase2.CenterPoint = center;
            phase2.Rotation = center.transform.rotation;
        }

        #endregion

        #region phase3 : �ü��� ��ġ

        /// <summary>
        /// �ü��� ������ ����
        /// </summary>
        private void SetPhase3_CreateResource()
        {
            phase3.instanceObject = Instantiate(phase3.prefab);
            phase3.instance_data = phase3.instanceObject.GetComponent<VoxelTestData2>();
        }

        /// <summary>
        /// �ü��� ��ġ, ���� ���
        /// </summary>
        private void SetPhase3_AfterPhase2_SetResource()
        {
            phase3.instance_data.SetResource();
        }

        /// <summary>
        /// �ü��� ��ġ, ���� ����
        /// </summary>
        private void SetPhase3_SetDemoObject()
        {
            phase1.Distance = Vector3.Distance(phase1.Points[0].transform.position, phase1.Points[1].transform.position);
            //float dist = Vector3.Distance(phase1.Points[0].transform.position, phase1.Points[1].transform.position);

            Vector3 position = phase1.Points[0].transform.position;
            float height = Get3D_To_Height(new Vector3(position.x, 0, position.z));

            position = new Vector3(position.x, -2.4f, position.z);


            phase3.instanceObject.transform.position = position;
            phase3.instanceObject.transform.rotation = phase2.Rotation;
            phase3.instanceObject.transform.Rotate(new Vector3(0, 90, 0));
        }
        #endregion

        #region phase4 : octree ���� ��ġ

        /// <summary>
        /// octree ���� ��ġ
        /// </summary>
        private void SetPhase4_SetVoxel()
        {
            GameObject obj = new GameObject("voxelizer2");

            //phase3.instanceObject.transform.rotation;

            phase4.voxelizer2 = obj.AddComponent<Voxelizer2>();

            
            phase4.voxelizer2.Prepare(
                depth: 7, 
                minVDepth: 0, 
                maxVDepth: 5);

            phase4.voxelizer2.ArrangeVoxels(phase3.instance_data.Bound, phase2.CenterPoint.transform.position, 
                phase3.instanceObject.transform.rotation, phase3.instance_data);
        }

        #endregion

        #endregion

        // ==========

        #region Plus Features

        #region phase5 : �������� �Ÿ� ���ϰ� �ð�ȭ

        /// <summary>
        /// �������� ���� �Ÿ� ���, �ð�ȭ
        /// </summary>
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

        #region phase6 : Ư�� ������ �� ���ϱ�

        /// <summary>
        /// Ư�� ������ �� ���ϱ�
        /// </summary>
        private void SetPhase6_GetHeightFromLatLon()
        {
            double lat = 37.845823;
            double lon = 126.767088;

            //// get tile ID
            //// ���� ��ġ id
            //var tileIDUnWrapped = GetLatLonToTileId(lat, lon);

            //// get tile
            //UnityTile tile = GetTileIdToUnityTile(tileIDUnWrapped);

            //Vector2 diffIndex = GetTile_To_DiffIndex(lat, lon, tile);

            //// height in unity units
            //var h = tile.QueryHeightData(diffIndex.x, diffIndex.y);
            ////var h = tile.QueryHeightData(diffIndex.x, diffIndex.y);

            //// lat lon to unity units
            //Vector3 location = Conversions.GeoToWorldPosition(lat, lon, absMap.CenterMercator, absMap.WorldRelativeScale).ToVector3xz();
            //// replace y in position
            //location = new Vector3(location.x, -h, location.z);

            Vector3 _location = GetLatLon_To_Location(lat, lon, absMap);

            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obj.transform.localScale = new Vector3(1, 5, 1);
            obj.transform.position = _location;
        }

        #endregion

        #region phase7 : �������� �̵� ���� ���� ����Ʈ ���ϱ�

        /// <summary>
        /// �̵� ���� ���� ���� ���
        /// </summary>
        private void SetPhase7_SetVoxelList()
        {
            phase7.CreateList(phase4.voxelizer2);
            phase4.voxelizer2.m_rootVoxel.SetVoxelList(ref phase7.nearVoxels, ref phase7.nearCollisionVoxels);
        }

        #endregion

        #endregion
    }
}
