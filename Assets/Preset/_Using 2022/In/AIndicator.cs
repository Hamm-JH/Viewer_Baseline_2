using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Indicator
{
    public abstract class AIndicator : MonoBehaviour, IIndicator
    {
        // 표시기 내부의 요소 리스트
        protected List<Element.AElement> elements;
        //protected Manager.ViewSceneStatus sceneStatus;
        [SerializeField] protected RectTransform ElementPanel;

        public List<Element.AElement> Elements
        {
            get => elements;
            set => elements = value;
        }

        // 구현 코드의 타입 명시
        public IndicatorType Type { get; set; }

        // 개별 표시기의 타이틀 문자열
        public TMPro.TextMeshProUGUI titleText;

        private void Awake()
        {
            elements = new List<Element.AElement>();
        }

        /// <summary>
        /// UIManager 내부의 개별 요소 분류
        /// </summary>
        /// <param name="issue"></param>
        public abstract void SetPanelElements(List<Issue.AIssue> _issue);

        /// <summary>
        /// 표시기의 타이틀텍스트 설정
        /// </summary>
        protected abstract void SetTitleText();

        /// <summary>
        /// 내부 요소 비우기
        /// </summary>
        protected abstract void ClearElements();

        /// <summary>
        /// 내부 요소 설정
        /// </summary>
        /// <param name="_issue"></param>
        protected abstract void SetElements(List<Issue.AIssue> _issue);
    }
}
