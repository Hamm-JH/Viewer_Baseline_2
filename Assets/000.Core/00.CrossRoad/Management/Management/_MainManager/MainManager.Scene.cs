using Definition;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
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
			else if(Platforms.IsSmartInspectPlatform(_pCode))
            {
				sceneName = SceneName.SmartInspectViewer;
            }
			else
            {
				throw new Definition.Exceptions.PlatformNotDefinedException(_pCode);
            }

			EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
			int index = scenes.Length;
            for (int i = 0; i < index; i++)
            {
				//Debug.Log(scenes[i].path.ToString());
				// buildSetting에 존재하는 scene 이름이 sceneName과 같은가?
				if(scenes[i].path.Split('/').Last().Split('.').First() == sceneName.ToString())
                {
					SceneManager.LoadScene(sceneName.ToString());
					return;	// 존재하면 메서드 끝내기
                }
            }

			// 여기 넘어왔다는 뜻은 맞는 scene이 없다는뜻
			throw new Definition.Exceptions.SceneNotExisted(sceneName);

		}
	}
}
