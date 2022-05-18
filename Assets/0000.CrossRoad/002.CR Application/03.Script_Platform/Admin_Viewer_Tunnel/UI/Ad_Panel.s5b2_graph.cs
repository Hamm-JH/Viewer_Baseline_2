﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdminViewer.UI
{
	using Definition._Issue;
	using Indicator.Element;
	using Issue;
	using Management;
	using System;
	using System.Data;
	using System.Linq;
	using TMPro;

	public partial class Ad_Panel : MonoBehaviour
	{

        #region 날짜 분류변수

        public enum IssueClass
        {
            Dmg,
            Rcv
        }

        /// <summary>
        /// 시각화 옵션
        /// </summary>
        public enum ViewTypeGraph
        {
            NULL,
            Year_1,
            Year_5,
            Year_10,
            Year_Total
        }

        Dictionary<int, List<RecordInstance>> sortByYears;

        // [손상, 보수] [손상 타입 (균열, 박리, 백태, 파손)] [날짜] [
        // 손상, 날짜, 카운트
        // raw data
        //public Dictionary<IssueClass, Dictionary<IssueCode, Dictionary<DateTime, int>>> dateTimeIssues;

        public List<dateElement> dateTimeIssues;

        /// <summary>
        /// RawData에 쓸 정보집합
        /// </summary>
        public class dateElement
        {
            public IssueClass _class;
            public IssueCode _code;
            public DateTime _date;
            public int _count;
            public List<Definition._Issue.Issue> _issues;

            public override string ToString()
            {
                string result = "";

                string strClass = _class.ToString();
                string strCode = _code.ToString();
                string strDate = _date != null ? _date.ToString() : "";
                string strCount = _count.ToString();
                string issueCodes = GetIssueCodes(_issues);

                result = $"Class : {strClass}\t Code : {strCode}\t Date : {strDate}\t Count : {strCount}{issueCodes}";

                return result;
            }

            private string GetIssueCodes(List<Definition._Issue.Issue> __issues)
            {
                string result = "";

                int index = __issues.Count;
                for (int i = 0; i < index; i++)
                {
                    result += $"\t issue{i + 1} : {_issues[i].IssueOrderCode}";
                }

                return result;
            }
        }

        //public Dictionary<IssueClass, Dictionary<IssueCode, List<int>>> dateByYears;

        /// <summary>
        /// 연도별로 정보를 나열한 리스트 (날짜를 리스트의 인덱스 값으로 대체함)
        /// </summary>
        //public Dictionary<int, List<dateElement>> dateByYears;

        // -- 연도변환의 결과값 형태

        /// <summary>
        /// 손상정보에 필요함
        /// </summary>
        public class finalSortElement
        {
            public DateTime startDate;  // 시작일
            public DateTime endDate;    // 종료일
            public int count;           // count
        }

        // 이 클래스에서 하는 행위는 다음과 같다.
        // - 데이터를 날짜, 시간 단위로 정렬한다.
        // - 정렬한 데이터를 다시 손상과 보수정보에 맞게 분배한다.
        // - 패널을 생성하기에 앞서 두 가지 옵션을 고려한다.
        // -- 시각화 옵션 (1년, 5년, 10년, 전체)
        // 

        // 두 개의 dictionary를 준비한다.

        // 발생한 이슈에 대한 집합
        Dictionary<IssueClass, Dictionary<IssueCode, Dictionary<DateTime, int>>> issueElements;

        // 보수정보의 집합
        //Dictionary<IssueCode, Dictionary<DateTime, int>> rElements;
        // 손상정보의 집합
        //Dictionary<IssueCode, Dictionary<DateTime, int>> dElements;

        // - 하나는 보수정보 (1날짜, 숫자 카운트)
        // -- 보수정보의 경우 보수정보만 손상타입 단위로 총 4개의 변수를 묶어낸다.
        //Dictionary<DateTime, int> rCrackElements;
        //Dictionary<DateTime, int> rEffloresElements;
        //Dictionary<DateTime, int> rSpallingElements;
        //Dictionary<DateTime, int> rBreakageElements;

        // - 하나는 손상정보 (1날짜, 숫자 카운트)
        // -- 손상정보의 경우 손상정보, 보수정보 모두 손상타입 단위로 총 4개의 변수를 묶어낸다.
        // -- 이때 손상타입은 카운트를 +로 넣고
        // -- 보수타입은 카운트를 -로 넣는다.
        //Dictionary<DateTime, int> dCrackElements;
        //Dictionary<DateTime, int> dEffloresElements;
        //Dictionary<DateTime, int> dSpallingElements;
        //Dictionary<DateTime, int> dBreakageElements;

        private ViewTypeGraph viewType;

        public ViewTypeGraph ViewTypeOption
        {
            get => viewType;
            set => viewType = value;
        }

        #endregion

        #region Values

        [Header("s5b2 graph values")]
        public GameObject prevYBtn;
        public GameObject nextYBtn;

        [Header("element가 배치되는 transform")]
        public RectTransform rcvElement;
        public RectTransform dmgElement;

        [Header("element 배치시 수평 가이드라인 (이슈분류)")]
        public List<RectTransform> rcvRows;
        public List<RectTransform> dmgRows;

        [Header("element 배치시 수직 가이드라인 (날짜분류)")]
        public List<RectTransform> rcvColumns;
        public List<RectTransform> dmgColumns;

        public Transform YearGridPanel;

        // 연도별 패널 할당
        [SerializeField] bool isMain;
        private Dictionary<int, Transform> yearDictionary;

        public TextMeshProUGUI titleYear;
        public int currentYear;

        public bool isCaptureReady;

        //[SerializeField] private State5_BP2_Indicator l2Indicator;

        [SerializeField] private List<GameObject> togglesBeforeCapture;

        public List<GameObject> TogglesBeforeCapture
        {
            get => togglesBeforeCapture;
            //set => togglesBeforeCapture = value;
        }
        #endregion

        #region Event - Set ViewType

        public void ChangeViewType(int code)
        {
            // 선택한 버튼의 변수를 할당한다.
            ViewTypeGraph clicked = ViewTypeGraph.NULL;
            switch (code)
            {
                case 1: clicked = ViewTypeGraph.Year_1; break;
                case 2: clicked = ViewTypeGraph.Year_5; break;
                case 3: clicked = ViewTypeGraph.Year_10; break;
                case 4: clicked = ViewTypeGraph.Year_Total; break;
                default: clicked = ViewTypeGraph.NULL; break;
            }

            // 같은 상태를 선택하지 않고, 선택 버튼값이 NULL이 아닌 경우 이벤트 실행
            if (clicked != ViewTypeOption && clicked != ViewTypeGraph.NULL)
            {
                ViewTypeOption = clicked;

                ContentManager.Instance._API.RequestHistoryData(GetHistoryTable);
            }

        }

        #endregion

        #region overrides
        public void SetPanelElements(List<AIssue> _issue)
        {
            // 시각화 정보 초기값 설정
            // Awake에 초기값 할당하는걸로 변경
            //ViewTypeOption = ViewType.Year_1;

            //SetSubElement(_issue, l2Indicator);

            ClearElements();

            SetChangeYearBtn();     // 연도 변환 버튼 옵션별 설정

            SetElements(_issue);

            //Manager.UIManager.Instance.GetRoutineCode(IndicatorType.State5_BP2);
        }

        //public void SetSubElement(List<AIssue> _issue)
        //{
        //    ClearElements();

        //    SetChangeYearBtn();     // 연도 변환 버튼 옵션별 설정

        //    SetElements(_issue);
        //}

        private void SetChangeYearBtn()
        {
            bool isOn = ViewTypeOption == ViewTypeGraph.Year_1 || ViewTypeOption == ViewTypeGraph.Year_5 || ViewTypeOption == ViewTypeGraph.Year_10;

            prevYBtn.SetActive(isOn);
            nextYBtn.SetActive(isOn);
        }

        #region Reset & ClearElements
        protected void ClearElements()
        {
            int index = rcvElement.childCount;
            for (int i = index - 1; i >= 0; i--)
            {
                Destroy(rcvElement.GetChild(i).gameObject);
            }

            index = dmgElement.childCount;
            for (int i = index - 1; i >= 0; i--)
            {
                Destroy(dmgElement.GetChild(i).gameObject);
            }

            sortByYears.Clear();

            yearDictionary.Clear();
            index = YearGridPanel.childCount;
            for (int i = index - 1; i >= 0; i--)
            {
                Destroy(YearGridPanel.GetChild(i).gameObject);
            }

            ResetIssueElements();
        }

        private void ResetIssueElements()
        {
            if (issueElements != null)
            {
                issueElements.Clear();
            }
            else
            {
                issueElements = new Dictionary<IssueClass, Dictionary<IssueCode, Dictionary<DateTime, int>>>();
            }

            issueElements.Add(IssueClass.Dmg, new Dictionary<IssueCode, Dictionary<DateTime, int>>());
            issueElements.Add(IssueClass.Rcv, new Dictionary<IssueCode, Dictionary<DateTime, int>>());

            issueElements[IssueClass.Dmg].Add(IssueCode.Crack, new Dictionary<DateTime, int>());
            issueElements[IssueClass.Dmg].Add(IssueCode.Efflorescense, new Dictionary<DateTime, int>());
            issueElements[IssueClass.Dmg].Add(IssueCode.Spalling, new Dictionary<DateTime, int>());
            issueElements[IssueClass.Dmg].Add(IssueCode.Breakage, new Dictionary<DateTime, int>());

            issueElements[IssueClass.Rcv].Add(IssueCode.Crack, new Dictionary<DateTime, int>());
            issueElements[IssueClass.Rcv].Add(IssueCode.Efflorescense, new Dictionary<DateTime, int>());
            issueElements[IssueClass.Rcv].Add(IssueCode.Spalling, new Dictionary<DateTime, int>());
            issueElements[IssueClass.Rcv].Add(IssueCode.Breakage, new Dictionary<DateTime, int>());
        }
        #endregion

        /// <summary>
        /// 교량 손상 이력 불러오기 요청
        /// </summary>
        /// <param name="_issue"></param>
        protected void SetElements(List<AIssue> _issue)
        {
            // 0215 : 교량에 대한 전체 Issue의 이력정보 필요
            //Manager.JSONManager.Instance.LoadHistory(Manager.JSONLoadType.TotalHistory, Manager.MainManager.Instance.BridgeCode, "");
        }

        /// <summary>
        /// X
        /// </summary>
        //protected void SetTitleText()
        //{
        //    throw new System.NotImplementedException();
        //}
        #endregion

        /// <summary>
        /// IssueLoader에서 갓 받아온 이력정보를 정리하는 구간
        /// </summary>
        /// <param name="_dataTable"></param>
        public void GetHistoryTable(DataTable _dataTable)
        {
            // 메인
            SetHistoryTable(_dataTable);

            // 서브 데이터 전달
            ContentManager.Instance.Function_S5b2_SetSubHistoryTable(_dataTable);
        }

        public void SetSubHistoryTable(DataTable _dataTable)
		{
            // 서브
            SetHistoryTable(_dataTable);
        }

        private void SetHistoryTable(DataTable _dataTable)
		{
            ClearElements();
            SetChangeYearBtn();     // 연도 변환 버튼 옵션별 설정
            //GetHistoryTable(_dataTable);

            List<RecordInstance> _recordInstanceList = new List<RecordInstance>();

            int index = _dataTable.Rows.Count;
            for (int i = 0; i < index; i++)
            {
                RecordInstance _record = new RecordInstance();

                string _date = _dataTable.Rows[i]["date"].ToString();

                _record.dateTime = new DateTime(
                    int.Parse(_date.Split('-')[0]),
                    int.Parse(_date.Split('-')[1]),
                    int.Parse(_date.Split('-')[2]));

                _record.DCrackList = SetRowToList(_dataTable.Rows[i]["Dcrack"]);
                _record.DEfflorescenceList = SetRowToList(_dataTable.Rows[i]["Dbagli"]);
                _record.DSpallingList = SetRowToList(_dataTable.Rows[i]["Dbaegtae"]);
                _record.DBreakageList = SetRowToList(_dataTable.Rows[i]["Ddamage"]);
                _record.DScour_ErosionList = SetRowToList(_dataTable.Rows[i]["Dsegul"]);

                _record.RCrackList = SetRowToList(_dataTable.Rows[i]["Rcrack"]);
                _record.REfflorescenceList = SetRowToList(_dataTable.Rows[i]["Rbagli"]);
                _record.RSpallingList = SetRowToList(_dataTable.Rows[i]["Rbaegtae"]);
                _record.RBreakageList = SetRowToList(_dataTable.Rows[i]["Rdamage"]);
                _record.RScour_ErosionList = SetRowToList(_dataTable.Rows[i]["Rsegul"]);

                // 세굴 없애기
                ResetRecord(_record);

                if (!IsNullInstance(_record) == true)
                {
                    _recordInstanceList.Add(_record);
                }
            }

            // 데이터 날짜, 카운트 정렬단계
            SetDateTimeIssue(
                _list: _recordInstanceList,
                _elements: ref issueElements);

            SetDateTimePanels(ref issueElements);
        }

        #region Set Record Instances
        /// <summary>
        /// 세굴 삭제용 임시 메서드
        /// </summary>
        /// <param name="instance"></param>
        private void ResetRecord(RecordInstance instance)
        {
            instance.DScour_ErosionList = null;
            instance.RScour_ErosionList = null;
        }

        /// <summary>
        /// 할당된게 없는 인스턴스인지 확인
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        private bool IsNullInstance(RecordInstance instance)
        {
            bool result = true;

            bool DBreak = instance.DBreakageList == null;
            bool RBreak = instance.RBreakageList == null;
            bool DCrack = instance.DCrackList == null;
            bool RCrack = instance.RCrackList == null;
            bool DEfflor = instance.DEfflorescenceList == null;
            bool REfflor = instance.REfflorescenceList == null;
            bool DScour = instance.DScour_ErosionList == null;
            bool RScour = instance.RScour_ErosionList == null;
            bool DSpall = instance.DSpallingList == null;
            bool RSpall = instance.RSpallingList == null;

            result = DBreak && RBreak &&
                     DCrack && RCrack &&
                     DEfflor && REfflor &&
                     DScour && RScour &&
                     DSpall && RSpall;
            return result;
        }

        private List<Definition._Issue.Issue> SetRowToList(object _dataRow)
        {
			List<Definition._Issue.Issue> list = (_dataRow as List<Definition._Issue.Issue>);

            if (list != null)
            {
                return list;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region Set DateTime Issue 초기 (날짜, 발생 회수) 정렬
        /// <summary>
        /// 날짜 정렬된 손상, 보강 정보 업데이트
        /// </summary>
        /// <param name="_list"></param>
        /// <param name="_elements"></param>
        private void SetDateTimeIssue(List<RecordInstance> _list, ref Dictionary<IssueClass, Dictionary<IssueCode, Dictionary<DateTime, int>>> _elements)
        {
            // 기존 DateTime, int를 가지는 _element는 최종 출력정보로 두고, 중복정보 제거를 위해 새 Dictionary를 사용한다.

            Dictionary<IssueClass, Dictionary<IssueCode, Dictionary<DateTime, dateElement>>> _rawData = InitRawData();

            int index = _list.Count;
            for (int i = index - 1; i >= 0; i--)
            {
                // Raw Data 초기 정렬
                SetSingleDateTime(_list[i], ref _rawData);
            }


            // 중복된 정보들을 정리한다.
            ClearDuplicatedData(ref _rawData);

            // rawData 정상 입력 확인
            // - 엑셀 참조
            //DebugRawData(ref _rawData);

            // 정렬된 rawData -> _elements로 전달
            SetElementsData(ref _rawData, ref _elements);

            // 날짜 순으로 출력되는지 확인
            DebugElements(ref _elements);
        }

        #region Raw Data 초기화, 초기 정렬
        /// <summary>
        /// 중간처리용 데이터셋 초기화
        /// </summary>
        /// <returns></returns>
        private Dictionary<IssueClass, Dictionary<IssueCode, Dictionary<DateTime, dateElement>>> InitRawData()
        {
            Dictionary<IssueClass, Dictionary<IssueCode, Dictionary<DateTime, dateElement>>> result = new Dictionary<IssueClass, Dictionary<IssueCode, Dictionary<DateTime, dateElement>>>();

            result.Add(IssueClass.Dmg, new Dictionary<IssueCode, Dictionary<DateTime, dateElement>>());
            result.Add(IssueClass.Rcv, new Dictionary<IssueCode, Dictionary<DateTime, dateElement>>());

            result[IssueClass.Dmg].Add(IssueCode.Crack, new Dictionary<DateTime, dateElement>());
            result[IssueClass.Dmg].Add(IssueCode.Efflorescense, new Dictionary<DateTime, dateElement>());
            result[IssueClass.Dmg].Add(IssueCode.Spalling, new Dictionary<DateTime, dateElement>());
            result[IssueClass.Dmg].Add(IssueCode.Breakage, new Dictionary<DateTime, dateElement>());

            result[IssueClass.Rcv].Add(IssueCode.Crack, new Dictionary<DateTime, dateElement>());
            result[IssueClass.Rcv].Add(IssueCode.Efflorescense, new Dictionary<DateTime, dateElement>());
            result[IssueClass.Rcv].Add(IssueCode.Spalling, new Dictionary<DateTime, dateElement>());
            result[IssueClass.Rcv].Add(IssueCode.Breakage, new Dictionary<DateTime, dateElement>());

            return result;
        }

        /// <summary>
        /// 손상정보별로 이력정보를 할당한다.
        /// </summary>
        /// <param name="_record"></param>
        /// <param name="_target"></param>
        private void SetSingleDateTime(RecordInstance _record, ref Dictionary<IssueClass, Dictionary<IssueCode, Dictionary<DateTime, dateElement>>> _target)
        {
            // 시간 값으로 인한 오동작 방지
            DateTime date = new DateTime(_record.dateTime.Year, _record.dateTime.Month, _record.dateTime.Day);

            if (_record.DCrackList != null)
            {
                SetSingleDateCount(
                    _count: _record.DCrackList.Count,
                    _date: date,
                    _class: IssueClass.Dmg, _code: IssueCode.Crack,
                    _issues: _record.DCrackList,
                    ref _target);
            }

            if (_record.DEfflorescenceList != null)
            {
                SetSingleDateCount(
                    _count: _record.DEfflorescenceList.Count,
                    _date: date,
                    _class: IssueClass.Dmg, _code: IssueCode.Efflorescense,
                    _issues: _record.DEfflorescenceList,
                    ref _target);
            }

            if (_record.DSpallingList != null)
            {
                SetSingleDateCount(
                    _count: _record.DSpallingList.Count,
                    _date: date,
                    _class: IssueClass.Dmg, _code: IssueCode.Spalling,
                    _issues: _record.DSpallingList,
                    ref _target);
            }

            if (_record.DBreakageList != null)
            {
                SetSingleDateCount(
                    _count: _record.DBreakageList.Count,
                    _date: date,
                    _class: IssueClass.Dmg, _code: IssueCode.Breakage,
                    _issues: _record.DBreakageList,
                    ref _target);
            }

            if (_record.RCrackList != null)
            {
                SetSingleDateCount(
                    _count: _record.RCrackList.Count,
                    _date: date,
                    _class: IssueClass.Rcv, _code: IssueCode.Crack,
                    _issues: _record.RCrackList,
                    ref _target);
            }

            if (_record.REfflorescenceList != null)
            {
                SetSingleDateCount(
                    _count: _record.REfflorescenceList.Count,
                    _date: date,
                    _class: IssueClass.Rcv, _code: IssueCode.Efflorescense,
                    _issues: _record.REfflorescenceList,
                    ref _target);
            }

            if (_record.RSpallingList != null)
            {
                SetSingleDateCount(
                    _count: _record.RSpallingList.Count,
                    _date: date,
                    _class: IssueClass.Rcv, _code: IssueCode.Spalling,
                    _issues: _record.RSpallingList,
                    ref _target);
            }

            if (_record.RBreakageList != null)
            {
                SetSingleDateCount(
                    _count: _record.RBreakageList.Count,
                    _date: date,
                    _class: IssueClass.Rcv, _code: IssueCode.Breakage,
                    _issues: _record.RBreakageList,
                    ref _target);
            }
        }

        /// <summary>
        /// 개별 이력정보를 Dictionary에 할당한다.
        /// </summary>
        /// <param name="_count"></param>
        /// <param name="_date"></param>
        /// <param name="_class"></param>
        /// <param name="_code"></param>
        /// <param name="_target"></param>
        private void SetSingleDateCount(int _count, DateTime _date, IssueClass _class, IssueCode _code, List<Definition._Issue.Issue> _issues, ref Dictionary<IssueClass, Dictionary<IssueCode, Dictionary<DateTime, dateElement>>> _target)
        {
            dateElement finalElement = new dateElement();
            finalElement._class = _class;
            finalElement._code = _code;
            finalElement._date = _date;
            finalElement._count = _count;
            finalElement._issues = _issues;

            if (_count != 0)
            {
                if (_class == IssueClass.Rcv)
                {
                    // 보수 정보의 개수만큼 카운트 값을 양수로 적용한다.
                    if (!_target[IssueClass.Rcv][_code].ContainsKey(_date))
                    {
                        _target[IssueClass.Rcv][_code].Add(_date, finalElement);
                    }
                    else
                    {
                        _target[IssueClass.Rcv][_code][_date]._count += finalElement._count;
                    }
                }
                else if (_class == IssueClass.Dmg)
                {
                    // 손상 정보의 개수만큼 카운트 값을 양수로 적용한다.
                    if (!_target[IssueClass.Dmg][_code].ContainsKey(_date))
                    {
                        _target[IssueClass.Dmg][_code].Add(_date, finalElement);
                    }
                    else
                    {
                        _target[IssueClass.Dmg][_code][_date]._count += finalElement._count;
                    }
                }
            }
        }
        #endregion

        #region Debug Raw Data

        private void DebugRawData(ref Dictionary<IssueClass, Dictionary<IssueCode, Dictionary<DateTime, dateElement>>> _target)
        {
            string result = "";

            foreach (IssueClass key in _target.Keys)
            {
                foreach (IssueCode _key in _target[key].Keys)
                {
                    foreach (DateTime __key in _target[key][_key].Keys)
                    {
                        result += $"{_target[key][_key][__key]}\n";
                    }
                }
            }

            Debug.Log(result);
        }

        private void DebugElements(ref Dictionary<IssueClass, Dictionary<IssueCode, Dictionary<DateTime, int>>> _element)
        {
            string result = "";

            foreach (IssueClass key in _element.Keys)
            {
                foreach (IssueCode _key in _element[key].Keys)
                {
                    foreach (DateTime __key in _element[key][_key].Keys)
                    {
                        result += ToStringElement(key, _key, __key, _element[key][_key][__key]);
                    }
                }
            }

            Debug.Log(result);
        }

        private string ToStringElement(IssueClass _class, IssueCode _code, DateTime _date, int _count)
        {
            string result = "";

            result = $"class : {_class.ToString()}\t type : {_code.ToString()}\t date : {_date.ToString()}\t count : {_count.ToString()}\n";

            return result;
        }

        #endregion

        #region ClearDuplicatedData 중복된 이슈정보 정리

        /// <summary>
        /// 중복된 이슈들을 정리한다.
        /// </summary>
        /// <param name="_target"></param>
        private void ClearDuplicatedData(ref Dictionary<IssueClass, Dictionary<IssueCode, Dictionary<DateTime, dateElement>>> _target)
        {
            // 손상 정보들에서 중복을 제거한다.
            foreach (IssueCode key in _target[IssueClass.Dmg].Keys)
            {
                //Debug.Log($"Class Dmg");
                ClearDuplicatedData_PerDmgIssueCode(_target[IssueClass.Dmg][key]);
            }
        }

        /// <summary>
        /// 손상정보들에서 중복된 정보들을 정리한다.
        /// </summary>
        /// <param name="_target"></param>
        private void ClearDuplicatedData_PerDmgIssueCode(Dictionary<DateTime, dateElement> _target)
        {
            // 중복 확인용 변수
            Dictionary<string, dateElement> keyChecker = new Dictionary<string, dateElement>();

            // 날짜 단위로 키값 반복
            foreach (DateTime key in _target.Keys)
            {

				// 리스트 요소 할당
				List<Definition._Issue.Issue> _issues = _target[key]._issues;

                int index = _issues.Count;
                if (index != 0)
                {
                    for (int i = index-1; i >= 0; i--)
                    {


                        // 키값에 이슈 코드가 존재하는가?
                        if (!keyChecker.ContainsKey(_issues[i].IssueOrderCode))
                        {
                            // 존재하지 않는 경우
                            // - keyChecker에 값 추가
                            keyChecker.Add(_issues[i].IssueOrderCode, _target[key]);
                        }
                        else
                        {
                            // 손상 코드키 할당
                            string _key = _issues[i].IssueOrderCode;

                            //-----------------------------------------------
                            // check의 날짜와 같은 target 요소간 날짜비교
                            // 날짜가 빠른 쪽의 요소를 살리고
                            // 날짜가 느린 쪽의 요소를 없앤다

                            // 존재하는 경우
                            // - 서로의 날짜를 비교한다.
                            DateTime beforeDateTime = keyChecker[_key]._date;
                            DateTime nowDateTime = _target[key]._date;

                            int dateCount = beforeDateTime.CompareTo(nowDateTime);

                            // before값이 더 일찍인 경우
                            if (dateCount < 0)
                            {
                                // 이후 값에 관해
                                _target[key]._count--;  // 카운트 1 감소
                                RemoveListElement(_key, ref _target[key]._issues);      // - 이후 값 recordIssue 삭제
                            }
                            // 두 날짜가 같은경우
                            else if (dateCount == 0)
                            {
                                // 날이 같으므로 카운트 값만 뺀다
                                _target[key]._count--;
                            }
                            // now값이 더 일찍인 경우
                            else
                            {
                                // 이전 값에 관해
                                keyChecker[_key]._count--;  // 카운트 1 감소
                                RemoveListElement(_key, ref keyChecker[_key]._issues);  // - 이전 값 recordIssue 삭제
                                // - 이전 값 dictionary 키값으로 삭제
                                keyChecker.Remove(_key);

                                // 이후 값에 관해
                                // - dictionary에 추가
                                keyChecker.Add(_issues[i].IssueOrderCode, _target[key]);
                            }

                        }
                    }
                }
            }
        }

        /// <summary>
        /// 중복으로 확인된 리스트의 요소를 삭제한다.
        /// </summary>
        /// <param name="codeKey"></param>
        /// <param name="_list"></param>
        private void RemoveListElement(string codeKey, ref List<Definition._Issue.Issue> _list)
        {
            int index = _list.Count;
            for (int i = index-1; i >= 0; i--)
            {
                if (_list[i].IssueOrderCode == codeKey)
                {
                    _list.RemoveAt(i);
                }
            }
        }

        #endregion

        #region Set Elements Data

        private void SetElementsData(ref Dictionary<IssueClass, Dictionary<IssueCode, Dictionary<DateTime, dateElement>>> _rawData,
            ref Dictionary<IssueClass, Dictionary<IssueCode, Dictionary<DateTime, int>>> _elements)
        {
            foreach (IssueClass key in _rawData.Keys)
            {
                foreach (IssueCode _key in _rawData[key].Keys)
                {
                    if (key == IssueClass.Dmg)
                    {
                        SetDmgElementsByRawData(_elements[key][_key], _rawData[IssueClass.Dmg][_key], _rawData[IssueClass.Rcv][_key]);
                    }
                    else if (key == IssueClass.Rcv)
                    {
                        SetRcvElementsByRawData(_elements[key][_key], _rawData[IssueClass.Rcv][_key]);
                    }
                }
            }

            // Dmg, Rcv
            foreach (IssueClass key in _elements.Keys)
            {
                //Crack = 0, Efflorescense = 1, Spalling = 2, Breakage = 3, Scour_Erosion = 4
                foreach (IssueCode _key in _elements[key].Keys)
                {
                    Dictionary<DateTime, int> _inElement = _elements[key][_key];
                    Clear0CountDataElement(ref _inElement, key);
                }
            }
        }

        private void SetDmgElementsByRawData(Dictionary<DateTime, int> _inElements, Dictionary<DateTime, dateElement> _dmgData, Dictionary<DateTime, dateElement> _rcvData)
        {
            foreach (DateTime key in _dmgData.Keys)
            {
                if (!_inElements.ContainsKey(key))
                {
                    _inElements.Add(key, _dmgData[key]._count);
                }
                else
                {
                    _inElements[key] += _dmgData[key]._count;
                }
            }

            foreach (DateTime key in _rcvData.Keys)
            {
                if (!_inElements.ContainsKey(key))
                {
                    _inElements.Add(key, -_rcvData[key]._count);
                }
                else
                {
                    _inElements[key] -= _rcvData[key]._count;
                }
            }


        }

        private void SetRcvElementsByRawData(Dictionary<DateTime, int> _inElements, Dictionary<DateTime, dateElement> _rcvData)
        {
            foreach (DateTime key in _rcvData.Keys)
            {
                if (!_inElements.ContainsKey(key))
                {
                    _inElements.Add(key, _rcvData[key]._count);
                }
                else
                {
                    _inElements[key] += _rcvData[key]._count;
                }
            }
        }

        /// <summary>
        /// 정렬 결과 카운트가 0이 된 요소를 제거한다.
        /// </summary>
        /// <param name="_inElement"></param>
        private void Clear0CountDataElement(ref Dictionary<DateTime, int> _inElement, IssueClass _class)
        {
            Dictionary<DateTime, int> newElement = new Dictionary<DateTime, int>();

            // foreach 반복중에 Remove가 불가능해 새 변수를 만들고 거기에 올바른 변수만 할당후 데이터 변환
            foreach (DateTime key in _inElement.Keys)
            {
                if (_inElement[key] != 0)
                {
                    newElement.Add(key, _inElement[key]);
                }
            }

            // 기존 변수 초기화
            _inElement.Clear();

            foreach (DateTime key in newElement.Keys)
            {
                _inElement.Add(key, newElement[key]);
            }
            // 정리된 변수로 할당
            //_inElement = newElement;
        }

        #endregion

        #endregion

        private void SetDateTimePanels(ref Dictionary<IssueClass, Dictionary<IssueCode, Dictionary<DateTime, int>>> _elements)
        {
            // 넘길때 필요한 정보
            // - 최소, 최대 연도
            // - 날짜 리스트
            // - 뷰 옵션

            // 최소 연도 [0]
            // 최대 연도 [1]
            int[] minMaxYears = GetMinMaxYear(ref _elements);

            //ViewTypeOption // 시각화 옵션 확인
            // 연 단위 옵션
            SetPanel(
                minMaxYear: minMaxYears,
                viewOption: ViewTypeOption,
                _elements: ref _elements);
        }

        /// <summary>
        /// 최소 / 최대 연도 반환
        /// </summary>
        /// <param name="_elements"></param>
        /// <returns></returns>
        private int[] GetMinMaxYear(ref Dictionary<IssueClass, Dictionary<IssueCode, Dictionary<DateTime, int>>> _elements)
        {
            Dictionary<int, int> years = new Dictionary<int, int>();

            int[] result = { 0, 0 };

            foreach (IssueClass key in _elements.Keys)
            {
                foreach (IssueCode _key in _elements[key].Keys)
                {
                    foreach (DateTime _date in _elements[key][_key].Keys)
                    {
                        // 연도 취합
                        if (years.ContainsKey(_date.Year))
                        {
                            years[_date.Year]++;
                        }
                        else
                        {
                            years.Add(_date.Year, 1);
                        }
                    }
                }
            }

            if (years.Count > 0)
            {
                result[0] = years.Keys.Min<int>();
                result[1] = years.Keys.Max<int>();
            }

            //result[1] = 2022;
            if (result[1] != 0 && result[1] != DateTime.Now.Year)
            {
                result[1] = DateTime.Now.Year;
            }

            return result;
        }

        /// <summary>
        /// 옵션에 따라 패널생성
        /// 최소, 최대 연도 / 날짜 변수들 생성된 패널에 전달
        /// </summary>
        /// <param name="minMaxYear"></param>
        /// <param name="viewOption"></param>
        /// <param name="_elements"></param>
        private void SetPanel(int[] minMaxYear, ViewTypeGraph viewOption, ref Dictionary<IssueClass, Dictionary<IssueCode, Dictionary<DateTime, int>>> _elements)
        {
            // 최대 연도로 타이틀 세팅
            // 옵션별로 연도 타이틀명 변경
            if (minMaxYear[1] == 0)
            {
                minMaxYear[0] = DateTime.Now.Year;
                minMaxYear[1] = DateTime.Now.Year;
            }

            SetYear(minMaxYear[1], viewOption);

            // 현재 연도 변수 최신 연도로 할당
            currentYear = minMaxYear[1];

            // 옵션별로 연도 인덱스를 할당한다.
            List<int> yIndex = GetYearIndex(minMaxYear, viewOption);

            string prefabName = GetPrefabName(viewOption);

            int index = yIndex.Count;
            for (int i = 0; i < index; i++)
            {

                GameObject obj = Instantiate<GameObject>(Resources.Load<GameObject>(prefabName), YearGridPanel);
                State5_BP2_Element element = obj.GetComponent<State5_BP2_Element>();

                yearDictionary.Add(yIndex[i], obj.transform);


                StartCoroutine(element.SetGraphPerYear(
                    currentYear: yIndex[i],
                    minMaxYear: minMaxYear,
                    viewType: viewOption,
                    _elements: _elements));
            }
        }



        private List<int> GetYearIndex(int[] minMaxYear, ViewTypeGraph _type)
        {
            List<int> result = new List<int>();

            int decCount = 0;
            if (_type == ViewTypeGraph.Year_1) decCount = 1;
            else if (_type == ViewTypeGraph.Year_5) decCount = 5;
            else if (_type == ViewTypeGraph.Year_10) decCount = 10;
            else if (_type == ViewTypeGraph.Year_Total) decCount = 100;

            result.Add(minMaxYear[1]);
            if (decCount != 0)
            {
                int currIndex = minMaxYear[1];
                while (true)
                {
                    currIndex -= decCount;
                    if (currIndex >= minMaxYear[0])
                    {
                        result.Add(currIndex);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return result;
        }

        private string GetPrefabName(ViewTypeGraph _type)
        {
            string result = "";

            if (_type == ViewTypeGraph.Year_1)
            {
                result = "Indicator/State5_B2_Year1";
            }
            else if (_type == ViewTypeGraph.Year_5)
            {
                result = "Indicator/State5_B2_Year5";
            }
            else if (_type == ViewTypeGraph.Year_10)
            {
                result = "Indicator/State5_B2_Year10";
            }
            else if (_type == ViewTypeGraph.Year_Total)
            {
                result = "Indicator/State5_B2_Year100";
            }

            return result;
        }


        #region Events
        public void PrevYear()
        {
            List<int> prevYears = new List<int>();

            foreach (int key in yearDictionary.Keys)
            {
                if (currentYear > key)
                {
                    prevYears.Add(key);
                }

            }

            int latestYear = 0;
            if (prevYears.Count > 0)
            {
                latestYear = prevYears[0];

                int index = prevYears.Count;
                for (int i = 0; i < index; i++)
                {
                    if (latestYear < prevYears[i])
                    {
                        latestYear = prevYears[i];
                    }
                }

                SetVisibleYearPanel(latestYear);
            }


        }

        public void NextYear()
        {
            List<int> nextYears = new List<int>();

            foreach (int key in yearDictionary.Keys)
            {
                if (currentYear < key)
                {
                    nextYears.Add(key);
                }
            }

            int latestYear = 0;
            if (nextYears.Count > 0)
            {
                latestYear = nextYears[0];

                int index = nextYears.Count;
                for (int i = 0; i < index; i++)
                {
                    if (latestYear > nextYears[i])
                    {
                        latestYear = nextYears[i];
                    }
                }

                SetVisibleYearPanel(latestYear);
            }
        }

        private void SetVisibleYearPanel(int yearKey)
        {
            foreach (int key in yearDictionary.Keys)
            {
                if (key == yearKey)
                {
                    SetYear(key, ViewTypeOption);
                }
                yearDictionary[key].gameObject.SetActive((key == yearKey));
            }

            currentYear = yearKey;
        }

        public void SetYear(int year, ViewTypeGraph _type)
        {
            if (_type == ViewTypeGraph.Year_1)
            {
                titleYear.text = $"{year}년";
            }
            else if (_type == ViewTypeGraph.Year_5)
            {
                titleYear.text = $"{year-5} ~ {year}년";
            }
            else if (_type == ViewTypeGraph.Year_10)
            {
                titleYear.text = $"{year-10} ~ {year}년";
            }
            else if (_type == ViewTypeGraph.Year_Total)
            {
                titleYear.text = $"{year-100} ~ {year}년";
            }
        }

        #endregion

        private void Awake()
        {
            sortByYears = new Dictionary<int, List<RecordInstance>>();

            yearDictionary = new Dictionary<int, Transform>();


            ViewTypeOption = ViewTypeGraph.Year_1;
        }

        public IEnumerator ReadyCapture()
        {
            isCaptureReady = false;

            StartCoroutine(ReadyToCapture());

            yield return new WaitUntil(() => isCaptureReady == true);

            //Manager.UIManager.Instance.isCapReady2 = true;

            yield break;
        }

        private IEnumerator ReadyToCapture()
        {
            ViewTypeOption = ViewTypeGraph.Year_1;

            //SetPanelElements(RuntimeData.RootContainer.Instance.IssueObjectList);

            while (true)
            {
                yield return new WaitForEndOfFrame();

                if (yearDictionary != null)
                {
                    bool result = true;
                    foreach (int key in yearDictionary.Keys)
                    {
                        result = result && yearDictionary[key].gameObject.activeSelf;
                    }

                    if (result == false)
                    {
                        break;
                    }
                }
            }

            isCaptureReady = true;

            //Manager.UIManager.Instance.isCapReady2 = true;

            yield break;
        }
    }
}
