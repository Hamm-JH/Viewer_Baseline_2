using Definition;
using Definition.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Platform.Feature.Camera
{
	public partial class FreeCamera : ICamera
	{

		/// <summary>
		/// 카메라 모드 변경
		/// </summary>
		/// <param name="_mode"></param>
		protected override void ChangeCameraMode(CameraModes _mode)
		{
			if (_mode == CameraModes.FREE_ISO
				|| _mode == CameraModes.FREE_Top
				|| _mode == CameraModes.FREE_Side
				|| _mode == CameraModes.FREE_Bottom)
			{
				this.enabled = false;
			}
			else
			{
				this.enabled = false;
			}
		}

		/// <summary>
		/// 객체 선택
		/// </summary>
		/// <param name="_target">선택 객체</param>
		public override void OnSelect(GameObject _target)
		{
			if (!this.enabled) return;
			if(_target != null)
			{
				Debug.Log($"In free camera onselect : name {_target.name}");
			}
		}

		/// <summary>
		/// 클릭
		/// </summary>
		/// <param name="mousePos">마우스 위치</param>
		public override void OnClick(Vector3 mousePos)
		{
			if (!this.enabled) return;
			Debug.Log($"In free camera onclick : pos {mousePos}");
		}

		/// <summary>
		/// 드래그
		/// </summary>
		/// <param name="btn">버튼 인덱스</param>
		/// <param name="delta">드래그 거리</param>
		public override void OnDrag(int btn, Vector2 delta)
		{
			if (!this.enabled) return;
			Debug.Log($"In free camera ondrag : delta {delta}");

			// 드래그 이벤트 구성

			// 마우스 드래그
			GetInputLookRotation(delta);
			//var mouseMovement = delta;


			//m_TargetCameraState.
		}

		/// <summary>
		/// 포커스
		/// </summary>
		/// <param name="mousePos">마우스 위치</param>
		/// <param name="delta">포커스 거리</param>
		public override void OnFocus(Vector3 mousePos, float delta)
		{
			if (!this.enabled) return;
			Debug.Log($"In free camera onfocus : pos {mousePos}, delta {delta}");
		}

		/// <summary>
		/// 키 입력
		/// </summary>
		/// <param name="_key">키 리스트</param>
		public override void OnKey(List<KeyData> _key)
		{
			if (!this.enabled) return;
			Debug.Log($"In free camera onkey : key {_key.ToString()}");

			GetInputTranslationDirection(_key);
		}

		/// <summary>
		/// 데이터 할당
		/// </summary>
		/// <param name="_camData">카메라 데이터</param>
		public override void SetData(Data _camData)
		{
			Debug.Log("데이터 할당");
		}
	}
}
