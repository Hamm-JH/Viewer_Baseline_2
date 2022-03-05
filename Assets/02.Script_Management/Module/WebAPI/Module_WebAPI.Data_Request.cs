using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.WebAPI
{
	using Definition;
	using UnityEngine.Events;
	using UnityEngine.Networking;

	public partial class Module_WebAPI : AModule
	{
		public void RequestData(string _uri, WebType _webT, 
			UnityAction<string, WebType> action )
		{
			StartCoroutine(Request(_uri, _webT, action));
		}

		private IEnumerator Request(string _uri, WebType _webT,
			UnityAction<string, WebType> action )
		{
			using (UnityWebRequest wr = UnityWebRequest.Get(_uri))
			{
				yield return wr.SendWebRequest();

				if(wr.result == UnityWebRequest.Result.Success)
				{
					action.Invoke(wr.downloadHandler.text, _webT);
				}
				else
				{
					Debug.LogError($"Web Request Failed :: \n" +
						$"Type : {_webT.ToString()}\n" +
						$"URI : {_uri}\n" +
						$"fail code : {wr.result.ToString()}");
				}
			}

			yield break;
		}

	}
}
