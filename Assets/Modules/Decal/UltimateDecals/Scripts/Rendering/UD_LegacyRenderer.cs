using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Bearroll.UltimateDecals {

	public class UD_LegacyRenderer: UD_Renderer {

		public List<AddedCommandBuffer> addedBuffers = new List<AddedCommandBuffer>();

		public UD_LegacyRenderer(UD_Camera camera): base(camera) {
		}

		protected void AddCommandBuffer(CameraEvent cameraEvent, CommandBuffer commandBuffer) {

			if (commandBuffer == null) return;

			addedBuffers.Add(new AddedCommandBuffer {
				commandBuffer = commandBuffer,
				cameraEvent = cameraEvent
			}); 

			camera.camera.AddCommandBuffer(cameraEvent, commandBuffer);

		}

		protected void RemoveBuffers() {

			foreach (var e in addedBuffers) {

				if (e.commandBuffer == null) continue;

				camera.camera.RemoveCommandBuffer(e.cameraEvent, e.commandBuffer);
			}

			addedBuffers.Clear();

		}

		protected override void OnFinalizeRebuild() {
			
			RemoveBuffers();

			SetBuffers();

		}

		protected override void OnInit() {

			RemoveBuffers();

			SetBuffers();

		}

		protected virtual void SetBuffers() {

		}

		protected override void OnBeforeClean() {

			RemoveBuffers();

		}

	}

}
