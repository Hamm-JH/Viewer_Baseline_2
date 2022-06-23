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
        [SerializeField] private GameObject demoGameObject;

        public GameObject DemoGameObject { get => demoGameObject; set => demoGameObject = value; }
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

            // 특정 위치로 이동 (시점/종점)
            if (Input.GetKeyDown(KeyCode.A))
            {
                //absMap.UpdateMap(new Vector2d(37.556911, 126.894852), 15);
                //absMap.UpdateMap(new Vector2d(37.847293, 126.766481), 15);
                absMap.UpdateMap(new Vector2d(37.845823, 126.767088), 15);
            }
            // 특정 위치로 이동 (시점/종점)
            else if (Input.GetKeyDown(KeyCode.S))
            {
                //absMap.UpdateMap(new Vector2d(37.556806, 126.895479), 15);
                //absMap.UpdateMap(new Vector2d(37.848874, 126.765911), 15);
                absMap.UpdateMap(new Vector2d(37.843780, 126.767954), 15);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                // 경위도 -> 3D 위치검출
                Vector3 pos = Mapbox.Unity.Utilities.Conversions.GeoToWorldGlobePosition(Lantitude, Longitude, 1);
                Debug.Log($"base pos : {pos}");

                // pos1, pos2
                Vector3 pos1 = Mapbox.Unity.Utilities.Conversions.GeoToWorldGlobePosition(37.556711, 126.893982, 1);
                Vector3 pos2 = Mapbox.Unity.Utilities.Conversions.GeoToWorldGlobePosition(37.547711, 126.888532, 1);

                

                Debug.Log($"pos1 : {pos1}");
                Debug.Log($"pos2 : {pos2}");

                test.transform.position = pos;
            }
            else if (Input.GetKeyDown(KeyCode.F))
            {
                double xTile = Mapbox.Unity.Utilities.Conversions.TileXToNWLongitude(37, 1);

                Debug.Log($"xTile : {xTile}");
            }
            else if (Input.GetKeyDown(KeyCode.G))
            {
                Vector2d llpos = new Vector2d(Lantitude, Longitude);
                Vector2d pos = Mapbox.Unity.Utilities.Conversions.GeoToWorldPosition(llpos, new Vector2d(37.556911, 126.894852), 1);
                var gg = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                gg.transform.position = new Vector3((float)pos.x, 0, (float)pos.y);
            }
            else if (Input.GetKeyDown(KeyCode.H))
            {
                SetPhase1_SetStartEndPosition();
                //ContentManager.Instance.SetPhase1_StartEndPosition();
                //SetPhase2_SetCenterPositionRotation();
                //SetPhase3_SetDemoObject();
            }
            else if (Input.GetKeyDown(KeyCode.J))
            {
                //SetPhase1_SetStartEndPosition();
                SetPhase2_SetCenterPositionRotation();
                //ContentManager.Instance.SetPhase2_CenterRotation();
                //SetPhase3_SetDemoObject();
            }
            else if (Input.GetKeyDown(KeyCode.K))
            {
                //SetPhase1_SetStartEndPosition();
                //SetPhase2_SetCenterPositionRotation();
                SetPhase3_SetDemoObject();
                //ContentManager.Instance.SetPhase3_SetModelObject(phase1.Distance, phase2.CenterPoint.transform);
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

        private Vector3 GetLatLonToXZ(Vector2d _value )
        {
            Vector3 result = default(Vector3);

            result = Mapbox.Unity.Utilities.Conversions.GeoToWorldPosition(_value.x, _value.y, absMap.CenterMercator, absMap.WorldRelativeScale).ToVector3xz();

            return result;
        }

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

        private void SetPhase2_SetCenterPositionRotation()
        {
            GameObject start = phase1.Points[0];
            GameObject end = phase1.Points[1];

            Vector3 centerPos = (start.transform.position + end.transform.position) / 2;

            GameObject center = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            center.name = "center";

            center.transform.position = centerPos;
            center.transform.LookAt(start.transform);

            phase2.CenterPoint = center;
            phase2.Rotation = center.transform.rotation;
        }

        private void SetPhase3_SetDemoObject()
        {
            phase1.Distance = Vector3.Distance(phase1.Points[0].transform.position, phase1.Points[1].transform.position);
            //float dist = Vector3.Distance(phase1.Points[0].transform.position, phase1.Points[1].transform.position);

            GameObject demoObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            demoObj.name = "demo object";

            demoObj.transform.position = phase2.CenterPoint.transform.position;
            demoObj.transform.rotation = phase2.Rotation;
            demoObj.transform.localScale = new Vector3(1, 1, phase1.Distance);

            phase3.DemoGameObject = demoObj;

        }
    }
}
