
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


		#region 주 관리자에서 요청할 데이터를 프로퍼티로 전달하는 구간

		public Camera MainCamera
		{
			get => _core.MainCam;
		}

		public string ModelURI
		{
			get => _core.ModelURI;
		}

		public ContentManager Content
		{
			get => _content.Content;
		}

		#endregion


	}
}
