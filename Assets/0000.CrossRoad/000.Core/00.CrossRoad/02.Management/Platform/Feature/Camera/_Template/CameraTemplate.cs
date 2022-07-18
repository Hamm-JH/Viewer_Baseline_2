using Definition;
using Definition.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platform.Feature.Camera
{
	public class CameraTemplate : ICamera
	{
		/// <summary>
		/// ī�޶� ��� ����
		/// </summary>
		/// <param name="_mode"></param>
		protected override void ChangeCameraMode(CameraModes _mode)
		{
			if (_mode == CameraModes.FREE_Bottom
				|| _mode == CameraModes.FREE_ISO
				|| _mode == CameraModes.FREE_Side
				|| _mode == CameraModes.FREE_Top)
			{
				this.enabled = true;
			}

			this.enabled = true;
		}

		/// <summary>
		/// ��ü ����
		/// </summary>
		/// <param name="target">���� ��ü</param>
		public override void OnSelect(GameObject target)
		{
			if (!this.enabled) return;
			Debug.Log($"In orbit camera onselect : name {target.name}");
		}

		/// <summary>
		/// ���콺 Ŭ��
		/// </summary>
		/// <param name="mousePos">���콺 ��ġ</param>
		public override void OnClick(Vector3 mousePos)
		{
			if (!this.enabled) return;
			Debug.Log($"In orbit camera onclick : pos {mousePos}");
		}

		/// <summary>
		/// �巡��
		/// </summary>
		/// <param name="btn">��ư �ε���</param>
		/// <param name="delta">�巡�� �Ÿ�</param>
		public override void OnDrag(int btn, Vector2 delta)
		{
			if (!this.enabled) return;
			Debug.Log($"In orbit camera ondrag : delta {delta}");
		}

		/// <summary>
		/// ��Ŀ��
		/// </summary>
		/// <param name="mousePos">���콺 ��ġ</param>
		/// <param name="delta">��Ŀ�� �Ÿ�</param>
		public override void OnFocus(Vector3 mousePos, float delta)
		{
			if (!this.enabled) return;
			Debug.Log($"In orbit camera onfocus : pos {mousePos}, delta {delta}");
		}

		/// <summary>
		/// Ű���� �Է�
		/// </summary>
		/// <param name="_key">Ű ����Ʈ</param>
		public override void OnKey(List<KeyData> _key)
		{
			if (!this.enabled) return;
			Debug.Log($"In orbit camera onkey : key {_key.ToString()}");
		}

		/// <summary>
		/// ������ �Ҵ�
		/// </summary>
		/// <param name="_camData">ī�޶� ������</param>
		public override void SetData(Data _camData)
		{
			Debug.Log("���� ������ �Ҵ�");
		}
	}
}
