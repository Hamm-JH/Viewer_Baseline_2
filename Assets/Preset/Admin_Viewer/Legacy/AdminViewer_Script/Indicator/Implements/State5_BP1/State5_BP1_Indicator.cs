using Issue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Indicator
{
    public class State5_BP1_Indicator : AIndicator
    {
        [SerializeField] private float maxLineHeight;       // 최대 라인높이
        [SerializeField] private int maxCount;              // 최대 개수

        [SerializeField] private RectTransform baseInPanel;

        [SerializeField] private State5_BP1_Indicator l1Indicator;

        [SerializeField] private List<GameObject> togglesBeforeCapture;

        public List<GameObject> TogglesBeforeCapture
        {
            get => togglesBeforeCapture;
            //set => togglesBeforeCapture = value;
        }

        #region Count Index

        [Header("실제 존재하는 손상/보수의 개수")]
        [SerializeField] private List<int> dmgIndexes;
        [SerializeField] private List<int> rcvIndexes;

        #endregion

        #region Count Texts
        [Header("총 손상/보수 개수")]
        [SerializeField] private TextMeshProUGUI TotalDmgCountText;
        [SerializeField] private TextMeshProUGUI TotalRcvCountText;

        [Header("각 이슈별 손상/보수 개수")]
        [SerializeField] private List<TextMeshProUGUI> dmgCounts;
        [SerializeField] private List<TextMeshProUGUI> rcvCounts;
        #endregion

        #region Count Lines

        [Header("각 이슈별 손상/보수 그래프")]
        [SerializeField] private List<RectTransform> dmgLines;
        [SerializeField] private List<RectTransform> rcvLines;

        #endregion

        public override void SetPanelElements(List<AIssue> _issue)
        {
            Debug.Log($"B1 RS rect : {baseInPanel.sizeDelta}");

            // 캡쳐용 패널 초기화
            l1Indicator.SetSubElement(_issue);

            ClearElements();

            SetElements(_issue);

            Manager.UIManager.Instance.GetRoutineCode(IndicatorType.State5_BP1);
        }

        // 캡쳐용 패널 초기화
        public void SetSubElement(List<AIssue> _issue)
        {
            ClearElements();

            SetElements(_issue);
        }

        protected override void ClearElements()
        {
            maxCount = 0;

            dmgIndexes.Clear();
            rcvIndexes.Clear();

            // 손상 분류만큼 손상/보강 리스트 재할당
            for (int i = 0; i < 4; i++)
            {
                dmgIndexes.Add(0);
                rcvIndexes.Add(0);
            }
        }

        protected override void SetElements(List<AIssue> _issue)
        {
            maxLineHeight = baseInPanel.sizeDelta.y - 33f;  // 그래프의 최대높이 제한 (0 ~ 최대높이 사이)

            SetIssueIndexes(_issue);  // 손상/보강 정보들을 인덱스 별로 재분류

            maxCount = SetMaxCount();   // 최대수 할당

            SetTotalCountText();        // 손상/보강 총 개수 텍스트 할당

            SetCountsPerIndexText();    // 손상/보강 종류별 텍스트 할당

            SetHeightsPerLines();       // 손상/보강 종류별 그래프 길이 할당
        }

        protected override void SetTitleText()
        {
            throw new System.NotImplementedException();
        }

        #region implements in SetElements
        private void SetIssueIndexes(List<AIssue> _issue)
        {
            int index = _issue.Count;
            for (int i = 0; i < index; i++)
            {
                int issueIndex = (int)_issue[i].IssueCodes;

                if(issueIndex >= 0 && issueIndex < 4)
                {
                    if( _issue[i].GetType() == typeof(DamagedIssue))
                    {
                        dmgIndexes[(int)_issue[i].IssueCodes]++;
                    }
                    else if( _issue[i].GetType() == typeof(RecoveredIssue))
                    {
                        rcvIndexes[(int)_issue[i].IssueCodes]++;
                    }
                }
            }
        }

        private int SetMaxCount()
        {
            int result = 0;

            int index = dmgIndexes.Count;
            for (int i = 0; i < index; i++)
            {
                result = result < dmgIndexes[i] ? dmgIndexes[i] : result;
            }

            index = rcvIndexes.Count;
            for (int i = 0; i < index; i++)
            {
                result = result < rcvIndexes[i] ? rcvIndexes[i] : result;
            }

            return result;
        }

        private void SetTotalCountText()
        {
            int maxDmgCount = 0;
            int maxRcvCount = 0;

            int index = dmgIndexes.Count;
            for (int i = 0; i < index; i++)
            {
                maxDmgCount += dmgIndexes[i];
            }

            index = rcvIndexes.Count;
            for (int i = 0; i < index; i++)
            {
                maxRcvCount += rcvIndexes[i];
            }

            TotalDmgCountText.text = string.Format("{0:00}", maxDmgCount);
            TotalRcvCountText.text = string.Format("{0:00}", maxRcvCount);
        }

        private void SetCountsPerIndexText()
        {
            if(dmgCounts.Count == dmgIndexes.Count)
            {
                int index = dmgCounts.Count;
                for (int i = 0; i < index; i++)
                {
                    dmgCounts[i].text = string.Format("{0:00}", dmgIndexes[i]);
                }
            }

            if(rcvCounts.Count == rcvIndexes.Count)
            {
                int index = rcvCounts.Count;
                for (int i = 0; i < index; i++)
                {
                    rcvCounts[i].text = string.Format("{0:00}", rcvIndexes[i]);
                }
            }
        }

        private void SetHeightsPerLines()
        {
            //maxCount 최대 개수
            //maxHeight 최대 높이
            Debug.Log($"line 0 sizeDelta : {dmgLines[0].sizeDelta}");
            Debug.Log($"line 0 local position: {dmgLines[0].localPosition}");
            Debug.Log($"line 0 position: {dmgLines[0].position}");
            Debug.Log($"line 0 anchored position : {dmgLines[0].anchoredPosition}");

            // 최대 인덱스에서 특정 인덱스와 비교해 비율을 측정한다.
            // 최대 높이 * 인덱스 비율 = 0 ~ 1 비율 계산

            // sizeDelta.y에 최대 (높이 * 비율) 계산결과 반영
            // anchoredPosition.y에 (높이 * 비율 / 2) 계산결과 반영

            int index = dmgIndexes.Count;
            for (int i = 0; i < index; i++)
            {
                int _index = dmgIndexes[i];     // 현재 인덱스
                float proportion = _index == 0 ? 0 : (float)_index / (float)maxCount;               // 최대수 대비 비율
                float proportionAppliedHeight = proportion == 0 ? 0 : proportion * maxLineHeight;   // 최대 높이에 비율 적용한 높이

                Vector2 deltaPos = dmgLines[i].sizeDelta;
                Vector2 anchorPos = dmgLines[i].anchoredPosition;

                dmgLines[i].sizeDelta = new Vector2(deltaPos.x, proportionAppliedHeight);
                dmgLines[i].anchoredPosition = new Vector2(anchorPos.x, proportionAppliedHeight / 2);
            }

            index = rcvIndexes.Count;
            for (int i = 0; i < index; i++)
            {
                int _index = rcvIndexes[i];     // 현재 인덱스
                float proportion = _index == 0 ? 0 : (float)_index / (float)maxCount;               // 최대수 대비 비율
                float proportionAppliedHeight = proportion == 0 ? 0 : proportion * maxLineHeight;   // 최대 높이에 비율 적용한 높이

                Vector2 deltaPos = rcvLines[i].sizeDelta;
                Vector2 anchorPos = rcvLines[i].anchoredPosition;

                rcvLines[i].sizeDelta = new Vector2(deltaPos.x, proportionAppliedHeight);
                rcvLines[i].anchoredPosition = new Vector2(anchorPos.x, proportionAppliedHeight / 2);
            }
        }

        #endregion

        private void Awake()
        {
            dmgIndexes = new List<int>();
            rcvIndexes = new List<int>();
        }
    }
}
