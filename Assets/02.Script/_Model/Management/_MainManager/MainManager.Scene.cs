using Definition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Management
{
	public partial class MainManager : IManager<MainManager>
	{
		public void Load_Scene(PlatformCode _pCode)
		{
			Definition.SceneName sceneName = SceneName.NotDef;

			switch(_pCode)
			{
				case PlatformCode.PC_Maker1:
					sceneName = SceneName.Maker;
					break;

				case PlatformCode.PC_Viewer1:
					sceneName = SceneName.PC_Viewer;
					break;
			}

			if(_pCode == PlatformCode.NotDef)
			{
				Debug.LogError("올바른 플랫폼 코드가 아닙니다.");
				return;
			}

			SceneManager.LoadScene(sceneName.ToString());
			
		}
	}
}
