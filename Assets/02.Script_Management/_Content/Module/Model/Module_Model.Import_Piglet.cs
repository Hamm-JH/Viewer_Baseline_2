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
	public partial class Module_Model : MonoBehaviour, IModule
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

		private void OnProgress(GltfImportStep step, int completed, int total)
		{
			Debug.LogFormat("{0}: {1}/{2}", step, completed, total);
		}

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
