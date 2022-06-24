using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test
{
    using Mapbox.Unity.Map;
    using Mapbox.Unity.Utilities;
    using Mapbox.Utils;

    public class SimpleLiner : MonoBehaviour
    {
        public List<Vector2d> lines;
        public List<Transform> trs;
        public double distance;

        public void SetLine(Transform start, Transform end, AbstractMap map, double _distance)
        {
            if (lines == null)
            {
                lines = new List<Vector2d>();
                lines.Add(start.GetGeoPosition(map.CenterMercator, map.WorldRelativeScale));
                lines.Add(end.GetGeoPosition(map.CenterMercator, map.WorldRelativeScale));
            }
            if (trs == null)
            {
                trs = new List<Transform>();
                GameObject _start = new GameObject("_start");
                GameObject _end = new GameObject("_end");
                trs.Add(_start.transform);
                trs.Add(_end.transform);
            }

            UpdateLine(start, end, map, _distance);
        }

        public void UpdateLine(Transform start, Transform end, AbstractMap map, double _distance)
        {
            lines[0] = start.GetGeoPosition(map.CenterMercator, map.WorldRelativeScale);
            lines[1] = end.GetGeoPosition(map.CenterMercator, map.WorldRelativeScale);

            trs[0].position = start.position;
            trs[1].position = end.position;

            distance = _distance;
        }



        private void OnRenderObject()
        {
            if (lines == null)
            {
                return;
            }
            if (trs == null)
            {
                return;
            }

            TestGLCode.CreateLineMaterial();
            // Apply the line material
            TestGLCode.lineMaterial.SetPass(0);

            GL.PushMatrix();
            // Set transformation matrix for drawing to
            // match our transform
            GL.MultMatrix(transform.localToWorldMatrix);

            GL.Begin(GL.LINES);

            GL.Color(Color.magenta);

            GL.Vertex(trs[0].position); GL.Vertex(trs[1].position);

            GL.End();
            GL.PopMatrix();
        }
    }
}
