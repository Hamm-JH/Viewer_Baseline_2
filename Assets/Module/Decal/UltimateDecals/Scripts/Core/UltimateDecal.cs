using System.Collections.Generic;
using UnityEngine;

namespace Bearroll.UltimateDecals {

	[ExecuteInEditMode]
	public class UltimateDecal: UD_Component {

		public UltimateDecalRenderingMode renderingMode;

		public bool controlObjectName = true;
		public UltimateDecalType type = UltimateDecalType.Static;
		[Range(-20,20)]
		public int order;
		public int atlasIndex;
		public Material material;

		void Reset() {

			if (material == null) {
				material = Resources.Load<Material>("UD_Default");
			}
		}

		void OnEnable() {
			
			UD_Manager.AddDecal(this);

			OnValidate();

		}

		void OnValidate() {

			//if (controlObjectName && material && gameObject.scene.IsValid()) {
			//    gameObject.name = material.name;
			//}
		}

		void OnDisable() { 

			if (type == UltimateDecalType.PermanentMark) return;

			UD_Manager.RemoveDecal(this);

		}

		public void OnDrawGizmos() {

			#if UNITY_EDITOR
			Gizmos.color = UnityEditor.Selection.Contains(gameObject) ? Color.yellow : Color.yellow * 0.33f;
			#endif

			Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);

			Gizmos.DrawWireCube(Vector3.zero, transform.lossyScale);
		}

	}

}