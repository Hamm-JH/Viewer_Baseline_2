using System;

namespace MGS.UCamera
{
    /// <summary>
    /// Settings of mouse input.
    /// </summary>
    [Serializable]
    public struct MouseSettings
    {
        /// <summary>
        /// ID of mouse button.
        /// </summary>
        public int mouseButtonID;

        /// <summary>
        /// Sensitivity of mouse pointer.
        /// </summary>
        public float pointerSensitivity;

        /// <summary>
        /// Sensitivity of mouse ScrollWheel.
        /// </summary>
        public float wheelSensitivity;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="mouseButtonID">ID of mouse button.</param>
        /// <param name="pointerSensitivity">Sensitivity of mouse pointer.</param>
        /// <param name="wheelSensitivity">Sensitivity of mouse ScrollWheel.</param>
        public MouseSettings(int mouseButtonID, float pointerSensitivity, float wheelSensitivity)
        {
            this.mouseButtonID = mouseButtonID;
            this.pointerSensitivity = pointerSensitivity;
            this.wheelSensitivity = wheelSensitivity;
        }
    }
}