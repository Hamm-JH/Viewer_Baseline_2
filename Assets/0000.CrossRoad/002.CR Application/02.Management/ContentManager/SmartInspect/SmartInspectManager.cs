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
        /// UI �Ǵ� �ܺ��� Ư�� �������� �б�/���⸦ �����ϴ� ������������ ��Ƶ�.
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
            // DOTween �ʱ�ȭ
            //DOTween.Init();

            //UD_Manager.instance.DoDestroy();
        }
    }
}
