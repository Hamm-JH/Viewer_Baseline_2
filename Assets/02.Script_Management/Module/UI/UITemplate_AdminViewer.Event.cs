using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.UI
{
	using AdminViewer;
	using Definition;
	using UnityEngine.UI;

	public partial class UITemplate_AdminViewer : AUI
	{
		#region 타이틀 패널컨
		public void SetTitleText(string value)
		{
			TitData.oName.text = value;
		}

		public void SetPartText(string value)
		{
			TitData.pName.text = value;
		}
		#endregion

		#region Ad_nav
		public void Ad_nav_state1_NAV()
		{
			NavData.nv1_bg.color = Colors.Set(ColorType.UI_Ad_Highlight, 1);
			NavData.nv1_ic.color = Colors.Set(ColorType.UI_Ad_Highlight, 1);
			NavData.nv1_tx.color = Colors.Set(ColorType.UI_Ad_Highlight, 1);
			NavData.nv2_bg.color = Colors.Set(ColorType.UI_Ad_Default, 1);
			NavData.nv2_ic.color = Colors.Set(ColorType.UI_Ad_Default, 1);
			NavData.nv2_tx.color = Colors.Set(ColorType.UI_Ad_Default, 1);
			NavData.nv3_bg.color = Colors.Set(ColorType.UI_Ad_Default, 1);
			NavData.nv3_ic.color = Colors.Set(ColorType.UI_Ad_Default, 1);
			NavData.nv3_tx.color = Colors.Set(ColorType.UI_Ad_Default, 1);
			NavData.nv4_bg.color = Colors.Set(ColorType.UI_Ad_Default, 1);
			NavData.nv4_ic.color = Colors.Set(ColorType.UI_Ad_Default, 1);
			NavData.nv4_tx.color = Colors.Set(ColorType.UI_Ad_Default, 1);
			NavData.nv5_bg.color = Colors.Set(ColorType.UI_Ad_Default, 1);
			NavData.nv5_ic.color = Colors.Set(ColorType.UI_Ad_Default, 1);
			NavData.nv5_tx.color = Colors.Set(ColorType.UI_Ad_Default, 1);
			NavData.nv_ar1.color = Colors.Set(ColorType.UI_Ad_Default, 1);
			NavData.nv_ar2.color = Colors.Set(ColorType.UI_Ad_Default, 1);
			NavData.nv_ar3.color = Colors.Set(ColorType.UI_Ad_Default, 1);
			NavData.nv_ar4.color = Colors.Set(ColorType.UI_Ad_Default, 1);
		}
		public void Ad_nav_state2_NAV()
		{
			NavData.nv1_bg.color = Colors.Set(ColorType.UI_Ad_Highlight, 1);
			NavData.nv1_ic.color = Colors.Set(ColorType.UI_Ad_Highlight, 1);
			NavData.nv1_tx.color = Colors.Set(ColorType.UI_Ad_Highlight, 1);
			NavData.nv2_bg.color = Colors.Set(ColorType.UI_Ad_Highlight, 1);
			NavData.nv2_ic.color = Colors.Set(ColorType.UI_Ad_Highlight, 1);
			NavData.nv2_tx.color = Colors.Set(ColorType.UI_Ad_Highlight, 1);
			NavData.nv3_bg.color = Colors.Set(ColorType.UI_Ad_Default, 1);
			NavData.nv3_ic.color = Colors.Set(ColorType.UI_Ad_Default, 1);
			NavData.nv3_tx.color = Colors.Set(ColorType.UI_Ad_Default, 1);
			NavData.nv4_bg.color = Colors.Set(ColorType.UI_Ad_Default, 1);
			NavData.nv4_ic.color = Colors.Set(ColorType.UI_Ad_Default, 1);
			NavData.nv4_tx.color = Colors.Set(ColorType.UI_Ad_Default, 1);
			NavData.nv5_bg.color = Colors.Set(ColorType.UI_Ad_Default, 1);
			NavData.nv5_ic.color = Colors.Set(ColorType.UI_Ad_Default, 1);
			NavData.nv5_tx.color = Colors.Set(ColorType.UI_Ad_Default, 1);
			NavData.nv_ar1.color = Colors.Set(ColorType.UI_Ad_Highlight, 1);
			NavData.nv_ar2.color = Colors.Set(ColorType.UI_Ad_Default, 1);
			NavData.nv_ar3.color = Colors.Set(ColorType.UI_Ad_Default, 1);
			NavData.nv_ar4.color = Colors.Set(ColorType.UI_Ad_Default, 1);
		}
		public void Ad_nav_state3_NAV()
		{
			NavData.nv1_bg.color = Colors.Set(ColorType.UI_Ad_Highlight, 1);
			NavData.nv1_ic.color = Colors.Set(ColorType.UI_Ad_Highlight, 1);
			NavData.nv1_tx.color = Colors.Set(ColorType.UI_Ad_Highlight, 1);
			NavData.nv2_bg.color = Colors.Set(ColorType.UI_Ad_Highlight, 1);
			NavData.nv2_ic.color = Colors.Set(ColorType.UI_Ad_Highlight, 1);
			NavData.nv2_tx.color = Colors.Set(ColorType.UI_Ad_Highlight, 1);
			NavData.nv3_bg.color = Colors.Set(ColorType.UI_Ad_Highlight, 1);
			NavData.nv3_ic.color = Colors.Set(ColorType.UI_Ad_Highlight, 1);
			NavData.nv3_tx.color = Colors.Set(ColorType.UI_Ad_Highlight, 1);
			NavData.nv4_bg.color = Colors.Set(ColorType.UI_Ad_Default, 1);
			NavData.nv4_ic.color = Colors.Set(ColorType.UI_Ad_Default, 1);
			NavData.nv4_tx.color = Colors.Set(ColorType.UI_Ad_Default, 1);
			NavData.nv5_bg.color = Colors.Set(ColorType.UI_Ad_Default, 1);
			NavData.nv5_ic.color = Colors.Set(ColorType.UI_Ad_Default, 1);
			NavData.nv5_tx.color = Colors.Set(ColorType.UI_Ad_Default, 1);
			NavData.nv_ar1.color = Colors.Set(ColorType.UI_Ad_Highlight, 1);
			NavData.nv_ar2.color = Colors.Set(ColorType.UI_Ad_Highlight, 1);
			NavData.nv_ar3.color = Colors.Set(ColorType.UI_Ad_Default, 1);
			NavData.nv_ar4.color = Colors.Set(ColorType.UI_Ad_Default, 1);
		}
		public void Ad_nav_state4_NAV()
		{
			NavData.nv1_bg.color = Colors.Set(ColorType.UI_Ad_Highlight, 1);
			NavData.nv1_ic.color = Colors.Set(ColorType.UI_Ad_Highlight, 1);
			NavData.nv1_tx.color = Colors.Set(ColorType.UI_Ad_Highlight, 1);
			NavData.nv2_bg.color = Colors.Set(ColorType.UI_Ad_Highlight, 1);
			NavData.nv2_ic.color = Colors.Set(ColorType.UI_Ad_Highlight, 1);
			NavData.nv2_tx.color = Colors.Set(ColorType.UI_Ad_Highlight, 1);
			NavData.nv3_bg.color = Colors.Set(ColorType.UI_Ad_Highlight, 1);
			NavData.nv3_ic.color = Colors.Set(ColorType.UI_Ad_Highlight, 1);
			NavData.nv3_tx.color = Colors.Set(ColorType.UI_Ad_Highlight, 1);
			NavData.nv4_bg.color = Colors.Set(ColorType.UI_Ad_Highlight, 1);
			NavData.nv4_ic.color = Colors.Set(ColorType.UI_Ad_Highlight, 1);
			NavData.nv4_tx.color = Colors.Set(ColorType.UI_Ad_Highlight, 1);
			NavData.nv5_bg.color = Colors.Set(ColorType.UI_Ad_Default, 1);
			NavData.nv5_ic.color = Colors.Set(ColorType.UI_Ad_Default, 1);
			NavData.nv5_tx.color = Colors.Set(ColorType.UI_Ad_Default, 1);
			NavData.nv_ar1.color = Colors.Set(ColorType.UI_Ad_Highlight, 1);
			NavData.nv_ar2.color = Colors.Set(ColorType.UI_Ad_Highlight, 1);
			NavData.nv_ar3.color = Colors.Set(ColorType.UI_Ad_Highlight, 1);
			NavData.nv_ar4.color = Colors.Set(ColorType.UI_Ad_Default, 1);
		}
		public void Ad_nav_state5_NAV()
		{
			NavData.nv1_bg.color = Colors.Set(ColorType.UI_Ad_Highlight, 1);
			NavData.nv1_ic.color = Colors.Set(ColorType.UI_Ad_Highlight, 1);
			NavData.nv1_tx.color = Colors.Set(ColorType.UI_Ad_Highlight, 1);
			NavData.nv2_bg.color = Colors.Set(ColorType.UI_Ad_Highlight, 1);
			NavData.nv2_ic.color = Colors.Set(ColorType.UI_Ad_Highlight, 1);
			NavData.nv2_tx.color = Colors.Set(ColorType.UI_Ad_Highlight, 1);
			NavData.nv3_bg.color = Colors.Set(ColorType.UI_Ad_Highlight, 1);
			NavData.nv3_ic.color = Colors.Set(ColorType.UI_Ad_Highlight, 1);
			NavData.nv3_tx.color = Colors.Set(ColorType.UI_Ad_Highlight, 1);
			NavData.nv4_bg.color = Colors.Set(ColorType.UI_Ad_Highlight, 1);
			NavData.nv4_ic.color = Colors.Set(ColorType.UI_Ad_Highlight, 1);
			NavData.nv4_tx.color = Colors.Set(ColorType.UI_Ad_Highlight, 1);
			NavData.nv5_bg.color = Colors.Set(ColorType.UI_Ad_Highlight, 1);
			NavData.nv5_ic.color = Colors.Set(ColorType.UI_Ad_Highlight, 1);
			NavData.nv5_tx.color = Colors.Set(ColorType.UI_Ad_Highlight, 1);
			NavData.nv_ar1.color = Colors.Set(ColorType.UI_Ad_Highlight, 1);
			NavData.nv_ar2.color = Colors.Set(ColorType.UI_Ad_Highlight, 1);
			NavData.nv_ar3.color = Colors.Set(ColorType.UI_Ad_Highlight, 1);
			NavData.nv_ar4.color = Colors.Set(ColorType.UI_Ad_Highlight, 1);
		}
		#endregion

		#region bot bar
		public void Ad_nav_state1_BOT()
		{
			botBarData.state1.SetActive(true);
			botBarData.state2.SetActive(false);
			botBarData.state3.SetActive(false);
			botBarData.state4.SetActive(false);
			botBarData.state5.SetActive(false);
		}
		public void Ad_nav_state2_BOT()
		{
			botBarData.state1.SetActive(false);
			botBarData.state2.SetActive(true);
			botBarData.state3.SetActive(false);
			botBarData.state4.SetActive(false);
			botBarData.state5.SetActive(false);
		}
		public void Ad_nav_state3_BOT()
		{
			botBarData.state1.SetActive(false);
			botBarData.state2.SetActive(false);
			botBarData.state3.SetActive(true);
			botBarData.state4.SetActive(false);
			botBarData.state5.SetActive(false);
		}
		public void Ad_nav_state4_BOT()
		{
			botBarData.state1.SetActive(false);
			botBarData.state2.SetActive(false);
			botBarData.state3.SetActive(false);
			botBarData.state4.SetActive(true);
			botBarData.state5.SetActive(false);
		}
		public void Ad_nav_state5_BOT()
		{
			botBarData.state1.SetActive(false);
			botBarData.state2.SetActive(false);
			botBarData.state3.SetActive(false);
			botBarData.state4.SetActive(false);
			botBarData.state5.SetActive(true);
		}
		#endregion

		#region 봇 패널컨

		public void SetAddressText(string value)
		{
			BotData.adrName.text = value;
		}

		#endregion

		#region State1 pin컨

		private void SetPin(bool isOn)
		{
			Items.State1_rootPin.SetActive(isOn);
		}

		private void SetPins_blindFire(Bounds _bound)
		{
			items.State1_pins.ForEach(x =>
			{
				float xR = Random.Range(_bound.min.x, _bound.max.x);
				float yR = Random.Range(_bound.min.y, _bound.max.y);
				float zR = Random.Range(_bound.min.z, _bound.max.z);

				Vector3 pos = new Vector3(xR, yR, zR);

				Vector3 screenPos = Camera.main.WorldToScreenPoint(pos);

				x.transform.position = screenPos;

			});
		}

		#endregion

		#region 특수 이벤

		private void SetSt3_Toggle()
		{
			bool bpm1ON = Panels.bpm1.activeSelf;
			bool bpm2ON = Panels.bpm2.activeSelf;

			if(bpm1ON && bpm2ON)
			{
				Panels.bpm1.SetActive(false);
				Panels.bpm2.SetActive(false);
			}
			else
			{
				Panels.bpm1.SetActive(true);
				Panels.bpm2.SetActive(true);
			}
		}

		private void SetSt4_Toggle()
		{
			bool bpm1ON = Panels.bpm1.activeSelf;
			bool bpm2ON = Panels.bpmm.activeSelf;

			if (bpm1ON && bpm2ON)
			{
				Panels.bpm1.SetActive(false);
				Panels.bpmm.SetActive(false);
			}
			else
			{
				Panels.bpm1.SetActive(true);
				Panels.bpmm.SetActive(true);
			}
		}

		private void SetSt5_Toggle()
		{
			bool bpm1ON = Panels.s5b1.activeSelf;
			bool bpm2ON = Panels.s5b2.activeSelf;

			if (bpm1ON && bpm2ON)
			{
				Panels.s5b1.SetActive(false);
				Panels.s5b2.SetActive(false);
			}
			else
			{
				Panels.s5b1.SetActive(true);
				Panels.s5b2.SetActive(true);
			}
		}

		#endregion

		#region 상태변경

		private void SetPanel_State1()
		{
			Panels.mainLocation	.SetActive(true);
			Panels.mainPicture	.SetActive(true);
			Panels.keymap		.SetActive(false);
			Panels.bpm1			.SetActive(false);
			Panels.bpm2			.SetActive(false);
			Panels.bpmm			.SetActive(false);
			Panels.s5b1			.SetActive(false);
			Panels.s5b2			.SetActive(false);
			Panels.s5m1			.SetActive(false);
		}
		private void SetPanel_State2()
		{
			Panels.mainLocation.SetActive(false);
			Panels.mainPicture.SetActive(false);
			Panels.keymap.SetActive(false);
			Panels.bpm1.SetActive(false);
			Panels.bpm2.SetActive(false);
			Panels.bpmm.SetActive(true);
			Panels.s5b1.SetActive(false);
			Panels.s5b2.SetActive(false);
			Panels.s5m1.SetActive(false);
		}
		private void SetPanel_State3()
		{
			Panels.mainLocation.SetActive(false);
			Panels.mainPicture.SetActive(false);
			Panels.keymap.SetActive(false);
			Panels.bpm1.SetActive(true);
			Panels.bpm2.SetActive(true);
			Panels.bpmm.SetActive(false);
			Panels.s5b1.SetActive(false);
			Panels.s5b2.SetActive(false);
			Panels.s5m1.SetActive(false);
		}
		private void SetPanel_State4()
		{
			Panels.mainLocation.SetActive(false);
			Panels.mainPicture.SetActive(false);
			Panels.keymap.SetActive(false);
			Panels.bpm1.SetActive(true);
			Panels.bpm2.SetActive(false);
			Panels.bpmm.SetActive(true);
			Panels.s5b1.SetActive(false);
			Panels.s5b2.SetActive(false);
			Panels.s5m1.SetActive(false);
		}
		private void SetPanel_State5()
		{
			Panels.mainLocation.SetActive(false);
			Panels.mainPicture.SetActive(false);
			Panels.keymap.SetActive(false);
			Panels.bpm1.SetActive(false);
			Panels.bpm2.SetActive(false);
			Panels.bpmm.SetActive(false);
			Panels.s5b1.SetActive(true);
			Panels.s5b2.SetActive(true);
			Panels.s5m1.SetActive(true);
		}

		#endregion
	}
}
