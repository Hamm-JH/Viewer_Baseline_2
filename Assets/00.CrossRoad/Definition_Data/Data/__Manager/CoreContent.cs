using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition.Data
{
	using Management;

	/// <summary>
	/// 주관리자에서 컨텐츠 관리용
	/// </summary>
	[System.Serializable]
	public class CoreContent : IData
	{
		[SerializeField] ContentManager _content;

		public ContentManager Content { get => _content; set => _content=value; }

		/// <summary>
		/// 컨텐츠 영역에서 컨텐츠 씬의 상태를 관리하는 레이어 코드
		/// </summary>
		//public List<LayerCode> LayerCodes { get => _content.LayerCodes; }
	}
}
