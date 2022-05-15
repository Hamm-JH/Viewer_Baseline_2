
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management
{
	using Definition;
	using Definition.Control;
	using Definition.Data;
	using Platform.Feature.Camera;
	using UnityEngine.Events;
    using UnityEngine.Rendering.Universal;

    /// <summary>
    /// 컨텐츠의 핵심 데이터를 관리하는 관리자 클래스
    /// </summary>
    public partial class MainManager : IManager<MainManager>
	{
		
		/// <summary>
		/// 시스템 주 관리 데이터
		/// </summary>
		[Header("Data management core")]
		[SerializeField] CoreManagement _core;

		/// <summary>
		/// 컨텐츠 씬 데이터 관리
		/// </summary>
		[Header("managed content data")]
		[SerializeField] CoreContent _content;

		/// <summary>
		/// 프로그램 시작시 초기화를 위해 필요한 데이터 모음
		/// </summary>
		[Header("Dataset from start sequence")]
		[SerializeField] CoreData _data;

		[Header("Dataset test template")]
		[SerializeField] int m_templateIndex;
		[SerializeField] List<CoreData> _templateDatas;

		#region 주 관리자에서 요청할 데이터를 프로퍼티로 전달하는 구간

		[SerializeField]
		private bool test_isIssueDecal;
		public bool Test_IsIssueDecal
        {
			get => test_isIssueDecal;
        }

		public Camera MainCamera
		{
			get => _core.MainCam;
		}

		public string ModelURI
		{
			get => _data.ModelURI;
		}

		public PlatformCode Platform
		{
			get => _core._Platforms;
		}

		public GraphicCode Graphic
        {
			get => _core.GraphicMode;
        }

		public ContentManager Content
		{
			get => _content.Content;
		}

		public CoreData Data
		{
			get => _data;
		}
		
		public Material DimLineMat
		{
			get => _core.m_dimLineMat;
		}

		public Material OutlineMat
		{
			get => _core.m_outlineMat;
		}

		public ForwardRendererData RenderSetting
        {
			get => _core.m_renderSetting;
        }
		#endregion


	}
}
