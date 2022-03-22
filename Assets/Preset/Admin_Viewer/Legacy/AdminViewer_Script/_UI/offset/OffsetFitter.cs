using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
	public class OffsetFitter : MonoBehaviour
	{
		public bool isReplaced;
		public Vector3 offset;

		public class distValue
		{
			public bool isUp;
			public float distance;
		}

		distValue upValue;
		distValue downValue;

		public void Init(Vector3 _offset)
		{
			isReplaced = false;
			offset = _offset;

			upValue = null;
			downValue = null;
		}

		private void OnTriggerEnter(Collider other)
		{
			
		}

		private void OnTriggerExit(Collider other)
		{
			isReplaced = true;

			//Destroy(gameObject.GetComponent<BoxCollider>());
			//Destroy(gameObject.GetComponent<Rigidbody>());
		}

		private void OnTriggerStay(Collider other)
		{
			if(isRealDimText(other))
			{
				if(!isReplaced)
				{
					isReplaced = true;

					transform.localPosition = new Vector3(
						2 * offset.x,
						2 * offset.y,
						2 * offset.z
						);
				}
				else
				{
					float localDist = Vector3.Distance(Vector3.zero, other.transform.localPosition);
					float offsetDist = Vector3.Distance(Vector3.zero, offset) * 3 / 2;

					// 1차적으로 아래의 코드를 실행한다.
						// 각 포지션 변경시의 객체간의 WORLDPOS 거리값을 구해둔다.
					// 두 거리값이 0이 아닐때 예외연산을 실행한다.
						// 거리값, 상하 조건값 

					if (upValue != null && downValue != null)
					{
						// 위 값이 아래 값보다 멀리 있는 경우
						if(upValue.distance > downValue.distance)
						{
							//아래 값이 가까우므로 위로 올린다.
							transform.localPosition = new Vector3(
								2 * offset.x,
								2 * offset.y,
								2 * offset.z
								);
						}
						// 위 값이 아래 값보다 가까이 있는 경우
						else
						{
							// 위 값이 가까우므로 아래로 내린다.
							transform.localPosition = new Vector3(
								offset.x,
								offset.y,
								offset.z
								);
						}

						Destroy(gameObject.GetComponent<BoxCollider>());
						Destroy(gameObject.GetComponent<Rigidbody>());

						return;
					}

					if(localDist > offsetDist)
					{
						upValue = new distValue();
						upValue.isUp = true;
						upValue.distance = Vector3.Distance(transform.position, other.transform.position);

						// 현재 객체를 아래로 내린다 -> 두 객체 모두 위에 있다.
						transform.localPosition = new Vector3(
							offset.x,
							offset.y,
							offset.z
							);
					}
					else
					{
						downValue = new distValue();
						downValue.isUp = false;
						downValue.distance = Vector3.Distance(transform.position, other.transform.position);

						// 현재 객체를 위로 올린다 -> 두 객체 모두 아래에 있다.
						transform.localPosition = new Vector3(
							2 * offset.x,
							2 * offset.y,
							2 * offset.z
							);
					}

					//if (other.GetComponent<OffsetFitter>().isReplaced)
					//{
					//	transform.localPosition = new Vector3(
					//		offset.x,
					//		offset.y,
					//		offset.z
					//		);
					//}
				}


				//// 상대 객체도 위치변경되지 않은 경우
				//if(!other.GetComponent<OffsetFitter>().isReplaced)
				//{
				//	isReplaced = true;

				//	// 서로의 위치 차이를 둔다
				//	// 상대 객체는 기본 위치
				//	// 현재 객체는 변환 위치
				//	//other.gameObject.transform.localPosition = new Vector3(
				//	//	offset.x,
				//	//	offset.y,
				//	//	offset.z
				//	//	);

				//	transform.localPosition = new Vector3(
				//		2 * offset.x,
				//		2 * offset.y,
				//		2 * offset.z
				//		);

				//	//Destroy(gameObject.GetComponent<BoxCollider>());
				//	//Destroy(gameObject.GetComponent<Rigidbody>());
				//}
				//// 상대 객체가 위치변경된 경우
				//else
				//{
				//	float localDist = Vector3.Distance(Vector3.zero, transform.localPosition);
				//	float offsetDist = Vector3.Distance(Vector3.zero, offset) * 3 / 2;

				//	if(localDist > offsetDist)
				//	{
				//		transform.localPosition = new Vector3(
				//			offset.x,
				//			offset.y,
				//			offset.z
				//			);
				//	}
				//	else
				//	{
				//		transform.localPosition = new Vector3(
				//			2 * offset.x,
				//			2 * offset.y,
				//			2 * offset.z
				//			);
				//	}

				//	//Destroy(gameObject.GetComponent<BoxCollider>());
				//	//Destroy(gameObject.GetComponent<Rigidbody>());
				//}
			}
		}

		private bool isRealDimText(Collider other)
		{
			bool result = false;

			int layer = other.gameObject.layer;

			if(layer >= 13 && layer <= 18)
			{
				result = true;
			}

			return result;
		}
	}
}
