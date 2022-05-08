using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management.Content
{
    public class SmartInspectManager : ContentManager
    {
        public override void OnCreate()
        {
            base.OnCreate();

            // DOTween √ ±‚»≠
            DOTween.Init();
        }
    }
}
