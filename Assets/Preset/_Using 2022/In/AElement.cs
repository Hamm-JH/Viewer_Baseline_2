using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Indicator.Element
{
    public class AElement : MonoBehaviour, IElement,
        IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {
        public ElementType Type { get; set; }
        public bool isSelected;

        public virtual void SetElement(params object[] arg)
        {

        }

        public virtual void SetElement(Issue.AIssue _issue, params object[] arg)
        {

        }

        #region Event

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
#if UNITY_EDITOR
            //EditDebug.PrintEvent_DEFAULT("pointer enter");
#endif
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
#if UNITY_EDITOR
            //EditDebug.PrintEvent_DEFAULT("pointer exit");
#endif
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
#if UNITY_EDITOR
            //EditDebug.PrintEvent_DEFAULT("pointer up");
#endif
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
#if UNITY_EDITOR
            //EditDebug.PrintEvent_DEFAULT("pointer click");
#endif
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
#if UNITY_EDITOR
            //EditDebug.PrintEvent_DEFAULT("pointer down");
#endif
        }

        #endregion

        /// <summary>
        /// 리스트 클릭시 리스트 요소들의 시각화 상태를 변경한다.
        /// </summary>
        /// <param name="isMe"></param>
        public void ToggleRecordColors(bool isMe)
        {
            Image img;

            if(transform.TryGetComponent<Image>(out img))
            {
                img.color =
                    isMe ?
                    new Color((float)0xef / 0xff, (float)0xe5 / 0xff, (float)0x5b / 0xff, 1) :
                    new Color(1, 1, 1, 1);
            }
        }
    }
}
