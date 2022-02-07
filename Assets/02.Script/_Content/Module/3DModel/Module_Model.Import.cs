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

			Transform[] children = _model.transform.GetComponentsInChildren<Transform>();

			SetChildren(children);
		}

		private void SetChildren(Transform[] children)
		{
			Material mat = Resources.Load<Material>("Outlines");

			foreach(Transform tr in children)
			{
				MeshRenderer render;
				if(tr.TryGetComponent<MeshRenderer>(out render))
				{
					render.material = mat;
				}
			}

		}
	}
}
