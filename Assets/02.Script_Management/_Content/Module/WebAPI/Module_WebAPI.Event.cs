using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.WebAPI
{
	using Definition;
	using Definition._Issue;
	using Management;

	public partial class Module_WebAPI : AModule
	{
		private void OnComplete(WebType _webT, List<Issue> _data)
		{
			// 모델로 이슈 데이터 전달
			ContentManager.Instance._Model.GetIssue(_webT, _data);
		}
	}
}
