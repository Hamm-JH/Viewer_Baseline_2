using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View;

namespace Management.Events
{
	public class EventData
	{
		public IInteractable target;

		public EventData(IInteractable _target)
		{
			target = _target;
		}

		public static bool IsEqual(EventData A, EventData B)
		{
			if(A.target.Target == B.target.Target)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}
