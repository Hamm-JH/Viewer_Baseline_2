using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Bearroll.UltimateDecals {

	public class UD_Component: MonoBehaviour {
		
		Transform cachedTransform;
		bool isTransformCached;

		GameObject cachedGO;
		bool isGoCached;

		public new Transform transform {
			get {
				if (!isTransformCached) {
					cachedTransform = base.transform;
					isTransformCached = true;
				}
				return cachedTransform; 
			}
		}

		public new GameObject gameObject {
			get {
				if (!isGoCached) {
					cachedGO = base.gameObject;
					isGoCached = true;
				}
				return cachedGO; 
			}
		}

	}

}