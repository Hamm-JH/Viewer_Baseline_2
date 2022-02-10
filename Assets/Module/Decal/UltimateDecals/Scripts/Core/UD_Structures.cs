using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Bearroll.UltimateDecals {

	public enum UD_StereoMode {
		SinglePass
	}

	public enum UD_ForwardNormalsSource {
		Restored,
		CameraDepthNormals,
	}

	public enum UD_DebugMode {
		None,
		Normals,
		Edges,
	}

	public enum UltimateDecalRenderingMode {
		ScreenSpace
	}

	public enum UltimateDecalType {
		PermanentMark,
		Static,
		Dynamic,
		//Unmanaged,
	}

	public struct UD_DecalData {
		public Vector3 position;
		public float x;
		public Vector3 size;
		public float y;
	}



	public struct UD_Light {
		public Vector3 position;
		public float range;
		public Vector4 color;
		public Vector3 direction;
		public float angle;
		public Vector4 align;
	}

	public enum UD_Error {
		None = 0,
		NoManager,
		NoCamera,
		SecondInstance,
		UnsupportedRenderingPath,
		Max
	}

	public enum UD_SettingMode {
		Default,
		Override,
	}

	public struct AddedCommandBuffer {
		public CommandBuffer commandBuffer;
		public CameraEvent cameraEvent;
	}

	public enum UD_PBRWorkflow {
		MetallicSmoothness,
		MetallicRoughness,
		SpecularSmoothness,
		SpecularRoughness,
		// OcclusionRoughnessMetallic,
	}

	public enum UD_SRMode {
		SpecularAlpha,
		Separate,
	}

	public enum UD_SRModeMetallic {
		MetallicAlpha,
		Separate,
	}

	public enum UD_StencilMode {
		Disabled,
		LayerMask,
		InvertedLayerMask,
		ManualStencil,
	}

	public enum UD_UnlitBlendingMode {
		Off,
		AlphaBlending,
		Additive,
		Multiplicative,
	}

	public enum UD_BlendingMode {
		Off,
		On,
	}

}