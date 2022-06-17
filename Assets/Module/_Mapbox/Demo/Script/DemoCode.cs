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

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                //absMap.UpdateMap(new Vector2d(37.556911, 126.894852), 15);
                //absMap.UpdateMap(new Vector2d(37.847293, 126.766481), 15);
                absMap.UpdateMap(new Vector2d(37.845823, 126.767088), 15);
            }
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
            Vector3 pos = Mapbox.Unity.Utilities.Conversions.GeoToWorldPosition(llpos.x, llpos.y, absMap.CenterMercator, absMap.WorldRelativeScale).ToVector3xz()
                /*+ new Vector3(0, 3, 0)*/;
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
