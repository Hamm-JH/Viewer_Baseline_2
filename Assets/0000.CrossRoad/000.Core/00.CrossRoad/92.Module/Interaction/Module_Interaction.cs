using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.Interaction
{
	using Definition;
	using Management;
	using Module.UI;
	using UnityEngine.UI;

	/// <summary>
	/// 상호작용 처리 모듈
	/// 3D, UI
	/// </summary>
	public partial class Module_Interaction : AModule
	{
		[Header("UI Elements")]
		[SerializeField] private Canvas rootCanvas;
		[SerializeField] private GraphicRaycaster grRaycaster;

		[Header("Instances")]
		[SerializeField] private List<GameObject> m_templates;
		[SerializeField] private List<AUI> m_uiInstances;

		public Canvas RootCanvas { get => rootCanvas; set => rootCanvas=value; }
		public GraphicRaycaster GrRaycaster { get => grRaycaster; set => grRaycaster=value; }

		public List<GameObject> Templates { get => m_templates; set => m_templates=value; }
		public List<AUI> UiInstances { get => m_uiInstances; set => m_uiInstances=value; }

		private void Start()
		{
			OnCreate(ModuleID.Interaction, FunctionCode.Interaction_UI);
			InitCanvas();
		}

		private void InitCanvas()
		{
			if(RootCanvas == null)
			{
				GameObject canvasObj = new GameObject("RootCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
				Canvas canvas = canvasObj.GetComponent<Canvas>();
				GraphicRaycaster grRaycaster = canvasObj.GetComponent<GraphicRaycaster>();

				canvas.renderMode = RenderMode.ScreenSpaceOverlay;

				RootCanvas = canvas;
				GrRaycaster = grRaycaster;
			}
		}

		public override void OnStart()
		{
			Debug.LogWarning($"{this.GetType().ToString()} OnStart");

			// 컨텐츠 관리자에서 주관리자로 플랫폼 코드 요청, 플랫폼 코드 기반 불러오는 방식으로 변경
			ContentManager.Instance.RunModule_UIInstantiate(UI_Instantiate);
		}

		/// <summary>
		/// 받아온 ui 템플릿을 생성
		/// </summary>
		/// <param name="_resource"></param>
		private void UI_Instantiate(List<GameObject> _resources)
		{
			m_templates = new List<GameObject>();
			m_uiInstances = new List<AUI>();

			_resources.ForEach(x =>
			{
				GameObject ui = Instantiate<GameObject>(x, RootCanvas.transform);
				AUI aui = ui.GetComponent<AUI>();
				Templates.Add(ui);
				UiInstances.Add(aui);

				aui.OnStart();
			});

			//GameObject ui = Instantiate<GameObject>(_resource, RootCanvas.transform);
			//Template = ui;
			//UiInstance = ui.GetComponent<AUI>();

			//// 생성된 UI 인스턴스의 시작을 알린다.
			//UiInstance.OnStart();

			// Interaction 관리자에서 컨텐츠 관리자로 동작 완료를 알린다.
			ContentManager.Instance.CheckInitModuleComplete(ID);
		}

		public bool TryGetUITempalte<T>(out T _t) where T : AUI
        {
			bool result = false;
			_t = null;

			foreach(var ui in UiInstances)
            {
				if(Utilities.Objects.TryGetValue<T>(ui.gameObject, out _t))
                {
					return result;
                }
            }

			return result;
        }

		/// <summary>
		/// 사전 초기화 단계 완료후 초기화 시작
		/// </summary>
		public void LoadModuleComplete()
		{
			UiInstances.ForEach(x => x.OnModuleComplete());
		}

		public void ReInvokeStatusEvent()
		{
			UiInstances.ForEach(x => x.ReInvokeEvent());
		}
	}
}
