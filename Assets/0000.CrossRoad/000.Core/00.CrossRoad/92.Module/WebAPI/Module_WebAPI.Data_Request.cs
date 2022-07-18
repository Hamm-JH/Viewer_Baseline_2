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
	using UnityEngine.UI;

	public partial class Module_WebAPI : AModule
	{
		#region Request 1
		
		/// <summary>
		/// 데이터 요청
		/// </summary>
		/// <param name="_uri"></param>
		/// <param name="_webT"></param>
		/// <param name="action"></param>
		private void RequestData(string _uri, WebType _webT, UnityAction<string, WebType> action )
		{
			StartCoroutine(Request(_uri, _webT, action));
		}

		/// <summary>
		/// 이미지 히스토리 데이터 가져오는 메서드
		/// </summary>
		/// <param name="_uri"></param>
		/// <param name="_webT"></param>
		/// <param name="_callback"></param>
		/// <param name="_action"></param>
		private void RequestData(string _uri, WebType _webT, UnityAction<string> _callback, UnityAction<string, WebType, UnityAction<string>> _action)
		{
			StartCoroutine(Request(_uri, _webT, _callback, _action));
		}

		/// <summary>
		/// 데이터 요청
		/// </summary>
		/// <param name="_uri"></param>
		/// <param name="_webT"></param>
		/// <param name="_callback"></param>
		/// <param name="_action"></param>
		private void RequestData(string _uri, WebType _webT, UnityAction<AAPI> _callback, UnityAction<string, WebType, UnityAction<AAPI>> _action)
		{
			StartCoroutine(Request(_uri, _webT, _callback, _action));
		}

		/// <summary>
		/// 이미지 요청
		/// </summary>
		/// <param name="_uri"></param>
		/// <param name="_webT"></param>
		/// <param name="_callback"></param>
		/// <param name="_action"></param>
		private void RequestImage(string _uri, WebType _webT, UnityAction<Texture2D> _callback, UnityAction<Texture2D, WebType, UnityAction<Texture2D>> _action)
		{
			StartCoroutine(Request(_uri, _webT, _callback, _action));
		}

		/// <summary>
		/// 이미지 요청
		/// </summary>
		/// <param name="_uri"></param>
		/// <param name="_webT"></param>
		/// <param name="_rImage"></param>
		/// <param name="_callback"></param>
		/// <param name="_action"></param>
		private void RequestImage(string _uri, WebType _webT, RawImage _rImage, UnityAction<RawImage, Texture2D> _callback, UnityAction<RawImage, Texture2D, WebType, UnityAction<RawImage, Texture2D>> _action)
		{
			StartCoroutine(Request(_uri, _webT, _rImage, _callback, _action));
		}

		/// <summary>
		/// 데이터 요청
		/// </summary>
		/// <param name="_uri"></param>
		/// <param name="_webT"></param>
		/// <param name="_callback"></param>
		/// <param name="_action"></param>
		private void RequestData(string _uri, WebType _webT, UnityAction<DataTable> _callback, UnityAction<string, WebType, UnityAction<DataTable>> _action)
		{
			StartCoroutine(Request(_uri, _webT, _callback, _action));
		}
		#endregion

		#region execute request

		/// <summary>
		/// 필요 데이터 요청
		/// </summary>
		/// <param name="_uri"></param>
		/// <param name="_webT"></param>
		/// <param name="_action"></param>
		/// <returns></returns>
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

		/// <summary>
		/// 필요 데이터 요청
		/// </summary>
		/// <param name="_uri"></param>
		/// <param name="_webT"></param>
		/// <param name="_callback"></param>
		/// <param name="_action"></param>
		/// <returns></returns>
		private IEnumerator Request(string _uri, WebType _webT, UnityAction<string> _callback, UnityAction<string, WebType, UnityAction<string>> _action)
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

		/// <summary>
		/// 필요 데이터 요청
		/// </summary>
		/// <param name="_uri"></param>
		/// <param name="_webT"></param>
		/// <param name="_callback"></param>
		/// <param name="_action"></param>
		/// <returns></returns>
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

		/// <summary>
		/// 필요 데이터 요청
		/// </summary>
		/// <param name="_uri"></param>
		/// <param name="_webT"></param>
		/// <param name="_callback"></param>
		/// <param name="_action"></param>
		/// <returns></returns>
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

		/// <summary>
		/// 필요 데이터 요청
		/// </summary>
		/// <param name="_uri"></param>
		/// <param name="_webT"></param>
		/// <param name="_rImage"></param>
		/// <param name="_callback"></param>
		/// <param name="_action"></param>
		/// <returns></returns>
		private IEnumerator Request(string _uri, WebType _webT, RawImage _rImage, UnityAction<RawImage, Texture2D> _callback, UnityAction<RawImage, Texture2D, WebType, UnityAction<RawImage, Texture2D>> _action)
		{
			using (UnityWebRequest wr = UnityWebRequestTexture.GetTexture(_uri))
			{
				yield return wr.SendWebRequest();

				if(wr.result == UnityWebRequest.Result.Success)
				{
					_action.Invoke(_rImage, ((DownloadHandlerTexture)wr.downloadHandler).texture, _webT, _callback);
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

		/// <summary>
		/// 필요 데이터 요청
		/// </summary>
		/// <param name="_uri"></param>
		/// <param name="_webT"></param>
		/// <param name="_callback"></param>
		/// <param name="_action"></param>
		/// <returns></returns>
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
