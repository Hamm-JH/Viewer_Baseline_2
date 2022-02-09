using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management
{
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

		private void OnUpdate_System(Definition.Data.CoreManagement _cData)
		{
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
