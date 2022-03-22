using System.Collections;
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
		//private void GetHistoryTable(DataTable _dTable)
		//{
		//	Debug.LogError("Hello table");
        //
        //    //if (isMain)
        //    //{
        //    //    ClearElements();
        //    //    SetChangeYearBtn();     // ���� ��ȯ ��ư �ɼǺ� ����
        //    //    GetHistoryTable(_dataTable);
        //    //}
        //
        //    //List<RecordInstance> _recordInstanceList = new List<RecordInstance>();
        //
        //    //int index = _dataTable.Rows.Count;
        //    //for (int i = 0; i < index; i++)
        //    //{
        //    //    RecordInstance _record = new RecordInstance();
        //
        //    //    string _date = _dataTable.Rows[i]["date"].ToString();
        //
        //    //    _record.dateTime = new DateTime(
        //    //        int.Parse(_date.Split('-')[0]),
        //    //        int.Parse(_date.Split('-')[1]),
        //    //        int.Parse(_date.Split('-')[2]));
        //
        //    //    _record.DCrackList = SetRowToList(_dataTable.Rows[i]["Dcrack"]);
        //    //    _record.DEfflorescenceList = SetRowToList(_dataTable.Rows[i]["Dbagli"]);
        //    //    _record.DSpallingList = SetRowToList(_dataTable.Rows[i]["Dbaegtae"]);
        //    //    _record.DBreakageList = SetRowToList(_dataTable.Rows[i]["Ddamage"]);
        //    //    _record.DScour_ErosionList = SetRowToList(_dataTable.Rows[i]["Dsegul"]);
        //
        //    //    _record.RCrackList = SetRowToList(_dataTable.Rows[i]["Rcrack"]);
        //    //    _record.REfflorescenceList = SetRowToList(_dataTable.Rows[i]["Rbagli"]);
        //    //    _record.RSpallingList = SetRowToList(_dataTable.Rows[i]["Rbaegtae"]);
        //    //    _record.RBreakageList = SetRowToList(_dataTable.Rows[i]["Rdamage"]);
        //    //    _record.RScour_ErosionList = SetRowToList(_dataTable.Rows[i]["Rsegul"]);
        //
        //    //    // ���� ���ֱ�
        //    //    ResetRecord(_record);
        //
        //    //    if (!IsNullInstance(_record) == true)
        //    //    {
        //    //        _recordInstanceList.Add(_record);
        //    //    }
        //    //}
        //
        //    //// ������ ��¥, ī��Ʈ ���Ĵܰ�
        //    //SetDateTimeIssue(
        //    //    _list: _recordInstanceList,
        //    //    _elements: ref issueElements);
        //
        //    //// TODO : ���� 1��, 5��, 10��, ��ü ������ �Ǵ�
        //    //// 1�� ���� �۾�
        //    //// �г� ���� / ������ ���� �ܰ�
        //    //SetDateTimePanels(ref issueElements);
        //}

        //public Element.State5_BP2_Element element;

        #region ��¥ �з�����

        public enum IssueClass
        {
            Dmg,
            Rcv
        }

        /// <summary>
        /// �ð�ȭ �ɼ�
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

        // [�ջ�, ����] [�ջ� Ÿ�� (�տ�, �ڸ�, ����, �ļ�)] [��¥] [
        // �ջ�, ��¥, ī��Ʈ
        // raw data
        //public Dictionary<IssueClass, Dictionary<IssueCode, Dictionary<DateTime, int>>> dateTimeIssues;

        public List<dateElement> dateTimeIssues;

        /// <summary>
        /// RawData�� �� ��������
        /// </summary>
        public class dateElement
        {
            public IssueClass _class;
            public IssueCode _code;
            public DateTime _date;
            public int _count;
            public List<Issue> _issues;

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

            private string GetIssueCodes(List<Issue> __issues)
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
        /// �������� ������ ������ ����Ʈ (��¥�� ����Ʈ�� �ε��� ������ ��ü��)
        /// </summary>
        //public Dictionary<int, List<dateElement>> dateByYears;

        // -- ������ȯ�� ����� ����

        /// <summary>
        /// �ջ������� �ʿ���
        /// </summary>
        public class finalSortElement
        {
            public DateTime startDate;  // ������
            public DateTime endDate;    // ������
            public int count;           // count
        }

        // �� Ŭ�������� �ϴ� ������ ������ ����.
        // - �����͸� ��¥, �ð� ������ �����Ѵ�.
        // - ������ �����͸� �ٽ� �ջ�� ���������� �°� �й��Ѵ�.
        // - �г��� �����ϱ⿡ �ռ� �� ���� �ɼ��� ����Ѵ�.
        // -- �ð�ȭ �ɼ� (1��, 5��, 10��, ��ü)
        // 

        // �� ���� dictionary�� �غ��Ѵ�.

        // �߻��� �̽��� ���� ����
        Dictionary<IssueClass, Dictionary<IssueCode, Dictionary<DateTime, int>>> issueElements;

        // ���������� ����
        //Dictionary<IssueCode, Dictionary<DateTime, int>> rElements;
        // �ջ������� ����
        //Dictionary<IssueCode, Dictionary<DateTime, int>> dElements;

        // - �ϳ��� �������� (1��¥, ���� ī��Ʈ)
        // -- ���������� ��� ���������� �ջ�Ÿ�� ������ �� 4���� ������ �����.
        //Dictionary<DateTime, int> rCrackElements;
        //Dictionary<DateTime, int> rEffloresElements;
        //Dictionary<DateTime, int> rSpallingElements;
        //Dictionary<DateTime, int> rBreakageElements;

        // - �ϳ��� �ջ����� (1��¥, ���� ī��Ʈ)
        // -- �ջ������� ��� �ջ�����, �������� ��� �ջ�Ÿ�� ������ �� 4���� ������ �����.
        // -- �̶� �ջ�Ÿ���� ī��Ʈ�� +�� �ְ�
        // -- ����Ÿ���� ī��Ʈ�� -�� �ִ´�.
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

        [Header("element�� ��ġ�Ǵ� transform")]
        public RectTransform rcvElement;
        public RectTransform dmgElement;

        [Header("element ��ġ�� ���� ���̵���� (�̽��з�)")]
        public List<RectTransform> rcvRows;
        public List<RectTransform> dmgRows;

        [Header("element ��ġ�� ���� ���̵���� (��¥�з�)")]
        public List<RectTransform> rcvColumns;
        public List<RectTransform> dmgColumns;

        public Transform YearGridPanel;

        // ������ �г� �Ҵ�
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
            // ������ ��ư�� ������ �Ҵ��Ѵ�.
            ViewTypeGraph clicked = ViewTypeGraph.NULL;
            switch (code)
            {
                case 1: clicked = ViewTypeGraph.Year_1; break;
                case 2: clicked = ViewTypeGraph.Year_5; break;
                case 3: clicked = ViewTypeGraph.Year_10; break;
                case 4: clicked = ViewTypeGraph.Year_Total; break;
                default: clicked = ViewTypeGraph.NULL; break;
            }

            // ���� ���¸� �������� �ʰ�, ���� ��ư���� NULL�� �ƴ� ��� �̺�Ʈ ����
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
            // �ð�ȭ ���� �ʱⰪ ����
            // Awake�� �ʱⰪ �Ҵ��ϴ°ɷ� ����
            //ViewTypeOption = ViewType.Year_1;

            //SetSubElement(_issue, l2Indicator);

            ClearElements();

            SetChangeYearBtn();     // ���� ��ȯ ��ư �ɼǺ� ����

            SetElements(_issue);

            //Manager.UIManager.Instance.GetRoutineCode(IndicatorType.State5_BP2);
        }

        //public void SetSubElement(List<AIssue> _issue)
        //{
        //    ClearElements();

        //    SetChangeYearBtn();     // ���� ��ȯ ��ư �ɼǺ� ����

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
        /// ���� �ջ� �̷� �ҷ����� ��û
        /// </summary>
        /// <param name="_issue"></param>
        protected void SetElements(List<AIssue> _issue)
        {
            // TODO 0215 : ������ ���� ��ü Issue�� �̷����� �ʿ�
            Manager.JSONManager.Instance.LoadHistory(Manager.JSONLoadType.TotalHistory, Manager.MainManager.Instance.BridgeCode, "");
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
        /// IssueLoader���� �� �޾ƿ� �̷������� �����ϴ� ����
        /// </summary>
        /// <param name="_dataTable"></param>
        public void GetHistoryTable(DataTable _dataTable)
        {
            ClearElements();
            SetChangeYearBtn();     // ���� ��ȯ ��ư �ɼǺ� ����
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

                // ���� ���ֱ�
                ResetRecord(_record);

                if (!IsNullInstance(_record) == true)
                {
                    _recordInstanceList.Add(_record);
                }
            }

            // ������ ��¥, ī��Ʈ ���Ĵܰ�
            SetDateTimeIssue(
                _list: _recordInstanceList,
                _elements: ref issueElements);

            // TODO : ���� 1��, 5��, 10��, ��ü ������ �Ǵ�
            // 1�� ���� �۾�
            // �г� ���� / ������ ���� �ܰ�
            SetDateTimePanels(ref issueElements);
        }

        #region Set Record Instances
        /// <summary>
        /// ���� ������ �ӽ� �޼���
        /// </summary>
        /// <param name="instance"></param>
        private void ResetRecord(RecordInstance instance)
        {
            instance.DScour_ErosionList = null;
            instance.RScour_ErosionList = null;
        }

        /// <summary>
        /// �Ҵ�Ȱ� ���� �ν��Ͻ����� Ȯ��
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

        private List<Issue> SetRowToList(object _dataRow)
        {
            List<Issue> list = (_dataRow as List<Issue>);

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

        #region Set DateTime Issue �ʱ� (��¥, �߻� ȸ��) ����
        /// <summary>
        /// ��¥ ���ĵ� �ջ�, ���� ���� ������Ʈ
        /// </summary>
        /// <param name="_list"></param>
        /// <param name="_elements"></param>
        private void SetDateTimeIssue(List<RecordInstance> _list, ref Dictionary<IssueClass, Dictionary<IssueCode, Dictionary<DateTime, int>>> _elements)
        {
            // ���� DateTime, int�� ������ _element�� ���� ��������� �ΰ�, �ߺ����� ���Ÿ� ���� �� Dictionary�� ����Ѵ�.

            Dictionary<IssueClass, Dictionary<IssueCode, Dictionary<DateTime, dateElement>>> _rawData = InitRawData();

            int index = _list.Count;
            for (int i = index - 1; i >= 0; i--)
            {
                // Raw Data �ʱ� ����
                SetSingleDateTime(_list[i], ref _rawData);
            }


            // �ߺ��� �������� �����Ѵ�.
            ClearDuplicatedData(ref _rawData);

            // rawData ���� �Է� Ȯ��
            // - ���� ����
            //DebugRawData(ref _rawData);

            // ���ĵ� rawData -> _elements�� ����
            SetElementsData(ref _rawData, ref _elements);

            // ��¥ ������ ��µǴ��� Ȯ��
            DebugElements(ref _elements);
        }

        #region Raw Data �ʱ�ȭ, �ʱ� ����
        /// <summary>
        /// �߰�ó���� �����ͼ� �ʱ�ȭ
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
        /// �ջ��������� �̷������� �Ҵ��Ѵ�.
        /// </summary>
        /// <param name="_record"></param>
        /// <param name="_target"></param>
        private void SetSingleDateTime(RecordInstance _record, ref Dictionary<IssueClass, Dictionary<IssueCode, Dictionary<DateTime, dateElement>>> _target)
        {
            // �ð� ������ ���� ������ ����
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
        /// ���� �̷������� Dictionary�� �Ҵ��Ѵ�.
        /// </summary>
        /// <param name="_count"></param>
        /// <param name="_date"></param>
        /// <param name="_class"></param>
        /// <param name="_code"></param>
        /// <param name="_target"></param>
        private void SetSingleDateCount(int _count, DateTime _date, IssueClass _class, IssueCode _code, List<Issue> _issues, ref Dictionary<IssueClass, Dictionary<IssueCode, Dictionary<DateTime, dateElement>>> _target)
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
                    // ���� ������ ������ŭ ī��Ʈ ���� ����� �����Ѵ�.
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
                    // �ջ� ������ ������ŭ ī��Ʈ ���� ����� �����Ѵ�.
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

        #region ClearDuplicatedData �ߺ��� �̽����� ����

        /// <summary>
        /// �ߺ��� �̽����� �����Ѵ�.
        /// </summary>
        /// <param name="_target"></param>
        private void ClearDuplicatedData(ref Dictionary<IssueClass, Dictionary<IssueCode, Dictionary<DateTime, dateElement>>> _target)
        {
            // �ջ� �����鿡�� �ߺ��� �����Ѵ�.
            foreach (IssueCode key in _target[IssueClass.Dmg].Keys)
            {
                //Debug.Log($"Class Dmg");
                ClearDuplicatedData_PerDmgIssueCode(_target[IssueClass.Dmg][key]);
            }
        }

        /// <summary>
        /// �ջ������鿡�� �ߺ��� �������� �����Ѵ�.
        /// </summary>
        /// <param name="_target"></param>
        private void ClearDuplicatedData_PerDmgIssueCode(Dictionary<DateTime, dateElement> _target)
        {
            // �ߺ� Ȯ�ο� ����
            Dictionary<string, dateElement> keyChecker = new Dictionary<string, dateElement>();

            // ��¥ ������ Ű�� �ݺ�
            foreach (DateTime key in _target.Keys)
            {

                // ����Ʈ ��� �Ҵ�
                List<Issue> _issues = _target[key]._issues;

                int index = _issues.Count;
                if (index != 0)
                {
                    for (int i = index-1; i >= 0; i--)
                    {


                        // Ű���� �̽� �ڵ尡 �����ϴ°�?
                        if (!keyChecker.ContainsKey(_issues[i].IssueOrderCode))
                        {
                            // �������� �ʴ� ���
                            // - keyChecker�� �� �߰�
                            keyChecker.Add(_issues[i].IssueOrderCode, _target[key]);
                        }
                        else
                        {
                            // �ջ� �ڵ�Ű �Ҵ�
                            string _key = _issues[i].IssueOrderCode;

                            //-----------------------------------------------
                            // check�� ��¥�� ���� target ��Ұ� ��¥��
                            // ��¥�� ���� ���� ��Ҹ� �츮��
                            // ��¥�� ���� ���� ��Ҹ� ���ش�

                            // �����ϴ� ���
                            // - ������ ��¥�� ���Ѵ�.
                            DateTime beforeDateTime = keyChecker[_key]._date;
                            DateTime nowDateTime = _target[key]._date;

                            int dateCount = beforeDateTime.CompareTo(nowDateTime);

                            // before���� �� ������ ���
                            if (dateCount < 0)
                            {
                                // ���� ���� ����
                                _target[key]._count--;  // ī��Ʈ 1 ����
                                RemoveListElement(_key, ref _target[key]._issues);      // - ���� �� recordIssue ����
                            }
                            // �� ��¥�� �������
                            else if (dateCount == 0)
                            {
                                // ���� �����Ƿ� ī��Ʈ ���� ����
                                _target[key]._count--;
                            }
                            // now���� �� ������ ���
                            else
                            {
                                // ���� ���� ����
                                keyChecker[_key]._count--;  // ī��Ʈ 1 ����
                                RemoveListElement(_key, ref keyChecker[_key]._issues);  // - ���� �� recordIssue ����
                                // - ���� �� dictionary Ű������ ����
                                keyChecker.Remove(_key);

                                // ���� ���� ����
                                // - dictionary�� �߰�
                                keyChecker.Add(_issues[i].IssueOrderCode, _target[key]);
                            }

                        }
                    }
                }
            }
        }

        /// <summary>
        /// �ߺ����� Ȯ�ε� ����Ʈ�� ��Ҹ� �����Ѵ�.
        /// </summary>
        /// <param name="codeKey"></param>
        /// <param name="_list"></param>
        private void RemoveListElement(string codeKey, ref List<Issue> _list)
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
        /// ���� ��� ī��Ʈ�� 0�� �� ��Ҹ� �����Ѵ�.
        /// </summary>
        /// <param name="_inElement"></param>
        private void Clear0CountDataElement(ref Dictionary<DateTime, int> _inElement, IssueClass _class)
        {
            Dictionary<DateTime, int> newElement = new Dictionary<DateTime, int>();

            // foreach �ݺ��߿� Remove�� �Ұ����� �� ������ ����� �ű⿡ �ùٸ� ������ �Ҵ��� ������ ��ȯ
            foreach (DateTime key in _inElement.Keys)
            {
                if (_inElement[key] != 0)
                {
                    newElement.Add(key, _inElement[key]);
                }
            }

            // ���� ���� �ʱ�ȭ
            _inElement.Clear();

            foreach (DateTime key in newElement.Keys)
            {
                _inElement.Add(key, newElement[key]);
            }
            // ������ ������ �Ҵ�
            //_inElement = newElement;
        }

        #endregion

        #endregion

        private void SetDateTimePanels(ref Dictionary<IssueClass, Dictionary<IssueCode, Dictionary<DateTime, int>>> _elements)
        {
            // �ѱ涧 �ʿ��� ����
            // - �ּ�, �ִ� ����
            // - ��¥ ����Ʈ
            // - �� �ɼ�

            // �ּ� ���� [0]
            // �ִ� ���� [1]
            int[] minMaxYears = GetMinMaxYear(ref _elements);

            //ViewTypeOption // �ð�ȭ �ɼ� Ȯ��
            // �� ���� �ɼ�
            SetPanel(
                minMaxYear: minMaxYears,
                viewOption: ViewTypeOption,
                _elements: ref _elements);
        }

        /// <summary>
        /// �ּ� / �ִ� ���� ��ȯ
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
                        // ���� ����
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
        /// �ɼǿ� ���� �гλ���
        /// �ּ�, �ִ� ���� / ��¥ ������ ������ �гο� ����
        /// </summary>
        /// <param name="minMaxYear"></param>
        /// <param name="viewOption"></param>
        /// <param name="_elements"></param>
        private void SetPanel(int[] minMaxYear, ViewTypeGraph viewOption, ref Dictionary<IssueClass, Dictionary<IssueCode, Dictionary<DateTime, int>>> _elements)
        {
            // �ִ� ������ Ÿ��Ʋ ����
            // �ɼǺ��� ���� Ÿ��Ʋ�� ����
            if (minMaxYear[1] == 0)
            {
                minMaxYear[0] = DateTime.Now.Year;
                minMaxYear[1] = DateTime.Now.Year;
            }

            SetYear(minMaxYear[1], viewOption);

            // ���� ���� ���� �ֽ� ������ �Ҵ�
            currentYear = minMaxYear[1];

            // �ɼǺ��� ���� �ε����� �Ҵ��Ѵ�.
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
                titleYear.text = $"{year}��";
            }
            else if (_type == ViewTypeGraph.Year_5)
            {
                titleYear.text = $"{year-5} ~ {year}��";
            }
            else if (_type == ViewTypeGraph.Year_10)
            {
                titleYear.text = $"{year-10} ~ {year}��";
            }
            else if (_type == ViewTypeGraph.Year_Total)
            {
                titleYear.text = $"{year-100} ~ {year}��";
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

            Manager.UIManager.Instance.isCapReady2 = true;

            yield break;
        }

        private IEnumerator ReadyToCapture()
        {
            ViewTypeOption = ViewTypeGraph.Year_1;

            SetPanelElements(RuntimeData.RootContainer.Instance.IssueObjectList);

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

            Manager.UIManager.Instance.isCapReady2 = true;

            yield break;
        }
    }
}
