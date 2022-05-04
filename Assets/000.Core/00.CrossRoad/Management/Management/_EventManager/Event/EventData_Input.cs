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
			//IItem item;


			if (Selected3D.TryGetComponent<Obj_Selectable>(out sObj))
			{
				Elements = new List<IInteractable>();
				//Elements.Add(sObj);

				//Element = sObj;

				PlatformCode _pCode = MainManager.Instance.Platform;
				if(Platforms.IsBridgePlatform(_pCode))
                {
					// 교량 플랫폼의 경우.
					//Debug.Log("Hello click");
					IInteractable _interactable;
					List<GameObject> objs = ContentManager.Instance._ModelObjects;

					// ',' 스플릿 길이 1이 아닌 경우 (지간, 경간)
					string pName = Selected3D.transform.parent.name;
					if(pName.Split(",".ToCharArray()).Length == 1 &&
						pName.Split("_".ToCharArray()).Length != 1)
                    {
						// 같은 이름의 부재가 여러개 있는 객체 
						// 부모객체 가져오기
						GameObject pObj = Selected3D.transform.parent.gameObject;
						int index = pObj.transform.childCount;
                        for (int i = 0; i < index; i++)
                        {
							GameObject cObj = pObj.transform.GetChild(i).gameObject;
							if (cObj.TryGetComponent<IInteractable>(out _interactable))
                            {
								// 자식 객체에 상호작용 가능하면
								Elements.Add(_interactable);
                            }
                        }
                    }
					else
                    {
						// , 단위로 이름 나누기
						string sName = Selected3D.transform.name.Split(",".ToCharArray())[0];

						objs.ForEach(x =>
						{
							// sName을 포함한 개체가 있는 경우
							if(x.name.Contains(sName))
                            {
								// 상호작용 가능 객체인 경우
								if(x.TryGetComponent<IInteractable>(out _interactable))
                                {
									Elements.Add(_interactable);
                                }
                            }
						});

						// 그냥 바로 위에 지간, 경간 있으면 바로고
						//Elements.Add(sObj);
                    }
                }
				else
                {
					Elements.Add(sObj);
				}
				//else if(Platforms.IsSmartInspectPlatform(_pCode))
				//{
				//	//m_clickEvent.RemoveListener(ContentManager.Instance.Get_SelectedData_UpdateUI);
				//	//m_clickEvent.AddListener(ContentManager.Instance.Get_SelectedData_UpdateUI);
				//}
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
			//// 다른 어떤 아이템을 선택한 경우
			//else if(Selected3D.TryGetComponent<IItem>(out item))
            //{
			//	if(_Items.IsLocationElement(Selected3D))
            //    {
			//		// 캐시 객체가 있는지 확인한다.
			//		if(EventManager.Instance._CachePin == null)
            //        {
			//			// 선택 객체의 위치 ?
			//			//m_hit.point
			//
			//			//GameObject obj = _Items.CreateCachePin(m_hit);
			//			EventManager.Instance._CachePin = _Items.CreateCachePin(m_hit);
			//		}
			//		else
            //        {
			//			_Items.MoveCachePin(EventManager.Instance._CachePin, m_hit);
            //        }
            //    }
            //}
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
