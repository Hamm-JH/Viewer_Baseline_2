using System;
using UnityEngine;

namespace MGS.UCamera
{
    /// <summary>
    /// Target of camera align.
    /// </summary>
    [Serializable]
    public struct AlignTarget
    {
        /// <summary>
        /// Center of align target.
        /// </summary>
        public Transform center;

        /// <summary>
        /// Angles of align.
        /// </summary>
        public Vector2 angles;

        /// <summary>
        /// Distance from camera to target center.
        /// </summary>
        public float distance;

        /// <summary>
        /// Range limit of angle.
        /// </summary>
        public Range angleRange;

        /// <summary>
        /// Range limit of distance.
        /// </summary>
        public Range distanceRange;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="center">Center of align target.</param>
        /// <param name="angles">Angles of align.</param>
        /// <param name="distance">Distance from camera to target center.</param>
        /// <param name="angleRange">Range limit of angle.</param>
        /// <param name="distanceRange">Range limit of distance.</param>
        public AlignTarget(Transform center, Vector2 angles, float distance, Range angleRange, Range distanceRange)
        {
            this.center = center;
            this.angles = angles;
            this.distance = distance;
            this.angleRange = angleRange;
            this.distanceRange = distanceRange;
        }
    }
}