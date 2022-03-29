using Management;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items.UIPin
{
	public class UIPin : MonoBehaviour
	{
		public Transform m_targetObject;

		private void Update()
		{
			if(m_targetObject != null)
			{
				Bounds _b = ContentManager.Instance._Model.CenterBounds;
				Camera _cam = MainManager.Instance.MainCamera;

				transform.position = _cam.WorldToScreenPoint(m_targetObject.position);
			}
		}
	}
}
