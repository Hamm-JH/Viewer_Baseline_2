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
	}
}
