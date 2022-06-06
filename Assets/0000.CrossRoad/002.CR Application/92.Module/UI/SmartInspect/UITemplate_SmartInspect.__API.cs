﻿using Data.API;
using Definition;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using View;

namespace Module.UI
{
    public partial class UITemplate_SmartInspect : RootUI
    {
        /// <summary>
        /// API에서 주소값을 받아온다.
        /// </summary>
        /// <param name="_data"></param>
        public void API_GetAddress(AAPI _data)
        {
            Initialize_API_GetAddress(_data);
        }

        /// <summary>
        /// API에서 주 이미지를 가져온다.
        /// </summary>
        /// <param name="_image"></param>
        public void API_getMainTexture(Texture2D _image)
        {
            General_SetMainImage(_image);
        }

        //public void API_GetHistoryData(DataTable _table)
        //{

        //}
    }
}
