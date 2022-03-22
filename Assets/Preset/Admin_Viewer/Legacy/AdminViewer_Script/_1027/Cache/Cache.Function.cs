using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data.Viewer
{
	public partial class Cache : MonoBehaviour
	{
		public void SetView(Transform _target)
		{
			//SetOff(_target, models.Objects);

			//Debug.LogError("Cache.Function // 1101 ");

			//GameObject tr = Dim.DimScript.Instance.selectedList.Find(x => x.transform.parent.name == "Dim");
			//tr.transform.parent.gameObject.SetActive(true);
			//tr.SetActive(true);

			//GameObject tr3D = Dim.DimScript.Instance.selectedList.Find(x => x.transform.parent.name == "Control root");
			//tr3D.transform.parent.gameObject.SetActive(true);
			//tr3D.SetActive(true);
		}

		#region 3D 객체 On/Off

		public void SetOn()
		{
			List<GameObject> objs = models.Objects;

			foreach(GameObject obj in objs)
			{
				obj.SetActive(true);
			}
		}

		public void SetOn(int layerNum, int mode = 0)
		{
			MeshRenderer render;
			Collider coll;
			foreach (GameObject obj in models.Objects)
			{
				if(obj.TryGetComponent<MeshRenderer>(out render))
				{
					if(mode == 0)
					{
						// 1. MeshRenderer 활성
						render.enabled = true;

						// 2. Material 기본값 변경
						//Datas.ObjectType param = Utilities.NameParameter.GetMatParameter(obj.name, 1);
						//Utilities.ReturnMaterial.SetMaterials(render, param);
					}
					else if(mode == 1)
					{
						render.material = Resources.Load<Material>("Material/MAT_Transparent");
					}
				}

				if(obj.TryGetComponent<Collider>(out coll))
				{
					// 3. Collider 활성
					coll.enabled = true;
				}
				obj.layer = layerNum;

				for (int i = 0; i < obj.transform.childCount; i++)
				{
					obj.transform.GetChild(i).gameObject.layer = layerNum;
				}
				//if(obj.name == "2,2,Etc_R_Ex,우측 비상구")
				//{
				//	Debug.Log($"+++++ {obj.name}");
				//}
			}
		}

		private void SetOff(Transform _target, List<GameObject> _objects)
		{
			foreach(GameObject obj in _objects)
			{
				obj.SetActive(obj.transform == _target);
			}
		}

		#endregion

		#region 2D 치수선 On/Off



		#endregion
	}
}
