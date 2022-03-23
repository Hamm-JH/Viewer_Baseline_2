using AdminViewer.UI;
using Indicator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdminViewer.API
{
	[System.Serializable]
	public class Ad_Capture
	{
		#region Env

		public Capture L1_Capture;
		public Capture L2_Capture;
		public Capture L3_Capture;

		public State5_BP1_Indicator L1_CaptureTarget;
		public State5_BP2_Indicator L2_CaptureTarget;

		public string L1_Result;
		public string L2_Result;
		public string L3_Result;

		public Ad_Panel s5b1_panel;
		public Ad_Panel s5b2_panel;

		#endregion

		public bool isCapReady1;
		public bool isCapReady2;

		public bool isCapEnd1;
		public bool isCapEnd2;
		public bool isCapEnd3;

		public string imgName1;
		public string imgName2;

		public string img1Base64;
		public string img2Base64;

		public Indicator.State5_BP1_Indicator IndicatorState5_BP1;
		public Indicator.State5_BP2_Indicator IndicatorState5_BP2;

		public RectTransform IndicatorTypeState5_BP1;
		public RectTransform IndicatorTypeState5_BP2;
	}
}
