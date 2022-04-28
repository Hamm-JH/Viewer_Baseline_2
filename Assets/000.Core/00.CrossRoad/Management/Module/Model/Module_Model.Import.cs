using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.Model
{
	using Definition;
	using Management;
	
	public partial class Module_Model : AModule
	{
		private void InImport(string URI)
		{
			StartImport(URI, OnComplete);
		}

		private void OnComplete(GameObject _model)
		{
			Model = _model;
			//---

			GameObject root = new GameObject("root");
			root.transform.position = default(Vector3);
			root.transform.rotation = Quaternion.identity;

			Model.transform.SetParent(root.transform);
			PlatformCode platform = MainManager.Instance.Platform;
			if(Platforms.IsTunnelPlatform(platform))
			{
			}
			else if(Platforms.IsBridgePlatform(platform))
			{
				Model.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
			}
			
			Transform[] children = Model.transform.GetComponentsInChildren<Transform>();
			SetChildren(children);

			// TODO 0223
			if(Platforms.IsTunnelPlatform(platform))
			{
				InitializeObjectTunnel(root);
			}
			else if(Platforms.IsBridgePlatform(platform))
			{
				InitializeObjectBridge(root);
			}
			else
            {
				throw new Definition.Exceptions.PlatformNotDefinedException(platform);
            }

			// Model Import 모듈 작업 완료
			ContentManager.Instance.CheckInitModuleComplete(ID);
			//ContentManager.Instance.SetCameraCenterPosition();
			
		}

		private void SetChildren(Transform[] children)
		{
			SetChildren_Material(children);

			SetChildren_Bounds(children);
		}

		private void SetChildren_Material(Transform[] children)
		{
			Material mat = Materials.Set(MaterialType.Default1);

			foreach (Transform tr in children)
			{
				MeshRenderer render;
				if (tr.TryGetComponent<MeshRenderer>(out render))
				{
					render.material = mat;
				}
			}
		}

		private void SetChildren_Bounds(Transform[] children)
		{
			float? minX = null;
			float? minY = null;
			float? minZ = null;
						  
			float? maxX = null;
			float? maxY = null;
			float? maxZ = null;

			foreach(Transform tr in children)
			{
				MeshRenderer render;
				if(tr.TryGetComponent<MeshRenderer>(out render))
				{
					Bounds _b = render.bounds;

					Vector3 min = _b.min;
					Vector3 max = _b.max;

					// 처음이 아니라면
					if(minX != null || minY != null || minZ != null)
					{
						minX = minX > min.x ? min.x : minX;
						minY = minY > min.y ? min.y : minY;
						minZ = minZ > min.z ? min.z : minZ;
					}
					else
					{
						minX = min.x;
						minY = min.y;
						minZ = min.z;
					}

					// 처음이 아니라면
					if (maxX != null || maxY != null || maxZ != null)
					{
						maxX = maxX <= max.x ? max.x : maxX;
						maxY = maxY <= max.y ? max.y : maxY;
						maxZ = maxZ <= max.z ? max.z : maxZ;
					}
					else
					{
						maxX = max.x;
						maxY = max.y;
						maxZ = max.z;
					}
				}
			}

			Vector3 rMin = new Vector3(
				(float)minX, (float)minY, (float)minZ);

			Vector3 rMax = new Vector3(
				(float)maxX, (float)maxY, (float)maxZ);

			Vector3 center = (rMin + rMax) / 2;

			Vector3 size = (rMax - rMin);

			Bounds result = new Bounds();
			result.center = center;
			result.size = size;

			CenterBounds = result;

			// 모델의 바운드 값으로 경계를 계산해 넘김
			MainManager.Instance.ResetCamdata_targetOffset();
			MainManager.Instance.UpdateCamData_maxOffset(CenterBounds.size.magnitude);
		}
	}
}
