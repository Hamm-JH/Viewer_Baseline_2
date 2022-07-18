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
		/// ī�޶� ��� ����
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
		/// ��ü ����
		/// </summary>
		/// <param name="_target">���� ��ü</param>
		public override void OnSelect(GameObject _target)
		{
			if (!this.enabled) return;
			if(_target != null)
			{
				Debug.Log($"In free camera onselect : name {_target.name}");
			}
		}

		/// <summary>
		/// Ŭ��
		/// </summary>
		/// <param name="mousePos">���콺 ��ġ</param>
		public override void OnClick(Vector3 mousePos)
		{
			if (!this.enabled) return;
			Debug.Log($"In free camera onclick : pos {mousePos}");
		}

		/// <summary>
		/// �巡��
		/// </summary>
		/// <param name="btn">��ư �ε���</param>
		/// <param name="delta">�巡�� �Ÿ�</param>
		public override void OnDrag(int btn, Vector2 delta)
		{
			if (!this.enabled) return;
			Debug.Log($"In free camera ondrag : delta {delta}");

			// �巡�� �̺�Ʈ ����

			// ���콺 �巡��
			GetInputLookRotation(delta);
			//var mouseMovement = delta;


			//m_TargetCameraState.
		}

		/// <summary>
		/// ��Ŀ��
		/// </summary>
		/// <param name="mousePos">���콺 ��ġ</param>
		/// <param name="delta">��Ŀ�� �Ÿ�</param>
		public override void OnFocus(Vector3 mousePos, float delta)
		{
			if (!this.enabled) return;
			Debug.Log($"In free camera onfocus : pos {mousePos}, delta {delta}");
		}

		/// <summary>
		/// Ű �Է�
		/// </summary>
		/// <param name="_key">Ű ����Ʈ</param>
		public override void OnKey(List<KeyData> _key)
		{
			if (!this.enabled) return;
			Debug.Log($"In free camera onkey : key {_key.ToString()}");

			GetInputTranslationDirection(_key);
		}

		/// <summary>
		/// ������ �Ҵ�
		/// </summary>
		/// <param name="_camData">ī�޶� ������</param>
		public override void SetData(Data _camData)
		{
			Debug.Log("������ �Ҵ�");
		}
	}
}
