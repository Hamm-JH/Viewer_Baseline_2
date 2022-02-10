using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace Bearroll.UltimateDecals {

	public class ResourceManager {

		static Mesh instanceMeshCached;
		static Mesh quadCached;
		
		static public Mesh instanceMesh {
			get {

				if (instanceMeshCached == null) {
					instanceMeshCached = CreateDecalMesh();
				}

				return instanceMeshCached;
			}
		}
		
		static public void ToggleKeyword(string name, bool toggle) {

			if (Shader.IsKeywordEnabled(name) == toggle) return;

			if (toggle) {
				Shader.EnableKeyword(name);
			} else {
				Shader.DisableKeyword(name);
			}

		}

		static public void ToggleKeyword(Material material, string name, bool toggle) {

			if (material.IsKeywordEnabled(name) == toggle) return;

			if (toggle) {
				material.EnableKeyword(name);
			} else {
				material.DisableKeyword(name);
			}

		}
		
		static public Mesh quadMesh {
			get {

				if (quadCached == null) {

					var quad = GameObject.CreatePrimitive(PrimitiveType.Quad);

					quadCached = quad.GetComponent<MeshFilter>().sharedMesh;

					Object.DestroyImmediate(quad);
				}

				return quadCached;
			}
		}

		static public Cubemap defaultReflections {
			get { return ReflectionProbe.defaultTexture as Cubemap; }
		}

		public static Mesh CreateDecalMesh() {

			var cubeX = 0.5f;
			var cubeY = 0.5f;
			var cubeZ = 0.5f;
   
			var p1 = new Vector3(cubeX-1,cubeY-1,cubeZ-1);
			var p2 = new Vector3(cubeX,cubeY-1,cubeZ-1);
			var p3 = new Vector3(cubeX-1,cubeY,cubeZ-1); 
			var p4 = new Vector3(cubeX-1,cubeY-1,cubeZ);
			var p5 = new Vector3(cubeX,cubeY-1,cubeZ);
			var p6 = new Vector3(cubeX,cubeY,cubeZ-1); 
			var p7 = new Vector3(cubeX-1,cubeY,cubeZ); 
			var p8 = new Vector3(cubeX,cubeY,cubeZ); 

			var mesh = new Mesh();

			mesh.vertices = new [] {p1, p2, p3, p4, p5, p6, p7, p8}; 

			mesh.triangles = new [] {
				0, 2, 1, 1, 2, 5,
				3, 0, 1, 1, 4, 3,
				0, 3, 2, 2, 3, 6,
				1, 5, 4, 5, 7, 4,
				6, 3, 4, 6, 4, 7,
				6, 5, 2, 7, 5, 6
			};

			mesh.UploadMeshData(true);

			return mesh;

		}

		static void DestroyResource(UnityEngine.Object e) {

			if (e == null) return;

			if (Application.isPlaying) {
				UnityEngine.Object.Destroy(e);
			} else {
				UnityEngine.Object.DestroyImmediate(e);
			}

		}

	}

}