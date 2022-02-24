using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace View
{
	using Definition;
	using Management;
	using System;

	public class Obj_Selectable : Interactable
	{
		UIEventType _uiEventType;

		private void Start()
		{
			_uiEventType = UIEventType.Mode_Isolate;
		}

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
					Color colr = render.material.color;

					render.material.SetColor("_Color", Colors.Set(ColorType.Default1, colr.a));
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
						Color colr = render.material.color;

						render.material.SetColor("_Color", Colors.Set(ColorType.Default1, colr.a));
					}
				}
			}
		}

		public override void OnDeselect<T>(T t)
		{
			if(typeof(T) == typeof(UIEventType))
			{
				UIEventType _type = (UIEventType)Enum.ToObject(typeof(UIEventType), t);
				
				_uiEventType = _type;

				OnDeselect();
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
					Color colr = render.material.color;

					render.material.SetColor("_Color", Colors.Set(ColorType.Selected1, colr.a));
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
						Color colr = render.material.color;

						render.material.SetColor("_Color", Colors.Set(ColorType.Selected1, colr.a));
					}
				}
			}
		}
	}
}
