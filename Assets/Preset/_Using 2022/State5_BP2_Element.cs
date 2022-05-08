using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Issue;
using TMPro;
using static AdminViewer.UI.Ad_Panel;

namespace Indicator.Element
{
    public class State5_BP2_Element : AElement
    {

        [SerializeField] int yearID;
        [SerializeField] ViewTypeGraph viewOption;
        [SerializeField] RectTransform m_rectTransform;
        [SerializeField] int[] _minMaxYear;

        // 수평선 텍스트 모음
        [SerializeField] List<TextMeshProUGUI> layerContexts;

        /// <summary>
        /// 손상 / 보강정보별로
        /// 날짜 분류 index / 개수 모음 딕셔너리
        /// </summary>
        private Dictionary<string, Dictionary<int, int>> issuesByDays;

        [SerializeField]
        private List<sElement> elements;

        /// <summary>
        /// 손상정보의 개별 요소 클래스
        /// </summary>
        public class sElement
        {
            public DateTime startDate;
            public DateTime endDate;
            public int count;

            public new void ToString()
            {
                Debug.Log($"St date : {startDate}");
                Debug.Log($"Ed date : {endDate}");
                Debug.Log($"count : {count}");
            }
        }

        private Dictionary<IssueCode, List<sElement>> dElements;
        //private List<sElement> dCracks;
        //private List<sElement> dEfflores;
        //private List<sElement> dSpalling;
        //private List<sElement> dBreakage;

        // 손상 / 보수정보 할당이 끝났는지 확인
        //private bool dmgEnd;
        //private bool rcvEnd;

        /// <summary>
        /// 연도 할당 프로퍼티
        /// </summary>
        public int YearID
        {
            get => yearID;
            set => yearID = value;
        }

        public ViewTypeGraph ViewOption
        {
            get => viewOption;
            set => viewOption = value;
        }

        public int[] MinMaxYear
        {
            get => _minMaxYear;
            set => _minMaxYear = value;
        }

        public IEnumerator SetGraphPerYear(int currentYear, int[] minMaxYear, ViewTypeGraph viewType,
            Dictionary<IssueClass, Dictionary<IssueCode, Dictionary<DateTime, int>>> _elements)
        {
            // 이 패널에 할당된 연도를 할당한다.
            YearID = currentYear;
            ViewOption = viewType;
            MinMaxYear = minMaxYear;

            // O 수평선 정보표시 텍스트 변경
            SetLayerConText(viewType);

            // 옵션별 날짜 정렬
            SetValuesByOption(ViewOption, ref _elements);

            // 보수정보는 이대로도 구현 가능하다.
            // 손상정보만 새 리스트를 만들어줘야 한다.
            dElements = GetAssignedIssues(ref _elements);

            SetDmgGraph(ref dElements);
            SetRcvGraph(ref _elements);

            gameObject.SetActive(minMaxYear[1] == YearID);

            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            

            yield break;
        }

        /// <summary>
        /// 열 표시정보 변경
        /// </summary>
        /// <param name="viewType"></param>
        private void SetLayerConText(ViewTypeGraph viewType)
        {
            if(viewType == ViewTypeGraph.Year_5)
            {
                int index = layerContexts.Count;
                for (int i = 0; i < index; i++)
                {
                    layerContexts[i].text = $"{YearID - (4 - i)}년";
                }
            }
            else if(viewType == ViewTypeGraph.Year_10)
            {
                //YearID
                int index = layerContexts.Count;
                for (int i = 0; i < index; i++)
                {
                    layerContexts[i].text = $"{YearID - (9 - i)}년";
                }
            }
            else if(viewType == ViewTypeGraph.Year_Total)
            {
                //int index = layerContexts.Count;
                //for (int i = 0; i < index; i++)
                //{
                //    layerContexts[i].text = $"{YearID - (9 - i)}년";
                //}
            }
        }

        #region Set Values By Option 옵션별 날짜 재정렬

        /// <summary>
        /// 옵션별로 날짜를 재정렬한다.
        /// </summary>
        /// <param name="_type"></param>
        /// <param name="_elements"></param>
        private void SetValuesByOption(ViewTypeGraph _type, ref Dictionary<IssueClass, Dictionary<IssueCode, Dictionary<DateTime, int>>> _elements)
        {
            foreach(IssueClass key in _elements.Keys)
            {
                foreach(IssueCode _key in _elements[key].Keys)
                {
                    Dictionary<DateTime, int> _inElements = _elements[key][_key];
                    SetDateTimeByOption(_type, ref _inElements);
                }
            }
        }

