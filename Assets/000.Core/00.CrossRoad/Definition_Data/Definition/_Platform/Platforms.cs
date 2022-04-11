using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition
{
	public static class Platforms
	{
		public static bool IsNotDefinition(PlatformCode _pCode)
		{
			switch(_pCode)
			{
				case PlatformCode.NotDef:
					return true;
			}

			return false;
		}

		public static bool IsMakerPlatform(PlatformCode _pCode)
		{
			switch(_pCode)
			{
				case PlatformCode.PC_Maker1:
					return true;
			}

			return false;
		}

		public static bool IsViewerPlatform(PlatformCode _pCode)
		{
			switch(_pCode)
			{
				case PlatformCode.WebGL_AdminViewer_Tunnel:
				case PlatformCode.WebGL_AdminViewer_Bridge:
				case PlatformCode.PC_Viewer_Tunnel:
				case PlatformCode.PC_Viewer_Bridge:
					return true;
			}

			return false;
		}

		public static bool IsTunnelPlatform(PlatformCode _pCode)
		{
			switch(_pCode)
			{
				case PlatformCode.WebGL_AdminViewer_Tunnel:
				case PlatformCode.PC_Viewer_Tunnel:
				case PlatformCode.WebGL_SmartInspect_Tunnel:
					return true;
			}

			return false;
		}

		public static bool IsBridgePlatform(PlatformCode _pCode)
		{
			switch(_pCode)
			{
				case PlatformCode.WebGL_AdminViewer_Bridge:
				case PlatformCode.PC_Viewer_Bridge:
					return true;
			}

			return false;
		}

		public static bool IsSmartInspectPlatform(PlatformCode _pCode)
		{
			switch(_pCode)
			{
				case PlatformCode.PC_Viewer_Bridge:
				case PlatformCode.PC_Viewer_Tunnel:
				case PlatformCode.WebGL_SmartInspect_Tunnel:
				case PlatformCode.WebGL_SmartInspect_Bridge:
					return true;
			}

			return false;
		}

		public static bool IsDemoAdminViewer(PlatformCode _pCode)
		{
			switch(_pCode)
			{
				case PlatformCode.WebGL_AdminViewer_Bridge:
				case PlatformCode.WebGL_AdminViewer_Tunnel:
					return true;
			}

			return false;
		}

		public static bool IsPCPlatform(PlatformCode _pCode)
		{
			switch(_pCode)
			{
				case PlatformCode.WebGL_AdminViewer_Bridge:
				case PlatformCode.WebGL_AdminViewer_Tunnel:
				case PlatformCode.PC_Viewer_Bridge:
				case PlatformCode.PC_Viewer_Tunnel:
					return true;
			}

			return false;
		}

		public static bool IsMobilePlatform(PlatformCode _pCode)
		{
			throw new System.NotImplementedException();
		}
	}
}
