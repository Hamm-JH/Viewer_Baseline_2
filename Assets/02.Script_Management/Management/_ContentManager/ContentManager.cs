using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management
{
	using Definition;
	using Module;
	using Module.Graphic;
	using Module.Interaction;
	using Module.Item;
	using Module.Model;
	using Module.UI;
	using Module.WebAPI;
	using System;
	using System.Linq;
	using UnityEngine.UI;

	public partial class ContentManager : IManager<ContentManager>
	{
		#region Modules

		/// <summary>
		/// 모듈 리스트
		/// </summary>
		[SerializeField] List<AModule> m_modules;
		Module_Model m_model;
		Module_WebAPI m_api;
		Module_Interaction m_interaction;
		Module_Graphic m_graphic;
		Module_Items m_items;

		public List<AModule> Modules { get => m_modules; set => m_modules=value; }

		public Module_Model _Model
		{
			get
			{
				if(m_model == null)
				{
					Module_Model mod = (Module_Model)Modules.Find(x => x.ID == ModuleID.Model);
					if (mod != null) m_model = mod;
					else
					{
						throw new System.Exception("Model is null");
					}
				}
				return m_model;
			}
		}

		public Module_WebAPI _API
		{
			get
			{
				if(m_api == null)
				{
					Module_WebAPI mod = (Module_WebAPI)Modules.Find(x => x.ID == ModuleID.WebAPI);
					if (mod != null) m_api = mod;
					else
					{
						//throw new System.Exception("WebAPI is null");
					}
				}
				return m_api;
			}
		}

		public Module_Interaction _Interaction
		{
			get
			{
				if(m_interaction == null)
				{
					Module_Interaction mod = (Module_Interaction)Modules.Find(x => x.ID == ModuleID.Interaction);
					if (mod != null) m_interaction = mod;
					else
					{
						throw new System.Exception("Interaction is null");
					}
				}
				return m_interaction;
			}
		}

		public Module_Graphic _Graphic
		{
			get
			{
				if(m_graphic == null)
				{
					Module_Graphic mod = (Module_Graphic)Modules.Find(x => x.ID == ModuleID.Graphic);
					if (mod != null) m_graphic = mod;
					else
					{
						throw new System.Exception("Graphic is null");
					}
				}
				return m_graphic;
			}
		}

		public Module_Items _Items 
		{ 
			get
			{
				if(m_items == null)
				{
					Module_Items mod = (Module_Items)Modules.Find(x => x.ID == ModuleID.Item);
					if (mod != null) m_items = mod;
					else
					{
						throw new Exception("Items module is null");
					}
				}
				return m_items;
			}
			
		}

		#endregion

		List<GameObject> m_modelObjects;
		List<GameObject> m_issueObjects;

		/// <summary>
		/// 모델 요소의 중심축 값 반환
		/// </summary>
		public Bounds _CenterBounds
		{
			get
			{
				return _Model.CenterBounds;
			}
		}

		public List<GameObject> _ModelObjects
		{
			get
			{
				if(m_modelObjects == null)
				{
					m_modelObjects = _Model.ModelObjects;
				}
				if(m_modelObjects.Count == 0)
				{
					m_modelObjects = _Model.ModelObjects;
				}
				return m_modelObjects;
			}
		}

		public List<GameObject> _IssueObjects 
		{ 
			get
			{
				if(m_issueObjects == null)
				{
					m_issueObjects = _Model.IssueObjs;
				}
				else if(m_issueObjects.Count == 0)
				{
					m_issueObjects = _Model.IssueObjs;
				}
				return m_issueObjects;
			}
		}

		public GameObject _SelectedObj
		{
			get
			{
				var _events = EventManager.Instance.EventStates;
				// 핀 등록 모드가 아닐땐 SuccessUp 코드에서 대상을 찾는다.
				if(!EventManager.Instance._ModuleList.Contains(ModuleCode.Work_Pinmode))
				{
					if(_events.ContainsKey(InputEventType.Input_clickSuccessUp))
					{
						return _events[InputEventType.Input_clickSuccessUp].Elements.Last().Target;
					}
					else
					{
						return null;
					}
				}
				else
				{
					return EventManager.Instance._Cache;
				}
			}
		}

		public Vector3 _SelectedAngle
		{
			get
			{
				GameObject obj = _SelectedObj;
				// Tunnel
				return obj.transform.parent.parent.rotation.eulerAngles;
			}
		}

		/// <summary>
		/// UI 메인 캔버스 반환
		/// </summary>
		public Canvas _Canvas
		{
			get
			{
				return _Interaction.RootCanvas;
			}
		}

		/// <summary>
		/// UI 메인 캔버스 Graphic Raycaster 반환
		/// </summary>
		public GraphicRaycaster _GrRaycaster
		{
			get
			{
				return _Interaction.GrRaycaster;
			}
		}

		/// <summary>
		/// UI 메인 템플릿 인스턴스 반환
		/// </summary>
		public AUI _UIInstance
		{
			get
			{
				return _Interaction.UiInstance;
			}
		}

		
	}
}
