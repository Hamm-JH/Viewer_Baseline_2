using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace View
{
	using Definition;
	using EPOOutline;
	using Management;
	using System;

	public class Obj_Selectable : Interactable
	{
		UIEventType _uiEventType;
		Outlinable m_outlinable;
		Bounds m_bounds;

		private void Start()
		{
			_uiEventType = UIEventType.Mode_Isolate;
			IsInteractable = true;
			m_outlinable = gameObject.AddComponent<Outlinable>();

			m_outlinable.AddAllChildRenderersToRenderingList();
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

		public Bounds Bounds 
		{ 
			get
			{
				if(m_bounds.size.magnitude < Vector3.one.magnitude * 0.001f)
				{
					MeshRenderer render;
					if(Target.TryGetComponent<MeshRenderer>(out render))
					{
						m_bounds = render.bounds;
					}
				}
				return m_bounds;
			}
		}

		public Outlinable Outlinable { get => m_outlinable; set => m_outlinable=value; }

		//==========================================================================================
		//==========================================================================================

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
					Materials.Set(render, ColorType.Default1, render.material.color.a);
					
					//Color colr = render.material.color;
					//render.material.SetColor("_Color", Colors.Set(ColorType.Default1, colr.a));
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
						Materials.Set(render, ColorType.Default1, render.material.color.a);
						
						//Color colr = render.material.color;
						//render.material.SetColor("_Color", Colors.Set(ColorType.Default1, colr.a));
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

			// Hide & Isolate Event
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
				PlatformCode pCode = MainManager.Instance.Platform;

				switch(type)
				{
					case UIEventType.Slider_Model_Transparency:
						OnDeselect_ModelTransparency(pCode, type, value);
						break;
				}
			}
		}

		private void OnDeselect_ModelTransparency(PlatformCode _platform, UIEventType _uiType, float _value)
		{
			if (!IsInteractable) return;

			// 최종 확인
			if(_uiType == UIEventType.Slider_Model_Transparency)
			{
				float boundary = 0.8f;

				bool isOpaque = _value > boundary ? true : false;

				MeshRenderer render;
				if (this.TryGetComponent<MeshRenderer>(out render))
				{
					Material mat = render.material;
					Color colr = mat.color;
					bool thisOpaque = colr.a > boundary ? true : false;

					Materials.Set(render, ColorType.Default1, _value);
					//render.material.SetColor("_Color", new Color(colr.r, colr.g, colr.b, _value));

					if (isOpaque && !thisOpaque)
					{
						Materials.ToOpaqueMode(render);
					}
					else if (!isOpaque && thisOpaque)
					{
						Materials.ToFadeMode(render);
					}
				}
			}
		}

		

		private void OnDeselect_3dModel(PlatformCode _platform, UIEventType _uiType, bool _isHide)
		{
			if (!IsInteractable) return;

			if(_uiType == UIEventType.Mode_Hide || _uiType == UIEventType.Mode_Hide_Off 
				|| _uiType == UIEventType.Mode_Isolate || _uiType == UIEventType.Mode_Isolate_Off)
			{
				bool eventHide = _uiType == UIEventType.Mode_Hide || _uiType == UIEventType.Mode_Hide_Off
					? true : false;

				if (_platform == PlatformCode.PC_Viewer_Tunnel)
				{
					List<GameObject> objs = Targets;
					foreach (GameObject obj in objs)
					{
						// Mode_Hide의 경우에는, 숨김 대상 제외하고 다른 객체들은 모두 현재 알파값 가짐
						// Mode_Hide ? Hide :: 0.1f, NotHide :: alpha
						// Mode_Isolate의 경우에는, 숨김 대상은 현재 알파값 가짐, 숨김 제외대상은 0.1f
						// Mode_Isolate ?

						// 카르노맵 정렬 결과
						// _isHide :: false인 경우에는 모두 color.a 적용
						// _isHide :: true인 경우에는 모두 a :: 0.1f 적용

						float hideValue = 0f;
						bool isOn = true;
						if(_uiType == UIEventType.Mode_Hide_Off || _uiType == UIEventType.Mode_Isolate_Off)
						{
							hideValue = 0f;
							isOn = false;
						}
						else
						{
							hideValue = 0.1f;
							isOn = true;
						}

						MeshRenderer render;
						if (obj.TryGetComponent<MeshRenderer>(out render))
						{
							Material mat = render.material;
							Color colr = mat.color;

							if (_isHide)
							{
								Materials.ToFadeMode(render);

								Materials.Set(render, ColorType.Default1, hideValue);

								obj.SetActive(isOn);
							}
							else
							{
								// TODO 매직넘버 투명 경계값 밖으로 빼기
								if(colr.a > 0.8f)
								{
									Materials.ToOpaqueMode(render);
								}
								else
								{
									Materials.ToFadeMode(render);
								}

								Materials.Set(render, ColorType.Default1, colr.a);
							}

						}
					}
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

							if(_isHide)
							{
								Materials.ToFadeMode(render);
								Materials.Set(render, ColorType.Default1, 0.1f);
							}
							else
							{
								Materials.ToOpaqueMode(render);
								Materials.Set(render, ColorType.Default1, 1);
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

			// 핀모드엔 색변경 중단
			if (EventManager.Instance._ModuleList.Contains(ModuleCode.Work_Pinmode)) return;

			PlatformCode platform = MainManager.Instance.Platform;

			List<GameObject> objs = Targets;
			foreach (GameObject obj in objs)
			{
				MeshRenderer render;
				if (obj.TryGetComponent<MeshRenderer>(out render))
				{
					Color colr = render.material.color;

					Materials.Set(render, ColorType.Selected1, render.material.color.a);
				}
			}
		}

		
	}
}
