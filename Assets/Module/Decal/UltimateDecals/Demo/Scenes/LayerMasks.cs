using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Snippets
{
	public class LayerMasks : MonoBehaviour
	{
		// Start is called before the first frame update
		void Start()
		{
			this.gameObject.layer = 0;

			//Camera.main.cullingMask = 1 << 2 | 1 << 1;
			Camera.main.cullingMask = -1 & ~(1 << 6);
		}

		// Update is called once per frame
		void Update()
		{

		}
	}
}