        private void SetDateTimeByOption(ViewTypeGraph _type, ref Dictionary<DateTime, int> _inElements)
        {
            Dictionary<DateTime, int> replacedDic = new Dictionary<DateTime, int>();

            // 특정 날짜로 카운트 값을 모아둔다.
            foreach(DateTime key in _inElements.Keys)
            {
                //DateTime target = new DateTime(key.Year, key.Month, key.Day);
                DateTime replacedDateTime = ReplaceDateTime(_type, key);
                //_inElements[key]
                if(replacedDic.ContainsKey(replacedDateTime))
                {
                    replacedDic[replacedDateTime] += _inElements[key];
                }
                else
                {
                    replacedDic.Add(replacedDateTime, _inElements[key]);
                }
            }

            // 기존 요소변수를 초기화한다.
            _inElements.Clear();

            // 기존 요소변수에 날짜 변환된 값을 넣는다.
            foreach(DateTime key in replacedDic.Keys)
            {
                // 합산값이 0이 아닌 경우에만 값을 넣는다.
                if(replacedDic[key] != 0)
                {
                    _inElements.Add(key, replacedDic[key]);
                }
            }
        }

        private DateTime ReplaceDateTime(ViewTypeGraph _type, DateTime _target)
        {
            DateTime replaced;

            if(_type == ViewTypeGraph.Year_1)
            {
                int totalDays = DateTime.DaysInMonth(_target.Year, _target.Month);
                int percent = (int)(((float)_target.Day / totalDays) * 100);

                // 퍼센트가 절반 아래인 경우 (월 상반기)
                if(percent / 50 == 0)
                {
                    replaced = new DateTime(_target.Year, _target.Month, 1);
                }
                // 퍼센트가 절반 위인 경우 (월 하반기)
                else
                {
                    int halfOfTotalDays = totalDays / 2;
                    replaced = new DateTime(_target.Year, _target.Month, halfOfTotalDays);
                }
            }
            else if(_type == ViewTypeGraph.Year_5)
            {
                int targetMonth = _target.Month;
                int percent = (int)(((float)targetMonth / 12) * 100);

                // 퍼센트가 절반 아래인 경우 (연 상반기)
                if(percent / 50 == 0)
                {
                    replaced = new DateTime(_target.Year, 1, 1);
                }
                else
                {
                    replaced = new DateTime(_target.Year, 7, 1);
                }

            }
            else if(_type == ViewTypeGraph.Year_10)
            {
                replaced = new DateTime(_target.Year, 1, 1);
            }
            // 10년 단위와 동일하게 배치
            else
            {
                //Debug.Log($"target y : {_target.Year}");
                //Debug.Log($"last y : {MinMaxYear[1]}");
                //Debug.Log($"last - target : {MinMaxYear[1] - _target.Year}");
                // 0 1 2 3 4 5 6 7 8 9 == 최신연도 - 10 * 1
                int repIndex = (((MinMaxYear[1] - _target.Year) / 10) + 1) * 10 - 1;
                replaced = new DateTime(MinMaxYear[1] - repIndex, 1, 1);
            }

            return replaced;
        }

        #endregion

        #region 각 손상 타입별로 정렬된 데이터를 만들어낸다.
        private Dictionary<IssueCode, List<sElement>> GetAssignedIssues(ref Dictionary<IssueClass, Dictionary<IssueCode, Dictionary<DateTime, int>>> _elements)
        {
            Dictionary<IssueCode, List<sElement>> result = new Dictionary<IssueCode, List<sElement>>();

            // 각 손상별로 정렬된 요소 리스트를 받는다.
            result.Add(IssueCode.Crack, GetTargetElement_PerSingleIssue(_elements[IssueClass.Dmg][IssueCode.Crack]));
            result.Add(IssueCode.Efflorescense, GetTargetElement_PerSingleIssue(_elements[IssueClass.Dmg][IssueCode.Efflorescense]));
            result.Add(IssueCode.Spalling, GetTargetElement_PerSingleIssue(_elements[IssueClass.Dmg][IssueCode.Spalling]));
            result.Add(IssueCode.Breakage, GetTargetElement_PerSingleIssue(_elements[IssueClass.Dmg][IssueCode.Breakage]));

            return result;
        }

