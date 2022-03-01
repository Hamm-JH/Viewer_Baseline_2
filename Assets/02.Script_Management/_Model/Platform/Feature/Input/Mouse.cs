using Management;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Platform.Feature._Input
{
	using Definition;

	public partial class Mouse : IInput
	{
		#region Pre-defined 사전 정의

		[System.Serializable]
		public class Data
		{
			/// <summary>
			/// 드래그 경계값
			/// 클릭 또는 드래그 시작에 관련된 연산에 씀
			/// </summary>
			public float dragBoundary;

			/// <summary>
			/// 드래그 반전여부
			/// 각 벡터 요소의 값을 드래그 전달자(Vec2)에 곱연산으로 보정
			/// </summary>
			public Vector2 dragInvert;

			/// <summary>
			/// 포커스 반전여부
			/// 포커스 요소 전달자(float::delta)에 곱연산으로 보정
			/// </summary>
			public int focusInvert;

			/// <summary>
			/// 클릭 버튼 지정
			/// </summary>
			public int clickBtnIndex;

			/// <summary>
			/// 드래그 버튼 지정
			/// </summary>
			public int dragBtnIndex;

			//------------------ BIM Camera

			/// <summary>
			/// 최대 오프셋 거리
			/// </summary>
			public float bMaxOffsetDistance;

			/// <summary>
			/// 궤도회전 속도
			/// </summary>
			public float bOrbitSpeed;

			/// <summary>
			/// 평면이동 속도
			/// </summary>
			public float bPanSpeed;

			/// <summary>
			/// 줌 속도
			/// </summary>
			public float bZoomSpeed;
		}

		[SerializeField] Data defData;

		#endregion

		// Update is called once per frame
		void Update()
		{
			// 왼쪽 마우스 버튼에 대한 이벤트 체크
			MouseCheck(defData.clickBtnIndex);

			// 오른쪽 마우스 버튼에 대한 이벤트 체크
			MouseCheck(defData.dragBtnIndex);

			// 스크롤 체크
			ScrollCheck();
		}

		#region fields
		/// <summary>
		/// 마우스 클릭 확인변수
		/// </summary>
		bool onClick = false;

		/// <summary>
		/// 마우스 드래그 거리합
		/// </summary>
		Vector2 deltaDistance = default(Vector2);

		/// <summary>
		/// 마우스 스크롤 델타수집
		/// </summary>
		Vector2 scrollDelta = default(Vector2);

		#endregion

		#region 마우스 체크

		/// <summary>
		/// 마우스 인덱스에 대응하는 이벤트 체크 수행
		/// </summary>
		/// <param name="btnIndex"></param>
		private void MouseCheck(int btnIndex)
		{
			// 마우스 버튼 0, 1, 2에서만 동작함
			if (!(btnIndex >= 0 && btnIndex < 3))
			{
				Debug.LogError($"Access index is not validate this index ({btnIndex}). " +
					"Mouse event can access only left(0), right(1), scroll(2) button index");
				return;
			}

			if(Input.GetMouseButtonDown(btnIndex))
			{
				onClick = true;     // 클릭 시작
				deltaDistance = default(Vector2);

				// 클릭 다운 이벤트
				SetClickEvent(InputEventType.Input_clickDown, btnIndex, Input.mousePosition);
				//if(btnIndex == 0)
				//{
				//}
			}
			else if(Input.GetMouseButton(btnIndex))
			{
				if(onClick)	// 클릭 상태 중에만 실행
				{
					//Debug.Log($"Mouse X : {Input.GetAxis("Mouse X")}");		// 마우스 X축 프레임별 이동거리 수집
					//Debug.Log($"Mouse Y : {Input.GetAxis("Mouse Y")}");		// 마우스 Y축 프레임별 이동거리 수집

					// 반전값을 보간한 축별 이동거리 계산
					float xDelta = Input.GetAxis("Mouse X") * defData.dragInvert.x;
					float yDelta = Input.GetAxis("Mouse Y") * defData.dragInvert.y;

					// 마우스 델타값 할당
					Vector2 mouseDelta = new Vector2(xDelta, yDelta);

					// 마우스 이동거리합 갱신 (절대값 정정 후 갱신함)
					deltaDistance += new Vector2(Mathf.Abs(mouseDelta.x), Mathf.Abs(mouseDelta.y));	

					// 드래그 이동거리합 > (드래그 : 클릭 경계) * 0.8 배수일때 -> 드래그 시작
					if(deltaDistance.magnitude > defData.dragBoundary * 0.8f)
					{
						// 좌클릭 드래그 실행 btnIndex : 0
						// 우클릭 드래그 실행 btnIndex : 1
						m_InputEvents.dragEvent.Invoke(InputEventType.Input_drag, btnIndex, mouseDelta);
					}
				}
			}
			else if(Input.GetMouseButtonUp(btnIndex))
			{
				//Debug.Log($"moused delta magnitude : {deltaDistance.magnitude}");

				bool clickable = false;

				// 드래그 이동거리합 > (드래그 : 클릭 경계) * 1 배수 -> 클릭 이벤트
				if(deltaDistance.magnitude < defData.dragBoundary)
				{
					clickable = true;
				}
				// 드래그 실행함 : 클릭 실패 이벤트
				else
				{
					clickable = false;
				}

				//Debug.Log($"clickable : {clickable}, btnIndex : {btnIndex}");

				SetClickEvent(
					clickable ? InputEventType.Input_clickSuccessUp : InputEventType.Input_clickFailureUp,
					btnIndex,
					Input.mousePosition);

				// 초기화
				onClick = false;
				deltaDistance = default(Vector2);
			}
		}

		/// <summary>
		/// 스크롤 체크
		/// </summary>
		private void ScrollCheck()
		{
			scrollDelta = Input.mouseScrollDelta;
			if (scrollDelta.magnitude != 0)
			{
				// *** 포커스 이벤트 실행
				m_InputEvents.focusEvent.Invoke(InputEventType.Input_focus, Input.mousePosition, scrollDelta.y);
			}
		}

		#endregion

		private void SetClickEvent(InputEventType type, int btn, Vector3 pos)
		{
			//Debug.Log($"type : {type.ToString()}, pos : {pos}");
			m_InputEvents.clickEvent.Invoke(type, btn, pos);
		}
	}
}
