using Definition.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platform.Feature.Camera
{
	public class CameraTemplate : ICamera
	{
		/// <summary>
		/// 카메라 모드 변경
		/// </summary>
		/// <param name="_mode"></param>
		protected override void ChangeCameraMode(CameraMode _mode)
		{
			if (_mode == CameraMode.FREE_Bottom
				|| _mode == CameraMode.FREE_ISO
				|| _mode == CameraMode.FREE_Side
				|| _mode == CameraMode.FREE_Top)
			{
				this.enabled = true;
			}

			this.enabled = true;
		}

		public override void OnSelect(GameObject target)
		{
			if (!this.enabled) return;
			Debug.Log($"In orbit camera onselect : name {target.name}");
		}

		public override void OnClick(Vector3 mousePos)
		{
			if (!this.enabled) return;
			Debug.Log($"In orbit camera onclick : pos {mousePos}");
		}

		public override void OnDrag(int btn, Vector2 delta)
		{
			if (!this.enabled) return;
			Debug.Log($"In orbit camera ondrag : delta {delta}");
		}

		public override void OnFocus(Vector3 mousePos, float delta)
		{
			if (!this.enabled) return;
			Debug.Log($"In orbit camera onfocus : pos {mousePos}, delta {delta}");
		}

		public override void OnKey(List<KeyCode> _key)
		{
			if (!this.enabled) return;
			Debug.Log($"In orbit camera onkey : key {_key.ToString()}");
		}

		public override void SetData(Data _camData)
		{
			Debug.Log("동작 데이터 할당");
		}
	}
}
