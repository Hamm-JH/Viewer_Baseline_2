using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition
{
	public static class Platforms
	{
		/// <summary>
		/// Á¤ÀÇµÇÁö ¾ÊÀº ÇÃ·§ÆûÀÎ°¡?
		/// </summary>
		/// <param name="_pCode"></param>
		public static bool IsNotDefinition(PlatformCode _pCode)
		{
			switch(_pCode)
			{
				case PlatformCode.NotDef:
					return true;
			}

			return false;
		}

		/// <summary>
		/// ¸ÞÀÌÄ¿ ÇÃ·§ÆûÀÎ°¡?
		/// </summary>
		/// <param name="_pCode"></param>
		/// <returns></returns>
		public static bool IsMakerPlatform(PlatformCode _pCode)
		{
			switch(_pCode)
			{
				case PlatformCode.PC_Maker1:
					return true;
			}

			return false;
		}

		/// <summary>
		/// ºä¾î ÇÃ·§ÆûÀÎ°¡?
		/// </summary>
		/// <param name="_pCode"></param>
		/// <returns></returns>
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

		/// <summary>
		/// ÅÍ³Î ÇÃ·§ÆûÀÎ°¡?
		/// </summary>
		/// <param name="_pCode"></param>
		/// <returns></returns>
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

		/// <summary>
		/// ±³·® ÇÃ·§ÆûÀÎ°¡?
		/// </summary>
		/// <param name="_pCode"></param>
		/// <returns></returns>
		public static bool IsBridgePlatform(PlatformCode _pCode)
		{
			switch(_pCode)
			{
				case PlatformCode.WebGL_AdminViewer_Bridge:
				case PlatformCode.PC_Viewer_Bridge:
				case PlatformCode.WebGL_SmartInspect_Bridge:
					return true;
			}

			return false;
		}

		/// <summary>
		/// ½º¸¶Æ® ÀÎ½ºÆåÆ® ÇÃ·§ÆûÀÎ°¡?
		/// </summary>
		/// <param name="_pCode"></param>
		/// <returns></returns>
		public static bool IsSmartInspectPlatform(PlatformCode _pCode)
		{
			switch(_pCode)
			{
				// IsViewerPlatform°ú Áßº¹µÇ¼­ ÁÖ¼®Ã³¸®
				//case PlatformCode.PC_Viewer_Bridge:
				//case PlatformCode.PC_Viewer_Tunnel:
				case PlatformCode.WebGL_SmartInspect_Tunnel:
				case PlatformCode.WebGL_SmartInspect_Bridge:
					return true;
			}

			return false;
		}

		/// <summary>
		/// µ¥¸ð À¥ºä¾î ÇÃ·§ÆûÀÎ°¡?
		/// </summary>
		/// <param name="_pCode"></param>
		/// <returns></returns>
		public static bool IsDemoWebViewer(PlatformCode _pCode)
        {
			switch(_pCode)
            {
				case PlatformCode.PC_Viewer_Bridge:
				case PlatformCode.PC_Viewer_Tunnel:
					return true;
            }

			return false;
        }

		/// <summary>
		/// µ¥¸ð °ü¸®ÀÚ ÇÃ·§ÆûÀÎ°¡?
		/// </summary>
		/// <param name="_pCode"></param>
		/// <returns></returns>
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

		/// <summary>
		/// PC ÇÃ·§ÆûÀÎ°¡?
		/// </summary>
		/// <param name="_pCode"></param>
		/// <returns></returns>
		public static bool IsPCPlatform(PlatformCode _pCode)
		{
			switch(_pCode)
			{
				case PlatformCode.WebGL_Template1:
				case PlatformCode.WebGL_Template2:
				case PlatformCode.WebGL_AdminViewer_Tunnel:
				case PlatformCode.WebGL_AdminViewer_Bridge:
				case PlatformCode.PC_Viewer_Bridge:
				case PlatformCode.PC_Viewer_Tunnel:

				case PlatformCode.WebGL_SmartInspect_Tunnel:
				case PlatformCode.WebGL_SmartInspect_Bridge:
					return true;
			}

			return false;
		}

		/// <summary>
		/// ¸ð¹ÙÀÏ ÇÃ·§ÆûÀÎ°¡?
		/// </summary>
		/// <param name="_pCode"></param>
		/// <returns></returns>
		public static bool IsMobilePlatform(PlatformCode _pCode)
		{
			switch(_pCode)
            {
				//case PlatformCode.PC_Viewer_Tunnel:
				case PlatformCode.Mobile_Template1:
					return true;
            }

			return false;
		}
	}
}
