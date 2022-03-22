using System;
using UnityEngine;

namespace MGS.UCamera
{
    /// <summary>
    /// Rectangle area on plane.
    /// </summary>
    [Serializable]
    public struct PlaneArea
    {
        /// <summary>
        /// Center of area.
        /// </summary>
        public Vector3 center;

        /// <summary>
        /// Width of area.
        /// </summary>
        public float width;

        /// <summary>
        /// Length of area.
        /// </summary>
        public float length;

        /// <summary>
        /// height of area.
        /// </summary>
        public float height;

        /// <summary>
        /// Constructor.
        /// </summary>
        public PlaneArea(Vector3 center, float width, float length, float height)
        {
            this.center = center;
            this.width = width;
            this.length = length;
            this.height = height;
        }
    }
}