        /// <summary>
        /// 손상 타입별로 요소 리스트를 할당한다.
        /// </summary>
        /// <param name="_element"></param>
        /// <returns></returns>
        private List<sElement> GetTargetElement_PerSingleIssue(Dictionary<DateTime, int> _element)
        {
            // 최종 출력
            List<sElement> _list = new List<sElement>();

            //issueElement.Keys.ToArray<DateTime>().Length;
            // 패널의 타겟 연도에 맞는 요소 할당용
            // 패널의 타겟 연도에 맞는 요소 숫자 검출까지 수행
            Dictionary<DateTime, int> matchedDates = GetMatchedDate(_element);

            // 전년도 인덱스에 대한 계산 수행
            // - 앞에 있던 이슈들을 순차적으로 더한 결과 count 값이 0 초과인 경우
            // - 타겟 해로 넘어오면서 초과된 값을 올해 1.1일에 할당하는 메서드 필요
            int prevYearCount = GetPrevDatesCount(_element);

            // 전년도 인덱스값의 합이 0 위일 경우
            // - 새 요소 추가
            CheckAndAddDateIndex(ref matchedDates, prevYearCount);

            // 요소 인스턴스 리스트를 생성합니다.
            _list = SetElementList(ref matchedDates);

            {
                //if(matchedDates.Keys.Count > 0)
                //{



                //    return _list;

                //    {
                //        // 전년도 이슈합과 타겟 연도 수집된 변수를 보내 올해 카운트 최종값을 구한다.
                //        //int currLastCount = GetCurrAfterDatesCount(matchedDates, prevYearCount);

                //        //// 타겟 연도의 최종값이 0 초과일 경우
                //        //if(currLastCount > 0)
                //        //{
                //        //    // 1.1 2
                //        //    // 5.3 5
                //        //    // 최종 7임
                //        //}


                //        // 매칭된 dic 필요
                //        // 

                //        // 현재년도의 시작 카운트값
                //        // - 전년도 카운트 값에서 연결되서 계산 수행
                //        //int currentYearCount = prevYearCount;

                //        // ---

                //        // 정렬된 날짜 키를 반환한다.
                //        //DateTime[] keys = matchedDates.Keys.OrderBy(x => x.Date).ToArray<DateTime>();

                //        //int currentDateCount = matchedDates[keys[0]];       // 초기값은 첫 이슈일자의 카운트
                //        //int index = keys.Length;
                //        //for (int i = 0; i < index - 1; i++)
                //        //{
                //        //    // 현재 이슈 일자
                //        //    //matchedDates[keys[i]];
                //        //    // 바로 다음 이슈 일자
                //        //    //matchedDates[keys[i + 1]];
                //        //    // 현재 이슈 일자의 카운트
                //        //    //currentDateCount;

                //        //    sElement element = new sElement();
                //        //    element.startDate = keys[i];
                //        //    element.endDate = keys[i + 1];
                //        //    element.count = matchedDates[keys[i]];
                //        //}
                //    }
                //}
                //else
                //{
                //    // 현재 연도에 이슈가 없을 경우
                //    // - 전년도에 이슈 카운트를 확인해야 함.
                //}
            }

            return _list;
        }
        #endregion

        #region 1년 단위로 올해의 값이 있는지 확인
        /// <summary>
        /// 이 패널의 연도와 같은 해의 이슈가 있는지 검색, 반환한다.
        /// </summary>
        /// <param name="_element"></param>
        /// <returns></returns>
        private Dictionary<DateTime, int> GetMatchedDate(Dictionary<DateTime, int> _element)
        {
            Dictionary<DateTime, int> result = new Dictionary<DateTime, int>();

            foreach(DateTime date in _element.Keys)
            {
                if(ViewOption == ViewTypeGraph.Year_1)
                {
                    if(date.Year == YearID)
                    {
                        result.Add(date, _element[date]);
                    }
                }
                else if(ViewOption == ViewTypeGraph.Year_5)
                {
                    if(date.Year <= YearID && date.Year > YearID - 5)
                    {
                        result.Add(date, _element[date]);
                    }
                }
                else if(ViewOption == ViewTypeGraph.Year_10)
                {
                    if(date.Year <= YearID && date.Year > YearID - 10)
                    {
                        result.Add(date, _element[date]);
                    }
                }
                else if(ViewOption == ViewTypeGraph.Year_Total)
                {
                    if (date.Year <= YearID && date.Year > YearID - 100)
                    {
                        result.Add(date, _element[date]);
                    }
                }
            }

            return result;
        }
        #endregion

        #region Get prev index & set index
        /// <summary>
        /// 이 패널의 연도id 이전의 연도들의 카운트합 반환
        /// </summary>
        /// <param name="_element"></param>
        /// <returns></returns>
        private int GetPrevDatesCount(Dictionary<DateTime, int> _element)
        {
            int result = 0;

            foreach(DateTime date in _element.Keys)
            {
                int range = 0;
                switch(ViewOption)
                {
                    case ViewTypeGraph.Year_1:      range = 0;  break;
                    case ViewTypeGraph.Year_5:      range = 4;  break;
                    case ViewTypeGraph.Year_10:     range = 9;  break;
                    case ViewTypeGraph.Year_Total:  range = 99;  break;
                }

                // 패널 연도id - 구역 값보다 이전의 연도일 경우
                if (date.Year < YearID - range)
                {
                    result += _element[date];
                }
            }

            return result;
        }

        /// <summary>
        /// 전년도의 카운트를 확인하고 새 이슈 항목을 추가한다.
        /// </summary>
        /// <param name="_element"></param>
        /// <param name="prevCount"></param>
        private void CheckAndAddDateIndex(ref Dictionary<DateTime, int> _element, int prevCount)
        {
            if(prevCount > 0)
            {
                int firstYearIndex = 0;
                switch(ViewOption)
                {
                    case ViewTypeGraph.Year_1:      firstYearIndex = 0; break;
                    case ViewTypeGraph.Year_5:      firstYearIndex = 4; break;
                    case ViewTypeGraph.Year_10:     firstYearIndex = 9; break;
                    case ViewTypeGraph.Year_Total:  firstYearIndex = 99; break;
                }

                // 1.1일 신규 생성
                DateTime newStartDate = new DateTime(YearID - firstYearIndex, 1, 1);
                // 타겟 이슈모음에 이슈를 추가한다.
                if(_element.ContainsKey(newStartDate))
                {
                    _element[newStartDate] += prevCount;
                }
                else
                {
                    _element.Add(newStartDate, prevCount);
                }
            }
        }
        #endregion

