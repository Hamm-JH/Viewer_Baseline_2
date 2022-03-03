#if UNITY_EDITOR

using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Bearroll.UltimateDecals {

	public partial class UD_Manager {

		public bool drawHandles = true;

		static void DrawDecalHandle(SceneView sceneView, UltimateDecal decal) {

			var size = 1.4f; //decal.transform.lossyScale.magnitude * 0.2f;
			var rot = decal.transform.rotation * Quaternion.Euler(-90, 0, 0);

			if (Event.current.control || Selection.activeGameObject != decal.gameObject || Selection.objects.Length > 1) {

				if (Handles.Button(decal.transform.position, rot, size, size, Handles.ArrowHandleCap)) {

					if (Event.current.control) {

						if (Selection.Contains(decal.gameObject)) {

							Selection.objects = Selection.objects.Where(se => se != decal.gameObject).ToArray();

						} else {

							var array = new UnityEngine.Object[] {decal.gameObject};
							Selection.objects = Selection.objects.Concat(array).ToArray();

						}
							 
					} else {

						Selection.activeGameObject = decal.gameObject;

					}

				}

			}

		}

		public static void DrawDecalHandles(SceneView sceneView) {

			if (instance == null) return;

			if (sceneView.camera.GetComponent<UD_Camera>() == null) {

				sceneView.camera.gameObject.AddComponent<UD_Camera>();

			}

            var manager = instance;

            if (instance.drawHandles)
            {

                Handles.color = Color.yellow;

                foreach (var e in manager.passByDecal)
                {

                    var decal = e.Key;

                    if (decal == null) continue;

                    DrawDecalHandle(sceneView, decal);

                }

            }

            var ev = Event.current;

			if (Selection.activeGameObject != null && Selection.objects.Length == 1 && ev.shift && !ev.control) {

				if (ev.type == EventType.Layout) {
					HandleUtility.AddDefaultControl(0);
				}

				var sample = Selection.activeGameObject.GetComponent<UltimateDecal>();

				if (sample != null) {

					if (ev.type == EventType.MouseUp) {

						CreateDecal(sample, sceneView);

						//ev.control = false;
						//ev.shift = false;
						ev.type = EventType.Ignore;
						HandleUtility.AddDefaultControl(0);

					}

				}

			}

			

		}

		[MenuItem("GameObject/3D Object/Ultimate Decal")]
		public static void CreateDecal() {
			CreateDecal(null, null);
		}

		public static void CreateDecal(UltimateDecal sample, SceneView sceneView) {

			var gameObject = new GameObject("Decal", typeof(UltimateDecal));

			var t = Selection.activeGameObject;

			if (t != null) {
				gameObject.transform.SetParent(t.transform, true);
			}

			Selection.activeGameObject = gameObject;

			var c = sceneView != null ? sceneView.camera : (SceneView.currentDrawingSceneView != null ? SceneView.currentDrawingSceneView.camera : null);

			if (c != null) {

				Ray ray;
				RaycastHit hit;

				if (sceneView != null) {
					var mousePosition = Event.current.mousePosition;
					mousePosition.y = c.pixelHeight - mousePosition.y;
					ray = c.ScreenPointToRay(mousePosition);
				} else {
					ray = c.ViewportPointToRay(Vector3.one * 0.5f);
				}

				if (Physics.Raycast(ray, out hit, 500)) {
					gameObject.transform.position = hit.point;
					gameObject.transform.up = hit.normal;
				} else {
					gameObject.transform.position = c.transform.position + c.transform.forward * 10;
				}

			}

			var decal = gameObject.GetComponent<UltimateDecal>();

			if (sample != null) {
				decal.type = sample.type;
				decal.name = sample.name;
				decal.transform.parent = sample.transform.parent;
				decal.material = sample.material;
				decal.transform.localScale = sample.transform.localScale;
				decal.atlasIndex = sample.atlasIndex;
			} else {
				decal.transform.localScale = Vector3.one * 3;
			}

			UpdateDecal(decal);

		}

		

	}

}

#endif