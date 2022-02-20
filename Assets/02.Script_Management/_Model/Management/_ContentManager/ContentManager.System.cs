using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management
{
	public partial class ContentManager : IManager<ContentManager>
	{
		// Start is called before the first frame update
		void Start()
		{
			OnCreate();
		}

		/// <summary>
		/// 컨텐츠 관리자가 생성될때 실행
		/// </summary>
		public override void OnCreate()
		{
			Debug.Log("Content OnCreate");

			OnInitialize();
		}

		/// <summary>
		/// 컨텐츠 관리자가 삭제될때 실행
		/// </summary>
		public override void OnDismiss()
		{
			Debug.Log("Content OnDismiss");
		}

		/// <summary>
		/// 컨텐츠 관리자가 갱신될때 실행
		/// </summary>
		public override void OnUpdate()
		{
			Debug.Log("Content OnUpdate");
		}

		private void OnDestroy()
		{
			OnDismiss();
		}
	}
}