        #region set graph index

        private List<sElement> SetElementList(ref Dictionary<DateTime, int> _elements)
        {
            List<sElement> result = new List<sElement>();

            DateTime[] keys = _elements.Keys.OrderBy(x => x.Date).ToArray<DateTime>();

            if(_elements.Count > 0)
            {
                int currentDateCount = _elements[keys[0]];  // 초기값은 첫 이슈일자의 카운트 할당

                int index = keys.Length;
                // 키 값이 2개 이상일 경우
                if(index >= 2)
                {
                    for (int i = 0; i < index - 1; i++)
                    {
                        if(currentDateCount > 0)
                        {
                            sElement fEl = new sElement();
                            fEl.startDate = keys[i];
                            fEl.endDate = keys[i + 1];
                            fEl.count = currentDateCount;

                            result.Add(fEl);
                        }


                        currentDateCount += _elements[keys[i+1]]; // 카운트 값을 다음 인덱스와 더한다.

                        // 마지막 반복문에 도달한 상황
                        if(i == index - 2)
                        {
                            // 최종 카운트가 0 위일 경우
                            if(currentDateCount > 0)
                            {
                                sElement fEl = new sElement();
                                fEl.startDate = keys[i+1];

                                DateTime nowTime = DateTime.Now;
                                //int currentYear = DateTime.Now.Year;
                                if (YearID != nowTime.Year)
                                {
                                    fEl.endDate = new DateTime(YearID, 12, 31);
                                }
                                else
                                {
                                    fEl.endDate = new DateTime(YearID, nowTime.Month, nowTime.Day);
                                    //if(ViewOption == ViewTypeGraph.Year_1)
                                    //{
                                    //}
                                }
                                fEl.count = currentDateCount;

                                result.Add(fEl);
                            }
                        }

                        // 0 1 2
                        // 0 1
                        //   1 2
                    }
                    return result;
                }
                // 인덱스 값이 하나밖에 없을 경우
                else if(index == 1)
                {
                    // 인덱스 값이 0 위일 경우
                    if(_elements[keys[0]] > 0)
                    {
                        sElement fEl = new sElement();
                        fEl.startDate = keys[0];

                        DateTime nowTime = DateTime.Now;
                        if(YearID != nowTime.Year)
                        {
                            fEl.endDate = new DateTime(YearID, 12, 31);
                        }
                        else
                        {
                            fEl.endDate = new DateTime(YearID, nowTime.Month, nowTime.Day);
                        }
                        fEl.count = _elements[keys[0]];

                        result.Add(fEl);
                    }

                    return result;
                }
            }

            return result;
        }

        #endregion

        // - 1년 배열 초기화 끝

        #region Draw rcv graph

        private void SetRcvGraph(ref Dictionary<IssueClass, Dictionary<IssueCode, Dictionary<DateTime, int>>> _elements)
        {
            foreach(IssueCode key in _elements[IssueClass.Rcv].Keys)
            {
                StartCoroutine(SetRcvGraphPerIssue(key, _elements[IssueClass.Rcv][key]));
            }
        }

        private IEnumerator SetRcvGraphPerIssue(IssueCode code, Dictionary<DateTime, int> _element)
        {
            yield return new WaitForEndOfFrame();

            string prefabName = GetPrefabName(IssueClass.Rcv, code);

            float rectWidth = m_rectTransform.rect.width;

            foreach(DateTime date in _element.Keys)
            {
                //date.Year == YearID
                if (IsValidYearBoundary(YearID, date.Year, ViewOption))
                {
                    float basePoint = GetRcvBasePoint(rectWidth, date);

                    GameObject obj = Instantiate<GameObject>(Resources.Load<GameObject>($"BP5_Icon/{prefabName}"), gameObject.transform);
                    obj.transform.localPosition = new Vector3(m_rectTransform.rect.xMin+6 + basePoint, 9, 0);

                    StartCoroutine(obj.GetComponent<UI.GraphCode>().SetIndexText(_element[date]));
                }
            }
        }

        private bool IsValidYearBoundary(int _yearID, int _targetYear, ViewTypeGraph _type)
        {
            bool isValid = false;

            if(_type == ViewTypeGraph.Year_1)
            {
                isValid = _targetYear == _yearID;
            }
            else if(_type == ViewTypeGraph.Year_5)
            {
                isValid = (_targetYear <= _yearID && _targetYear > _yearID - 5);
            }
            else if(_type == ViewTypeGraph.Year_10)
            {
                isValid = (_targetYear <= _yearID && _targetYear > _yearID - 10);
            }
            else if(_type == ViewTypeGraph.Year_Total)
            {
                isValid = (_targetYear <= _yearID && _targetYear > _yearID - 100);
            }

            return isValid;
        }

