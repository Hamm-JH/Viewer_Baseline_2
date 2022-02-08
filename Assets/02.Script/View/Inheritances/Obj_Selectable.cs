using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace View
{
	public class Obj_Selectable : Interactable
	{
		public override GameObject Target
		{
			get => gameObject;
		}

		public override void OnDeselect()
		{
			Debug.Log($"OnDeselect : {this.name}");

			MeshRenderer render;
			if (gameObject.TryGetComponent<MeshRenderer>(out render))
			{
				render.material.SetColor("_BaseColor", Color.white);
			}
		}

		public override void OnSelect()
		{
			Debug.Log($"OnSelect : {this.name}");

			MeshRenderer render;
			if(gameObject.TryGetComponent<MeshRenderer>(out render))
			{
				render.material.SetColor("_BaseColor", Color.green);
			}
		}
	}
}
