using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Legacy.UI
{
	public class OffsetFitter : MonoBehaviour
	{
		public bool isReplaced;
		public Vector3 offset;

		public void Init(Vector3 _offset)
		{
			isReplaced = false;
			offset = _offset;
		}

		private void OnTriggerStay(Collider other)
		{
			if(!isReplaced)
			{
				if(isRealDimText(other))
				{
					isReplaced = true;

					other.gameObject.transform.localPosition = new Vector3(
						offset.x,
						offset.y,
						offset.z
						);

					transform.localPosition = new Vector3(
						transform.localPosition.x + offset.x,
						transform.localPosition.y + offset.y,
						transform.localPosition.z + offset.z
						);
					
					Destroy(gameObject.GetComponent<BoxCollider>());
					Destroy(gameObject.GetComponent<Rigidbody>());
				}

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
