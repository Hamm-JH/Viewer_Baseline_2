using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management
{
	/// <summary>
	/// 템플릿
	/// </summary>
	public partial class EventManager : IManager<EventManager>
	{
		public override void OnCreate()
		{
			Debug.Log("OnCreate");
		}

		public override void OnDismiss()
		{
			Debug.Log("OnDismiss");
		}

		public override void OnUpdate()
		{
			Debug.Log("OnUpdate");
		}

		private void Awake()
		{
			cacheDownObj = null;
		}

		private void OnEnable()
		{
			cacheDownObj = null;
		}

		private void OnDisable()
		{
			cacheDownObj = null;
		}
	}
}
