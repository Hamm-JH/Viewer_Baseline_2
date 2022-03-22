using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Control
{
    public abstract class AContoller : MonoBehaviour, IController
    {
        #region Singleton

        private static AContoller instance;

        public static AContoller Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = FindObjectOfType<AContoller>() as AContoller;
                }
                return instance;
            }
        }

        #endregion

        private Bounds panningBounds;
        private float dragSensitivity;
        private bool isDragInverted;

        public Bounds PanningBounds
        {
            get => panningBounds;
            set => panningBounds = value;
        }

        public float DragSensitivity
        {
            get => dragSensitivity;
            set => dragSensitivity = value;
        }

        public bool IsDragInverted
        {
            get => isDragInverted;
            set => isDragInverted = value;
        }
    }
}
