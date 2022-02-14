
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

	/// <summary>
	/// 컨텐츠의 핵심 데이터를 관리하는 관리자 클래스
	/// </summary>
	public partial class MainManager : IManager<MainManager>
	{
		[SerializeField] CoreManagement _core;

		public Camera MainCamera
		{
			get => _core.MainCam;
		}

		/// <summary>
		/// 요구된 사항에 따라 새로 생성된 씬의 주요 데이터를 저장하는 코드
		/// </summary>
		[SerializeField] CoreContent _content;

		/// <summary>
		/// 프로그램 시작시 초기화를 위해 필요한 데이터 모음
		/// </summary>
		[SerializeField] CoreData _data;

		public ContentManager Content
		{
			get => _content.Content;
		}

		/// <summary>
		/// 0215
		/// </summary>
		public List<LayerCode> LayerCodes
		{
			get => _content.LayerCodes;
		}

		public string ModelURI
		{
			get => _core.ModelURI;
		}

		// Start is called before the first frame update
		void Start()
		{
			DontDestroyOnLoad(this);
			
			OnCreate();
		}

		
	}
}