        /// <summary>
        /// 보수 정보의 기준위치 반환
        /// </summary>
        /// <param name="rectWidth"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        private float GetRcvBasePoint(float rectWidth, DateTime date)
        {
            float result = 0f;

            if(ViewOption == ViewTypeGraph.Year_1)
            {
                int day = date.DayOfYear;
                float aspectOfDay = (float)day / (DateTime.IsLeapYear(YearID) ? 366 : 365);

                result = (rectWidth-12) * aspectOfDay;
            }
            else
            {
                int yearCount = 0;
                switch(ViewOption)
                {
                    case ViewTypeGraph.Year_5: yearCount = 5; break;
                    case ViewTypeGraph.Year_10: yearCount = 10; break;
                    case ViewTypeGraph.Year_Total: yearCount = 100; break;
                }

                List<int> dayIndex = GetDayIndex(YearID, yearCount);
                int totalDayCount = GetTotalDayCount(dayIndex);

                // 주어진 날짜의 날짜 변환값을 구한다.
                int currDayCount = GetCurrDayCount(dayIndex, date);

                float ratioOfDay = (float)currDayCount / totalDayCount;

                result = (rectWidth - 12) * ratioOfDay;
            }
            //else if(ViewOption == ViewTypeGraph.Year_5)
            //{
            //    List<int> dayIndex = GetDayIndex(YearID, 5);
            //    int totalDayCount = GetTotalDayCount(dayIndex);

            //    // 주어진 날짜의 날짜 변환값을 구한다.
            //    int currDayCount = GetCurrDayCount(dayIndex, date);

            //    float ratioOfDay = (float)currDayCount / totalDayCount;
            //}
            //else if(ViewOption == ViewTypeGraph.Year_10)
            //{
            //    List<int> dayIndex = GetDayIndex(YearID, 10);
            //    int totalDayCount = GetTotalDayCount(dayIndex);

            //    int currDayCount = GetCurrDayCount(dayIndex, date);
            //}
            //else if(ViewOption == ViewTypeGraph.Year_Total)
            //{
            //    List<int> dayIndex = GetDayIndex(YearID, 10);
            //    int totalDayCount = GetTotalDayCount(dayIndex);

            //    int currDayCount = GetCurrDayCount(dayIndex, date);
            //}

            return result;
        }
        #endregion


        private List<int> GetDayIndex(int currYear, int _index)
        {
            List<int> result = new List<int>();

            for (int i = 0; i < _index; i++)
            {
                //result.Add(DateTime.IsLeapYear(currYear - (_index - 1 - i)) ? 366 : 365);
                result.Add(365);
            }

            return result;
        }

        private int GetTotalDayCount(List<int> _list)
        {
            int result = 0;

            int index = _list.Count;
            for (int i = 0; i < index; i++)
            {
                result += _list[i];
            }

            return result;
        }

        private int GetCurrDayCount(List<int> _yearIndexes, DateTime targetDate)
        {
            int result = 0;

            List<int> years = GetYearsIndex();

            //Debug.Log($"years : {years.Count}");
            Debug.Log(targetDate.Year);

            int index = _yearIndexes.Count;
            for (int i = 0; i < index; i++)
            {
                //Debug.Log($"Year : {targetDate.Year}");
                //Debug.Log($"years[i] : {years[i]}");
                // 연도 값보다 타겟 날짜 연도가 위일 경우
                if(targetDate.Year > years[i])
                {
                    // 이전 연도의 총 일수를 더함
                    result += _yearIndexes[i];
                }
                // 연도 값과 타겟 날짜 연도가 같을 경우
                else if(targetDate.Year == years[i])
                {
                    // 이번 연도의 이전 월들의 날짜값들을 더함
                    result += GetBeforeMonthDays(targetDate);

                    // 이번 연도의 현재 월의 날들을 더함
                    result += targetDate.Day;

                    // 모든 값들을 더했으므로 종료
                    break;
                }
            }

            return result;
        }

        private List<int> GetYearsIndex()
        {
            List<int> result = new List<int>();

            if(ViewOption == ViewTypeGraph.Year_5)
            {
                for (int i = 0; i < 5; i++)
                {
                    result.Add(YearID - (4 - i));
                }
            }
            else if(ViewOption == ViewTypeGraph.Year_10)
            {
                for (int i = 0; i < 10; i++)
                {
                    result.Add(YearID - (9 - i));
                }
            }
            else if(ViewOption == ViewTypeGraph.Year_Total)
            {
                for (int i = 0; i < 100; i++)
                {
                    result.Add(YearID - (99 - i));
                }
            }

            return result;
        }

        private int GetBeforeMonthDays(DateTime targetDate)
        {
            int result = 0;

            for (int i = 1; i <= targetDate.Month; i++)
            {
                if (i == targetDate.Month) break;

                //result +=  DateTime.DaysInMonth(targetDate.Year, i);
                // 월당 30일 균등분배
                result += 30;
            }

            // 일수 균등분할을 위한 값 보간
            result += targetDate.Month / 3;

            return result;
        }

        #region Draw dmg graph

