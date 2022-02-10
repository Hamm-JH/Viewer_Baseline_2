using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Rendering;

namespace Bearroll.UltimateDecals {

	public class UD_Batch {

		public UD_Manager manager { get; private set; }

        const int n = 64;

        public readonly UltimateDecalType type;
		public readonly Material material;
		public UltimateDecal[] decals;

		public Matrix4x4[] matrices;
		public float[] ctimes;
		public float[] ctests;
		public float[] atlasIndices;
		public Vector4[] sizes;

		public Vector4[] SHAr;
		public Vector4[] SHAg;
		public Vector4[] SHAb;
		public Vector4[] SHBr;
		public Vector4[] SHBg;
		public Vector4[] SHBb;
		public Vector4[] SHC;

		public Vector3[] positions;
		public Vector3[] scales;
		public Quaternion[] rotations;

		public MaterialPropertyBlock props;
		public int count { get; private set; }

		public bool isFull {
			get { return count >= n; }
		}

		public bool needsRebuild { get; private set; }

		public bool isLit { get; private set; }

		public int sortingOrder { get; set; }

		Shader shader;
		int replaceIndex = 0;

		public UD_Batch(UD_Manager manager, UltimateDecalType type, Material material, int sortingOrder) {

			this.manager = manager;
			this.type = type;
			this.material = material;
			this.shader = material.shader;
			this.sortingOrder = sortingOrder;

			isLit = material.shader.name.EndsWith("/Lit");

			matrices = new Matrix4x4[n];

			if (type == UltimateDecalType.Dynamic || type == UltimateDecalType.Static) {
				decals = new UltimateDecal[n];
			}

			if (type == UltimateDecalType.Dynamic || Application.isEditor) {

				positions = new Vector3[n];
				rotations = new Quaternion[n];
				scales = new Vector3[n];

			}

			ctimes = new float[n];
			ctests = new float[n];
			sizes = new Vector4[n];
			atlasIndices = new float[n];

		}

		public override string ToString() {
			return $"{material?.name}:{sortingOrder}";
		}

		int PreAdd(Transform t, int order = 0) {

			int index;

			if (type == UltimateDecalType.PermanentMark) {

				if (count < n) {
					replaceIndex = count;
					index = replaceIndex;
					count++;
				} else {
					replaceIndex = (replaceIndex + 1) % n;
					index = replaceIndex;
				}

			} else if (isFull) {

				replaceIndex = (replaceIndex + 1) % n;
				index = replaceIndex;
			
			} else {

				index = count;
				count++;

				for (var i = 0; i < count - 1; i++) {

					if (decals[i].order <= order) continue;

					Insert(i);

					index = i;

					break;

				}

			}

			if (type == UltimateDecalType.Dynamic) {

				positions[index] = t.position;
				rotations[index] = t.rotation;
				scales[index] = t.localScale;

			}

			return index;

		}

		void SetMatrix(Transform t, int index) {
			matrices[index] = Matrix4x4.TRS(t.position, t.rotation, t.localScale);
		}

		void SetSH(int index) {

			var pos = matrices[index].GetColumn(3);

			SphericalHarmonicsL2 sh;
			LightProbes.GetInterpolatedProbe(pos, null, out sh);

			CheckSH();

			SHAr[index] = new Vector4(sh[0, 3], sh[0, 1], sh[0, 2], sh[0, 0] - sh[0, 6]);
			SHAg[index] = new Vector4(sh[1, 3], sh[1, 1], sh[1, 2], sh[1, 0] - sh[1, 6]);
			SHAb[index] = new Vector4(sh[2, 3], sh[2, 1], sh[2, 2], sh[2, 0] - sh[2, 6]);

			SHBr[index] = new Vector4(sh[0, 4], sh[0, 6], sh[0, 5] * 3, sh[0, 7]);
			SHBg[index] = new Vector4(sh[1, 4], sh[1, 6], sh[1, 5] * 3, sh[1, 7]);
			SHBb[index] = new Vector4(sh[2, 4], sh[2, 6], sh[2, 5] * 3, sh[2, 7]);

			SHC[index] = new Vector4(sh[0, 8], sh[2, 8], sh[1, 8], 1);

		}

		void CheckSH() {

			if (SHAr != null) return;
			
			SHAr = new Vector4[n];
			SHAg = new Vector4[n];
			SHAb = new Vector4[n];
			SHBr = new Vector4[n];
			SHBg = new Vector4[n];
			SHBb = new Vector4[n];
			SHC = new Vector4[n];

		}

		void UpdateProps() {

			if (manager.perDecalLightProbes) {

				if (props == null) {
					props = new MaterialPropertyBlock();
				}
				
				for (var i = 0; i < count; i++) {
					SetSH(i);
				}

				if (count > 0) {

					props.SetVectorArray("_SHAr", SHAr);
					props.SetVectorArray("_SHAg", SHAg);
					props.SetVectorArray("_SHAb", SHAb);
					props.SetVectorArray("_SHBr", SHBr);
					props.SetVectorArray("_SHBg", SHBg);
					props.SetVectorArray("_SHBb", SHBb);
					props.SetVectorArray("_SHC", SHC);

				}

			} else {

				if (props != null) {

					props = null;

					SHAr = null;
					SHAg = null;
					SHAb = null;
					SHBr = null;
					SHBg = null;
					SHBb = null;
					SHC = null;

				}

			}

		}

		void PostAdd() {
			needsRebuild = true;
		}

