using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace View
{
	public class Obj_3D1 : Interactable_3D
	{
		public override void OnDeselect()
		{
			Debug.Log($"OnDeselect : {this.name}");
		}

		public override void OnSelect()
		{
			Debug.Log($"OnSelect : {this.name}");
		}
	}
}
