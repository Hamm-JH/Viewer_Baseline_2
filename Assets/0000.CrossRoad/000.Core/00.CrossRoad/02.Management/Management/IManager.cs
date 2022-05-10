using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management
{
	public abstract class IManager<T> : Utilities.MonoInstance<T> where T : class
	{

		/// <summary>
		/// Manager가 생성될때
		/// </summary>
		public abstract void OnCreate();

		/// <summary>
		/// Manager가 업데이트될때
		/// </summary>
		public abstract void OnUpdate();

		/// <summary>
		/// Manager가 삭제될때
		/// </summary>
		public abstract void OnDismiss();
	}
}
