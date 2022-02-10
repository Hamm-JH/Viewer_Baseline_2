using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace _Archiving
{
	public class SubscribUnityAction : MonoBehaviour
	{
		public UnityAction subscribeAction;

		// Start is called before the first frame update
		void Start()
		{
			Set();
		}

		// Update is called once per frame
		void Update()
		{

		}

		public void Remove()
		{
			subscribeAction -= Method1;

		}

		public void Set()
		{
			subscribeAction += Method1;
			subscribeAction += Method2;
			//subscribeAction += Method3;
		}

		public void Method1()
		{
			Debug.Log("Method1");
		}

		public void Method2()
		{
			Debug.Log("Method2");
		}

		public void Method3()
		{
			Debug.Log("Method3");
		}
	}
}
