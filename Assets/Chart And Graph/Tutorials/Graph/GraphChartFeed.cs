#define Graph_And_Chart_PRO
using UnityEngine;
using ChartAndGraph;
using System.Collections.Generic;

public class GraphChartFeed : MonoBehaviour
{
    public enum GraphTemplate
	{
        Year1,
        Year5,
        Year10,
        Year50,
    }

    public GraphTemplate m_gTemplate;
    public HorizontalAxis m_horizontalAxis;

	void Start ()
    {
        GraphChartBase graph = GetComponent<GraphChartBase>();

        switch(m_gTemplate)
		{
            case GraphTemplate.Year1:
                Demo_Set_Year1(graph, m_horizontalAxis);
                break;

            case GraphTemplate.Year5:
                Demo_Set_Year5(graph, m_horizontalAxis);
                break;

            case GraphTemplate.Year10:
                Demo_Set_Year10(graph, m_horizontalAxis);
                break;

            case GraphTemplate.Year50:
                Demo_Set_Year50(graph, m_horizontalAxis);
                break;
		}

        return;
        if (graph != null)
        {
            graph.HorizontalValueToStringMap[0.0] = "Zero"; // example of how to set custom axis strings
            graph.DataSource.StartBatch();
            graph.DataSource.ClearCategory("Player 1");
            graph.DataSource.ClearAndMakeBezierCurve("Player 2");
            
            for (int i = 0; i <5; i++)
            {
                graph.DataSource.AddPointToCategory("Player 1",i,Random.value*10f + 20f);
                if (i == 0)
                    graph.DataSource.SetCurveInitialPoint("Player 2",i, Random.value * 10f + 10f);
                else
                    graph.DataSource.AddLinearCurveToCategory("Player 2", 
                                                                    new DoubleVector2(i , Random.value * 10f + 10f));
            }
            graph.DataSource.MakeCurveCategorySmooth("Player 2");
            graph.DataSource.EndBatch();
        }
    }

    private void Demo_Set_Year1(GraphChartBase _graph, HorizontalAxis _hAxis)
	{
        _hAxis.MainDivisions.Total = 12;

        List<int> dmgIndexes = new List<int>();
        List<int> rcvIndexes = new List<int>();

		for (int i = 0; i < 49; i++)
		{
            dmgIndexes.Add(i);
            rcvIndexes.Add(49 - i);
		}

        Demo_Set_Year1(_graph, "Player 1", dmgIndexes);
        Demo_Set_Year1(_graph, "Player 2", rcvIndexes);
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

    private void Demo_Set_Year5(GraphChartBase _graph, HorizontalAxis _hAxis)
	{
        _hAxis.MainDivisions.Total = 10;

        int currYear = 2023;
        List<int> dmgIndexes = new List<int>();
        List<int> rcvIndexes = new List<int>();

        for (int i = 0; i < 11; i++)
        {
            dmgIndexes.Add(i);
            rcvIndexes.Add(11 - i);
        }

        Demo_Set_Year5(_graph, "Player 1", currYear, dmgIndexes);
        Demo_Set_Year5(_graph, "Player 2", currYear, rcvIndexes);
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

    private void Demo_Set_Year10(GraphChartBase _graph, HorizontalAxis _hAxis)
	{
        _hAxis.MainDivisions.Total = 10;

        int currYear = 2023;
        List<int> dmgIndexes = new List<int>();
        List<int> rcvIndexes = new List<int>();

        for (int i = 0; i < 11; i++)
        {
            dmgIndexes.Add(i);
            rcvIndexes.Add(11 - i);
        }

        Demo_Set_Year10(_graph, "Player 1", currYear, dmgIndexes);
        Demo_Set_Year10(_graph, "Player 2", currYear, rcvIndexes);
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

    private void Demo_Set_Year50(GraphChartBase _graph, HorizontalAxis _hAxis)
	{
        _hAxis.MainDivisions.Total = 10;

        int currYear = 2023;
        List<int> dmgIndexes = new List<int>();
        List<int> rcvIndexes = new List<int>();

        for (int i = 0; i < 11; i++)
        {
            dmgIndexes.Add(i);
            rcvIndexes.Add(11 - i);
        }

        Demo_Set_Year50(_graph, "Player 1", currYear, dmgIndexes);
        Demo_Set_Year50(_graph, "Player 2", currYear, rcvIndexes);
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
            //string k2 = _indexes[i].ToString();
            //if (i%4 != 0)
            //{
            //_graph.HorizontalValueToStringMap[i+1] = "";
            //}
        }

        _graph.DataSource.EndBatch();
    }
}
