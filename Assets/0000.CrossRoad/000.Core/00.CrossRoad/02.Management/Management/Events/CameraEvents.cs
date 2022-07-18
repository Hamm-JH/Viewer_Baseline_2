using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management.Events
{
	using Definition;
	using Platform.Feature;
	using Platform.Feature.Camera;
	using UnityEngine.Events;

	/// <summary>
	/// 주 관리자에서 관리하는 카메라 이벤트
	/// </summary>
	[System.Serializable]
	public class CameraEvents : IEvents
	{
		/// <summary>
		/// 카메라 이벤트 생성자
		/// </summary>
		/// <param name="_camera">카메라 정보</param>
		public CameraEvents(ICamera.Data _camera)
		{
			this.selectEvent = new UnityEvent<GameObject>();
			this.clickEvent = new UnityEvent<Vector3>();
			this.dragEvent = new UnityEvent<int, Vector2>();
			this.focusEvent = new UnityEvent<Vector3, float>();
			this.keyEvent = new UnityEvent<List<KeyData>>();

			CameraData = _camera;
		}

		/// <summary>
		/// 객체 선택 이벤트
		/// Definition.ObjectType :: 객체유형 (Object, UI)
		/// GameObject :: 클릭된 대상
		/// </summary>
		public UnityEvent<GameObject> selectEvent;

		/// <summary>
		/// Main // 클릭 이벤트
		/// Vec3 :: 마우스 클릭 위치
		/// // GameObject :: 클릭된 대상
		/// // Definition.ObjectType :: 객체유형 (Object, UI)
		/// </summary>
		public UnityEvent<Vector3> clickEvent;

		/// <summary>
		/// Main // 드래그 이벤트
		/// Vec2 :: 드래그 정도
		/// </summary>
		public UnityEvent<int, Vector2> dragEvent;

		/// <summary>
		/// Main // 포커스 이벤트
		/// Vec3 :: 포커스 위치
		/// float :: 포커스 정도
		/// </summary>
		public UnityEvent<Vector3, float> focusEvent;

		/// <summary>
		/// Main // 키 입력 이벤트
		/// </summary>
		public UnityEvent<List<KeyData>> keyEvent;
	}
}
