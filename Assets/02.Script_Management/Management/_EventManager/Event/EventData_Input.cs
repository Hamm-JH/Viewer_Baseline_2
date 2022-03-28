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
	using Items;

	[System.Serializable]
	public abstract class EventData_Input : AEventData
	{
		public Camera m_camera;
		public GraphicRaycaster m_grRaycaster;

		// 마우스 버튼 번호
		protected int m_btn;
		public int BtnIndex { get => m_btn; set => m_btn=value; }


		public override void OnProcess(List<ModuleCode> _mList) { }

		public override void DoEvent(Dictionary<InputEventType, AEventData> _sEvents) { }

		protected void Func_Input_clickSuccessUp(Status _success)
		{
			Obj_Selectable sObj;
			Issue_Selectable iObj;

			if (Selected3D.TryGetComponent<Obj_Selectable>(out sObj))
			{
				Elements = new List<IInteractable>();
				Elements.Add(sObj);

				//Element = sObj;

				PlatformCode _pCode = MainManager.Instance.Platform;
				if(Platforms.IsSmartInspectPlatform(_pCode))
				{
					//m_clickEvent.RemoveListener(ContentManager.Instance.Get_SelectedData_UpdateUI);
					//m_clickEvent.AddListener(ContentManager.Instance.Get_SelectedData_UpdateUI);
				}
				StatusCode = _success;
				return;
			}
			else if (Selected3D.TryGetComponent<Issue_Selectable>(out iObj))
			{
				Elements = new List<IInteractable>();
				Elements.Add(iObj);

				//Element = iObj;

				//m_clickEvent.RemoveListener(ContentManager.Instance.Get_SelectedData_UpdateUI);
				StatusCode = _success;
				return;
			}
		}

		#region Click - 객체 선택

		/// <summary>
		/// UI를 건드렸을 경우를 제외한, 3D 객체 선택상태인지 확인한다.
		/// </summary>
		/// <param name="_mousePos"></param>
		/// <param name="obj"></param>
		/// <param name="_hit"></param>
		protected void Get_Collect3DObject(Vector3 _mousePos, out GameObject obj, out RaycastHit _hit, out List<RaycastResult> _results)
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

		#region Click - 다중 선택 조건 확인

		/// <summary>
		/// Click + 다중 선택 조건 확인
		/// </summary>
		/// <param name="_sEvents"></param>
		/// <returns></returns>
		protected bool isMultiCondition(Dictionary<InputEventType, AEventData> _sEvents)
		{
			bool result = false;
			List<KeyData> kd = null;

			// 키 입력이 있는 상태인가?
			if (_sEvents.ContainsKey(InputEventType.Input_key))
			{
				Inputs.Event_Key _ev = (Inputs.Event_Key)_sEvents[InputEventType.Input_key];
				kd = _ev.m_keys;
			}

			// 입력 키 존재하는 경우
			if (kd != null)
			{
				KeyCode targetCode = MainManager.Instance.Data.KeyboardData.keyCtrl;

				// 키 정보중에 LeftCtrl이 존재하는가?
				if (kd.Find(x => x.m_keyCode == targetCode) != null)
				{
					result = true;
				}
			}

			return result;
		}

		#endregion
	}
}
