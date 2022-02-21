using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.Interaction
{
	using Definition;
	using UnityEngine.UI;

	/// <summary>
	/// 상호작용 처리 모듈
	/// 3D, UI
	/// </summary>
	public partial class Module_Interaction : AModule
	{
		[SerializeField] private Canvas rootCanvas;

		[SerializeField] private GraphicRaycaster grRaycaster;

		public Canvas RootCanvas { get => rootCanvas; set => rootCanvas=value; }
		public GraphicRaycaster GrRaycaster { get => grRaycaster; set => grRaycaster=value; }

		private void Start()
		{
			OnCreate(ModuleID.Interaction, FunctionCode.Interaction_UI);
			InitCanvas();
		}

		public override void OnCreate(ModuleID _id, FunctionCode _code)
		{
			base.OnCreate(_id, _code);
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
			Debug.LogError($"{this.GetType().ToString()} Run");

			// TODO 0222 컨텐츠 관리자에서 주관리자로 플랫폼 코드 요청, 플랫폼 코드 기반 불러오는 방식으로 변경
			GameObject ui = Instantiate<GameObject>(Resources.Load<GameObject>("UI/TestView0221"), RootCanvas.transform);
		}
	}
}
