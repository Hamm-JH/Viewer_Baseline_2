using Issue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Indicator
{
    public class State5_MP1_Indicator : AIndicator
    {
        public List<GameObject> issueTables;
        public List<bool> issueChecker;
        public List<TextMeshProUGUI> titleCounttexts;
        public List<Image> tagImages;

        public List<AIssue> dmgIssues;
        public List<AIssue> rcvIssues;
        public List<AIssue> reinIssues;

        public override void SetPanelElements(List<AIssue> _issue)
        {
            Manager.UIManager.Instance.GetRoutineCode(IndicatorType.State5_MP1);

            // 기존 요소 제거
            ClearElements();

            // 데이터 업데이트 (손상 수, 보수 수, 보강 수)
            SetIssueList(_issue);

            // 태그 스위칭
            SwitchTag(0);

            // 타이틀 태그별 카운트 설정
            SetTitleText();

            // 객체 내부테이블 요소 할당
            SetElements(_issue);
        }

        protected override void ClearElements()
        {
            int index = ElementPanel.childCount;
            List<Transform> childs = new List<Transform>();
            //Transform[] transforms = ElementPanel.GetComponentsInChildren<Transform>();

            for (int i = 0; i < index; i++)
            {
                childs.Add(ElementPanel.GetChild(i));
            }

            for (int i = 0; i < childs.Count; i++)
            {
                Destroy(childs[i].gameObject);
            }

            dmgIssues.Clear();
            rcvIssues.Clear();
            reinIssues.Clear();

            // 이슈테이블 객체 리스트 비우기
            issueTables.Clear();

            issueTables.Add(null);      // Dmg
            issueTables.Add(null);      // Rcv
            issueTables.Add(null);      // Rein

            // 이슈체커 초기화
            issueChecker.Clear();

            issueChecker.Add(false);
            issueChecker.Add(false);
            issueChecker.Add(false);
        }

        /// <summary>
        /// 이슈 정보 분류작업
        /// </summary>
        /// <param name="_issue"></param>
        public void SetIssueList(List<AIssue> _issue)
        {
            GameObject dmgTable = Resources.Load<GameObject>("Indicator/State5_Dmg_Table");
            GameObject rcvTable = Resources.Load<GameObject>("Indicator/State5_Rcv_Table");
            GameObject reinTable = Resources.Load<GameObject>("Indicator/State5_Rein_Table");

            // 이슈테이블의 개수가 3개가 되기 전까지 코드 실행
            while(issueTables.Count < 3)
            {
                issueTables.Add(null);
            }
            // 이슈체커 카운트 동기화
            while(issueChecker.Count < 3)
            {
                issueChecker.Add(false);
            }

            issueTables[0] = Instantiate<GameObject>(dmgTable, ElementPanel);
            issueTables[1] = Instantiate<GameObject>(rcvTable, ElementPanel);
            issueTables[2] = Instantiate<GameObject>(reinTable, ElementPanel);

            int index = _issue.Count;
            for (int i = 0; i < index; i++)
            {
                if (_issue[i].GetType() == typeof(DamagedIssue))
                {
                    dmgIssues.Add(_issue[i]);
                    continue;
                }

                if (_issue[i].GetType() == typeof(RecoveredIssue))
                {
                    rcvIssues.Add(_issue[i]);
                    continue;
                }

                if (_issue[i].GetType() == typeof(ReinforcementIssue))
                {
                    reinIssues.Add(_issue[i]);
                    continue;
                }
            }
        }

        #region 
        protected override void SetElements(List<AIssue> _issue)
        {
            issueTables[0].GetComponent<Element.TableElement>().CreateRecords<Issue.DamagedIssue>(_issue);
            issueTables[1].GetComponent<Element.TableElement>().CreateRecords<Issue.RecoveredIssue>(_issue);
            issueTables[2].GetComponent<Element.TableElement>().CreateRecords<Issue.ReinforcementIssue>(_issue);
        }

        protected override void SetTitleText()
        {
            titleCounttexts[0].text = string.Format("{0:000}", dmgIssues.Count);
            titleCounttexts[1].text = string.Format("{0:000}", rcvIssues.Count);
            titleCounttexts[2].text = string.Format("{0:000}", reinIssues.Count);
        }
        #endregion

        private void Awake()
        {
            issueTables = new List<GameObject>();
            issueChecker = new List<bool>();

            dmgIssues = new List<AIssue>();
            rcvIssues = new List<AIssue>();
            reinIssues = new List<AIssue>();
        }

        #region events

        public void SwitchTables(int _index)
        {
            int index = issueTables.Count;

            if(_index >= 0 && _index < index)
            {
                SwitchTag(_index);
                SwitchPanel(_index);
            }
        }

        private void SwitchTag(int _index)
        {
            // 태그 색 변경
            int index = tagImages.Count;
            for (int i = 0; i < index; i++)
            {
                tagImages[i].color = new Color(
                    i == _index ? 0xe6 / 255f : 0xf7 / 255f,
                    i == _index ? 0xe9 / 255f : 0xf8 / 255f,
                    i == _index ? 0xf0 / 255f : 0xfa / 255f
                    );
            }
        }

        private void SwitchPanel(int _index)
        {
            int index = issueTables.Count;
            for (int i = 0; i < index; i++)
            {
                issueTables[i].SetActive(i == _index);
            }
        }

        #endregion
    }
}
