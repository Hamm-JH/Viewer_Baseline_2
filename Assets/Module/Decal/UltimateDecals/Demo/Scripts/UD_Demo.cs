using System;
using Bearroll.UltimateDecals;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bearroll.UltimateDecals_Demo {

	public class UD_Demo: MonoBehaviour {

		public AudioSource audioSource;
		public Texture2D crosshair;
		public Material decalMaterial;
		public UD_FPSController fpsController;

		public Texture logoTex;
		public bool showHint = true;

		public KeyCode toggleKey = KeyCode.F1;
		public KeyCode toggleHintKey = KeyCode.F2;
		public KeyCode togglePathKey = KeyCode.F3;

		new public UD_Camera camera;
		public UD_Manager manager;

		LayerMask layerMask = ~(1 << 2);
		float nextShoot;
		bool locked = true;
		GUIStyle hintStyle = new GUIStyle();
		float lastTime;
		int lastFrameCount;
		float nextStringUpdate;
		float renderTime;
		string s;
		float touchLock;

		void Start() {

			lastTime = Time.time;
			lastFrameCount = Time.frameCount;

		}

		void Shoot() {
		
			audioSource.Play();

			RaycastHit hit;

			var pos = Vector3.one * 0.5f;

			var camera = Camera.main;

			var ray = camera.ViewportPointToRay(pos);

			if (!Physics.Raycast(ray, out hit, 1000, layerMask)) return;

			var mask = manager.layerMask;
			var t = hit.collider.gameObject;

			if (mask == (mask | (1 << t.layer))) {

				var dir = Vector3.Lerp(hit.normal, (camera.transform.position - hit.point).normalized, 0.5f);
				var r = Quaternion.LookRotation(dir) * Quaternion.Euler(90, 0, 0) * Quaternion.Euler(0, UnityEngine.Random.Range(0,360f), 0);
				var m = Matrix4x4.TRS(hit.point, r, new Vector3(0.5f, 0.5f, 0.5f));

				UD_Manager.AddPermanentMark(decalMaterial, m);

			}

		}

		void UpdateString() {

			var frameCount = Time.frameCount - lastFrameCount;
			lastFrameCount = Time.frameCount;

			if (frameCount < 1) frameCount = 1;

			renderTime = Mathf.Lerp(renderTime, Time.time - lastTime, 0.8f) / frameCount;
			lastTime = Time.time;

			var fps = (renderTime > 0) ? Mathf.RoundToInt(1 / renderTime) : 0;

			#if UD_URP || UD_LWRP
			var format = "<b>Ultimate Decals</b>\n{0}\n\n[{1}] Toggle decals\n[{2}] Toggle info\n[Tab] Lock/unlock cursor\n\nTime: {4:0.0} ms\nFPS: {5}";
			#else
			var format = "<b>Ultimate Decals</b>\n{0}\n\n[{1}] Toggle decals\n[{2}] Toggle info\n[{3}] Forward/Deferred\n[Tab] Lock/unlock cursor\n\nTime: {4:0.0} ms\nFPS: {5}";
			#endif

			var rendererName = camera.decalRenderer != null ? camera.decalRenderer.name : "Disabled";

			s = string.Format(format, rendererName, toggleKey, toggleHintKey, togglePathKey, renderTime * 1000, fps);

		}

		void Update() {

			if (Time.time > nextStringUpdate) {
				UpdateString();
				nextStringUpdate = Time.time + 1f;
			}

			if (Input.GetKeyUp(toggleKey)) {
				manager.enabled = !manager.enabled;
				UpdateString();
			}

			if (Input.GetKeyUp(toggleHintKey)) {
				showHint = !showHint;
				UpdateString();
			}

			if (Input.GetKeyUp(togglePathKey)) {
				camera.camera.renderingPath = camera.isDeferred ? RenderingPath.Forward : RenderingPath.DeferredShading;
			}

			if (Input.GetKeyUp(KeyCode.Tab)) {
				locked = !locked;
			}

			if (fpsController != null) {

				Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;

				fpsController.enabled = locked;

				if (!locked) return;

			}

			if (decalMaterial != null) {

				if (Input.GetKey(KeyCode.Mouse0) && Time.time >= nextShoot) {
					Shoot();
					nextShoot = Time.time + 0.1f;
				}

			}

		}

		void OnGUI() {

			if (!showHint) return;

			var s = 40;

			if (crosshair != null) {
				var rect = new Rect(Screen.width / 2 - s / 2, Screen.height / 2 - s / 2, s, s);
				GUI.DrawTexture(rect, crosshair);
			}

			hintStyle.normal.textColor = Color.white * 0.9f;
			hintStyle.fontSize = (int) (24f * (Camera.main.pixelHeight / 1080f));

			GUI.DrawTexture(new Rect(32, 32, 64, 64), logoTex);

			GUI.Label(new Rect(30, 110, 400, 200), this.s, hintStyle);

			#if MOBILE_INPUT
			if (touchLock < Time.time && Input.touchCount > 1) {
				manager.enabled = !manager.enabled;
				UpdateString();
				touchLock = Time.time + 0.5f;
			}
			#endif

		}

	}

	[Serializable]
	public class UD_DemoMode {
		public Material skybox;
		public GameObject gameObject;
		public float ambientIntensity;
	}

}
