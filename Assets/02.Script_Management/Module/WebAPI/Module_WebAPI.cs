using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.WebAPI
{
	using Definition;
	using Definition.Data;
	using Management;

	/// <summary>
	/// 웹 API 모듈
	/// Front, Back
	/// </summary>
	public partial class Module_WebAPI : AModule
	{
		private void Start()
		{
			OnCreate(ModuleID.WebAPI, FunctionCode.API);
		}

		public override void OnStart()
		{
			//Debug.LogError($"{this.GetType().ToString()} Run");

			InitializeModelIssue();
		}

		public void InitializeModelIssue()
		{
			CoreData data = MainManager.Instance.Data;

			string dmgURI = data.IssueDmgURI;
			string rcvURI = data.IssueRcvURI;

			RequestData(dmgURI, WebType.Issue_Dmg, GetData);
			RequestData(rcvURI, WebType.Issue_Rcv, GetData);
		}
	}
}
