using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management.Events.Inputs
{
	using Definition;
	using UnityEngine.Events;
	using UnityEngine.EventSystems;
	using UnityEngine.UI;
	using View;

	public class Event_ClickDown : EventData_Input
	{
		private Vector3 m_clickPosition;
		private UnityEvent<GameObject> m_clickEvent;

		private GameObject m_cacheObject;

		/// <summary>
		/// 클릭 다운 이벤트 생성자
		/// </summary>
		/// <param name="_eType">이벤트 분류</param>
		/// <param name="_btn">버튼 인덱스</param>
		/// <param name="_mousePos">마우스 위치</param>
		/// <param name="_camera">카메라</param>
		/// <param name="_grRaycaster">그래픽 레이캐스터</param>
		/// <param name="_event"></param>
		public Event_ClickDown(InputEventType _eType,
			int _btn, Vector3 _mousePos,
			Camera _camera, GraphicRaycaster _grRaycaster,
			UnityEvent<GameObject> _event)
		{
			StatusCode = Status.Ready;

			EventType = _eType;
			m_camera = _camera;
			m_grRaycaster = _grRaycaster;
			m_clickPosition = _mousePos;
			m_clickEvent = _event;
		}

		/// <summary>
		/// ClickDown Event 처리
		/// </summary>
		/// <param name="_cObj"></param>
		/// <param name="_mList"></param>
		public override void OnProcess(List<ModuleCode> _mList)
		{
			Status _success = Status.Update;
			Status _fail = Status.Update;

			Elements = new List<View.IInteractable>();
			m_selected3D = null;
			m_hit = default(RaycastHit);
			m_results = new List<RaycastResult>();

			Get_Collect3DObject(m_clickPosition, out m_selected3D, out m_hit, out m_results);

			if(Selected3D != null)
			{
				IInteractable interactable;
				if(Selected3D.TryGetComponent<IInteractable>(out interactable))
				{
					Elements.Add(interactable);
					StatusCode = _success;

					CachingObject(Selected3D);
					return;
				}
			}

			Elements = null;
			StatusCode = _fail;
		}
		
		/// <summary>
		/// 이벤트 후처리
		/// </summary>
		/// <param name="_sEvents">현재 이벤트 상태</param>
		public override void DoEvent(Dictionary<InputEventType, AEventData> _sEvents)
		{
			if (_sEvents.ContainsKey(InputEventType.Input_clickDown))
			{
				_sEvents[InputEventType.Input_clickDown] = this;
			}
			else
			{
				_sEvents.Add(this.EventType, this);
			}
		}

		/// <summary>
		/// 선택 객체를 임시 캐싱 객체에 할당한다.
		/// </summary>
		/// <param name="_obj">선택 객체</param>
		private void CachingObject(GameObject _obj)
		{
			m_cacheObject = _obj;
		}

		#region Click - 객체 선택 TODO 상위 개체로 메서드 집단 이동 준비

		/// <summary>
		/// 3D 객체를 마우스 위치에서 가져옴
		/// </summary>
		/// <param name="_mousePos"></param>
		/// <param name="_hitPoint"></param>
		/// <returns></returns>
		private GameObject Get_GameObject3D(Vector3 _mousePos, out RaycastHit _hitPoint)
		{
			GameObject obj = null;

			// 3D 선택
			RaycastHit _hit;
			Ray _ray = m_camera.ScreenPointToRay(_mousePos);
			if (Physics.Raycast(_ray, out _hit))
			{
				obj = _hit.collider.gameObject;
				_hitPoint = _hit;
				return obj;
			}
			else
			{
				_hitPoint = default(RaycastHit);
				return obj;
			}
		}

		/// <summary>
		/// UI 객체를 마우스 위치에서 가져옴
		/// </summary>
		/// <param name="_mousePos"></param>
		/// <returns></returns>
		private List<RaycastResult> Get_GameObjectUI(Vector3 _mousePos)
		{
			List<RaycastResult> results = new List<RaycastResult>();

			// UI 선택
			PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
			pointerEventData.position = _mousePos;

			m_grRaycaster.Raycast(pointerEventData, results);

			//Debug.Log($"***** raycast count : {results.Count}");

			return results;
		}

		#endregion
	}
}
