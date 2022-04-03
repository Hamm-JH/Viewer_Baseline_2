using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.UI
{
	using AdminViewer;
	using Data.API;
	using Definition;
	using Management;
	using System;
	using System.Linq;
	using UnityEngine.Events;
	using UnityEngine.Rendering.Universal;
	using UnityEngine.UI;
	using View;

	public partial class UITemplate_AdminViewer : AUI
	{
		#region Toggle Dimension

		private void ToggleDimension(bool isOn)
		{
			Transform lv2 = ContentManager.Instance.Container.m_dimView.dimLevel2;
			List<Transform> lv4 = ContentManager.Instance.Container.m_dimView.dimLevel4;

			lv2.gameObject.SetActive(isOn);
			lv4.ForEach(x => x.gameObject.SetActive(false));
		}

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
				ToggleDimension(true);

				//Transform lv2 = ContentManager.Instance.Container.m_dimView.dimLevel2;
				List<Transform> lv4 = ContentManager.Instance.Container.m_dimView.dimLevel4;

				// 매칭되는 대상 찾기
				Transform _find = lv4.FindAll(x => name.Contains(x.name)).Last();

				// 활성화
				//lv2.gameObject.SetActive(true);
				//lv4.ForEach(x => x.gameObject.SetActive(false));

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

			dimData.fr_complete = false;
			dimData.ba_complete = false;
			dimData.to_complete = false;
			dimData.bo_complete = false;
			dimData.le_complete = false;
			dimData.re_complete = false;

			GameObject selected = EventManager.Instance._SelectedObject;

			Camera frCam = SetDimCamera(0, offset, selected);
			Camera baCam = SetDimCamera(1, offset, selected);
			Camera toCam = SetDimCamera(2, offset, selected);
			Camera boCam = SetDimCamera(3, offset, selected);
			Camera leCam = SetDimCamera(4, offset, selected);
			Camera reCam = SetDimCamera(5, offset, selected);

			// TODO 카메라 종류별로 이미지 찍어서 전송

			StartCoroutine(PrintDim(0, selected, frCam, Layers.SetMask(21), Layers.SetMask(11)));
			StartCoroutine(PrintDim(1, selected, baCam, Layers.SetMask(22), Layers.SetMask(12)));
			StartCoroutine(PrintDim(2, selected, toCam, Layers.SetMask(23), Layers.SetMask(13)));
			StartCoroutine(PrintDim(3, selected, boCam, Layers.SetMask(24), Layers.SetMask(14)));
			StartCoroutine(PrintDim(4, selected, leCam, Layers.SetMask(25), Layers.SetMask(15)));
			StartCoroutine(PrintDim(5, selected, reCam, Layers.SetMask(26), Layers.SetMask(16)));
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
			int lineCullMask = 0;
			Vector3 angle = default(Vector3);

			switch(_index)
			{
				case 0:
					{
						name = "fr cam";
						tex = ContentManager.Instance.Container.m_dim.tex_fr;
						cullMask = Layers.SetMask(11);
						lineCullMask = Layers.SetMask(21);
						angle = Angle.Set(UIEventType.Viewport_ViewMode_SIDE_FRONT);
					}
					break;

				case 1:
					{
						name = "ba cam";
						tex = ContentManager.Instance.Container.m_dim.tex_ba;
						cullMask = Layers.SetMask(12);
						lineCullMask = Layers.SetMask(22);
						angle = Angle.Set(UIEventType.Viewport_ViewMode_SIDE_BACK);
					}
					break;

				case 2:
					{
						name = "to cam";
						tex = ContentManager.Instance.Container.m_dim.tex_to;
						cullMask = Layers.SetMask(13);
						lineCullMask = Layers.SetMask(23);
						angle = Angle.Set(UIEventType.Viewport_ViewMode_TOP);
					}
					break;

				case 3:
					{
						name = "bo cam";
						tex = ContentManager.Instance.Container.m_dim.tex_bo;
						cullMask = Layers.SetMask(14);
						lineCullMask = Layers.SetMask(24);
						angle = Angle.Set(UIEventType.Viewport_ViewMode_BOTTOM);
					}
					break;

				case 4:
					{
						name = "le cam";
						tex = ContentManager.Instance.Container.m_dim.tex_le;
						cullMask = Layers.SetMask(15);
						lineCullMask = Layers.SetMask(25);
						angle = Angle.Set(UIEventType.Viewport_ViewMode_SIDE_LEFT);
					}
					break;

				case 5:
					{
						name = "re cam";
						tex = ContentManager.Instance.Container.m_dim.tex_re;
						cullMask = Layers.SetMask(16);
						lineCullMask = Layers.SetMask(26);
						angle = Angle.Set(UIEventType.Viewport_ViewMode_SIDE_RIGHT);
					}
					break;
			}
			
			GameObject temp = new GameObject(name);
			GameObject obj = Instantiate(temp, _target.transform);
			obj.transform.rotation = Quaternion.Euler(angle);
			Camera cam = obj.AddComponent<Camera>();

			Light light = obj.AddComponent<Light>();
			light.type = LightType.Directional;
			light.cullingMask = lineCullMask;

			cam.clearFlags = CameraClearFlags.SolidColor;
			cam.backgroundColor = Color.black;
			cam.orthographic = true;
			cam.orthographicSize = 5;
			cam.targetTexture = tex;
			cam.cullingMask = cullMask;

			obj.transform.Translate(Vector3.back * _offset);

			Destroy(temp);

			return cam;
		}

		private IEnumerator PrintDim(int _index, GameObject _selected, Camera _cam, int _firstCullmask, int _secondCullmask)
		{
			yield return new WaitForEndOfFrame();
			{
				_cam.cullingMask = _firstCullmask;

				RenderTexture currentTexture = RenderTexture.active;
				RenderTexture.active = _cam.targetTexture;

				Rect rect = new Rect(0, 0, _cam.targetTexture.width, _cam.targetTexture.height);

				byte[] imgByte;
				Texture2D _tex = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);

				_tex.ReadPixels(rect, 0, 0, false);
				_tex.Apply();

				RenderTexture.active = currentTexture;

				imgByte = _tex.EncodeToPNG();
				DestroyImmediate(_tex);

				string base64 = Convert.ToBase64String(imgByte);

				switch(_index)
				{
					case 0:
						dimData.fr_result1 = base64;
						break;

					case 1:
						dimData.ba_result1 = base64;
						break;

					case 2:
						dimData.to_result1 = base64;
						break;

					case 3:
						dimData.bo_result1 = base64;
						break;

					case 4:
						dimData.le_result1 = base64;
						break;

					case 5:
						dimData.re_result1 = base64;
						break;
				}
			}

			yield return new WaitForEndOfFrame();
			{
				_cam.cullingMask = _secondCullmask;

				RenderTexture currentTexture = RenderTexture.active;
				RenderTexture.active = _cam.targetTexture;

				Rect rect = new Rect(0, 0, _cam.targetTexture.width, _cam.targetTexture.height);

				byte[] imgByte;
				Texture2D _tex = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);

				_tex.ReadPixels(rect, 0, 0, false);
				_tex.Apply();

				RenderTexture.active = currentTexture;

				imgByte = _tex.EncodeToPNG();
				DestroyImmediate(_tex);

				string base64 = Convert.ToBase64String(imgByte);

				switch (_index)
				{
					case 0:
						dimData.fr_result2 = base64;
						dimData.fr_complete = true;
						break;

					case 1:
						dimData.ba_result2 = base64;
						dimData.ba_complete = true;
						break;

					case 2:
						dimData.to_result2 = base64;
						dimData.to_complete = true;
						break;

					case 3:
						dimData.bo_result2 = base64;
						dimData.bo_complete = true;
						break;

					case 4:
						dimData.le_result2 = base64;
						dimData.le_complete = true;
						break;

					case 5:
						dimData.re_result2 = base64;
						dimData.re_complete = true;
						break;
				}

			}

			CheckDimPrintEnd(_selected);

			Destroy(_cam.gameObject);

			yield break;
		}

		private void CheckDimPrintEnd(GameObject _selected)
		{
			bool result = dimData.fr_complete && dimData.ba_complete &&
				dimData.to_complete && dimData.bo_complete &&
				dimData.le_complete && dimData.re_complete;

			if(result)
			{
#if UNITY_EDITOR
				/*
				* OnReadyToDrawingPrint
				* 1 : 현재 선택된 부재명
				* 2 : 앞면 도면 front
				* 3 : 뒷면 도면 back
				* 4 : 좌면 도면 left
				* 5 : 우면 도면 right
				* 6 : 윗면 도면 top
				* 7 : 밑면 도면 bottom
				* 8 : 앞면 이미지 front
				* 9 : 뒷면 이미지 back
				* 10 : 좌면 이미지 left
				* 11 : 우면 이미지 right
				* 12 : 윗면 이미지 top
				* 13 : 밑면 이미지 bottom
				* 14 : 손상/보수 선택 bool
				*/
#else
				ExternalAPI.OnReadyToDrawingPrint(
					_selected.name,
					dimData.fr_result1,
					dimData.ba_result1,
					dimData.le_result1,
					dimData.re_result1,
					dimData.to_result1,
					dimData.bo_result1,
					dimData.fr_result2,
					dimData.ba_result2,
					dimData.le_result2,
					dimData.re_result2,
					dimData.to_result2,
					dimData.bo_result2,
					true
					);
#endif
				Debug.Log($"name : {_selected.name}");
				Debug.Log($"fr 1");
				Debug.Log($"{dimData.fr_result1}");
				Debug.Log($"ba 1");
				Debug.Log($"{dimData.ba_result1}");
				Debug.Log($"to 1");
				Debug.Log($"{dimData.to_result1}");
				Debug.Log($"bo 1");
				Debug.Log($"{dimData.bo_result1}");
				Debug.Log($"le 1");
				Debug.Log($"{dimData.le_result1}");
				Debug.Log($"re 1");
				Debug.Log($"{dimData.re_result1}");

				Debug.Log($"fr 2");
				Debug.Log($"{dimData.fr_result2}");
				Debug.Log($"ba 2");
				Debug.Log($"{dimData.ba_result2}");
				Debug.Log($"to 2");
				Debug.Log($"{dimData.to_result2}");
				Debug.Log($"bo 2");
				Debug.Log($"{dimData.bo_result2}");
				Debug.Log($"le 2");
				Debug.Log($"{dimData.le_result2}");
				Debug.Log($"re 2");
				Debug.Log($"{dimData.re_result2}");


				Debug.Log("dim print is end");
			}
		}
	}
}
