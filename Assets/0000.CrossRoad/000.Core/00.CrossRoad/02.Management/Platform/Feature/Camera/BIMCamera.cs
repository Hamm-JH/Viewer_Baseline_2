﻿using Definition;
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
		/// <param name="_mode"></param>
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

		public override void OnClick(Vector3 mousePos)
		{
			if (!this.enabled) return;
			//Debug.Log($"In bim camera onclick : pos {mousePos}");
		}

		public override void OnDrag(int btn, Vector2 delta)
		{
			if (!this.enabled) return;
			//Debug.Log($"In bim camera ondrag : delta {delta}");

			InDrag(btn, delta);
		}

		public override void OnFocus(Vector3 mousePos, float delta)
		{
			if (!this.enabled) return;
			//Debug.Log($"In bim camera onfocus : pos {mousePos}, delta {delta}");

			InFocus(mousePos, delta);
		}

		public override void OnKey(List<KeyData> _key)
		{
			if (!this.enabled) return;
			//Debug.Log($"In bim camera onkey : key {_key.ToString()}");
		}

		public override void SetData(Data _camData)
		{
			Debug.Log($"***** data : {_camData.bMaxOffsetDistance}");
			maxOffsetDistance = _camData.bMaxOffsetDistance;
			orbitSpeed = _camData.bOrbitSpeed;
			freeSpeed = _camData.bFreeSpeed;
			panSpeed = _camData.bPanSpeed;
			zoomSpeed = _camData.bZoomSpeed;
		}

		public override void ResetData_targetOffset()
		{
			targetOffset = default(Vector3);
		}

		public override void SetData_MaxOffset(float _value)
		{
			maxOffsetDistance = _value;
		}
	}
}
