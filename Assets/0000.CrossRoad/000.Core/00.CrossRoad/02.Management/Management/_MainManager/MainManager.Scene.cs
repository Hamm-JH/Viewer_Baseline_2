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
		/// <summary>
		/// 플랫폼 코드에 따라 씬을 생성한다.
		/// </summary>
		/// <param name="_pCode">플랫폼 코드</param>
		/// <exception cref="Definition.Exceptions.PlatformNotDefinedException">지정되지 않은 플랫폼 코드</exception>
		/// <exception cref="Definition.Exceptions.SceneNotExisted">생성하고자 하는 Scene이 존재하지 않음</exception>
		public void Load_Scene(PlatformCode _pCode)
		{
			Definition.SceneName sceneName = SceneName.NotDef;

			
			if(Platforms.IsDemoWebViewer(_pCode))
            {
				//sceneName = SceneName.Viewer;
				sceneName = SceneName.Mapbox_Demo;
			}
			else if(Platforms.IsDemoAdminViewer(_pCode))
			{
				sceneName = SceneName.AdminViewer;
			}
			else if(Platforms.IsMakerPlatform(_pCode))
			{
				sceneName = SceneName.Maker;
			}
			//else if(Platforms.IsViewerPlatform(_pCode))
			//{
			//	sceneName = SceneName.Viewer;
			//}
			else if(Platforms.IsSmartInspectPlatform(_pCode))
            {
				sceneName = SceneName.SmartInspectViewer;
            }
			else
            {
				throw new Definition.Exceptions.PlatformNotDefinedException(_pCode);
            }

			SceneManager.LoadScene(sceneName.ToString());

#if UNITY_EDITOR
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
#endif

		}
	}
}
