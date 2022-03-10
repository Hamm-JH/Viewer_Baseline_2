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

		private void OnEnable()
		{
			cacheDownObj = null;
		}

		private void OnDisable()
		{
			cacheDownObj = null;
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
		private void AddEvent<K, V>(K _key, V _eData, Dictionary<K, V> _event, bool isMultiple = false) where V : EventData
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
	}
}
