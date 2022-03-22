using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UI
{
    /// <summary>
    /// 싱글 인스턴스에 생성해둔 색 값을 받아가는 클래스
    /// </summary>
    public class Colors
    {
        #region Enums

        public enum TargetLayer
        {
            Top_Navigation
        }

        public enum TargetItem
        {
            Navigation_Button,
            Navigation_Arrow,
        }

        public enum SetOption
        {
            On,
            Off,
        }

        public enum Palette
        {
            White = 0,
            CustomCyan1 = 1,
            CustomCyan2 = 2,
            CustomRed1 = 3,
            CustomGrey = 4,
            CustomBlue1 = 5,
            CustomPurple1 = 6,
        }

        #endregion

        #region Instance

        private static Colors instance;

        public static Colors Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Colors();
                }
                return instance;
            }
        }

        #endregion

        #region 초기화 & 색 저장

        /// <summary>
        /// 색 테이블 (UI 색데이터 저장용)
        /// </summary>
        private static Dictionary<TargetLayer,
                        Dictionary<TargetItem,
                        Dictionary<SetOption, Color>>> colorTable;

        public Colors()
        {
            InitColorTable();

            CustomInitNavigationColor();
        }

        /// <summary>
        /// colorTable 변수 초기화
        /// </summary>
        private void InitColorTable()
        {
            colorTable = new Dictionary<TargetLayer, Dictionary<TargetItem, Dictionary<SetOption, Color>>>();

            foreach (TargetLayer item in Enum.GetValues(typeof(TargetLayer)))
            {
                colorTable.Add(item, new Dictionary<TargetItem, Dictionary<SetOption, Color>>());

                foreach (TargetItem _item in Enum.GetValues(typeof(TargetItem)))
                {
                    colorTable[item].Add(_item, new Dictionary<SetOption, Color>());

                    foreach (SetOption __item in Enum.GetValues(typeof(SetOption)))
                    {
                        colorTable[item][_item].Add(__item, new Color());
                    }
                }
            }
        }

        /// <summary>
        /// Navigation 영역 필요색상 지정 초기화
        /// </summary>
        private void CustomInitNavigationColor()
        {
            //colorTable[TargetLayer.Top_Navigation]
            //            [TargetItem.Navigation_Button]
            //            [SetOption.On]

            colorTable[TargetLayer.Top_Navigation]
                        [TargetItem.Navigation_Button]
                        [SetOption.On] = new Color(1, 1, 1, 1);

            colorTable[TargetLayer.Top_Navigation]
                        [TargetItem.Navigation_Button]
                        [SetOption.Off] = new Color(85 / 255f, 85 / 255f, 85 / 255f, 1);

            colorTable[TargetLayer.Top_Navigation]
                        [TargetItem.Navigation_Arrow]
                        [SetOption.On] = new Color(0xb8 / 255f, 0xff / 255f, 0xbe / 255f, 1);
            //new Color(76 / 255f, 110 / 255f, 245 / 255f, 1);

            colorTable[TargetLayer.Top_Navigation]
                        [TargetItem.Navigation_Arrow]
                        [SetOption.Off] = new Color(230 / 255f, 233 / 255f, 240 / 255f, 1);
            //new Color(230 / 255f, 233 / 255f, 240 / 255f, 1);
        }

        #endregion

        #region Using area

        /// <summary>
        /// On 또는 Off 값을 가지는 단일 개체의 색상값 반환
        /// </summary>
        /// <param name="_layer"></param>
        /// <param name="_item"></param>
        /// <param name="_isOn"></param>
        /// <returns></returns>
        public Color Set(TargetLayer _layer, TargetItem _item, bool _isOn)
        {
            return colorTable[_layer][_item][_isOn ? SetOption.On : SetOption.Off];
        }

        public Color Set(Palette _pick)
        {
            switch (_pick)
            {
                case Palette.White:
                    return new Color(1, 1, 1, 1);

                case Palette.CustomCyan1:
                    return new Color((float)0x00 / 0xff, (float)0x9d / 0xff, (float)0xd8 / 0xff, 1);

                case Palette.CustomCyan2:
                    return new Color((float)0x25 / 0xff, (float)0x8c / 0xff, (float)0xef / 0xff, 1);

                case Palette.CustomRed1:
                    return new Color((float)0xfd / 0xff, (float)0x11 / 0xff, (float)0x6e / 0xff, 1);

                case Palette.CustomGrey:
                    return new Color((float)0x50 / 0xff, (float)0x5a / 0xff, (float)0x66 / 0xff, 1);

                case Palette.CustomBlue1:
                    return new Color((float)0x4c / 0xff, (float)0x6e / 0xff, (float)0xf5 / 0xff, 1);

                case Palette.CustomPurple1:
                    return new Color(0x55 / 255f, 0xd2 / 255f, 0x3c / 255f, 1);

                default:
                    return new Color(1, 1, 1, 1);
            }
        }
        
        public int[] GetIntCode(Palette _pick)
        {
            int[] values = new int[3] { 0, 0, 0 };

            switch (_pick)
            {
                case Palette.White:
                    {
                        values[0] = 0xff;
                        values[1] = 0xff;
                        values[2] = 0xff;
                        return values;
                    }

                case Palette.CustomCyan1:
                    {
                        values[0] = 0x00;
                        values[1] = 0x9d;
                        values[2] = 0xd8;
                        return values;
                    }

                case Palette.CustomCyan2:
                    {
                        values[0] = 0x25;
                        values[1] = 0x8c;
                        values[2] = 0xef;
                        return values;
                    }

                case Palette.CustomRed1:
                    {
                        values[0] = 0xfd;
                        values[1] = 0x11;
                        values[2] = 0x6e;
                        return values;
                    }

                case Palette.CustomGrey:
                    {
                        values[0] = 0x50;
                        values[1] = 0x5a;
                        values[2] = 0x66;
                        return values;
                    }

                case Palette.CustomBlue1:
                    {
                        values[0] = 0x4c;
                        values[1] = 0x6e;
                        values[2] = 0xf5;
                        return values;
                    }

                case Palette.CustomPurple1:
                    {
                        values[0] = 0x55;
                        values[1] = 0xd2;
                        values[2] = 0x3c;
                        return values;
                    }

                default:
                    {
                        values[0] = 0xff;
                        values[1] = 0xff;
                        values[2] = 0xff;
                        return values;
                    }
            }
        }

        public string GetStringCode(Palette _pick)
        {
            switch (_pick)
            {
                case Palette.White:
                    return "ffffff";

                case Palette.CustomCyan1:
                    return "009dd8";

                case Palette.CustomCyan2:
                    return "258cef";

                case Palette.CustomRed1:
                    return "FD116E";

                case Palette.CustomGrey:
                    return "505a66";

                case Palette.CustomBlue1:
                    return "4c6ef5";

                case Palette.CustomPurple1:
                    return "55d23c";

                default:
                    return "ffffff";
            }
        }
        #endregion
    }
}
