using Definition;
using Definition.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platform.Feature.Camera
{
	public partial class BIMCamera : ICamera
	{
		/// <summary>
		/// 카메라 모드 변경
		/// </summary>
		/// <param name="_mode">카메라 분류</param>
		protected override void ChangeCameraMode(CameraModes _mode)
		{
			if (_mode == CameraModes.BIM_ISO
				|| _mode == CameraModes.BIM_Top
				|| _mode == CameraModes.BIM_Side
				|| _mode == CameraModes.BIM_Bottom
				|| _mode == CameraModes.TunnelInside_Rotate
				|| _mode == CameraModes.BIM_Panning)
			{
				this.enabled = true;
			}
			else
			{
				this.enabled = false;
			}

		}

		/// <summary>
		/// 객체 선택시 
		/// </summary>
		/// <param name="_target">목표 객체</param>
		public override void OnSelect(GameObject _target)
		{
			if (!this.enabled) return;
			if(_target != null)
			{
				//Debug.Log($"In bim camera onselect : name {_target.name}");
				target = _target.transform;
			}
			else
			{
				target = null;
			}

		}

		/// <summary>
		/// 객체 클릭
		/// </summary>
		/// <param name="mousePos">마우스 위치</param>
		public override void OnClick(Vector3 mousePos)
		{
			if (!this.enabled) return;
			//Debug.Log($"In bim camera onclick : pos {mousePos}");
		}

		/// <summary>
		/// 드래그
		/// </summary>
		/// <param name="btn">마우스 버튼</param>
		/// <param name="delta">드래그 값</param>
		public override void OnDrag(int btn, Vector2 delta)
		{
			if (!this.enabled) return;
			//Debug.Log($"In bim camera ondrag : delta {delta}");

			InDrag(btn, delta);
		}

		/// <summary>
		/// 포커스
		/// </summary>
		/// <param name="mousePos">마우스 위치</param>
		/// <param name="delta">포커스 정도</param>
		public override void OnFocus(Vector3 mousePos, float delta)
		{
			if (!this.enabled) return;
			//Debug.Log($"In bim camera onfocus : pos {mousePos}, delta {delta}");

			InFocus(mousePos, delta);
		}

		/// <summary>
		/// 키 입력
		/// </summary>
		/// <param name="_key">키 리스트</param>
		public override void OnKey(List<KeyData> _key)
		{
			if (!this.enabled) return;
			//Debug.Log($"In bim camera onkey : key {_key.ToString()}");
		}

		/// <summary>
		/// 데이터 할당
		/// </summary>
		/// <param name="_camData">카메라 정보</param>
		public override void SetData(Data _camData)
		{
			Debug.Log($"***** data : {_camData.bMaxOffsetDistance}");
			maxOffsetDistance = _camData.bMaxOffsetDistance;
			orbitSpeed = _camData.bOrbitSpeed;
			freeSpeed = _camData.bFreeSpeed;
			panSpeed = _camData.bPanSpeed;
			zoomSpeed = _camData.bZoomSpeed;
		}

		/// <summary>
		/// 목표 offset값 초기화
		/// </summary>
		public override void ResetData_targetOffset()
		{
			targetOffset = default(Vector3);
		}

		/// <summary>
		/// 데이터 offset 최대값 할당
		/// </summary>
		/// <param name="_value"></param>
		public override void SetData_MaxOffset(float _value)
		{
			maxOffsetDistance = _value;
		}
	}
}
