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

			if(Platforms.IsDemoAdminViewer(_pCode))
			{
				sceneName = SceneName.AdminViewer;
			}
			else if(Platforms.IsMakerPlatform(_pCode))
			{
				sceneName = SceneName.Maker;
			}
			else if(Platforms.IsViewerPlatform(_pCode))
			{
				sceneName = SceneName.Viewer;
			}

			if(Platforms.IsNotDefinition(_pCode))
			{
				Debug.LogError("올바른 플랫폼 코드가 아닙니다.");
				return;
			}

			SceneManager.LoadScene(sceneName.ToString());
			
		}
	}
}
