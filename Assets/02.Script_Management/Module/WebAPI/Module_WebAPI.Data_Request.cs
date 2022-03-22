using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.WebAPI
{
	using Data.API;
	using Definition;
	using System.Data;
	using UnityEngine.Events;
	using UnityEngine.Networking;

	public partial class Module_WebAPI : AModule
	{
		#region Request 1
		private void RequestData(string _uri, WebType _webT, UnityAction<string, WebType> action )
		{
			StartCoroutine(Request(_uri, _webT, action));
		}

		private void RequestData(string _uri, WebType _webT, UnityAction<AAPI> _callback, UnityAction<string, WebType, UnityAction<AAPI>> _action)
		{
			StartCoroutine(Request(_uri, _webT, _callback, _action));
		}

		private void RequestImage(string _uri, WebType _webT, UnityAction<Texture2D> _callback, UnityAction<Texture2D, WebType, UnityAction<Texture2D>> _action)
		{
			StartCoroutine(Request(_uri, _webT, _callback, _action));
		}

		private void RequestData(string _uri, WebType _webT, UnityAction<DataTable> _callback, UnityAction<string, WebType, UnityAction<DataTable>> _action)
		{
			StartCoroutine(Request(_uri, _webT, _callback, _action));
		}
		#endregion

		#region execute request

		private IEnumerator Request(string _uri, WebType _webT, UnityAction<string, WebType> _action )
		{
			using (UnityWebRequest wr = UnityWebRequest.Get(_uri))
			{
				yield return wr.SendWebRequest();

				if(wr.result == UnityWebRequest.Result.Success)
				{
					_action.Invoke(wr.downloadHandler.text, _webT);
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

		private IEnumerator Request(string _uri, WebType _webT, UnityAction<AAPI> _callback, UnityAction<string, WebType, UnityAction<AAPI>> _action)
		{
			using (UnityWebRequest wr = UnityWebRequest.Get(_uri))
			{
				yield return wr.SendWebRequest();

				if (wr.result == UnityWebRequest.Result.Success)
				{
					_action.Invoke(wr.downloadHandler.text, _webT, _callback);
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

		private IEnumerator Request(string _uri, WebType _webT, UnityAction<Texture2D> _callback, UnityAction<Texture2D, WebType, UnityAction<Texture2D>> _action)
		{
			using (UnityWebRequest wr = UnityWebRequestTexture.GetTexture(_uri))
			{
				yield return wr.SendWebRequest();

				if (wr.result == UnityWebRequest.Result.Success)
				{
					_action.Invoke( ((DownloadHandlerTexture)wr.downloadHandler).texture, _webT, _callback);
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

		private IEnumerator Request(string _uri, WebType _webT, UnityAction<DataTable> _callback, UnityAction<string, WebType, UnityAction<DataTable>> _action)
		{
			using (UnityWebRequest wr = UnityWebRequest.Get(_uri))
			{
				yield return wr.SendWebRequest();

				if (wr.result == UnityWebRequest.Result.Success)
				{
					_action.Invoke(wr.downloadHandler.text, _webT, _callback);
				}
				else
				{
					Debug.LogError($"Web Request Failed :: \n" +
						$"Type : {_webT.ToString()}\n" +
						$"URI : {_uri}\n" +
						$"fail code : {wr.result.ToString()}");
				}
			}

			//yield break;

		}

		#endregion
	}
}