        /// <summary>
        /// 손상 그래프 그리기
        /// </summary>
        /// <param name="_elements"></param>
        private void SetDmgGraph(ref Dictionary<IssueCode, List<sElement>> _elements)
        {
            foreach(IssueCode key in _elements.Keys)
            {
                StartCoroutine(SetDmgGraphPerIssue(key, _elements[key]));
            }
        }

        /// <summary>
        /// 이슈별 손상 그래프 그리기
        /// </summary>
        /// <param name="code"></param>
        /// <param name="_element"></param>
        private IEnumerator SetDmgGraphPerIssue(IssueCode code, List<sElement> _element)
        {
            yield return new WaitForEndOfFrame();

            string prefabName = GetPrefabName(IssueClass.Dmg, code);

            float rectWidth = m_rectTransform.rect.width;

            int index = _element.Count;
            for (int i = 0; i < index; i++)
            {
                float basePoint = GetDmgBasePoint(rectWidth, _element[i]);
                float graphLength = GetGraphLength(rectWidth, _element[i]);

                GameObject obj = Instantiate<GameObject>(Resources.Load<GameObject>($"BP5_Icon/{prefabName}"), gameObject.transform);
                obj.transform.localPosition = new Vector3(m_rectTransform.rect.xMin + basePoint, m_rectTransform.rect.yMin + 28, 0);

                StartCoroutine(obj.GetComponent<UI.DmgGraphCode>().SetIndex(graphLength, _element[i].count));
            }

            //m_rectTransform.rect.width;
            yield break;
        }

        // 개별 UI 요소의 프리팹명 반환
        private string GetPrefabName(IssueClass _class, IssueCode code)
        {
            string result = "";

            if(_class == IssueClass.Dmg)
            {
                result = "D_";
            }
            else if(_class == IssueClass.Rcv)
            {
                result = "R_";
            }

            switch(code)
            {
                case IssueCode.Crack:
                    result = result + "Crack";
                    break;

                case IssueCode.Efflorescense:
                    result = result + "Efflor";
                    break;

                case IssueCode.Spalling:
                    result = result + "Spall";
                    break;

                case IssueCode.Breakage:
                    result = result + "Break";
                    break;
            }

            return result;
        }

        /// <summary>
        /// 패널의 길이에서 특정일이 위치할 기본포인트를 잡는다.
        /// </summary>
        /// <param name="rectWidth"></param>
        /// <param name="_element"></param>
        /// <returns></returns>
        private float GetDmgBasePoint(float rectWidth, sElement _element)
        {
            float result = 0f;

            if(ViewOption == ViewTypeGraph.Year_1)
            {
                int day = _element.startDate.DayOfYear;
                float aspectOfDay = (float)day / (DateTime.IsLeapYear(YearID) ? 366 : 365);    // 1년중에 특정일의 위치값을 구한다.

                result = (rectWidth-3) * aspectOfDay;
            }
            else
            {
                int yearCount = 0;
                switch(ViewOption)
                {
                    case ViewTypeGraph.Year_5: yearCount = 5; break;
                    case ViewTypeGraph.Year_10: yearCount = 10; break;
                    case ViewTypeGraph.Year_Total: yearCount = 100; break;
                }

                List<int> dayIndex = GetDayIndex(YearID, yearCount);
                int totalDayCount = GetTotalDayCount(dayIndex);

                // 주어진 날짜의 날짜 변환값을 구한다.
                int currDayCount = GetCurrDayCount(dayIndex, _element.startDate);

                float ratioOfDay = (float)currDayCount / totalDayCount;

                result = (rectWidth - 3) * ratioOfDay;
            }

            return result;
        }

        

        /// <summary>
        /// 손상 그래프의 길이를 구한다.
        /// </summary>
        /// <param name="rectWidth"></param>
        /// <param name="_element"></param>
        /// <returns></returns>
        private float GetGraphLength(float rectWidth, sElement _element)
        {
            float result = 0f;

            if(ViewOption == ViewTypeGraph.Year_1)
            {
                int startDay = _element.startDate.DayOfYear;
                int endDay = _element.endDate.DayOfYear;
                int dayDistance = endDay - startDay;
                float aspectOfDay = (float)dayDistance / (DateTime.IsLeapYear(YearID) ? 366 : 365);
                result = (rectWidth-3) * aspectOfDay;
            }
            else
            {
                int yearCount = 0;
                switch(ViewOption)
                {
                    case ViewTypeGraph.Year_5: yearCount = 5; break;
                    case ViewTypeGraph.Year_10: yearCount = 10; break;
                    case ViewTypeGraph.Year_Total: yearCount = 100; break;
                }

                List<int> dayIndex = GetDayIndex(YearID, yearCount);
                int totalDayCount = GetTotalDayCount(dayIndex);

                int dayDistance = CompareOfDays(_element.startDate, _element.endDate);

                float ratioOfDay = (float)dayDistance / totalDayCount;

                result = (rectWidth - 3) * ratioOfDay;
            }

            return result;
        }

        private int CompareOfDays(DateTime startDate, DateTime endDate)
        {
            //int result = 0;

            TimeSpan TS = endDate - startDate;

            return TS.Days;
        }
        #endregion

