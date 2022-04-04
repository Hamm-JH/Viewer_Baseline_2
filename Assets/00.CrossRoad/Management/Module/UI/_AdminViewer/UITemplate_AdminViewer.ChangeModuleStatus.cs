using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.UI
{
	using AdminViewer;
	using Data.API;
	using Definition;
	using Management;
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
					cache = ContentManager.Instance._SelectedObj;
					aStatus.CachingObject(cache);

					mCam.cullingMask = Layers.SetMask(1);
					sCam.cullingMask = Layers.SetMask(2);

					SetObjectToDefaultLayer(objs);          // 모든 객체 레이어 기본
					SetObjectSelectedLayer(cache);          // 선택 객체 레이어 캐시레이어

					// TODO 캐시 위치로 카메라 중심잡기
					SetFocusSelectedObject(cache, mCam);
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
	}
}
