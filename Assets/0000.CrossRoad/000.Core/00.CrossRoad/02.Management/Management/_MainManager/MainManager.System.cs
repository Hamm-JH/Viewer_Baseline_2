using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management
{
	using Definition;
	using Platform.Feature;
	using Definition.Control;
	using Platform.Feature.Camera;
	using UnityEngine.Events;
	using Definition.Data;
    using DG.Tweening;

    public partial class MainManager : IManager<MainManager>
	{
		// Start is called before the first frame update
		void Start()
		{
			DontDestroyOnLoad(this);

			OnCreate();
		}

		/// <summary>
		/// 주관리자가 생성될때 실행
		/// </summary>
		public override void OnCreate()
		{
			Debug.Log("OnCreate");

            DOTween.Init();

            // 설정 데이터값을 요청한다.
            // 데이터 설정이 끝나면 시스템 인스턴스를 초기화한다.
            RequestDataset(SetSystemInstance);
		}

		/// <summary>
		/// 주관리자가 삭제될때 실행
		/// </summary>
		public override void OnDismiss()
		{
			Debug.Log("OnDestroy");
		}

		/// <summary>
		/// 주관리자가 갱신될때 실행
		/// </summary>
		public override void OnUpdate()
		{
			Debug.Log("OnUpdate");
		}

		/// <summary>
		/// 컨텐츠 관리자를 할당
		/// </summary>
		/// <param name="content">컨텐츠 관리자</param>
		/// <param name="callback">할당 완료 이벤트</param>
		public void SetContentManager(ContentManager content, UnityAction<CoreManagement, CoreData> callback)
		{
			_content.Content = content;

			callback.Invoke(_core, _data);
		}

		private void OnDestroy()
		{
			OnDismiss();
		}

		
	}
}
