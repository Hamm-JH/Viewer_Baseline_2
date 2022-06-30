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

    [System.Serializable]
    public class Phase7_VoxelList
    {
        /// <summary>
        /// 근접 복셀
        /// </summary>
        [SerializeField]
        public List<Voxel2> nearVoxels;

        /// <summary>
        /// 충돌 복셀
        /// </summary>
        [SerializeField]
        public List<Voxel2> nearCollisionVoxels;

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
    /// 위/경도상의 단위 이동 위치를 관리하는 클래스
    /// </summary>
    [System.Serializable]
    public class UnitMovementPosition
    {
        [Header("중심 위경도 값")]
        public Vector2d centerLatLon;
        [Header("단위 거리만큼 떨어진 위경도 값")]
        public Vector2d targetLatLon;

        [Header("단위 거리값 (1km)")]
        public Vector2d targetKM_xy;
        [Header("1km 거리의 위경도 값")]
        public Vector2d targetKM_xy_LatLon;
    }

    public class DemoCode : MonoBehaviour
    {
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
        public Phase7_VoxelList phase7;

        public UnitMovementPosition uMPos_x;
        public UnitMovementPosition uMPos_y;

        private void Update()
        {
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

                    // 일정 위경도차가 얼마나 거리를 가지는가?
                    //Debug.Log(Mapbox.Unity.Utilities.Conversions.LatLonToMeters(diff2));

                    // 위경도에서 미터 추출
                    //Debug.Log(Mapbox.Unity.Utilities.Conversions.LatLonToMeters(diff));
                    //Debug.Log(Mapbox.Unity.Utilities.Conversions.LatLonToMeters(diff.x, 0));
                    //Debug.Log(Mapbox.Unity.Utilities.Conversions.LatLonToMeters(0, diff.y));

                    // 둘은 같다
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
                // 특정 위치로 이동 (시점/종점)
                if (Input.GetKeyDown(KeyCode.T))
                {
                    absMap.UpdateMap(new Vector2d(37.845823, 126.767088), 15);
                }
                // 특정 위치로 이동 (시점/종점)
                if (Input.GetKeyDown(KeyCode.Y))
                {
                    absMap.UpdateMap(new Vector2d(37.843780, 126.767954), 15);
                }
                // 두 점(시종점)의 3D 위치 확인
                else if (Input.GetKeyDown(KeyCode.G))
                {
                    SetPhase1_SetStartEndPosition();
                }
                // 배치 중심점, 각도 설정
                else if (Input.GetKeyDown(KeyCode.H))
                {
                    SetPhase2_SetCenterPositionRotation();
                }
                // 시설물 배치, 검출된 각도 설정
                else if (Input.GetKeyDown(KeyCode.J))
                {
                    SetPhase3_CreateResource();
                    SetPhase3_AfterPhase2_SetResource();
                    SetPhase3_SetDemoObject();
                }
                // 시설물에 복셀 배치
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
                    SetPhase6_GetHeightFromLatLon();
                }
                else if (Input.GetKeyDown(KeyCode.X))
                {
                    SetPhase7_SetVoxelList();
                }
            }
            //else if (Input.GetKeyDown(KeyCode.Z))
            //{
            //    // pos1, pos2
            //    Vector3 pos1 = Mapbox.Unity.Utilities.Conversions.GeoToWorldGlobePosition(37.556711, 126.893982, 1);

            //    Debug.Log($"pos1 : {pos1}");

            //    test.transform.position = pos1;
            //}
            //else if (Input.GetKeyDown(KeyCode.X))
            //{
            //    // pos1, pos2
            //    Vector3 pos2 = Mapbox.Unity.Utilities.Conversions.GeoToWorldGlobePosition(37.547711, 126.888532, 1);

            //    Debug.Log($"pos2 : {pos2}");

            //    test.transform.position = pos2;
            //}


        }

        private void __review()
        {
            //if (Input.GetKeyDown(KeyCode.D))
            //{
            //    // 경위도 -> 3D 위치검출
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
        /// 경도 기준으로 단위 거리만큼 떨어진 위경도를 구한다.
        /// </summary>
        private void GetUnitDistance_Lon_X()
        {
            if (uMPos_x == null)
            {
                uMPos_x = new UnitMovementPosition();
            }

            // 중심 위경도 값을 구한다.
            uMPos_x.centerLatLon = absMap.CenterLatitudeLongitude;

            // 중심에서 일정 거리의 1km를 구하기 위해 x축 1km 값을 설정한다.
            uMPos_x.targetKM_xy = new Vector2d(1000, 0);
            // 1km값을 위/경도로 변환한다.
            uMPos_x.targetKM_xy_LatLon = Mapbox.Unity.Utilities.Conversions.MetersToLatLon(uMPos_x.targetKM_xy);

            // 단위 위/경도만큼 이동한 거리의 새 위경도를 할당한다.
            uMPos_x.targetLatLon = uMPos_x.centerLatLon + uMPos_x.targetKM_xy_LatLon;

            //Debug.Log($"km x : {targetKM_x.x}");    // 1000
            //Debug.Log($"km y : {targetKM_x.y}");    // 0

            //Debug.Log($"lalo x : {targetKM_x_LatLon.x}");   // 0
            //Debug.Log($"lalo y : {targetKM_x_LatLon.y}");   // 0.00898315284119521

            GameObject obj = SetScenePosition(uMPos_x.targetLatLon.x, uMPos_x.targetLatLon.y, absMap, "posX");
        }

        /// <summary>
        /// 위도 기준으로 단위 거리만큼 떨어진 위경도를 구한다.
        /// </summary>
        private void GetUnitDistance_Lat_Y()
        {
            if (uMPos_y == null)
            {
                uMPos_y = new UnitMovementPosition();
            }

            // 중심 위경도 값을 구한다.
            uMPos_y.centerLatLon = absMap.CenterLatitudeLongitude;

            // 중심에서 일정 거리의 1km를 구하기 위해 y축 1km 값을 설정한다.
            uMPos_y.targetKM_xy = new Vector2d(0, 1000);
            // 1km값을 위/경도로 변환한다.
            uMPos_y.targetKM_xy_LatLon = Mapbox.Unity.Utilities.Conversions.MetersToLatLon(uMPos_y.targetKM_xy);

            // 단위 위/경도만큼 이동한 거리의 새 위경도를 할당한다.
            uMPos_y.targetLatLon = uMPos_y.centerLatLon + uMPos_y.targetKM_xy_LatLon;

            //Debug.Log($"km x : {targetKM_x.x}");    // 0
            //Debug.Log($"km y : {targetKM_x.y}");    // 1000

            //Debug.Log($"lalo x : {targetKM_x_LatLon.x}");   // 0.008983152804392
            //Debug.Log($"lalo y : {targetKM_x_LatLon.y}");   // 0

            GameObject obj = SetScenePosition(uMPos_y.targetLatLon.x, uMPos_y.targetLatLon.y, absMap, "posY");
        }

        // 3D - 위경도간 변환

        #region LatLon -> 3D

        public static Vector3 GetLatLonTo3D(Vector2d _value)
        {
            Vector3 result = default(Vector3);

            result = Mapbox.Unity.Utilities.Conversions.GeoToWorldPosition(_value.x, _value.y, Instance.absMap.CenterMercator, Instance.absMap.WorldRelativeScale).ToVector3xz();

            return result;
        }

        #endregion

        #region 3D -> LatLon 

        public static Vector2d Get3DToLatLon(Vector3 point)
        {
            Vector2d v2d = point.GetGeoPosition(Instance.absMap.CenterMercator, Instance.absMap.WorldRelativeScale);

            return v2d;
        }

        #endregion

        // 위경도 상의 타일 연산

        #region LatLon -> UnityTile

        /// <summary>
        /// 위경도 위치에서 타일 ID 구함
        /// </summary>
        public static UnwrappedTileId GetLatLonToTileId(double latitude, double longitude)
        {
            var tileIDWnWrapped = TileCover.CoordinateToTileId(new Vector2d(latitude, longitude), (int)Instance.absMap.Zoom);

            return tileIDWnWrapped;
        }

        /// <summary>
        /// 타일 ID 기반으로 유니티 타일 구함
        /// </summary>
        public static UnityTile GetTileIdToUnityTile(UnwrappedTileId tileID)
        {
            UnityTile tile = Instance.absMap.MapVisualizer.GetUnityTileFromUnwrappedTileId(tileID);

            return tile;
        }

        /// <summary>
        /// 위경도 위치의 타일 구함
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
        /// latlon -> meter 변환
        /// </summary>
        public static Vector2d GetLatLonToMeters(double latitude, double longitude)
        {
            Vector2d latlon = new Vector2d(latitude, longitude);

            Vector2d meter = Conversions.LatLonToMeters(latlon);

            return meter;
        }

        /// <summary>
        /// 타일 중앙 받아오기
        /// </summary>
        public static Vector2d GetTileCenter(UnityTile tile)
        {
            Vector2d center = tile.Rect.Center;

            return center;
        }

        /// <summary>
        /// 타일 위치점 기본 위치 구하기
        /// </summary>
        public static Vector2d GetTileBasePoint(UnityTile tile)
        {
            Vector2d _base = GetTileCenter(tile);

            _base = _base - new Vector2d(tile.Rect.Size.x / 2, tile.Rect.Size.y / 2);

            return _base;
        }

        /// <summary>
        /// 타일 특정 위치의 상대 위치 구하기
        /// </summary>
        /// <returns> 타일 특정 위치의 상대 위치 </returns>
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
        /// 타일 특정 위치의 인덱스 구하기
        /// </summary>
        /// <returns> 타일 특정 위치의 인덱스 </returns>
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

        public static float GetDiffIndex_To_Height(Vector2 diffIndex, UnityTile tile)
        {
            float h = tile.QueryHeightData(diffIndex.x, diffIndex.y);

            return h;
        }

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

        public static float Get3D_To_Height(Vector3 point)
        {
            Vector2d latlon = Get3DToLatLon(point);

            float height = GetLatLon_To_Height(latlon.x, latlon.y);

            return height;
        }

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

        #region phase1 : 시종점의 위경도 -> 3D 위치 구한다.

        private void SetPhase1_SetStartEndPosition()
        {
            phase1.Points = new List<GameObject>();
            //phase1.Points.Add(SetScenePosition(37.556711, 126.893982, absMap, "object 1"));
            //phase1.Points.Add(SetScenePosition(37.547711, 126.888532, absMap, "object 2"));
            phase1.Points.Add(SetScenePosition(37.8455847, 126.7671465, absMap, "object 1"));
            phase1.Points.Add(SetScenePosition(37.843780, 126.767954, absMap, "object 2"));
        }

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

        #region phase2 : 지도상 시종점 사이의 중심 위치 // 배치 각도를 구한다.

        private void SetPhase2_SetCenterPositionRotation()
        {
            GameObject start = phase1.Points[0];
            GameObject end = phase1.Points[1];

            // 중심점 계산
            Vector3 centerPos = (start.transform.position + end.transform.position) / 2;

            GameObject center = new GameObject();// GameObject.CreatePrimitive(PrimitiveType.Sphere);
            center.name = "center";

            center.transform.position = new Vector3(centerPos.x, -2.4f, centerPos.z);
            // 각도 계산
            center.transform.LookAt(start.transform);

            // 중심점, 각도 할당
            phase2.CenterPoint = center;
            phase2.Rotation = center.transform.rotation;
        }

        #endregion

        #region phase3 : 시설물 배치

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

            Vector3 position = phase1.Points[0].transform.position;
            float height = Get3D_To_Height(new Vector3(position.x, 0, position.z));

            position = new Vector3(position.x, -2.4f, position.z);


            phase3.instanceObject.transform.position = position;
            phase3.instanceObject.transform.rotation = phase2.Rotation;
            phase3.instanceObject.transform.Rotate(new Vector3(0, 90, 0));
        }
        #endregion

        #region phase4 : octree 복셀 배치

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

        #region phase5 : 시종점간 거리 구하고 시각화

        private void SetStartEndDistance()
        {
            Transform sTr = phase3.instance_data.start;
            Transform eTr = phase3.instance_data.end;

            

            // 시종점의 위경도 변환
            Vector2d startLaLo = sTr.GetGeoPosition(absMap.CenterMercator, absMap.WorldRelativeScale);
            Vector2d endLaLo = eTr.GetGeoPosition(absMap.CenterMercator, absMap.WorldRelativeScale);
            //Vector2d sGP = Mapbox.Unity.Utilities.Conversions.

            Vector2d diff = startLaLo - endLaLo;

            // 위경도 기반의 거리계산
            Vector2d diffMeter = Mapbox.Unity.Utilities.Conversions.LatLonToMeters(diff);

            //Debug.Log($"diff meter : {diffMeter}");

            // 위경도 기반 거리에서 실제 거리 계산
            double diffDistance = Mathd.Sqrt(diffMeter.x * diffMeter.x + diffMeter.y * diffMeter.y);

            //Debug.Log($"diff distance : {diffDistance}");

            // 시점과 종점의 위경도를 구한다.
            // 두 위경도차를 구한다.
            // 위경도차의 x,y 거리값을 구한다.
            // mathf.sqrt(x*x + y*y) 연산을 수행한다.

            GameObject lineObj = new GameObject("simpleLine");
            SimpleLiner sLiner = lineObj.AddComponent<SimpleLiner>();
            sLiner.SetLine(sTr, eTr, absMap, diffDistance);
            
        }

        #endregion

        #region phase6 : 특정 지점의 고도 구하기

        private void SetPhase6_GetHeightFromLatLon()
        {
            double lat = 37.845823;
            double lon = 126.767088;

            //// get tile ID
            //// 시점 위치 id
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

        #region phase7 : 복셀에서 이동 가능 복셀 리스트 구하기

        private void SetPhase7_SetVoxelList()
        {
            phase7.CreateList(phase4.voxelizer2);
            phase4.voxelizer2.m_rootVoxel.SetVoxelList(ref phase7.nearVoxels, ref phase7.nearCollisionVoxels);
        }

        #endregion
    }
}
