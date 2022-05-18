#define Graph_And_Chart_PRO
using UnityEngine;
using ChartAndGraph;
using System.Collections.Generic;

using Module.UI;
using System.Data;
using System;
using static AdminViewer.UI.Ad_Panel;
using Issue;

public class GraphChartFeed : MonoBehaviour
{
    public enum GraphTemplate
	{
        Year1,
        Year5,
        Year10,
        Year50,
    }

    public UITemplate_SmartInspect uiRoot;
    public GraphTemplate m_gTemplate;
    public HorizontalAxis m_horizontalAxis;

    public int m_currYear;
    public int m_currIssueIndex;

    public List<RecordInstance> m_records;

    // �߻��� �̽��� ���� ����
    Dictionary<IssueClass, Dictionary<IssueCode, Dictionary<DateTime, int>>> issueElements;

	#region Init

	public void Init()
    {
        m_gTemplate = GraphTemplate.Year1;

        ResetIssueElements();

        //issueElements = new Dictionary<IssueClass, Dictionary<IssueCode, Dictionary<DateTime, int>>>();

        // ������ ��û
        uiRoot.API_requestHistoryData(GetHistoryData);
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

    private void GetHistoryData(DataTable _table)
    {
        Debug.Log("Hello history data");

        m_records = new List<RecordInstance>();

        int index = _table.Rows.Count;
        //Debug.Log($"table count : {index}");
        for (int i = 0; i < index; i++)
        {
            RecordInstance _record = new RecordInstance();

            string _date = _table.Rows[i]["date"].ToString();

            _record.dateTime = new DateTime(
                int.Parse(_date.Split('-')[0]),
                int.Parse(_date.Split('-')[1]),
                int.Parse(_date.Split('-')[2]));

            _record.DCrackList = SetRowToList(_table.Rows[i]["Dcrack"]);
            _record.DEfflorescenceList = SetRowToList(_table.Rows[i]["Dbagli"]);
            _record.DSpallingList = SetRowToList(_table.Rows[i]["Dbaegtae"]);
            _record.DBreakageList = SetRowToList(_table.Rows[i]["Ddamage"]);
            _record.DScour_ErosionList = SetRowToList(_table.Rows[i]["Dsegul"]);

            _record.RCrackList = SetRowToList(_table.Rows[i]["Rcrack"]);
            _record.REfflorescenceList = SetRowToList(_table.Rows[i]["Rbagli"]);
            _record.RSpallingList = SetRowToList(_table.Rows[i]["Rbaegtae"]);
            _record.RBreakageList = SetRowToList(_table.Rows[i]["Rdamage"]);
            _record.RScour_ErosionList = SetRowToList(_table.Rows[i]["Rsegul"]);

            // ���� ���ֱ�
            ResetRecord(_record);

            if (!IsNullInstance(_record) == true)
            {
                m_records.Add(_record);
            }
        }

        // ������ ��¥, ī��Ʈ ���Ĵܰ�
        SetDateTimeIssue(
            _list: m_records,
            _elements: ref issueElements);

        // TODO : ���� 1��, 5��, 10��, ��ü ������ �Ǵ�
        // 1�� ���� �۾�
        // �г� ���� / ������ ���� �ܰ�
        //SetDateTimePanels(ref issueElements);

        SetDateTimePanel(DateTime.Today.Year, _issueIndex: 0, GraphTemplate.Year1);
    }

	#endregion

	#region 1 Set Record Instances
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

    #region 2 Set DateTime Issue �ʱ� (��¥, �߻� ȸ��) ����
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
            List<Definition._Issue.Issue> _issues = _target[key]._issues;

            int index = _issues.Count;
            if (index != 0)
            {
                for (int i = index - 1; i >= 0; i--)
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
    private void RemoveListElement(string codeKey, ref List<Definition._Issue.Issue> _list)
    {
        int index = _list.Count;
        for (int i = index - 1; i >= 0; i--)
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

	#region ----- Interface -----

    /// <summary>
    /// ���� ���� ��ư ���ý� ����
    /// </summary>
    /// <param name="_index"> 0 : Y1 // 1 : Y5 // 2 : Y10 / 3 : Y50</param>
    public void Update_YearTemplate(int _index)
	{
        GraphTemplate template = GraphTemplate.Year1;
        switch(_index)
		{
            case 0: template = GraphTemplate.Year1; break;
            case 1: template = GraphTemplate.Year5; break;
            case 2: template = GraphTemplate.Year10; break;
            case 3: template = GraphTemplate.Year50; break;
        }

        SetDateTimePanel(m_currYear, m_currIssueIndex, template);
	}

    /// <summary>
    /// �ջ� ���� ���� ��ư Ŭ���� ����
    /// </summary>
    /// <param name="_index"> 0 : ALL // 1 : Crack // 2 : Spalling // 3 : Efflorescence // 4 : Breakage </param>
    public void Update_IssueIndex(int _index)
	{
        m_currIssueIndex = _index;

        SetDateTimePanel(m_currYear, m_currIssueIndex, m_gTemplate);
	}

    public void Update_PrevYear()
	{
        int difference = 0;
        switch(m_gTemplate)
		{
            case GraphTemplate.Year1:   difference = 1; break;
            case GraphTemplate.Year5:   difference = 5; break;
            case GraphTemplate.Year10:   difference = 10; break;
            case GraphTemplate.Year50:   difference = 50; break;
        }

        m_currYear -= difference;

        if(m_currYear < DateTime.Now.Year - 50)
		{
            m_currYear = DateTime.Now.Year - 50;
		}

        SetDateTimePanel(m_currYear, m_currIssueIndex, m_gTemplate);
	}

    public void Update_NextYear()
	{
        int difference = 0;
        switch (m_gTemplate)
        {
            case GraphTemplate.Year1: difference = 1; break;
            case GraphTemplate.Year5: difference = 5; break;
            case GraphTemplate.Year10: difference = 10; break;
            case GraphTemplate.Year50: difference = 50; break;
        }

        m_currYear += difference;

        if(m_currYear > DateTime.Now.Year)
		{
            m_currYear = DateTime.Now.Year;
		}

        SetDateTimePanel(m_currYear, m_currIssueIndex, m_gTemplate);
    }

	#endregion

	/// <summary>
	/// �����ϰ��� �ϴ� time panel�� �����.
	/// </summary>
	/// <param name="_year"></param>
	/// <param name="_template"></param>
	private void SetDateTimePanel(int _year, int _issueIndex, GraphTemplate _template)
    {
        m_currYear = _year;
        m_currIssueIndex = _issueIndex;
        m_gTemplate = _template;

        int yearIndex = -1;
        switch(m_gTemplate)
		{
            case GraphTemplate.Year1:
                yearIndex = 0;
                break;

            case GraphTemplate.Year5:
                yearIndex = 1;
                break;

            case GraphTemplate.Year10:
                yearIndex = 2;
                break;

            case GraphTemplate.Year50:
                yearIndex = 3;
                break;
        }
        // TODO Debug
        //SetDateTimePanel_Year1(_year, _issueIndex);
        SetDateTimePanel_Year(
                    _year: _year,
                    _issueIndex: _issueIndex,
                    _yearIndex: yearIndex);
    }

    /// <summary>
    /// _year :: ���� ����
    /// _issueIndex :: ALL, Crack, Spall, Effro, Breakage ����
    /// _yearIndex :; Y1 0 // Y5 1 // Y10 2 // Y50 3
    /// </summary>
    /// <param name="_year"> ���� ���� </param>
    /// <param name="_issueIndex"> ALL 0 // Crack 1 // Spall 2 // Effro 3 // Breakage 4 </param>
    /// <param name="_yearIndex"> Y1 0 // Y5 1 // Y10 2 // Y50 3 </param>
    private void SetDateTimePanel_Year(int _year, int _issueIndex, int _yearIndex)
    {
        // ��� ��������� ó�� :: 0
        // Crack :: 1
        // Spalling :: 2
        // Efflorescence :: 3
        // Breakage :: 4

        List<int> dmgResult;
        List<int> rcvResult;

        // �ϴ� dmg all
        // ALL
        if(_issueIndex == 0)
        {
            dmgResult = CreateList(_yearIndex: _yearIndex);
            rcvResult = CreateList(_yearIndex: _yearIndex);

            // ��¥ �������� ���� �⵵���� ������ ��ȭ�� ����
            //int prevIndex = 0;  // ���۰� �Ҵ�

            foreach(IssueCode key in issueElements[IssueClass.Dmg].Keys)
            {
                SetIndexPerIssue(_year, _yearIndex: _yearIndex, issueElements[IssueClass.Dmg][key], dmgResult);
            }

            foreach(IssueCode key in issueElements[IssueClass.Rcv].Keys)
            {
                SetIndexPerIssue(_year, _yearIndex: _yearIndex, issueElements[IssueClass.Rcv][key], rcvResult);
            }

            // �ڡڡ� ��� ������ �Ҵ��� ���� ��, �׷����� ������ �Ҵ� ��û
            Set_YearGraph(_tgYear: _year, dmgResult, rcvResult, _yearIndex);
        }
        // Crack
        else if(_issueIndex == 1 || _issueIndex == 2 || _issueIndex == 3 || _issueIndex == 4)
        {
            dmgResult = CreateList(_yearIndex: _yearIndex);
            rcvResult = CreateList(_yearIndex: _yearIndex);

            IssueCode code = IssueCode.Null;
            switch(_issueIndex)
			{
                case 1: code = IssueCode.Crack; break;
                case 2: code = IssueCode.Spalling; break;
                case 3: code = IssueCode.Efflorescense; break;
                case 4: code = IssueCode.Breakage; break;
            }

            if(issueElements[IssueClass.Dmg].ContainsKey(code))
            {
                SetIndexPerIssue(_year, _yearIndex: _yearIndex, issueElements[IssueClass.Dmg][code], dmgResult);
            }

            if(issueElements[IssueClass.Rcv].ContainsKey(code))
            {
                SetIndexPerIssue(_year, _yearIndex: _yearIndex, issueElements[IssueClass.Rcv][code], rcvResult);
            }

            Set_YearGraph(_tgYear: _year, dmgResult, rcvResult, _yearIndex);
        }

        Debug.Log($"year {_year} :: index {_issueIndex}");

    }

    /// <summary>
    /// ����Ʈ ����
    /// Y1 : 49
    /// Y5 : 10
    /// Y10 : 10
    /// Y50 : 50
    /// </summary>
    /// <param name="_yearIndex"></param>
    /// <returns></returns>
    private List<int> CreateList(int _yearIndex)
    {
        int count = 0;
        switch(_yearIndex)
        {
            case 0:
                count = 49;
                break;

            case 1:
            case 2:
            case 3:
                count = 11;
                break;
        }

        List<int> list = new List<int>();
        for (int i = 0; i < count; i++)
        {
            list.Add(0);
        }

        return list;
    }

    #region Set Index per Issue

    /// <summary>
    /// ����Ʈ ���� ������ �Ҵ� //
    /// yearIndex
    /// 0 :: Y1 //
    /// 1 :: Y5 //
    /// 2 :: Y10 // 
    /// 3 :: Y50 //
    /// </summary>
    /// <param name="_values"></param>
    /// <param name="result"></param>
    private void SetIndexPerIssue(int _tgYear, int _yearIndex, Dictionary<DateTime, int> _values, List<int> result)
    {
        int prevCount = 0;  // ���� �⵵�� ���������� �� (�ջ��� ��� �̽��� ���ϱ��)

        int yearDifference = SetYearDifference(_yearIndex);

        // Y1
        foreach(DateTime _date in _values.Keys)
        {
            // Ű���� ������ ��ǥ �������� ���� ���
            if(IsPrevYear(_date.Year, _tgYear, yearDifference) /*_date.Year < _tgYear*/)
            {
                // ���� �����տ� value�� ���ϱ�
                prevCount += _values[_date];
            }
            // Ű���� ������ ��ǥ ������ ������
            else if(IsTargetYear(_date.Year, _tgYear, yearDifference) /*_date.Year == _tgYear*/)
            {
                // Key Value ���� �Ѱܼ� ���� �Ҵ�
                switch(_yearIndex)
                {
                    case 0:
                        SetIndexPerDateIssue_Year1(_date, _values[_date], result);
                        break;

                    case 1:
                        SetIndexPerDateIssue_Year5(_date, _values[_date], _tgYear, result);
                        break;

                    case 2:
                        SetIndexPerDateIssue_Year10(_date, _values[_date], _tgYear, result);
                        break;

                    case 3:
                        SetIndexPerDateIssue_Year50(_date, _values[_date], _tgYear, result);
                        break;
                }
                //SetIndexPerDateIssue_Year1(_date, _values[_date], result);
            }
        }

        // ������ ���� �⵵ �����͸� ��� ��� ����Ʈ�� �ջ�
        for (int i = 0; i < result.Count; i++)
        {
            result[i] += prevCount;
        }
    }

    private int SetYearDifference(int _yearIndex)
    {
        int result = 0;

        switch(_yearIndex)
        {
            case 0:
                result = 0;
                break;

            case 1:
                result = 5;
                break;

            case 2:
                result = 10;
                break;

            case 3:
                result = 50;
                break;
        }

        return result;
    }

    /// <summary>
    /// dateYear�� ���� �⵵���� ���ϴ°�?
    /// </summary>
    /// <param name="_dateYear"></param>
    /// <param name="_tgYear"></param>
    /// <param name="_yDifference"></param>
    /// <returns></returns>
    private bool IsPrevYear(int _dateYear, int _tgYear, int _yDifference)
    {
        bool result = false;

        if(_dateYear < _tgYear - _yDifference)
        {
            result = true;
        }

        return result;
    }

    /// <summary>
    /// dateYear�� ��ǥ ������ (���� �⵵�� ����)�� �����ϴ°�?
    /// </summary>
    /// <param name="_dateYear"></param>
    /// <param name="_tgYear"></param>
    /// <param name="_yDifference"></param>
    /// <returns></returns>
    private bool IsTargetYear(int _dateYear, int _tgYear, int _yDifference)
    {
        bool result = false;

        if(_dateYear >= _tgYear - _yDifference && _dateYear <= _tgYear)
        {
            result = true;
        }

        return result;
    }

    #endregion

    /// <summary>
    /// ��ǥ�⵵�� ���� ���������� �Ҵ�
    /// </summary>
    /// <param name="_date"></param>
    /// <param name="_index"></param>
    /// <param name="_result"></param>
    private void SetIndexPerDateIssue_Year1(DateTime _date, int _index, List<int> _result)
    {
        int index = 0;
        int month = _date.Month;

        int daysInMonth = DateTime.DaysInMonth(_date.Year, _date.Month);
        int day = _date.Day;

        // month �ݿ�
        index = (month - 1) * 4;
        
        // day �ݿ�
        float count = daysInMonth / 4;
        // day�� count / 2��ŭ ���ϰ� �̸� count�� ������ day�� ���� index ����
        index = (int)(index + (day + count / 2) / count);

        // Ư�� �������� ������
        for (int i = index; i < _result.Count; i++)
        {
            // Value ���� �ջ�
            _result[i] += _index;
        }
    }

    /// <summary>
    /// ��ǥ�⵵�� ���� ���������� �Ҵ�
    /// </summary>
    /// <param name="_date"></param>
    /// <param name="_index"></param>
    /// <param name="_result"></param>
    private void SetIndexPerDateIssue_Year5(DateTime _date, int _index, int _tgYear, List<int> _result)
    {
        int index = 0;

        // ��ǥ ������ date.Year�� ���̰�
        // 2022 - 2022 = 0
        // 2022 - 2017 = 5
        int yDifference = _tgYear - _date.Year;

        // ���� 2��
        int mIndex = (_date.Month - 1) / 6;

        index = (5 - yDifference) * 2 + mIndex;

		//index = (int)(index + (day + count / 2) / count);

		// Ư�� �������� ������
		for (int i = index; i < _result.Count; i++)
		{
			// Value ���� �ջ�
			_result[i] += _index;
		}
	}


    /// <summary>
    /// ��ǥ�⵵�� ���� ���������� �Ҵ�
    /// </summary>
    /// <param name="_date"></param>
    /// <param name="_index"></param>
    /// <param name="_result"></param>
    private void SetIndexPerDateIssue_Year10(DateTime _date, int _index, int _tgYear, List<int> _result)
    {
        int index = 10 - (_tgYear - _date.Year);

        // Ư�� �������� ������
        for (int i = index; i < _result.Count; i++)
        {
            // Value ���� �ջ�
            _result[i] += _index;
        }
    }

    /// <summary>
    /// ��ǥ�⵵�� ���� ���������� �Ҵ�
    /// </summary>
    /// <param name="_date"></param>
    /// <param name="_index"></param>
    /// <param name="_result"></param>
    private void SetIndexPerDateIssue_Year50(DateTime _date, int _index, int _tgYear, List<int> _result)
    {
        int index = (50 - (_tgYear - _date.Year)) / 5;

        // Ư�� �������� ������
        for (int i = index; i < _result.Count; i++)
        {
            // Value ���� �ջ�
            _result[i] += _index;
        }
    }




    #region Set Data to Graph

    //private void Demo_Set_Year1(GraphChartBase _graph, HorizontalAxis _hAxis)
    //{
    //    _hAxis.MainDivisions.Total = 12;
    //
    //    List<int> dmgIndexes = new List<int>();
    //    List<int> rcvIndexes = new List<int>();
    //
    //	for (int i = 0; i < 49; i++)
    //	{
    //        dmgIndexes.Add(i);
    //        rcvIndexes.Add(49 - i);
    //	}
    //
    //    Demo_Set_Year1(_graph, "Player 1", dmgIndexes);
    //    Demo_Set_Year1(_graph, "Player 2", rcvIndexes);
    //}

    //private void Demo_Set_Year5(GraphChartBase _graph, HorizontalAxis _hAxis)
    //{
    //    _hAxis.MainDivisions.Total = 10;
    //
    //    int currYear = 2023;
    //    List<int> dmgIndexes = new List<int>();
    //    List<int> rcvIndexes = new List<int>();
    //
    //    for (int i = 0; i < 11; i++)
    //    {
    //        dmgIndexes.Add(i);
    //        rcvIndexes.Add(11 - i);
    //    }
    //
    //    Demo_Set_Year5(_graph, "Player 1", currYear, dmgIndexes);
    //    Demo_Set_Year5(_graph, "Player 2", currYear, rcvIndexes);
    //}

    //private void Demo_Set_Year10(GraphChartBase _graph, HorizontalAxis _hAxis)
    //{
    //    _hAxis.MainDivisions.Total = 10;
    //
    //    int currYear = 2023;
    //    List<int> dmgIndexes = new List<int>();
    //    List<int> rcvIndexes = new List<int>();
    //
    //    for (int i = 0; i < 11; i++)
    //    {
    //        dmgIndexes.Add(i);
    //        rcvIndexes.Add(11 - i);
    //    }
    //
    //    Demo_Set_Year10(_graph, "Player 1", currYear, dmgIndexes);
    //    Demo_Set_Year10(_graph, "Player 2", currYear, rcvIndexes);
    //}

    //private void Demo_Set_Year50(GraphChartBase _graph, HorizontalAxis _hAxis)
    //{
    //    _hAxis.MainDivisions.Total = 10;
    //
    //    int currYear = 2023;
    //    List<int> dmgIndexes = new List<int>();
    //    List<int> rcvIndexes = new List<int>();
    //
    //    for (int i = 0; i < 11; i++)
    //    {
    //        dmgIndexes.Add(i);
    //        rcvIndexes.Add(11 - i);
    //    }
    //
    //    Demo_Set_Year50(_graph, "Player 1", currYear, dmgIndexes);
    //    Demo_Set_Year50(_graph, "Player 2", currYear, rcvIndexes);
    //}

    private void Set_YearGraph(int _tgYear, List<int> _dmgList, List<int> _rcvList, int _yearIndex)
    {
        int division = 0;
        switch(_yearIndex)
        {
            case 0: division = 12;  break;
            case 1:
            case 2:
            case 3: division = 10;  break;
        }
        m_horizontalAxis.MainDivisions.Total = division;

        GraphChartBase graph = GetComponent<GraphChartBase>();

        switch(_yearIndex)
		{
            case 0:
                Demo_Set_Year1(graph, "Player 1", _dmgList);
                Demo_Set_Year1(graph, "Player 2", _rcvList);
                break;

            case 1:
                Demo_Set_Year5(graph, "Player 1", _tgYear, _dmgList);
                Demo_Set_Year5(graph, "Player 2", _tgYear, _rcvList);
                break;

            case 2:
                Demo_Set_Year10(graph, "Player 1", _tgYear, _dmgList);
                Demo_Set_Year10(graph, "Player 2", _tgYear, _rcvList);
                break;

            case 3:
                Demo_Set_Year50(graph, "Player 1", _tgYear, _dmgList);
                Demo_Set_Year50(graph, "Player 2", _tgYear, _rcvList);
                break;
        }
        
    }

    /// <summary>
    /// index count 49
    /// </summary>
    /// <param name="_graph"></param>
    /// <param name="_cName"></param>
    /// <param name="_indexes"></param>
    private void Demo_Set_Year1(GraphChartBase _graph, string _cName, List<int> _indexes)
	{
        _graph.DataSource.StartBatch();

        _graph.DataSource.ClearCategory(_cName);

        int index = _indexes.Count;
		for (int i = 0; i < index; i++)
		{
            _graph.DataSource.AddPointToCategory(_cName, i+1, _indexes[i]);

            _graph.HorizontalValueToStringMap[i+1] = $"{(i+1)/4 + 1}";

            if(i > 47)
			{
                _graph.HorizontalValueToStringMap[i+1] = $"";
            }
			//string k2 = _indexes[i].ToString();
			//if (i%4 != 0)
			//{
				//_graph.HorizontalValueToStringMap[i+1] = "";
			//}
		}

        _graph.DataSource.EndBatch();
	}

    

    /// <summary>
    /// index count 11
    /// </summary>
    /// <param name="_graph"></param>
    /// <param name="_cName"></param>
    /// <param name="_currYear"></param>
    /// <param name="_indexes"></param>
    private void Demo_Set_Year5(GraphChartBase _graph, string _cName, int _currYear, List<int> _indexes)
	{
        _graph.DataSource.StartBatch();

        _graph.DataSource.ClearCategory(_cName);

        int index = _indexes.Count;
        for (int i = 0; i < index; i++)
        {
            _graph.DataSource.AddPointToCategory(_cName, i+1, _indexes[i]);

            _graph.HorizontalValueToStringMap[i+1] = $"{(_currYear - 5) + (i / 2)}";

            if (i % 2 == 1)
            {
                _graph.HorizontalValueToStringMap[i+1] = $"";
            }
            //string k2 = _indexes[i].ToString();
            //if (i%4 != 0)
            //{
            //_graph.HorizontalValueToStringMap[i+1] = "";
            //}
        }

        _graph.DataSource.EndBatch();
    }

    

    private void Demo_Set_Year10(GraphChartBase _graph, string _cName, int _currYear, List<int> _indexes)
	{
        _graph.DataSource.StartBatch();

        _graph.DataSource.ClearCategory(_cName);

        int index = _indexes.Count;
        for (int i = 0; i < index; i++)
        {
            _graph.DataSource.AddPointToCategory(_cName, i+1, _indexes[i]);

            _graph.HorizontalValueToStringMap[i+1] = $"{(_currYear - 10) + (i)}";

            //if (i % 2 == 1)
            //{
            //    _graph.HorizontalValueToStringMap[i+1] = $"";
            //}
        }

        _graph.DataSource.EndBatch();
    }

    

    private void Demo_Set_Year50(GraphChartBase _graph, string _cName, int _currYear, List<int> _indexes)
	{
        _graph.DataSource.StartBatch();

        _graph.DataSource.ClearCategory(_cName);

        int index = _indexes.Count;
        for (int i = 0; i < index; i++)
        {
            _graph.DataSource.AddPointToCategory(_cName, i+1, _indexes[i]);

            _graph.HorizontalValueToStringMap[i+1] = $"{(_currYear - 55) + (i * 5)} - {(_currYear - 55) + ((i+2) * 5)}";

            if (i % 2 == 0)
            {
                _graph.HorizontalValueToStringMap[i+1] = $"|";
            }
        }

        _graph.DataSource.EndBatch();
    }

    #endregion


}
