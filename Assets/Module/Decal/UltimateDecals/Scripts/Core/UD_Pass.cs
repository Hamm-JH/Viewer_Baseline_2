using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Bearroll.UltimateDecals {

	public class UD_Pass {

		public UltimateDecalType type { get; private set; }

		public UD_Manager manager {
			get {
				return UD_Manager.instance;
			}
		}

		List<UD_Batch> batches = new List<UD_Batch>(16);
		Dictionary<UltimateDecal, UD_Batch> batchByDecal = new Dictionary<UltimateDecal, UD_Batch>(16);

		public int batchCount {
			get { return batches.Count; }
		}

		public UD_Pass(UltimateDecalType type, UD_Manager manager) {
			this.type = type;
		}

		public int decalCount {
			get {
				var count = 0;
				foreach (var batch in batches) {
					count += batch.count;
				}
				return count;
			}
		}

		public void RebuildCommandBuffer(UD_Camera camera, CommandBuffer commandBuffer) {

			commandBuffer.Clear();

			camera.decalRenderer.StartPassLitBuffer(commandBuffer);

			foreach (var batch in batches) {

				if (batch.count == 0) continue;

				if (batch.material == null) continue;

				if (!batch.isLit) continue;

				commandBuffer.SetGlobalVectorArray("_Size", batch.sizes);
				commandBuffer.SetGlobalFloatArray("_AtlasIndexOffset", batch.atlasIndices);
				commandBuffer.SetGlobalFloatArray("_CreateTime", batch.ctimes);

				#if UD_LWRP || UD_URP
				var pass = 1;
				#else
				var pass = camera.isForward ? 0 : 2;
				#endif

				commandBuffer.DrawMeshInstanced(ResourceManager.instanceMesh, 0, batch.material, pass, batch.matrices, batch.count, batch.props);

			}

		}

		public void RebuildUnlitCommandBuffer(UD_Camera camera, CommandBuffer commandBuffer) {

			commandBuffer.Clear();

			camera.decalRenderer.StartPassUnlitBuffer(commandBuffer);

			foreach (var batch in batches) { 

				if (batch.count == 0) continue;

				if (batch.material == null) continue;

				if (batch.isLit) continue;
				
				commandBuffer.SetGlobalVectorArray("_Size", batch.sizes);
				commandBuffer.SetGlobalFloatArray("_AtlasIndexOffset", batch.atlasIndices);
				commandBuffer.SetGlobalFloatArray("_CreateTime", batch.ctimes);

				commandBuffer.DrawMeshInstanced(ResourceManager.instanceMesh, 0, batch.material, 0, batch.matrices, batch.count);

			}

		}

		public UD_Batch GetBatchByMaterial(Material material, int renderQueueOffset = 0) {

			UD_Batch first = null;
			UD_Batch result = null;
			var limit = manager.maxPermanentMarks;
			var sortingOrder = material.renderQueue + renderQueueOffset;

			for (var i = 0; i < batchCount; i++) {

				var e = batches[i];

				if (e.material != material) continue;

				if (e.sortingOrder != sortingOrder) {

					if (e.count > 0) continue;

					e.sortingOrder = sortingOrder;

					result = e;

					batches.RemoveAt(i);

					break;

				}

				if (first == null) first = e;

				if (e.isFull) {

					if (type != UltimateDecalType.PermanentMark) continue;

					limit -= e.count;

					if (limit > 0) continue;

					if (first != null) return first;

					break;

				}

				return e;

			}

			var index = 0;

			while (index < batchCount && batches[index].sortingOrder < sortingOrder) {
				index++;
			}

			if (result == null) {
				result = new UD_Batch(manager, type, material, sortingOrder);
			}

			batches.Insert(index, result);

			return result;

		}

		public void AddDecal(UltimateDecal decal) {

			if (decal.material == null) return;

			if (decal.material != null) {
				decal.material.enableInstancing = true;
			}

			var batch = GetBatchByMaterial(decal.material, decal.order);
				
			batch.AddDecal(decal);

			batchByDecal[decal] = batch;

		}

		public void RemoveDecal(UltimateDecal decal) {

			if (!batchByDecal.ContainsKey(decal)) return;

			batchByDecal[decal].RemoveDecal(decal);

			batchByDecal.Remove(decal);

		}

		public void AddDecal(Material material, Transform t, float normalizedAtlasOffset = 0, int renderQueueOffset = 0) {

			GetBatchByMaterial(material, renderQueueOffset).AddDecal(t, normalizedAtlasOffset);

		}

		public void AddDecal(Material material, Matrix4x4 matrix, float normalizedAtlasOffset = 0, int renderQueueOffset = 0) {

			GetBatchByMaterial(material, renderQueueOffset).AddDecal(matrix, normalizedAtlasOffset);

		}

		public bool Update() {

			var needsRebuild = false;

			foreach (var e in batches) {
				if (e.Update()) {
					needsRebuild = true;
				}
			}

			return needsRebuild;

		}

		public void Clean() {

			foreach (var batch in batches) {
				batch.Clean();
			}

			batches.Clear();

		}


	}

}