		public int AddDecal(Matrix4x4 matrix, float normalizedAtlasOffset = 0) {

			var index = PreAdd(null);

			matrices[index] = matrix;
			ctimes[index] = Time.time;
			ctests[index] = UnityEngine.Random.Range(0f, 1f);
			sizes[index] = matrix.lossyScale;

			var atlasData = material.GetVector("_AtlasData");
			var atlasLength = atlasData.x * atlasData.y;

			atlasIndices[index] = Mathf.FloorToInt(Mathf.Clamp(normalizedAtlasOffset, 0, 0.99f) * atlasLength);

			var r = index;

			PostAdd();

			return r;

		}

		public int AddDecal(Transform t, float normalizedAtlasOffset = 0) {

			var index = PreAdd(t);

			SetMatrix(t, index);

			ctimes[index] = Time.time;
			ctests[index] = UnityEngine.Random.Range(0f, 1f);
			sizes[index] = t.localScale;

			var atlasData = material.GetVector("_AtlasData");
			var atlasLength = atlasData.x * atlasData.y;

			atlasIndices[index] = Mathf.FloorToInt(Mathf.Clamp(normalizedAtlasOffset, 0, 0.99f) * atlasLength);

			var r = index;

			PostAdd();

			return r;

		}

		public int AddDecal(UltimateDecal decal) {

			var t = decal.transform;

			var index = PreAdd(t, decal.order);

			if (type == UltimateDecalType.Dynamic || type == UltimateDecalType.Static) {
				decals[index] = decal;
			}

			SetMatrix(t, index);

			ctimes[index] = Time.time;
			ctests[index] = UnityEngine.Random.Range(0f, 1f);
			sizes[index] = decal.transform.localScale;
			atlasIndices[index] = decal.atlasIndex;

			var r = index;

			PostAdd();

			return r;

		}

		void Insert(int index) {

			for (var j = count - 1; j > index; j--) {

				if (type == UltimateDecalType.Dynamic || type == UltimateDecalType.Static) {
					decals[j] = decals[j - 1];
				}

				if(type == UltimateDecalType.Dynamic) {
					positions[j] = positions[j - 1];
					rotations[j] = rotations[j - 1];
					scales[j] = scales[j - 1];
				}

				matrices[j] = matrices[j - 1];

				ctimes[j] = ctimes[j - 1];
				ctests[j] = ctests[j - 1];
				sizes[j] = sizes[j - 1];
				atlasIndices[j] = atlasIndices[j - 1];

				if (SHAr != null) {

					SHAr[j] = SHAr[j - 1];
					SHAg[j] = SHAg[j - 1];
					SHAb[j] = SHAb[j - 1];
					SHBr[j] = SHBr[j - 1];
					SHBg[j] = SHBg[j - 1];
					SHBb[j] = SHBb[j - 1];
					SHC[j] = SHC[j - 1];

				}

			}

		}

		void RemoveDecal(int index) {

			for (var j = index; j < count - 1; j++) {

				if (type == UltimateDecalType.Dynamic || type == UltimateDecalType.Static) {
					decals[j] = decals[j + 1];
				} 

				if(type == UltimateDecalType.Dynamic) {
					positions[j] = positions[j + 1];
					rotations[j] = rotations[j + 1];
					scales[j] = scales[j + 1];
				}

				matrices[j] = matrices[j + 1];

				ctimes[j] = ctimes[j + 1];
				ctests[j] = ctests[j + 1];
				sizes[j] = sizes[j + 1];
				atlasIndices[j] = atlasIndices[j + 1];

				if (SHAr != null) {

					SHAr[j] = SHAr[j + 1];
					SHAg[j] = SHAg[j + 1];
					SHAb[j] = SHAb[j + 1];
					SHBr[j] = SHBr[j + 1];
					SHBg[j] = SHBg[j + 1];
					SHBb[j] = SHBb[j + 1];
					SHC[j] = SHC[j + 1];

				}

			}

			count--;
			needsRebuild = true;

		}

		public void RemoveDecal(UltimateDecal decal) {

			if (type != UltimateDecalType.Dynamic && type != UltimateDecalType.Static) return;

			for (var i = count - 1; i >= 0; i--) {

				if (decals[i] != decal) continue;

				RemoveDecal(i);

			}

		}

		public bool Update() {

			if (material != null && material.shader != shader) {
				shader = material.shader;
				isLit = material.shader != null && material.shader.name.EndsWith("/Lit");
				return true;
			}

			if (!needsRebuild && (type == UltimateDecalType.Dynamic || type == UltimateDecalType.Static)) {

				for (var i = count - 1; i >= 0; i--) {

					if (decals[i] == null || !decals[i].gameObject.activeInHierarchy) {

						RemoveDecal(i);

						needsRebuild = true;

						continue;
					}

					if (!Application.isEditor && type == UltimateDecalType.Static) continue;

					var t = decals[i].transform;

					if (t.position != positions[i] || t.rotation != rotations[i] || t.localScale != scales[i]) {

						positions[i] = t.position;
						rotations[i] = t.rotation;
						scales[i] = t.localScale;

						SetMatrix(t, i);

						sizes[i] = t.localScale;
						needsRebuild = true;

					}

				}

			}

			if (needsRebuild) {
				UpdateProps();
			}

			var r = needsRebuild;

			needsRebuild = false;

			return r;

		}

		public void Clean() {

			count = 0;
			replaceIndex = 0;

			if (type == UltimateDecalType.Dynamic || type == UltimateDecalType.Static) {
				for (var i = 0; i < decals.Length; i++) {
					decals[i] = null;
				}
			}

		}

	}

}