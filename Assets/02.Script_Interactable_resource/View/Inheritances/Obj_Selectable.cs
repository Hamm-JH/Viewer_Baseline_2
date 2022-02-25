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
			IsInteractable = true;
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

		public override bool IsInteractable 
		{ 
			get => m_isInteractable; 
			set
			{
				m_isInteractable = value;

				MeshCollider collider;
				if(this.TryGetComponent<MeshCollider>(out collider))
				{
					collider.enabled = value;
				}
			}
		}

		public override void OnChangeValue(float _value)
		{
			throw new System.NotImplementedException();
		}

		public override void OnDeselect()
		{
			if (!IsInteractable) return;

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

		}

		public override void OnDeselect<T1, T2>(T1 t1, T2 t2)
		{
			if (!IsInteractable) return;

			UIEventType type;
			
			//if(t1 is UIEventType)
			//if (typeof(T1) == typeof(UIEventType))
			//if(typeof(T2) == typeof(bool))

			if(t1 is UIEventType && t2 is bool)
			{
				type = (UIEventType)Enum.ToObject(typeof(UIEventType), t1);
				bool isHide = (bool)(object)t2;

				OnDeselect_3dModel(MainManager.Instance.Platform, type, isHide);

				IsInteractable = !isHide;
			}

			if(t1 is UIEventType && t2 is float)
			{
				type = (UIEventType)Enum.ToObject(typeof(UIEventType), t1);
				float value = (float)(object)t2;

				OnDeselect_ModelTransparency(MainManager.Instance.Platform, type, value);
			}
		}

		private void OnDeselect_ModelTransparency(PlatformCode _platform, UIEventType _uiType, float _value)
		{
			if (!IsInteractable) return;

			if(_uiType == UIEventType.Fit_Center)
			{
				float boundary = 0.8f;

				bool isOpaque = _value > boundary ? true : false;

				MeshRenderer render;
				if (this.TryGetComponent<MeshRenderer>(out render))
				{
					Material mat = render.material;
					Color colr = mat.color;
					bool thisOpaque = colr.a > boundary ? true : false;

					render.material.SetColor("_Color", new Color(colr.r, colr.g, colr.b, _value));

					if (isOpaque && !thisOpaque)
					{
						Materials.ToOpaqueMode(render.material);
					}
					else if (!isOpaque && thisOpaque)
					{
						Materials.ToFadeMode(render.material);
					}
				}
			}
		}

		private void OnDeselect_3dModel(PlatformCode _platform, UIEventType _uiType, bool _isHide)
		{
			if (!IsInteractable) return;

			if(_uiType == UIEventType.Mode_Hide || _uiType == UIEventType.Mode_Isolate)
			{
				if (_platform == PlatformCode.PC_Viewer_Tunnel)
				{
					List<GameObject> objs = Targets;
					foreach (GameObject obj in objs)
					{

						MeshRenderer render;
						if (obj.TryGetComponent<MeshRenderer>(out render))
						{
							Material mat = render.material;
							Color colr = mat.color;

							if (_isHide)
							{
								Materials.ToFadeMode(render.material);
								render.material.SetColor("_Color", Colors.Set(ColorType.Default1, 0.1f));
							}
							else
							{
								Materials.ToOpaqueMode(render.material);
								render.material.SetColor("_Color", Colors.Set(ColorType.Default1, 1));
							}

						}
					}
					//MeshRenderer render;
					//if (gameObject.TryGetComponent<MeshRenderer>(out render))
					//{
					//	Color colr = render.material.color;

					//	render.material.SetColor("_Color", Colors.Set(ColorType.Default1, colr.a));
					//}
				}
				else if (_platform == PlatformCode.PC_Viewer_Bridge)
				{
					List<GameObject> objs = Targets;
					foreach (GameObject obj in objs)
					{

						MeshRenderer render;
						if (obj.TryGetComponent<MeshRenderer>(out render))
						{
							Material mat = render.material;
							Color colr = mat.color;
							//render.material.SetColor("_Color", Colors.Set(ColorType.Default1, colr.a));

							if(_isHide)
							{
								Materials.ToFadeMode(render.material);
								render.material.SetColor("_Color", Colors.Set(ColorType.Default1, 0.1f));
							}
							else
							{
								Materials.ToOpaqueMode(render.material);
								render.material.SetColor("_Color", Colors.Set(ColorType.Default1, 1));
							}

						}
					}
				}
			}
		}

		public override void OnSelect()
		{
			if (!IsInteractable) return;

			//Debug.Log($"OnSelect : {this.name}");

			PlatformCode platform = MainManager.Instance.Platform;

			if (platform == PlatformCode.PC_Viewer_Tunnel)
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
				//MeshRenderer render;
				//if (gameObject.TryGetComponent<MeshRenderer>(out render))
				//{
				//	Color colr = render.material.color;

				//	render.material.SetColor("_Color", Colors.Set(ColorType.Selected1, colr.a));
				//}
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
