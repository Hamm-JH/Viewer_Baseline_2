using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management
{
	using Management.Events;

	/// <summary>
	/// 템플릿
	/// </summary>
	public partial class EventManager : IManager<EventManager>
	{
		/// <summary>
		/// 이벤트 관리자 생성
		/// </summary>
		public override void OnCreate()
		{
			Debug.Log("OnCreate");
		}

		/// <summary>
		/// 이벤트 관리자 제거
		/// </summary>
		public override void OnDismiss()
		{
			Debug.Log("OnDismiss");
		}

		/// <summary>
		/// 이벤트 관리자 업데이트
		/// </summary>
		public override void OnUpdate()
		{
			Debug.Log("OnUpdate");
		}

		/// <summary>
		/// 이벤트를 추가한다.
		/// </summary>
		/// <typeparam name="K"></typeparam>
		/// <typeparam name="V"></typeparam>
		/// <param name="_key"> InputEventType </param>
		/// <param name="_eData"> EventData </param>
		/// <param name="_event"> [InputEventType, EventData] </param>
		/// <param name="isMultiple"> 다중선택? </param>
		public void AddEvent<K, V>(K _key, V _eData, Dictionary<K, V> _event, bool isMultiple = false) where V : AEventData
		{
			if (_event.ContainsKey(_key))
			{
				if (isMultiple)
				{
					_eData.Elements.ForEach(x => _event[_key].Elements.Add(x));
				}
				else
				{
					_event[_key] = _eData;
				}
			}
			else
			{
				_event.Add(_key, _eData);
			}
		}

		/// <summary>
		/// 지정된 이벤트 제거
		/// </summary>
		/// <typeparam name="K">이벤트 분류 키</typeparam>
		/// <typeparam name="V">이벤트 데이터 값</typeparam>
		/// <param name="_key">이벤트 분류</param>
		/// <param name="_eData">이벤트 데이터</param>
		/// <param name="_event">이벤트 상태관리 변수</param>
		public void DeleteEvent<K, V>(K _key, V _eData, Dictionary<K, V> _event) where V : AEventData
        {
			_event.Remove(_key);
        }
	}
}
