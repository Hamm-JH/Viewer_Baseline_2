using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management
{
	using Definition;

	public partial class ContentManager : IManager<ContentManager>
	{
		/// <summary>
		/// 컨텐츠 관리자가 시작될때 초기화 실행
		/// </summary>
		private void OnInitialize()
		{
			// 컨텐츠 관리자 전달
			MainManager.Instance.SetContentManager(this, OnUpdate_System);
		}

		//private void LayerUpdate(List<LayerCode> _codes)
		//{
		//	m_uiList.ForEach(x => x.Set(_codes));
		//}

		private void OnUpdate_System(Definition.Data.CoreManagement _cData)
		{
			// CoreManagement, CoreData를 받아온다.
			// 생성할 모듈, 기능 목록을 받아서 초기화 수행한다.

			// 1. 모듈 생성, 컨텐츠 관리자 자식에 할당
			// 2. 생성된 모듈에 기능코드 할당
			// 3. 각 모듈에 기능 실행 (Do) 진행
			// 4. 각 모듈에서 필요 데이터 알아서 가져가기
			Debug.LogError("시스템에서 필수 데이터를 받아옴. 이후 작업 필요");

			switch(_cData._Platform)
			{
				case Definition.PlatformCode.PC_Maker1:
					model.OnExport();
					break;

				case Definition.PlatformCode.PC_Viewer1:
					model.OnImport(MainManager.Instance.ModelURI);
					break;
			}
		}
	}
}
