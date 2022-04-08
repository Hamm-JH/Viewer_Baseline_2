using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.UI
{
	using AdminViewer;
	using Data.API;
	using Definition;
	using Management;
    using System.Linq;
    using UnityEngine.Events;
	using UnityEngine.UI;
	using View;

	public partial class UITemplate_AdminViewer : AUI
	{
		private ModuleStatus m_status;

		public void ChangeModuleStatus(ModuleStatus _mStatus)
		{
			PlatformCode pCode = MainManager.Instance.Platform;
			if(Platforms.IsDemoAdminViewer(pCode))
			{
				// 모듈 코드 업데이트
				ModuleCode mCode = ModuleCode.Issue_Administration;
				ContentManager.Instance.SetModuleStatus(mCode, _mStatus);

				// 1 캐시 객체
				GameObject cache = null;
				List<GameObject> caches = new List<GameObject>();

				// 3 메인 카메라, 서브 카메라
				Camera mCam = MainManager.Instance.MainCamera;
				Camera sCam = ContentManager.Instance.Container.m_keymap.m_keymapCamera;

				// 4 모델 리스트
				List<GameObject> objs = ContentManager.Instance._ModelObjects;

				// 현재 모듈의 상태가 DefaultMode인가
				AModuleStatus aStatus = EventManager.Instance._Statement.GetModuleStatus(mCode);

				// 기본 관리모드인가? (1, 2, 5 상태)
				if (aStatus.IsDefaultAdministrationMode())
				{
					aStatus.RemoveCache();

					mCam.cullingMask = Layers.SetMask(0);
					sCam.cullingMask = Layers.SetMask(0);

					SetObjectToDefaultLayer(objs);			// 모든 객체 레이어 기본

					// TODO 카메라 중심잡기..?
				}
				// 특수 보기상태인가? (3, 4 상태)
				else
				{
					//cache = ContentManager.Instance._SelectedObj;		///
					
					// 선택 객체가 있는 경우 객체 리스트를 가져온다.
					if(EventManager.Instance.EventStates.ContainsKey(InputEventType.Input_clickSuccessUp))
                    {
						EventManager.Instance.EventStates[InputEventType.Input_clickSuccessUp].Elements.ForEach(x =>
						{
							caches.Add(x.Target);
						});
                    }

					//aStatus.CachingObject(cache);	///
					aStatus.CachingObjects(caches);

					mCam.cullingMask = Layers.SetMask(1);
					sCam.cullingMask = Layers.SetMask(2);

					SetObjectToDefaultLayer(objs);          // 모든 객체 레이어 기본
					//SetObjectSelectedLayer(cache);          // 선택 객체 레이어 캐시레이어
					SetObjectsSelectedLayer(caches);

					// TODO 캐시 위치로 카메라 중심잡기
					//SetFocusSelectedObject(cache, mCam);
					SetFocusSelectedObjects(caches, mCam);
				}
			}
			else
			{
				Debug.LogError("신규 프로젝트?");
			}

			m_status = _mStatus;
		}

		/// <summary>
		/// 모든 객체의 레이어를 기본으로 변환
		/// </summary>
		/// <param name="_objs"></param>
		private void SetObjectToDefaultLayer(List<GameObject> _objs)
		{
			_objs.ForEach(x => x.layer = Layers.GetLayerIndex(0));
		}

		/// <summary>
		/// 선택된 객체의 레이어를 캐시레이어로 변환
		/// </summary>
		/// <param name="_cache"></param>
		private void SetObjectSelectedLayer(GameObject _cache)
		{
			_cache.layer = Layers.GetLayerIndex(1);
		}

		private void SetObjectsSelectedLayer(List<GameObject> _caches)
        {
			_caches.ForEach(x =>
			{
				x.layer = Layers.GetLayerIndex(1);
			});
        }

		private void SetFocusSelectedObject(GameObject _cache, Camera _cam)
		{
			Bounds _b = new Bounds();
			Canvas _canvas = ContentManager.Instance._Canvas;
			UIEventType _uType = UIEventType.Viewport_ViewMode_ISO;

			MeshRenderer render;
			if(_cache.TryGetComponent<MeshRenderer>(out render))
			{
				_b = render.bounds;

				Cameras.SetCameraCenterPosition(_cam, _b, _canvas, _uType);
			}
		}

		private void SetFocusSelectedObjects(List<GameObject> _caches, Camera _cam)
        {
			Bounds _b = new Bounds();
			Canvas _canvas = ContentManager.Instance._Canvas;
			UIEventType _uType = UIEventType.Viewport_ViewMode_ISO;

			MeshRenderer render;
			if(_caches.Count != 0)
            {
				Vector3 min = default(Vector3);
				Vector3 max = default(Vector3);

				if(_caches.First().TryGetComponent<MeshRenderer>(out render))
                {
					Bounds bound = render.bounds;

					min = bound.min;
					max = bound.max;
                }

				// 캐시들 반복하면서 경계값 업데이트
				_caches.ForEach(x =>
				{
					if(x.TryGetComponent<MeshRenderer>(out render))
                    {
						Bounds bound = render.bounds;

						min = new Vector3(
							min.x > bound.min.x ? bound.min.x : min.x ,
							min.y > bound.min.y ? bound.min.y : min.y ,
							min.z > bound.min.z ? bound.min.z : min.z
							);

						max = new Vector3(
							max.x < bound.max.x ? bound.max.x : max.x,
							max.y < bound.max.y ? bound.max.y : max.y,
							max.z < bound.max.z ? bound.max.z : max.z
							);
					}
				});

				_b.center = (max + min) / 2;
				_b.size = (max - min);
            }
        }
	}
}
