using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management.Events
{
	using Definition;
	using View;
	using UnityEngine.UI;
	using UnityEngine.EventSystems;

	public class EventData_Input : EventData
	{
		public Vector3 m_clickPosition;
		public Camera m_camera;
		public GraphicRaycaster m_graphicRaycaster;

		/// <summary>
		/// Ŭ�� �̺�Ʈ ������
		/// </summary>
		/// <param name="_eventType"></param>
		public EventData_Input(InputEventType _eventType, 
			int _btn, Vector3 _mousePos,
			Camera _camera, GraphicRaycaster _graphicRaycaster)
		{
			EventType = _eventType;
			m_clickPosition = _mousePos;
			m_camera = _camera;
			m_graphicRaycaster = _graphicRaycaster;
		}

		public override void OnProcess(GameObject _cObj)
		{
			GameObject selected3D = null;
			RaycastHit hit = default(RaycastHit);

			Get_Collect3DObject(m_clickPosition, out selected3D, out hit);

			if(selected3D != null)
			{
				IInteractable interactable;
				if(selected3D.TryGetComponent<IInteractable>(out interactable))
				{
					Element = interactable;
				}
			}

			// ����ó�� �Ϸ��� Element �˼��Ѵ�.
		}

		/// <summary>
		/// UI�� �ǵ���� ��츦 ������, 3D ��ü ���û������� Ȯ���Ѵ�.
		/// </summary>
		/// <param name="_mousePos"></param>
		/// <param name="obj"></param>
		/// <param name="_hit"></param>
		private void Get_Collect3DObject(Vector3 _mousePos, out GameObject obj, out RaycastHit _hit)
		{
			obj = null;

			//RaycastHit _hit = default(RaycastHit);
			GameObject _selected3D = Get_GameObject3D(_mousePos, out _hit);
			List<RaycastResult> results = Get_GameObjectUI(_mousePos);

			if (results.Count != 0)
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

			m_graphicRaycaster.Raycast(pointerEventData, results);

			return results;
		}
	}
}
