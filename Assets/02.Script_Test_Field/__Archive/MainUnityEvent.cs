using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace _Archiving
{
	public class MainUnityEvent : MonoBehaviour
	{
		public UnityEvent mainEventHandler;

		public SubscribUnityAction subscriber;

		// Start is called before the first frame update
		void Start()
		{
			mainEventHandler.AddListener(subscriber.subscribeAction);
		}

		// Update is called once per frame
		void Update()
		{
			// Invoke action
			if (Input.GetKeyDown(KeyCode.Space))
			{
				mainEventHandler.Invoke();
			}
			// all remove listener
			else if (Input.GetKeyDown(KeyCode.A))
			{
				mainEventHandler.RemoveAllListeners();
			}
			// set listener
			else if (Input.GetKeyDown(KeyCode.S))
			{
				mainEventHandler.AddListener(subscriber.subscribeAction);
			}
			// remove action
			else if (Input.GetKeyDown(KeyCode.D))
			{
				Debug.Log("D");
				subscriber.Remove();
			}
			// set action
			else if (Input.GetKeyDown(KeyCode.F))
			{
				subscriber.Set();
			}


		}
	}
}
