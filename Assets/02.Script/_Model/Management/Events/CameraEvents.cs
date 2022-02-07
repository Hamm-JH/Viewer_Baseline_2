using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management.Events
{
	using Control._Input;
	using Platform.Feature.Camera;
	using UnityEngine.Events;

	/// <summary>
	/// �� �����ڿ��� �����ϴ� ī�޶� �̺�Ʈ
	/// </summary>
	[System.Serializable]
	public class CameraEvents : IEvents
	{
		public CameraEvents(ICamera.Data _camera)
		{
			this.selectEvent = new UnityEvent<GameObject>();
			this.clickEvent = new UnityEvent<Vector3>();
			this.dragEvent = new UnityEvent<int, Vector2>();
			this.focusEvent = new UnityEvent<Vector3, float>();
			this.keyEvent = new UnityEvent<List<KeyCode>>();

			CameraData = _camera;
		}

		/// <summary>
		/// ��ü ���� �̺�Ʈ
		/// Definition.ObjectType :: ��ü���� (Object, UI)
		/// GameObject :: Ŭ���� ���
		/// </summary>
		public UnityEvent<GameObject> selectEvent;

		/// <summary>
		/// Main // Ŭ�� �̺�Ʈ
		/// Vec3 :: ���콺 Ŭ�� ��ġ
		/// // GameObject :: Ŭ���� ���
		/// // Definition.ObjectType :: ��ü���� (Object, UI)
		/// </summary>
		public UnityEvent<Vector3> clickEvent;
		//public UnityEvent<Definition.ObjectType, GameObject> clickEvent;

		/// <summary>
		/// Main // �巡�� �̺�Ʈ
		/// Vec2 :: �巡�� ����
		/// </summary>
		public UnityEvent<int, Vector2> dragEvent;

		/// <summary>
		/// Main // ��Ŀ�� �̺�Ʈ
		/// Vec3 :: ��Ŀ�� ��ġ
		/// float :: ��Ŀ�� ����
		/// </summary>
		public UnityEvent<Vector3, float> focusEvent;

		/// <summary>
		/// Main // Ű �Է� �̺�Ʈ
		/// </summary>
		public UnityEvent<List<KeyCode>> keyEvent;
	}
}
