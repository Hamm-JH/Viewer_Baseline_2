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
	using UnityEngine.Rendering.Universal;
	using UnityEngine.UI;
	using View;

	public partial class UITemplate_AdminViewer : AUI
	{
		#region Toggle Dimension

		private void ToggleDimension()
		{
			GameObject selected = EventManager.Instance._SelectedObject;
			if(selected == null)
			{
				Debug.LogError("현재 선택된 객체가 없음");
				return;
			}

			//Debug.Log("Hello");

			string name = GetName(selected.name);
			Debug.Log(name);

			PlatformCode pCode = MainManager.Instance.Platform;
			if(Platforms.IsTunnelPlatform(pCode))
			{
				Debug.LogError("dim 터널 작업 필요");
			}
			else if(Platforms.IsBridgePlatform(pCode))
			{
				Transform lv2 = ContentManager.Instance.Container.m_dimView.dimLevel2;
				List<Transform> lv4 = ContentManager.Instance.Container.m_dimView.dimLevel4;

				// 매칭되는 대상 찾기
				Transform _find = lv4.FindAll(x => name.Contains(x.name)).Last();

				// 활성화
				lv2.gameObject.SetActive(true);
				lv4.ForEach(x => x.gameObject.SetActive(false));

				if(_find != null)
				{
					_find.gameObject.SetActive(true);
				}
			}
		}

		private string GetName(string _value)
		{
			string result = "";

			PlatformCode pCode = MainManager.Instance.Platform;
			if(Platforms.IsTunnelPlatform(pCode))
			{
				Debug.LogError("이름 작업 필요");
			}
			else if(Platforms.IsBridgePlatform(pCode))
			{
				result = _value.Split(',')[0];
			}

			return result;
		}

		#endregion


		private void PrintDimension()
		{
			float offset = 5f;

			GameObject selected = EventManager.Instance._SelectedObject;

			Camera frCam = SetDimCamera(0, offset, selected);
			Camera baCam = SetDimCamera(1, offset, selected);
			Camera toCam = SetDimCamera(2, offset, selected);
			Camera boCam = SetDimCamera(3, offset, selected);
			Camera leCam = SetDimCamera(4, offset, selected);
			Camera reCam = SetDimCamera(5, offset, selected);

			// TODO 카메라 종류별로 이미지 찍어서 전송
		}

		/// <summary>
		/// DimCamera setting
		/// _index :;
		/// 0 fr // 1 ba // 2 to // 3 bo // 4 le // 5 ri
		/// </summary>
		/// <param name="_index"></param>
		/// <param name="_offset"></param>
		/// <param name="_target"></param>
		/// <returns></returns>
		private Camera SetDimCamera(int _index, float _offset, GameObject _target)
		{
			string name = "";
			RenderTexture tex = null;
			int cullMask = 0;
			Vector3 angle = default(Vector3);

			switch(_index)
			{
				case 0:
					{
						name = "fr cam";
						tex = ContentManager.Instance.Container.m_dim.tex_fr;
						cullMask = Layers.SetMask(11);
						angle = Angle.Set(UIEventType.Viewport_ViewMode_SIDE_FRONT);
					}
					break;

				case 1:
					{
						name = "ba cam";
						tex = ContentManager.Instance.Container.m_dim.tex_ba;
						cullMask = Layers.SetMask(12);
						angle = Angle.Set(UIEventType.Viewport_ViewMode_SIDE_BACK);
					}
					break;

				case 2:
					{
						name = "to cam";
						tex = ContentManager.Instance.Container.m_dim.tex_to;
						cullMask = Layers.SetMask(13);
						angle = Angle.Set(UIEventType.Viewport_ViewMode_TOP);
					}
					break;

				case 3:
					{
						name = "bo cam";
						tex = ContentManager.Instance.Container.m_dim.tex_bo;
						cullMask = Layers.SetMask(14);
						angle = Angle.Set(UIEventType.Viewport_ViewMode_BOTTOM);
					}
					break;

				case 4:
					{
						name = "le cam";
						tex = ContentManager.Instance.Container.m_dim.tex_le;
						cullMask = Layers.SetMask(15);
						angle = Angle.Set(UIEventType.Viewport_ViewMode_SIDE_LEFT);
					}
					break;

				case 5:
					{
						name = "re cam";
						tex = ContentManager.Instance.Container.m_dim.tex_re;
						cullMask = Layers.SetMask(16);
						angle = Angle.Set(UIEventType.Viewport_ViewMode_SIDE_RIGHT);
					}
					break;
			}

			GameObject obj = Instantiate(new GameObject(name), _target.transform);
			obj.transform.rotation = Quaternion.Euler(angle);
			Camera cam = obj.AddComponent<Camera>();

			cam.clearFlags = CameraClearFlags.SolidColor;
			cam.backgroundColor = Color.black;
			cam.orthographic = true;
			cam.orthographicSize = 5;
			cam.targetTexture = tex;
			cam.cullingMask = cullMask;

			obj.transform.Translate(Vector3.back * _offset);

			return cam;
		}
	}
}
