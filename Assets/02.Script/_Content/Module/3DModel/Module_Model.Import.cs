using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.Model
{
	public partial class Module_Model : MonoBehaviour, IModule
	{
		private void InImport()
		{
			GameObject root = new GameObject("root");
			root.transform.position = default(Vector3);
			root.transform.rotation = Quaternion.identity;

			Debug.LogError("여기서 이후에 웹 요청으로 GLTF 불러올 예정");

			GameObject _model = Instantiate(debugPrefab);

			_model.transform.SetParent(root.transform);

			Model = _model;
		}
	}
}
