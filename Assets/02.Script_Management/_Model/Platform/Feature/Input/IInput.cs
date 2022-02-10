using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Platform.Feature._Input
{
	using Definition.Control;
	using Management.Events;

	/// <summary>
	/// 입력 추상클래스
	/// </summary>
	public abstract class IInput : MonoBehaviour
	{
		/// <summary>
		/// 주 관리자에서 받아온 이벤트 리스트
		/// </summary>
		[SerializeField] protected InputEvents m_InputEvents;

		#region 액션의 경우, 이를 실행해야할 메서드를 가지고 있는 파트에서 액션을 관리한다.
		///// <summary>
		///// 클릭 이벤트
		///// GameObject :: 클릭된 대상
		///// Definition.ObjectType :: 객체유형 (Object, UI)
		///// </summary>
		//[SerializeField] protected UnityAction<Definition.ObjectType, GameObject> clickEvent;

		///// <summary>
		///// 드래그 이벤트
		///// Vec2 :: 드래그 정도
		///// </summary>
		//[SerializeField] protected UnityAction<Vector2> dragEvent;

		///// <summary>
		///// 포커스 이벤트
		///// Vec2 :: 포커스 위치
		///// float :: 포커스 정도
		///// </summary>
		//[SerializeField] protected UnityAction<Vector2, float> focusEvent;

		///// <summary>
		///// 키 입력 이벤트
		///// </summary>
		//[SerializeField] protected UnityAction<string> keyEvent;
		#endregion

		/// <summary>
		/// 실행 직전에 받은 플랫폼 코드를 키반으로 초기화 수행
		/// </summary>
		public abstract bool OnStart(ref InputEvents inputEvents);

		/// <summary>
		/// OnStart에서 inputEvents로 가져온 정의된 입력데이터 정의
		/// </summary>
		/// <typeparam name="TValue"></typeparam>
		/// <param name="_data"></param>
		protected abstract void SetDefinition<TValue>(TValue _data) where TValue : class;
	}
}
