using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace View
{
	using Definition;
	using Management;

	public class Obj_Selectable : Interactable
	{
		public override GameObject Target
		{
			get => gameObject;
		}

		public override List<GameObject> Targets
		{
			get
			{
				string _name = gameObject.name;

				// todo ?? 여기서 컨텐츠 관리자 쓰는게 맞나..
				List<GameObject> lst = ContentManager.Instance._ModelObjects;

				List<GameObject> result = lst.FindAll(x => x.name == _name);

				return result;
			}
		}

		public override void OnChangeValue(float _value)
		{
			throw new System.NotImplementedException();
		}

		public override void OnDeselect()
		{
			//Debug.Log($"OnDeselect : {this.name}");
			PlatformCode platform = MainManager.Instance.Platform;

			if(platform == PlatformCode.PC_Viewer_Tunnel)
			{
				MeshRenderer render;
				if (gameObject.TryGetComponent<MeshRenderer>(out render))
				{
					render.material.SetColor("_Color", Colors.Set(ColorType.Default1));
				}
			}
			else if(platform == PlatformCode.PC_Viewer_Bridge)
			{
				List<GameObject> objs = Targets;
				foreach(GameObject obj in objs)
				{
					MeshRenderer render;
					if (obj.TryGetComponent<MeshRenderer>(out render))
					{
						render.material.SetColor("_Color", Colors.Set(ColorType.Default1));
					}
				}
			}
		}

		public override void OnSelect()
		{
			//Debug.Log($"OnSelect : {this.name}");

			PlatformCode platform = MainManager.Instance.Platform;

			if (platform == PlatformCode.PC_Viewer_Tunnel)
			{
				MeshRenderer render;
				if (gameObject.TryGetComponent<MeshRenderer>(out render))
				{
					render.material.SetColor("_Color", Colors.Set(ColorType.Selected1));
				}
			}
			else if (platform == PlatformCode.PC_Viewer_Bridge)
			{
				List<GameObject> objs = Targets;
				foreach (GameObject obj in objs)
				{
					MeshRenderer render;
					if (obj.TryGetComponent<MeshRenderer>(out render))
					{
						render.material.SetColor("_Color", Colors.Set(ColorType.Selected1));
					}
				}
			}
		}
	}
}
