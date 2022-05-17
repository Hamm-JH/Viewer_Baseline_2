using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management.Events
{
	using Definition;
	using System.Linq;
	using UnityEngine.EventSystems;
	using View;

	/// <summary>
	/// 접근 데이터와 데이터에 접근하기 위한 메서드 원형 선언
	/// </summary>
	[System.Serializable]
	public abstract class AEventData
	{
		protected List<IInteractable> m_elements;
		protected InputEventType m_eventType;
		private Status status;

		/// <summary>
		/// 상호작용 가능한 요소
		/// </summary>
		//public IInteractable Element { get => m_element; set => m_element=value; }
		public List<IInteractable> Elements { get => m_elements; set => m_elements=value; }

		public List<GameObject> objects;

		/// <summary>
		/// 발생한 이벤트의 형식
		/// </summary>
		public InputEventType EventType { get => m_eventType; set => m_eventType=value; }

		/// <summary>
		/// 이벤트 처리 결과
		/// </summary>
		public Status StatusCode { get => status; set => status=value; }

		//----------------------------------------------------------------------------------------------

		[Header("Private 3D")]
		protected GameObject m_selected3D = null;
		protected RaycastHit m_hit = default(RaycastHit);
		protected List<RaycastResult> m_results = new List<RaycastResult>();

		public GameObject Selected3D { get => m_selected3D; set => m_selected3D=value; }
		public RaycastHit Hit { get => m_hit; set => m_hit=value; }
		public List<RaycastResult> Results { get => m_results; set => m_results=value; }

		//----------------------------------------------------------------------------------------------

		[Header("Private UI")]
		protected UIEventType m_uiEventType;
		protected ToggleType m_toggleType;

		public UIEventType UiEventType { get => m_uiEventType; set => m_uiEventType=value; }
		public ToggleType ToggleType { get => m_toggleType; set => m_toggleType=value; }
		


		/// <summary>
		/// 이벤트 전처리 메서드
		/// </summary>
		public abstract void OnProcess(List<ModuleCode> _mList);

		/// <summary>
		/// 이벤트 후처리 메서드
		/// </summary>
		/// <param name="_sEvents"></param>
		public abstract void DoEvent(Dictionary<InputEventType, AEventData> _sEvents);
		
		public static bool IsEqual(AEventData A, AEventData B)
		{
			if(A.Elements.Last().Target == B.Elements.Last().Target)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// AdminViewer 키맵 선택 이벤트
		/// </summary>
		protected bool IsClickOnKeymap(List<RaycastResult> _hits)
		{
			bool result = false;

			if (_hits.Count != 0)
			{
				_hits.ForEach(x =>
				{
					if (x.gameObject.name.Contains("Keymap"))
					{
						result = true;
					}
				});
			}

			return result;
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

		#region Check Module Method

		/// <summary>
		/// 모듈코드에서 PinMode가 존재하는가
		/// </summary>
		/// <param name="_mList"></param>
		/// <returns></returns>
		protected bool IsInPinMode(List<ModuleCode> _mList)
		{
			bool result = false;

			if (_mList.Contains(ModuleCode.Work_Pinmode))
			{
				result = true;
			}

			return result;
		}

		#endregion
	}
}
