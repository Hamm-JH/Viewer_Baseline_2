using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SystemEnum
{
    /// <summary>
    /// 현재 프로젝트에 정의된, 레이어의 인덱스 열거변수
    /// </summary>
    public enum Layer
    {
        Default = 0,
        TransparentFX = 1,
        Ignore_Raycast = 2,

        Water = 4,
        UI = 5,

        PPV = 8,
        Overlay = 9,
        Keymap = 10,

        FR = 13,
        BA = 14,
        LE = 15,
        RE = 16,
        TO = 17,
        BO = 18,
        
        Issue = 19,

    }
}

namespace Layer
{
    public static class Index
    {

        public static int XORLayerMaskIndex(int _layerMask, SystemEnum.Layer _exceptLayer)
        {
            int targetLayerMask = GetLayerMask(_exceptLayer);

            return _layerMask ^ targetLayerMask;
        }

        /// <summary>
        /// 레이어마스크 처리용 비트마스크 처리된 정수코드
        /// </summary>
        public static int GetLayerMask(SystemEnum.Layer[] _layers)
        {
            List<string> layerStringList = new List<string>();

            foreach(SystemEnum.Layer _layer in _layers)
            {
                layerStringList.Add(LayerName(_layer));
            }

            return LayerMask.GetMask(layerStringList.ToArray());
        }

        /// <summary>
        /// 레이어마스크 처리용 비트마스크 처리된 정수코드 반환
        /// </summary>
        public static int GetLayerMask(SystemEnum.Layer _layers)
        {
            return LayerMask.GetMask(LayerName(_layers));
        }

        /// <summary>
        /// 개별 레이어의 비트 시프트용 값
        /// </summary>
        public static int LayerIndex(SystemEnum.Layer _layer)
        {
            return LayerMask.NameToLayer(LayerName(_layer));
        }

        /// <summary>
        /// 개별 레이어의 이름
        /// </summary>
        public static string LayerName(SystemEnum.Layer _layer)
        {
            return LayerMask.LayerToName(ParseLayerIndex(_layer));
        }

        private static int ParseLayerIndex(SystemEnum.Layer _layer)
        {
            return (int)_layer;
        }
    }
}
