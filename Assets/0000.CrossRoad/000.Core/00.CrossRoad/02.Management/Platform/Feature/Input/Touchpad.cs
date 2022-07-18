using Definition;
using Management.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platform.Feature._Input
{
    public partial class Touchpad : IInput
    {
        public bool isDebug = false;
        public TMPro.TextMeshProUGUI debugText;

        #region Pre-defined 사전 정의

        [System.Serializable]
        public class Data
        {
            // 클릭


            // 드래그
            /// <summary>
            /// 드래그 경계값
            /// </summary>
            public float dragBoundary = 5f;

            public Vector2 dragInvert = new Vector2(1, -1);

            // 포커스
            public float focusBoundary = 10f;
        }

        [SerializeField] Data defData;

        #endregion

        #region fields

        /// <summary>
        /// 터치 클릭 확인변수
        /// </summary>
        bool onClick = false;

        /// <summary>
        /// 터치 드래그 거리합
        /// </summary>
        Vector2 deltaDistance = default(Vector2);

        /// <summary>
        /// 터치 위치
        /// </summary>
        //Vector3 touchStartPosition = default(Vector3);

        bool onMulti = false;

        Vector2 prev1Pos = default(Vector2);
        Vector2 prev2Pos = default(Vector2);
        Vector2 curr1Pos = default(Vector2);
        Vector2 curr2Pos = default(Vector2);

        #endregion

        private void Start()
        {
            // 4개 이벤트 할당
            //m_InputEvents.clickEvent
            //m_InputEvents.dragEvent
            //m_InputEvents.focusEvent
            //m_InputEvents.keyEvent
        }

        private void Update()
        {
            TouchpadCheck();
        }

        /// <summary>
        /// 터치패드 입력 업데이트
        /// </summary>
        private void TouchpadCheck()
        {
            //DebugTouches();

            //TouchPhase.Began  // 터치 시작
            //TouchPhase.Moved  // 터치 이동
            //TouchPhase.Stationary // 터치 위치에서 대기중
            //TouchPhase.Ended  // 터치 종료
            //TouchPhase.Canceled   // 터치 취소됨.

            // 클릭 이벤트 :: 터치 시작 -> 터치 종료시 실행
            // 조건
                // 터치 시작 ~ 터치 종료까지 이동한 거리의 비율이 일정 이하일때 터치가 완료했음을
            if(Input.touchCount != 0)
            {
                Touch[] touches = Input.touches;

                // 1개의 터치 발생
                if(touches.Length == 1)
                {
                    GetSingleTouch(touches[0]);
                }
                // 2개 이상의 터치 발생
                else if(touches.Length >= 2)
                {
                    GetMultiTouch(touches);
                }
            }
        }

        /// <summary>
        /// 터치 디버깅
        /// </summary>
        private void DebugTouches()
        {
            string str = "";

            if (Input.touchCount != 0)
            {
                Touch[] touches = Input.touches;

                int index = touches.Length;
                for (int i = 0; i < index; i++)
                {

                    str += $"touch {i} : {touches[i].phase.ToString()}\n";
                }

                //Debug.Log(str);
                debugText.text = str;
            }
            else
            {
                debugText.text = "";
            }
        }

        /// <summary>
        /// 단일 터치 
        /// </summary>
        /// <param name="_touch">터치 데이터</param>
        private void GetSingleTouch(Touch _touch)
        {
            if(_touch.phase == TouchPhase.Began)
            {
                ResetSingleTouch(true);

                //touchStartPosition = _touch.position;
                //debugText.text = $"single touch began : {touchStartPosition}";

                OnDebug($"single touch began : {_touch.position}");
                //Debug.Log($"single touch began : {_touch.position}");

                SetClickEvent(InputEventType.Input_clickDown, 0, _touch.position);
            }
            else if(_touch.phase == TouchPhase.Moved)
            {
                //debugText.text = $"touch delta : {_touch.deltaPosition}";
                // 틱당 이동거리 계산
                Vector2 delta = _touch.deltaPosition / 100;
                delta = new Vector2(
                    delta.x * defData.dragInvert.x,
                    delta.y * defData.dragInvert.y
                    );

                OnDebug($"touch delta : {delta}");

                // TODO :: CHECK :: 터치패드 : 델타 값에 대한 보간이 필요하다.
                // 이동거리 합산
                deltaDistance += new Vector2(Mathf.Abs(delta.x), Mathf.Abs(delta.y));

                // 드래그 이동거리 > (드래그 : 클릭 경계) * 0.8배수일때 -> 드래그 시작
                if(deltaDistance.magnitude > defData.dragBoundary * 0.8f)
                {
                    SetDragEvent(InputEventType.Input_drag, 0, delta);
                }
            }
            else if(_touch.phase == TouchPhase.Stationary)
            {
                // Stationary 상태는 이동 값이 없는 경우 발생한다.
                //debugText.text = $"touch delta : {_touch.deltaPosition}";
            }
            else if(_touch.phase == TouchPhase.Ended)
            {
                OnDebug($"total delta distance : {Vector2.Distance(Vector2.zero, deltaDistance)}");
                //debugText.text = $"total delta distance : {Vector2.Distance(Vector2.zero, deltaDistance)}";

                // 총 이동거리
                float totalDelta = Vector2.Distance(Vector2.zero, deltaDistance);

                if(onClick == true && totalDelta < defData.dragBoundary)
                {
                    OnDebug($"onClick true // defData.dragBoundary < totalDelta true\n totalDelta : {totalDelta}");
                    //debugText.text = $"onClick true // defData.dragBoundary < totalDelta true\n totalDelta : {totalDelta}";

                    SetClickEvent(InputEventType.Input_clickSuccessUp, 0, _touch.position);
                }
                else
                {
                    SetClickEvent(InputEventType.Input_clickFailureUp, 0, _touch.position);
                }

                ResetSingleTouch(false);
            }
            else if(_touch.phase == TouchPhase.Canceled)
            {
                SetClickEvent(InputEventType.Input_clickFailureUp, 0, _touch.position);
                ResetSingleTouch(false);
            }
        }

        /// <summary>
        /// 멀티 터치
        /// </summary>
        /// <param name="_touches">터치 데이터 배열</param>
        private void GetMultiTouch(Touch[] _touches)
        {
            // 일단 싱글터치 이벤트 막고보기
            if(onClick == true)
            {
                SetClickEvent(InputEventType.Input_clickFailureUp, 0, Vector3.zero);
                ResetSingleTouch(false);
            }

            // 초기 진입시
            if(!onMulti)
            {
                // 멀티상태 세팅
                _SetOnMulti(true);

                InMulti_GetTouchPos(_touches, 0, out prev1Pos);
                InMulti_GetTouchPos(_touches, 1, out prev2Pos);
            }
            // 초기 이후의 진입시
            else
            {
                // 특정 터치 단계가 종료되었을 경우
                if(InMulti_IsTouchEndPhase(_touches))
                {
                    // 다음 루프에서 멀티 터치 단계가 종료되므로 onMulti 변경
                    _SetOnMulti(false);
                }
                // 멀티 터치 지속 단계
                else
                {
                    bool c1 = InMulti_GetTouchPos(_touches, 0, out curr1Pos);
                    bool c2 = InMulti_GetTouchPos(_touches, 1, out curr2Pos);

                    // prev 단계의 거리 구하기
                    // curr 단계의 거리 구하기
                    // 두 단계 사이의 차 구하기
                    float prevDistance = Vector2.Distance(prev1Pos, prev2Pos);
                    float currDistance = Vector2.Distance(curr1Pos, curr2Pos);
                    float diffDelta = (currDistance - prevDistance) / 1000;

                    Vector2 center = (curr1Pos + curr2Pos) / 2;

                    if(c1)
                    {
                        prev1Pos = curr1Pos;
                    }
                    if(c2)
                    {
                        prev2Pos = curr2Pos;
                    }

                    
                    if(diffDelta > defData.focusBoundary)
                    {
                        SetFocusEvent(InputEventType.Input_focus, center, diffDelta);
                    }
                }
            }
        }

        /// <summary>
        /// 멀티 터치 상황에서 터치 위치 수집
        /// </summary>
        /// <param name="_touches">터치 배열</param>
        /// <param name="_tIndex">검출 대상 터치번호</param>
        /// <param name="_tPos">out : 터치 위치</param>
        /// <returns>true : 터치 위치 수집 완료</returns>
        private bool InMulti_GetTouchPos(Touch[] _touches, int _tIndex, out Vector2 _tPos)
        {
            bool result = false;
            _tPos = Vector2.zero;

            if(_touches[_tIndex].phase == TouchPhase.Began)
            {
                result = true;
                _tPos = _touches[_tIndex].position;
            }
            else if(_touches[_tIndex].phase == TouchPhase.Moved)
            {
                result = true;
                _tPos = _touches[_tIndex].position;
            }

            return result;
        }

        /// <summary>
        /// 디버깅 시작
        /// </summary>
        /// <param name="value">변수</param>
        private void OnDebug(string value)
        {
            if(isDebug)
            {
                debugText.text = value;
            }
        }

        #region Single Touch

        /// <summary>
        /// 단일 터치 리셋
        /// </summary>
        /// <param name="isOn">true : 참</param>
        private void ResetSingleTouch(bool isOn)
        {
            _SetOnClick(isOn);
            _ResetDeltaDistance();
        }

        /// <summary>
        /// 클릭 상태 리셋시 초기화 변수 할당
        /// </summary>
        /// <param name="_isActive">초기화 변수</param>
        private void _SetOnClick(bool _isActive)
        {
            onClick = _isActive;
        }

        /// <summary>
        /// 터치패드 확장 거리변수 초기화
        /// </summary>
        private void _ResetDeltaDistance()
        {
            deltaDistance = default(Vector2);
        }

        #endregion

        #region Multiple Touch

        /// <summary>
        /// 멀티 터치 세팅 변경
        /// </summary>
        /// <param name="_isActive">true : 활성화</param>
        private void _SetOnMulti(bool _isActive)
        {
            onMulti = _isActive;
        }

        /// <summary>
        /// 멀티 터치 종료단계
        /// </summary>
        /// <param name="_touches">터치 배열</param>
        /// <returns>true : 참</returns>
        private bool InMulti_IsTouchEndPhase(Touch[] _touches)
        {
            bool result = false;

            if(_touches.Length >= 2)
            {
                if(_touches[0].phase == TouchPhase.Ended || _touches[0].phase == TouchPhase.Canceled
                    || _touches[1].phase == TouchPhase.Ended || _touches[1].phase == TouchPhase.Canceled)
                {
                    result = true;
                }
            }

            return result;
        }

        #endregion

        /// <summary>
        /// 터치 클릭 이벤트
        /// </summary>
        /// <param name="_type"></param>
        /// <param name="_btn"></param>
        /// <param name="_pos"></param>
        private void SetClickEvent(InputEventType _type, int _btn, Vector3 _pos)
        {
            //OnDebug($"OnClick event Invoked : {_type.ToString()} \n" +
            //    $"Onclick Btn index : {_btn}\n" +
            //    $"Onclick position : {_pos}");

            m_InputEvents.clickEvent.Invoke(_type, _btn, _pos);
        }

        /// <summary>
        /// 터치 드래그 이벤트
        /// </summary>
        /// <param name="_type">입력 이벤트 타입</param>
        /// <param name="_btnIndex">버튼 인덱스</param>
        /// <param name="_delta">드래그 정도</param>
        private void SetDragEvent(InputEventType _type, int _btnIndex, Vector2 _delta)
        {
            //OnDebug($"OnDrag event Invoked : {_type.ToString()} \n" +
            //    $"OnDrag Btn index : {_btnIndex}\n" +
            //    $"OnDrag Delta : {_delta}");

            m_InputEvents.dragEvent.Invoke(_type, _btnIndex, _delta);
        }

        /// <summary>
        /// 터치 포커스 이벤트
        /// </summary>
        /// <param name="_type">입력 이벤트 타입</param>
        /// <param name="_tPos">포커스 중심점</param>
        /// <param name="_tDelta">포커스 정도</param>
        private void SetFocusEvent(InputEventType _type, Vector2 _tPos, float _tDelta)
        {
            //OnDebug($"OnFocus event Invoked : {_type.ToString()}\n" +
            //    $"OnFocus touch position : {_tPos}\n" +
            //    $"OnFocus delta : {_tDelta}");

            m_InputEvents.focusEvent.Invoke(_type, _tPos, _tDelta);
        }
    }
}
