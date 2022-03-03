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
		// 손상, 보수 정보 수집이 끝나면 컨텐츠 관리자의 _Model 모듈로 리스트 데이터 전달
		private void OnComplete(WebType _webT, List<Issue> _data)
		{
			// 모델로 이슈 데이터 전달
			ContentManager.Instance._Model.GetIssue(_webT, _data);
		}
	}
}
