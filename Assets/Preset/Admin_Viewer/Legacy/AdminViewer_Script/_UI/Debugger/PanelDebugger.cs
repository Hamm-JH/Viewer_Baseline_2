using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Debugger
{
    public class PanelDebugger : MonoBehaviour
    {
        public RectTransform target;

        public override string ToString()
        {
            string s =
                $"{nameof(target.gameObject.name)} {nameof(target.sizeDelta)} : {target.sizeDelta}\n" +
                $"{nameof(target.gameObject.name)} {nameof(target.anchoredPosition)} : {target.anchoredPosition}\n" +
                $"{nameof(target.gameObject.name)} {nameof(target.rect)} : {target.rect}"
                ;
            return s;
        }
    }
}
