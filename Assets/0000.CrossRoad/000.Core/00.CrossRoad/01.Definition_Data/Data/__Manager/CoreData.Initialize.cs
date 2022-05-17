using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition.Data
{
	using UnityEngine.Events;

	// 템플릿
	public partial class CoreData : IData
	{
		/// <summary>
		/// 데이터를 초기화하고, 초기화가 완료된 데이터를 반환한다.
		/// </summary>
		public IEnumerator Initialize(UnityAction<CoreData> action)
		{
			SetURL(out m_url, out m_baseURL, 
				out m_modelURI, out m_addressURI,
				out m_issueDmgURI, out m_issueRcvURI, out m_imageURL, out m_historyURL,
				out m_keyCode);

			action.Invoke(this);

			yield break;
		}

		private void SetURL(out string _url, out string _baseURL, 
			out string _modelURI, out string _addressURI,
			out string _issueDmgURI, out string _issueRcvURI, out string _imageURL, out string _historyURL,
			out string _keyCode)
		{
			_url = "";
			_baseURL = "";
			_modelURI = "";
			_addressURI = "";
			_issueDmgURI = "";
			_issueRcvURI = "";
			_imageURL = "";
			_historyURL = "";
			_keyCode = "";

			string addressURL = "";
			string dmgURL = "";
			string rcvURL = "";
			string imageURL = "";
			string historyURL = "";

			if(Platforms.IsTunnelPlatform(Platform))
			{
				//_url = "http://wesmart.synology.me:45001/unity/UnityWeb/?cdTunnel=20211202-00000265&cdTunnelSub=20211202-00000266";
				//Debug.LogError("Debugging mode");
#if UNITY_EDITOR
				_url = "http://wesmart.synology.me:55001/unity/UnityWeb/?cdTunnel=20211202-00000265&cdTunnelSub=20211202-00000266";
				//_url = "http://wesmart.synology.me:45001/unity/UnityWeb/?cdTunnel=20211202-00000265&cdTunnelSub=20211202-00000266";
				//_url = "http://wesmart.synology.me:45001/unity/UnityWeb/?cdTunnel=20211202-00000283&cdTunnelSub=20211202-00000284";


#else

				_url = Application.absoluteURL;
#endif
				addressURL = "/api/tunnel/search?cdTunnel=";
				dmgURL = "/api/tunnel/damage/state?cdTunnel=";
				rcvURL = "/api/tunnel/recover/state?cdTunnel=";
				historyURL = "/api/tunnel/damageDailyHistory?cdTunnel=";
				imageURL = "/api/common/file/dn?";
			}
			else if(Platforms.IsBridgePlatform(Platform))
			{
#if UNITY_EDITOR
				//_url = "http://wesmart.synology.me:45000/unity/UnityWeb/?cdBridge=20211102-00000051&cdBridgeSub=20211102-00000052";
				//_url = "http://wesmart.synology.me:55000/unity/UnityWeb/?cdBridge=20211102-00000051&cdBridgeSub=20211102-00000052";
				_url = "http://wesmart.synology.me:55000/unity/UnityWeb/?cdBridge=20211102-00000026&cdBridgeSub=20211102-00000027";
#else
						_url = Application.absoluteURL;
#endif
				addressURL = "/api/bridge/search?cdBridge=";	
				dmgURL = "/api/bridge/damage/state?cdBridge=";
				rcvURL = "/api/bridge/recover/state?cdBridge=";
				historyURL = "/api/bridge/damageDailyHistory?cdBridge=";
				imageURL = "/api/common/file/dn?";
			}
			else
            {
				throw new Definition.Exceptions.PlatformNotDefinedException(Platform);
            }

			//-----

			// 받아온 url을 split
			string[] urlArgs = _url.Split('/');
			_baseURL = string.Format("{0}//{1}", urlArgs[0], urlArgs[2]);
			_keyCode = _url.Split('?')[1].Split('&')[0].Split('=')[1];

			_modelURI = string.Format("{0}/3dmodel/{1}.gltf", _baseURL, _keyCode);
			_addressURI = string.Format("{0}{1}{2}", _baseURL, addressURL, _keyCode);
			_issueDmgURI = string.Format("{0}{1}{2}", _baseURL, dmgURL, _keyCode);
			_issueRcvURI = string.Format("{0}{1}{2}", _baseURL, rcvURL, _keyCode);
			_historyURL = string.Format("{0}{1}{2}", _baseURL, historyURL, _keyCode);
			_imageURL = string.Format("{0}{1}", _baseURL, imageURL);
		}
	}
}
