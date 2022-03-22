using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MasterTemplate
{
	public static string FinalReplace(string selected)
	{
		string value = selected;

		if(Manager.MainManager.Instance.AppUseCase == Manager.Definition.UseCase.Bridge)
		{
			if (selected.Contains("교대") && !selected.Contains("01"))
			{
				value = selected.Replace(value.Split(' ')[1], "02");
			}
			else if (selected.Contains("교각"))
			{
				string _value = value.Split(' ')[1];
				int integer;
				if (int.TryParse(_value, out integer))
				{
					integer--;
					string replaceValue = integer / 10 != 0 ? "" : "0";
					_value = _value.Replace(_value, $"{replaceValue}{integer.ToString()}");

					value = $"{value.Split(' ')[0]} {_value}";
					//value.Replace(value.Split(' ')[1], _value);
				}
			}
		}
		else if(Manager.MainManager.Instance.AppUseCase == Manager.Definition.UseCase.Tunnel)
		{
			Debug.LogWarning("MasterTemplate.cs // 1101 34 마스터 템플릿에서 이름 변경");
		}

		return value;
	}
}
