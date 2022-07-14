using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management.Content
{
    using Bearroll.UltimateDecals;
    using DG.Tweening;

    public class SmartInspectManager : ContentManager
    {
        public GameObject CenterPoint;

        /// <summary>
        /// UI 또는 외부의 특정 영역에서 읽기/쓰기를 수행하는 설정정보들을 모아둠.
        /// </summary>
        [System.Serializable]
        public class Settings
        {
            [SerializeField] private float m_iconScale = 1;
            [SerializeField] private float m_modelAlpha = 1;
            [SerializeField] private float m_zoomSensitivity = 1;
            [SerializeField] private float m_fontSize = 1;

            public float IconScale { get => m_iconScale; set => m_iconScale = value; }
            public float ModelAlpha { get => m_modelAlpha; set => m_modelAlpha = value; }
            public float ZoomSensitivity { get => m_zoomSensitivity; set => m_zoomSensitivity = value; }
            public float FontSize { get => m_fontSize; set => m_fontSize = value; }
        }

        public Settings setting;

        public override void OnCreate()
        {
            base.OnCreate();

            //UD_Manager.Restart();
            // DOTween 초기화
            //DOTween.Init();

            //UD_Manager.instance.DoDestroy();
        }
    }
}