        /// <summary>
        /// 이전 연도 카운트에서 현재 수집된 이슈들의 카운트를 모두 합한 한 해 최종 카운트 반환
        /// </summary>
        /// <param name="_targetElement"></param>
        /// <param name="prevCount"></param>
        /// <returns></returns>
        private int GetCurrAfterDatesCount(Dictionary<DateTime, int> _targetElement, int prevCount)
        {
            int result = prevCount;

            foreach(DateTime date in _targetElement.Keys)
            {
                prevCount += _targetElement[date];
            }

            return result;
        }

        /// <summary>
        /// 이전 버전 연도별 그래프 생성하기
        /// </summary>
        /// <param name="year"></param>
        /// <param name="recordList"></param>
        /// <param name="isOn"></param>
        /// <returns></returns>
        public IEnumerator SetGraphPerYear(int year, List<RecordInstance> recordList, bool isOn)
        {
            InitDics();

            yield return new WaitForEndOfFrame();

            YearID = year;

            int index = recordList.Count;
            // width, height 손상, 보강
            // width : 12월 기준점
            // height : 손상, 보강 기준점 필요

            // 어떤 손상/보수 종류인가? DBreak
            // 날짜
            // 개수 X
            // 코드


            // 배열 요소의 값을 딕셔너리에 배치
            for (int i = 0; i < index; i++)
            {
                TransformSingleInstance(recordList[i]);
            }

            

            // 손상정보 할당
            SetDmgGraph();
            // 보수정보 할당
            SetRcvGraph();

            //---
            {
                //GameObject objMin = Instantiate<GameObject>(new GameObject("img", typeof(Image)), transform);
                //RectTransform rectMin = objMin.GetComponent<RectTransform>();

                //rectMin.localPosition = m_rectTransform.rect.min;
            } // 최소점
            //---
            {
                //GameObject objMax = Instantiate<GameObject>(new GameObject("img2", typeof(Image)), transform);
                //RectTransform rectMax = objMax.GetComponent<RectTransform>();

                //rectMax.localPosition = m_rectTransform.rect.max;
            } // 최대점
            //---

            Debug.Log($"rect.width : {m_rectTransform.rect.width}");
            Debug.Log($"rect.xMin : {m_rectTransform.rect.xMin}");

            yield return new WaitForEndOfFrame();

            gameObject.SetActive(isOn);
        }

        //-------

        #region X Init
        private void InitDics()
        {
            issuesByDays = new Dictionary<string, Dictionary<int, int>>();

            issuesByDays.Add("DBreak", new Dictionary<int, int>());
            issuesByDays.Add("DCrack", new Dictionary<int, int>());
            issuesByDays.Add("DEfflor", new Dictionary<int, int>());
            issuesByDays.Add("DSpall", new Dictionary<int, int>());

            issuesByDays.Add("RBreak", new Dictionary<int, int>());
            issuesByDays.Add("RCrack", new Dictionary<int, int>());
            issuesByDays.Add("REfflor", new Dictionary<int, int>());
            issuesByDays.Add("RSpall", new Dictionary<int, int>());
        }
        #endregion

        #region Value transformation
        private void TrsfCheckerInstance(RecordInstance instance)
        {
            // 날짜는 내부에 세팅된 상태

            if(instance.DBreakageList != null)
            {
                int count = instance.DBreakageList.Count;
                for (int i = 0; i < count; i++)
                {

                }
            }
        }

        private void TransformSingleInstance(RecordInstance instance)
        {
            // 인스턴스 연월일 -> 변환 인덱스 변환
            int dateIndex = GetDateIndex(instance.dateTime);

            if(instance.DBreakageList != null)
            {
                int count = instance.DBreakageList.Count;

                SetDateIssueCount(
                    targetIssue: "DBreak",
                    _dateIndex: dateIndex,
                    count: count);
            }
            if(instance.DCrackList != null)
            {
                int count = instance.DCrackList.Count;

                SetDateIssueCount(
                    targetIssue: "DCrack",
                    _dateIndex: dateIndex,
                    count: count);
            }
            if(instance.DEfflorescenceList != null)
            {
                int count = instance.DEfflorescenceList.Count;

                SetDateIssueCount(
                    targetIssue: "DEfflor",
                    _dateIndex: dateIndex,
                    count: count);
            }
            if(instance.DSpallingList != null)
            {
                int count = instance.DSpallingList.Count;

                SetDateIssueCount(
                    targetIssue: "DSpall",
                    _dateIndex: dateIndex,
                    count: count);
            }

            if (instance.RBreakageList != null)
            {
                int count = instance.RBreakageList.Count;

                SetDateIssueCount(
                    targetIssue: "RBreak",
                    _dateIndex: dateIndex,
                    count: count);
            }
            if (instance.RCrackList != null)
            {
                int count = instance.RCrackList.Count;

                SetDateIssueCount(
                    targetIssue: "RCrack",
                    _dateIndex: dateIndex,
                    count: count);
            }
            if (instance.REfflorescenceList != null)
            {
                int count = instance.REfflorescenceList.Count;

                SetDateIssueCount(
                    targetIssue: "REfflor",
                    _dateIndex: dateIndex,
                    count: count);
            }
            if (instance.RSpallingList != null)
            {
                int count = instance.RSpallingList.Count;

                SetDateIssueCount(
                    targetIssue: "RSpall",
                    _dateIndex: dateIndex,
                    count: count);
            }
        }

