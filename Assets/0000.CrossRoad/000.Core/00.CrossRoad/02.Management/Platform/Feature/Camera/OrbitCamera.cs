using Definition;
using Definition.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platform.Feature.Camera
{
	public class OrbitCamera : ICamera
	{
		/// <summary>
		/// ī�޶� ��� ����
		/// </summary>
		/// <param name="_mode"></param>
		protected override void ChangeCameraMode(CameraModes _mode)
		{
			if(_mode == CameraModes.ORBIT_ISO
				|| _mode == CameraModes.ORBIT_Top
				|| _mode == CameraModes.ORBIT_Side
				|| _mode == CameraModes.ORBIT_Bottom)
			{
				this.enabled = true;
			}
			else
			{
				this.enabled = false;
			}
		}

		/// <summary>
		/// ��ü ����
		/// </summary>
		/// <param name="_target">���� ��ü</param>
		public override void OnSelect(GameObject _target)
		{
			if (!this.enabled) return;
			if(_target != null)
			{
				Debug.Log($"In orbit camera onselect : name {_target.name}");
			}
		}

		/// <summary>
		/// Ŭ��
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
		/// <param name="delta">�巡�� ����</param>
		public override void OnDrag(int btn, Vector2 delta)
		{
			if (!this.enabled) return;
			Debug.Log($"In orbit camera ondrag : delta {delta}");
		}

		/// <summary>
		/// ��Ŀ��
		/// </summary>
		/// <param name="mousePos">���콺 ��ġ</param>
		/// <param name="delta">��Ŀ�� ����</param>
		public override void OnFocus(Vector3 mousePos, float delta)
		{
			if (!this.enabled) return;
			Debug.Log($"In orbit camera onfocus : pos {mousePos}, delta {delta}");
		}

		/// <summary>
		/// Ű �Է�
		/// </summary>
		/// <param name="_key">�Է� Ű ����Ʈ</param>
		public override void OnKey(List<KeyData> _key)
		{
			if (!this.enabled) return;
			Debug.Log($"In orbit camera onkey : key {_key.ToString()}");
		}

		/// <summary>
		/// ������ �Ҵ�
		/// </summary>
		/// <param name="_camData">ī�޶� ����</param>
		public override void SetData(Data _camData)
		{
			Debug.Log("������ �Ҵ�");
		}
	}
}
