using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.Model
{
	using Piglet;
	using UnityEngine.Events;
	using static Piglet.GltfImportTask;

	/// <summary>
	/// Piglet import module
	/// Reference : RuntimeImportBehaviour.cs
	/// </summary>
	public partial class Module_Model : AModule
	{
		/// <summary>
		/// The currently running glTF import task.
		/// </summary>
		private GltfImportTask _task;

		/// <summary>
		/// Unity callback that is invoked before the first frame.
		/// Create the glTF import task and set up callbacks for
		/// progress messages and successful completion.
		/// </summary>
		void StartImport(string URI, CompletedCallback completed)
		{
			_task = RuntimeGltfImporter.GetImportTask(URI);

			_task.OnProgress = OnProgress;
			_task.OnCompleted = completed;

			StartCoroutine(Processing(_task));
		}

		/// <summary>
		/// GLTF 불러오기 진행
		/// </summary>
		/// <param name="step">GLTF 파일 임포트 분류</param>
		/// <param name="completed">완료 코드</param>
		/// <param name="total">전체 인덱스</param>
		private void OnProgress(GltfImportStep step, int completed, int total)
		{
			Debug.LogFormat("{0}: {1}/{2}", step, completed, total);
		}

		/// <summary>
		/// GLTF 모델 처리
		/// </summary>
		/// <param name="task">GLTF 가져오기 작업</param>
		/// <returns></returns>
		private IEnumerator Processing(GltfImportTask task)
		{
			yield return new WaitForEndOfFrame();

			while(task.MoveNext())
			{
				yield return new WaitForEndOfFrame();
			}

			yield break;
		}
	}
}
