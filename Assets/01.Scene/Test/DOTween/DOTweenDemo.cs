using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test
{
	using DG.Tweening;

	public class DOTweenDemo : MonoBehaviour
	{
		public GameObject m_target;

		public Transform m_from;
		public Transform m_to;

		public Transform m_rFrom;
		public Transform m_rTo;

		// Start is called before the first frame update
		void Start()
		{
			DOTween.Init();
			// Move
			//m_target.transform.position = m_from.position;
			//m_target.transform.DOMove(m_to.position, 2);

			// Rotate
			//m_target.transform.rotation = m_rTo.rotation;
			//m_target.transform.DORotateQuaternion(m_rFrom.rotation, 2);

			
		}

		// Update is called once per frame
		void Update()
		{

		}
	}
}

