using System;

namespace MGS.UCamera
{
    /// <summary>
    /// Range form min to max.
    /// </summary>
    [Serializable]
    public struct Range
    {
        /// <summary>
        /// Min value of range.
        /// </summary>
        public float min;

        /// <summary>
        /// Max value of range.
        /// </summary>
        public float max;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="min">Min value of range.</param>
        /// <param name="max">Max value of range.</param>
        public Range(float min, float max)
        {
            this.min = min;
            this.max = max;
        }
    }
}