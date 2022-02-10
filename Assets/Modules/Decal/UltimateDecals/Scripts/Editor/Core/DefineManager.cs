using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace Bearroll.UltimateDecals {

	public enum PipelineType {
		Legacy,
		URP,
		LWRP,
		Other
	}

	[InitializeOnLoad]
	public class DefineManager {
		
		static DefineManager() {

			var pipeline = сurrentPipeline;
			var group = EditorUserBuildSettings.selectedBuildTargetGroup;
			var s = PlayerSettings.GetScriptingDefineSymbolsForGroup(group);

			s = ToggleSymbol(s, "UD_URP", pipeline == PipelineType.URP);
			s = ToggleSymbol(s, "UD_LWRP", pipeline == PipelineType.LWRP);
			s = ToggleSymbol(s, "UD_LEGACY", pipeline == PipelineType.Legacy);

			s = ToggleSymbol(s, "UD_MOBILE", isMobilePlatform);

			PlayerSettings.SetScriptingDefineSymbolsForGroup(group, s);

			ToggleShaderKeyword("UD_URP", pipeline == PipelineType.URP);
			ToggleShaderKeyword("UD_LWRP", pipeline == PipelineType.LWRP);
			ToggleShaderKeyword("UD_LEGACY", pipeline == PipelineType.Legacy);

			ToggleShaderKeyword("UD_MOBILE", isMobilePlatform);

		}

		static string ToggleSymbol(string s, string symbol, bool toggle) {

			if (toggle == s.Contains(symbol)) return s;

			if (toggle) return s + " " + symbol;

			return s.Replace(symbol, "");

		}

		static void ToggleShaderKeyword(string name, bool toggle) {

			if (toggle == Shader.IsKeywordEnabled(name)) return;

			if (toggle) {
				Shader.EnableKeyword(name);
			} else {
				Shader.DisableKeyword(name);
			}

		}

		static bool isMobilePlatform {
			get {
			#if UNITY_STANDALONE
			return false;
			#else
			return true;
			#endif
			}
		}

		static PipelineType сurrentPipeline {

			get {

				var asset = GraphicsSettings.renderPipelineAsset;

				if (asset == null) return PipelineType.Legacy;

				var type = asset.GetType().ToString();

				if (type.Contains("Universal")) return PipelineType.URP;

				if (type.Contains("Lightweight")) return PipelineType.LWRP;

				return PipelineType.Other;

			}


		}

	}
	

}