        private int GetDateIndex(DateTime date)
        {
            int index = 0;

            int targetDay = date.Day;
            int inMonthDays = DateTime.DaysInMonth(
                year: date.Year,
                month: date.Month);

            int expr1 = ((int)(((float)targetDay / inMonthDays) * 1000) + 125) / 250;
            int expr2 = (date.Month - 1) * 4;

            index = expr1 + expr2;
            return index;
        }

        private void SetDateIssueCount(string targetIssue, int _dateIndex, int count)
        {
            if(issuesByDays[targetIssue].ContainsKey(_dateIndex))
            {
                issuesByDays[targetIssue][_dateIndex] += count;
            }
            else
            {
                issuesByDays[targetIssue].Add(_dateIndex, count);
            }
        }
        #endregion

        #region dmg graph
        private void SetDmgGraph()
        {
            SetDmgGraphPerIssue("DBreak");
            SetDmgGraphPerIssue("DCrack");
            SetDmgGraphPerIssue("DEfflor");
            SetDmgGraphPerIssue("DSpall");
        }

        private void SetDmgGraphPerIssue(string issueName)
        {
            int minDate = 0;
            int maxDate = 0;
            int totalCount = 0;

            GetMinMaxIndex(issueName, out minDate, out maxDate);
            GetTotalCount(issueName, out totalCount);

            float axisX = ((float)minDate / 49) * m_rectTransform.rect.width;
            float distance = ((maxDate - minDate) / (float)49) * m_rectTransform.rect.width;

            GameObject obj = Instantiate<GameObject>(Resources.Load<GameObject>($"BP5_Icon/{TrsfDmgResourceName(issueName)}"), gameObject.transform);
            obj.transform.localPosition = new Vector3(m_rectTransform.rect.xMin + axisX, m_rectTransform.rect.yMin, 0);

            StartCoroutine(obj.GetComponent<UI.DmgGraphCode>().SetIndex(distance, totalCount));
        }

        private void GetMinMaxIndex(string issueName, out int _minDate, out int _maxDate)
        {
            _minDate = 0;
            _maxDate = 0;

            List<int> keys = new List<int>();

            foreach (int key in issuesByDays[issueName].Keys)
            {
                keys.Add(key);
            }

            if(keys.Count > 0)
            {
                _minDate = keys[0];
                _maxDate = keys[0];

                int index = keys.Count;
                for (int i = 0; i < index; i++)
                {
                    if(_minDate > keys[i])
                    {
                        _minDate = keys[i];
                    }
                    if(_maxDate < keys[i])
                    {
                        _maxDate = keys[i];
                    }
                }

            }

            if (_maxDate - _minDate < 2)
            {
                _maxDate = _maxDate + 2;
            }
        }

        private void GetTotalCount(string issueName, out int _totalCount)
        {
            _totalCount = 0;

            foreach(int key in issuesByDays[issueName].Keys)
            {
                _totalCount += issuesByDays[issueName][key];
            }
        }

        private string TrsfDmgResourceName(string key)
        {
            string result = "";

            if (key == "DBreak")
            {
                result = "D_Break";
            }
            else if (key == "DCrack")
            {
                result = "D_Crack";
            }
            else if (key == "DEfflor")
            {
                result = "D_Efflor";
            }
            else if (key == "DSpall")
            {
                result = "D_Spall";
            }
            else
            {
                Debug.Log($"error key : {key}");
            }

            return result;
        }
        #endregion

        #region rcv graph
        private void SetRcvGraph()
        {
            SetRcvGraphPerIssue("RBreak");
            SetRcvGraphPerIssue("RCrack");
            SetRcvGraphPerIssue("REfflor");
            SetRcvGraphPerIssue("RSpall");
        }

        private void SetRcvGraphPerIssue(string issueName)
        {
            foreach(int key in issuesByDays[issueName].Keys)
            {
                float axisX = ((float)key / 49) * m_rectTransform.rect.width;

                GameObject obj = Instantiate<GameObject>(Resources.Load<GameObject>($"BP5_Icon/{TrsfRcvResourceName(issueName)}"), gameObject.transform);
                obj.transform.localPosition = new Vector3(m_rectTransform.rect.xMin + axisX, -5, 0);

                StartCoroutine(obj.GetComponent<UI.GraphCode>().SetIndexText(issuesByDays[issueName][key]));
            }
        }

        private string TrsfRcvResourceName(string key)
        {
            string result = "";

            if(key == "RBreak")
            {
                result = "R_Break";
            }
            else if(key == "RCrack")
            {
                result = "R_Crack";
            }
            else if(key == "REfflor")
            {
                result = "R_Efflor";
            }
            else if(key == "RSpall")
            {
                result = "R_Spall";
            }

            return result;
        }
        #endregion
    }
}
