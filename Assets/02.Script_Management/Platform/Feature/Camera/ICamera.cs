using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platform.Feature.Camera
{
	using Definition;
	using Definition.Control;
	using Management.Events;
	using UnityEngine.Events;

	[RequireComponent(typeof(UnityEngine.Camera))]
	public abstract class ICamera : MonoBehaviour
	{
		[System.Serializable]
		public class Data
		{
			#region BIM Camera
			public float bMaxOffsetDistance;// = 2000f;
			public float bOrbitSpeed;// = 15f;
			public float bPanSpeed;// = .5f;
			public float bZoomSpeed;// = 10f;
			#endregion
		}

		//public Data m_data;
		[SerializeField] CameraModes camMode;
		[SerializeField] CameraEvents cameraEvents;

		public CameraModes CamMode
		{ 
			get => camMode; 
			set
			{
				camMode=value;
				ChangeCameraMode(value);
			}
		}
		public CameraEvents CameraEvents { get => cameraEvents; set => cameraEvents=value; }

		public void SetAction(CameraEvents _events)
		{
			selectAction += OnSelect;
			clickAction += OnClick;
			dragAction += OnDrag;
			focusAction += OnFocus;
			keyAction += OnKey;

			_events.selectEvent.AddListener(selectAction);
			_events.clickEvent.AddListener(clickAction);
			_events.dragEvent.AddListener(dragAction);
			_events.focusEvent.AddListener(focusAction);
			_events.keyEvent.AddListener(keyAction);

			CameraEvents = _events;
		}

		/// <summary>
		/// 카메라 모드 변경
		/// </summary>
		/// <param name="_mode"></param>
		protected abstract void ChangeCameraMode(CameraModes _mode);

		public abstract void SetData(Data _camData);

		#region Actions
		/// <summary>
		/// 클릭 - 선택 발생시 실행
		/// </summary>
		UnityAction<GameObject> selectAction;

		/// <summary>
		/// 클릭 이벤트 발생시 실행
		/// </summary>
		UnityAction<Vector3> clickAction;

		/// <summary>
		/// 드래그 이벤트 발생시 실행
		/// </summary>
		UnityAction<int, Vector2> dragAction;

		/// <summary>
		/// 포커스 이벤트 발생시 실행
		/// </summary>
		UnityAction<Vector3, float> focusAction;

		/// <summary>
		/// 키 이벤트 발생시 실행
		/// </summary>
		UnityAction<List<KeyData>> keyAction;

		/// <summary>
		/// 선택
		/// </summary>
		public abstract void OnSelect(GameObject _target);

		/// <summary>
		/// 클릭
		/// </summary>
		public abstract void OnClick(Vector3 mousePos);

		/// <summary>
		/// 드래그
		/// </summary>
		/// <param name="delta">드래그 움직임값</param>
		public abstract void OnDrag(int btn, Vector2 delta);

		/// <summary>
		/// 포커스
		/// </summary>
		/// <param name="delta"> 포커스 정도 </param>
		public abstract void OnFocus(Vector3 mousePos, float delta);

		/// <summary>
		/// 키 입력
		/// </summary>
		/// <param name="_keys">입력된 키 </param>
		public abstract void OnKey(List<KeyData> _key);
		#endregion
	}
}
