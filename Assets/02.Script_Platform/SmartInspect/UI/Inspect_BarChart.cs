using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmartInspect
{
	using ChartAndGraph;
	using Management.Content;
	using Module.Model;

	public class Inspect_BarChart : MonoBehaviour
	{
		public enum Template
		{
			DMG,
			RCV
		}

		public BarChart m_barChart;
		public Template m_template;

		public void Init()
		{
			//Debug.Log("Hello");

			m_barChart = GetComponent<BarChart>();
			Module_Model model = SmartInspectManager.Instance.Module<Module_Model>(Definition.ModuleID.Model);

			if(m_template == Template.DMG)
			{
				SetDmgChart(model);
			}
			else if(m_template == Template.RCV)
			{
				SetRcvChart(model);
			}
		}

		private void SetDmgChart(Module_Model model)
		{
			List<Definition._Issue.Issue> dmgIssues = model.DmgData;

			Dictionary<Definition._Issue.IssueCodes, int> dmgIndexes = SetIndexes();

			SetIndex(dmgIssues, dmgIndexes);

			SetChartIndexes(dmgIndexes);
		}

		private void SetRcvChart(Module_Model model)
		{
			List<Definition._Issue.Issue> rcvIssues = model.RcvData;

			Dictionary<Definition._Issue.IssueCodes, int> rcvIndexes = SetIndexes();

			SetIndex(rcvIssues, rcvIndexes);

			SetChartIndexes(rcvIndexes);
		}

		private Dictionary<Definition._Issue.IssueCodes, int> SetIndexes()
		{
			Dictionary<Definition._Issue.IssueCodes, int> result = new Dictionary<Definition._Issue.IssueCodes, int>();

			result.Add(Definition._Issue.IssueCodes.Crack, 0);
			result.Add(Definition._Issue.IssueCodes.Spalling, 0);
			result.Add(Definition._Issue.IssueCodes.Efflorescence, 0);
			result.Add(Definition._Issue.IssueCodes.breakage, 0);

			return result;
		}

		private void SetIndex(List<Definition._Issue.Issue> _issues, Dictionary<Definition._Issue.IssueCodes, int> _indexes)
		{
			_issues.ForEach(x =>
			{
				Definition._Issue.IssueCodes _code = x.IssueCode;
				if(_indexes.ContainsKey(_code))
				{
					_indexes[_code]++;
				}
			});
		}

		private string GetGroupName(Template _template, Definition._Issue.IssueCodes _code)
		{
			bool isDmg = _template == Template.DMG ? true : false;

			string name = "";

			if(isDmg)
			{
				switch(_code)
				{
					case Definition._Issue.IssueCodes.Crack:
						name = "Group 1";
						break;

					case Definition._Issue.IssueCodes.Spalling:
						name = "Group 2";
						break;

					case Definition._Issue.IssueCodes.Efflorescence:
						name = "Group 3";
						break;

					case Definition._Issue.IssueCodes.breakage:
						name = "Group 4";
						break;
				}
			}
			else
			{
				switch (_code)
				{
					case Definition._Issue.IssueCodes.Crack:
						name = "Group 4";
						break;

					case Definition._Issue.IssueCodes.Spalling:
						name = "Group 3";
						break;

					case Definition._Issue.IssueCodes.Efflorescence:
						name = "Group 2";
						break;

					case Definition._Issue.IssueCodes.breakage:
						name = "Group 1";
						break;
				}
			}

			return name;
		}

		private void SetChartIndexes(Dictionary<Definition._Issue.IssueCodes, int> _indexes)
		{
			m_barChart.DataSource.StartBatch();

			//m_barChart.DataSource.ClearCategories();
			foreach(Definition._Issue.IssueCodes key in _indexes.Keys)
			{
				SetChartIndex(GetGroupName(m_template, key), _indexes[key]);
			}

			m_barChart.DataSource.EndBatch();
		}

		private void SetChartIndex(string _group, int _index)
		{
			m_barChart.DataSource.SetValue("Category 1", _group, _index);
		}
	}
}
