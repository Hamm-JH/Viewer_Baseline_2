using System;
using UnityEngine;

namespace MGS.UCamera
{
    /// <summary>
    /// Mark gameobject for camera align to it.
    /// </summary>
    [AddComponentMenu("MGS/UCamera/AlignMark")]
    public class AlignMark : MonoBehaviour
    {
        /// <summary>
        /// Target of camera align.
        /// </summary>
        public AlignTarget alignTarget;

        /// <summary>
        /// Reset component.
        /// </summary>
        protected virtual void Reset()
        {
            this.alignTarget = new AlignTarget(base.transform, new Vector2(30f, 0f), 5f, new Range(-90f, 90f), new Range(1f, 10f));
        }
    }
}