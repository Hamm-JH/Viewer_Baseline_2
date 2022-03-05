using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management.Events
{
	using Definition;
	using View;
	using UnityEngine.UI;
	using UnityEngine.EventSystems;
	using UnityEngine.Events;
	using System.Linq;

	[System.Serializable]
	public class EventData_Input : EventData
	{
		public Camera m_camera;
		public GraphicRaycaster m_graphicRaycaster;

		public GameObject m_cache;

		[Header("OnClick")]
		public Vector3 m_clickPosition;
		UnityEvent<GameObject> m_clickEvent;


		[Header("OnDrag")]
		//public int m_btn;
		public Vector2 m_delta;
		UnityEvent<int, Vector2> m_dragEvent;

		[Header("OnFocus")]
		public Vector3 m_focus;
		public float m_focusDelta;
		UnityEvent<Vector3, float> m_focusEvent;

		[Header("OnKey")]
		public List<KeyData> m_keys;
		UnityEvent<List<KeyData>> m_keyEvent;

		

		/// <summary>
		/// Ŭ�� �̺�Ʈ ������
		/// </summary>
		/// <param name="_eventType"></param>
		public EventData_Input(InputEventType _eventType, 
			int _btn, Vector3 _mousePos,
			Camera _camera, GraphicRaycaster _graphicRaycaster,
			UnityEvent<GameObject> _event)
		{
			StatusCode = Status.Ready;

			EventType = _eventType;
			m_camera = _camera;
			m_graphicRaycaster = _graphicRaycaster;
			m_clickPosition = _mousePos;
			m_clickEvent = _event;
		}

		/// <summary>
		/// �巡�� �̺�Ʈ ������
		/// </summary>
		/// <param name="_eventType"></param>
		/// <param name="_btn"></param>
		/// <param name="_delta"></param>
		/// <param name="_camera"></param>
		/// <param name="_grRaycaster"></param>
		/// <param name="_event"></param>
		public EventData_Input(InputEventType _eventType,
			int _btn, Vector2 _delta,
			Camera _camera, GraphicRaycaster _grRaycaster,
			UnityEvent<int, Vector2> _event)
		{
			StatusCode = Status.Ready;

			EventType = _eventType;
			m_camera = _camera;
			m_graphicRaycaster = _grRaycaster;
			m_btn = _btn;
			m_delta = _delta;
			m_dragEvent = _event;
		}

		/// <summary>
		/// ��Ŀ�� �̺�Ʈ ������
		/// </summary>
		/// <param name="_eventType"></param>
		/// <param name="_focus"></param>
		/// <param name="_delta"></param>
		/// <param name="_camera"></param>
		/// <param name="_grRaycaster"></param>
		/// <param name="_event"></param>
		public EventData_Input(InputEventType _eventType,
			Vector3 _focus, float _delta,
			Camera _camera, GraphicRaycaster _grRaycaster,
			UnityEvent<Vector3, float> _event)
		{
			StatusCode = Status.Ready;

			EventType = _eventType;
			m_camera = _camera;
			m_graphicRaycaster = _grRaycaster;
			m_focus = _focus;
			m_focusDelta = _delta;
			m_focusEvent = _event;
		}

		/// <summary>
		/// Ű �̺�Ʈ ������
		/// </summary>
		/// <param name="_eventType"></param>
		/// <param name="_kCode"></param>
		/// <param name="_camera"></param>
		/// <param name="_grRaycaster"></param>
		/// <param name="_event"></param>
		public EventData_Input(InputEventType _eventType,
			List<KeyData> _kData,
			Camera _camera, GraphicRaycaster _grRaycaster,
			UnityEvent<List<KeyData>> _event)
		{
			StatusCode = Status.Ready;

			EventType = _eventType;
			m_camera = _camera;
			m_graphicRaycaster = _grRaycaster;
			m_keys = _kData;
			m_keyEvent = _event;
		}

		public override void OnProcess(GameObject _cObj)
		{
			switch(EventType)
			{
				case InputEventType.Input_clickDown:
					// ��ü ���� �ܰ迡�� code :: pass, skip
					{
						Status _success = Status.Update;
						Status _fail = Status.Update;

						//Debug.Log(EventType.ToString());
						m_selected3D = null;
						m_hit = default(RaycastHit);
						m_results = new List<RaycastResult>();

						Get_Collect3DObject(m_clickPosition, out m_selected3D, out m_hit, out m_results);

						if (Selected3D != null)
						{
							IInteractable interactable;
							if (Selected3D.TryGetComponent<IInteractable>(out interactable))
							{
								Element = interactable;
								StatusCode = _success;
								return;
							}
						}
						Element = null;
						StatusCode = _fail;
					}
					// ��Į ��ġ �ܰ迡�� code :: skip

					return;

				case InputEventType.Input_clickSuccessUp:
					// ��ü ���� �ܰ迡�� code :: pass, drop
					{
						Status _success = Status.Pass;
						Status _fail = Status.Drop;

						//Debug.Log(EventType.ToString());
						m_selected3D = null;
						m_hit = default(RaycastHit);
						m_results = new List<RaycastResult>();

						Get_Collect3DObject(m_clickPosition, out m_selected3D, out m_hit, out m_results);

						if(Selected3D != null)
						{
							Obj_Selectable sObj;
							Issue_Selectable iObj;

							if (Selected3D.TryGetComponent<Obj_Selectable>(out sObj))
							{
								Elements = new List<IInteractable>();
								Elements.Add(sObj);

								Element = sObj;

								m_clickEvent.RemoveListener(ContentManager.Instance.Get_SelectedData_UpdateUI);
								m_clickEvent.AddListener(ContentManager.Instance.Get_SelectedData_UpdateUI);
								StatusCode = _success;
								return;
							}
							else if (Selected3D.TryGetComponent<Issue_Selectable>(out iObj))
							{
								Elements = new List<IInteractable>();
								Elements.Add(iObj);

								Element = iObj;

								m_clickEvent.RemoveListener(ContentManager.Instance.Get_SelectedData_UpdateUI);
								StatusCode = _success;
								return;
							}

							//IInteractable interactable;
							//if(Selected3D.TryGetComponent<IInteractable>(out interactable))
							//{
							//	Elements = new List<IInteractable>();
							//	Elements.Add(interactable);

							//	Element = interactable;

							//	m_clickEvent.RemoveListener(ContentManager.Instance.Get_SelectedData_UpdateUI);
							//	m_clickEvent.AddListener(ContentManager.Instance.Get_SelectedData_UpdateUI);
							//	StatusCode = _success;
							//	return;
							//}
						}
						// �� ������ ���� ���
						else if(m_results.Count == 0)
						{
							Element = null;
							Elements = null;
							StatusCode = Status.Pass;
							//StatusCode = _fail;
						}
						// UI ��ü�� ���� ��� m_results.Count != 0
						else
						{
							Element = null;
							Elements = null;
							StatusCode = Status.Skip;
						}
					}
					// ��Į ��ġ �ܰ迡�� code :: pass, drop
					break;

				case InputEventType.Input_clickFailureUp:
					{
						// ��ü ���� �ܰ迡�� code :: Update
						// ��Į ��ġ �ܰ迡�� code :: Update
						StatusCode = Status.Update;
					}
					break;

				case InputEventType.Input_drag:
					// ��ü ���� �ܰ迡�� code :: Update
					// ��Į ��ġ �ܰ迡�� code :: Update
					StatusCode = Status.Update;
					break;

				case InputEventType.Input_focus:
					// ��ü ���� �ܰ迡�� code :: Update
					// ��Į ��ġ �ܰ迡�� code :: Update
					StatusCode = Status.Update;
					break;

				case InputEventType.Input_key:
					// ��ü ���� �ܰ迡�� code :: Update
					// ��Į ��ġ �ܰ迡�� code :: Update

					// KeyData�� ���� ���� �ľ��� ���� ������ �����. ���� ó���� EventManager���� ó��
					// KeyData ����Ʈ�� null �Ǵ� 0�� �ƴ� ���
					if(m_keys != null && m_keys.Count != 0)
					{
						StatusCode = Status.Update;
					}
					// KeyData ����Ʈ�� null�� ���
					else
					{
						StatusCode = Status.Update;
					}
					break;
			}
		}

		/// <summary>
		/// �̺�Ʈ ���� ���ν� ����
		/// </summary>
		public override void DoEvent()
		{
			switch(EventType)
			{
				case InputEventType.Input_clickDown:
				case InputEventType.Input_clickFailureUp:
					break;

				case InputEventType.Input_clickSuccessUp:
					// ��ü�� �ùٸ��� ������ ���
					if(Elements != null)
					{
						GameObject obj = Elements.Last().Target;

						Obj_Selectable sObj;
						Issue_Selectable iObj;

						if(obj.TryGetComponent<Obj_Selectable>(out sObj))
						{
							m_clickEvent.Invoke(Elements.Last().Target);
							// TODO
							Debug.Log(1);
							ContentManager.Instance.OnSelect_3D(Elements.Last().Target);
							ContentManager.Instance.Toggle_ChildTabs(1);
						}
						else if(obj.TryGetComponent<Issue_Selectable>(out iObj))
						{
							m_clickEvent.Invoke(Elements.Last().Target);
							//ContentManager.Instance.OnSelect_Issue(Elements.Last().Target);
							ContentManager.Instance.Toggle_ChildTabs(1);
						}
					}
					// �� ������ ������ ���
					else
					{
						Debug.Log(2);
						m_clickEvent.Invoke(null);
						ContentManager.Instance.OnSelect_3D(null);
						ContentManager.Instance.Toggle_ChildTabs(1);
					}
					break;

				case InputEventType.Input_drag:
					m_dragEvent.Invoke(m_btn, m_delta);
					break;

				case InputEventType.Input_focus:
					m_focusEvent.Invoke(m_focus, m_focusDelta);
					break;

				case InputEventType.Input_key:
					m_keyEvent.Invoke(m_keys);
					break;
			}
		}

		public override void DoEvent(List<GameObject> _objs)
		{
			
		}

		public override void DoEvent(Dictionary<InputEventType, EventData> _sEvents)
		{
			
		}

		//#endregion

		#region Click - ��ü ����

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

			m_graphicRaycaster.Raycast(pointerEventData, results);

			//Debug.Log($"***** raycast count : {results.Count}");

			return results;
		}

		





		#endregion
	}
}
