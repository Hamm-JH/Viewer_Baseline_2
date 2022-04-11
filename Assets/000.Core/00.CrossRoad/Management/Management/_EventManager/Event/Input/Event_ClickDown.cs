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
		//private Camera m_camera;
		//private GraphicRaycaster m_grRaycaster;
		private Vector3 m_clickPosition;
		private UnityEvent<GameObject> m_clickEvent;

		private GameObject m_cacheObject;

		//public Camera Camera { get => m_camera; set => m_camera=value; }
		//public GraphicRaycaster GrRaycaster { get => m_grRaycaster; set => m_grRaycaster=value; }
		//public Vector3 ClickPosition { get => m_clickPosition; set => m_clickPosition=value; }
		//public UnityAction<GameObject> ClickEvent { get => m_clickEvent; set => m_clickEvent=value; }

		public GameObject CacheObject { get => m_cacheObject; set => m_cacheObject=value; }

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
		/// ClickDown Event ó��
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

		private void CachingObject(GameObject _obj)
		{
			m_cacheObject = _obj;
		}

		#region Click - ��ü ���� TODO ���� ��ü�� �޼��� ���� �̵� �غ�

		/// <summary>
		/// UI�� �ǵ���� ��츦 ������, 3D ��ü ���û������� Ȯ���Ѵ�.
		/// </summary>
		/// <param name="_mousePos"></param>
		/// <param name="obj"></param>
		/// <param name="_hit"></param>
		private void Get_Collect3DObject(Vector3 _mousePos, out GameObject obj, out RaycastHit _hit, out List<RaycastResult> _results)
		{
			obj = null;

			//RaycastHit _hit = default(RaycastHit);
			GameObject _selected3D = Get_GameObject3D(_mousePos, out _hit);
			_results = Get_GameObjectUI(_mousePos);

			if (_results.Count != 0)
			{

			}
			else
			{
				if (_selected3D != null)
				{
					obj = _selected3D;
				}
			}
		}

		/// <summary>
		/// 3D ��ü�� ���콺 ��ġ���� ������
		/// </summary>
		/// <param name="_mousePos"></param>
		/// <param name="_hitPoint"></param>
		/// <returns></returns>
		private GameObject Get_GameObject3D(Vector3 _mousePos, out RaycastHit _hitPoint)
		{
			GameObject obj = null;

			// 3D ����
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
		/// UI ��ü�� ���콺 ��ġ���� ������
		/// </summary>
		/// <param name="_mousePos"></param>
		/// <returns></returns>
		private List<RaycastResult> Get_GameObjectUI(Vector3 _mousePos)
		{
			List<RaycastResult> results = new List<RaycastResult>();

			// UI ����
			PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
			pointerEventData.position = _mousePos;

			m_grRaycaster.Raycast(pointerEventData, results);

			//Debug.Log($"***** raycast count : {results.Count}");

			return results;
		}

		#endregion
	}
}
