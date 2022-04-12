using Data.API;
using Definition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View;

namespace Module.UI
{
    public partial class UITemplate_SmartInspect : AUI
    {
        /// <summary>
        /// API에서 주소값을 받아온다.
        /// </summary>
        /// <param name="_data"></param>
        public override void API_GetAddress(AAPI _data)
        {
            Initialize_API_GetAddress(_data);
        }

        public void API_getMainTexture(Texture2D _image)
        {

        }
    }
}
