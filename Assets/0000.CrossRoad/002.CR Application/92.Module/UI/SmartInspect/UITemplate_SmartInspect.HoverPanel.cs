using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.UI
{
	using Definition;
	using TMPro;
	using View;

    public partial class UITemplate_SmartInspect : AUI
    {
		public void HoverPanel_OnHover(Canvas _rootCanvas, Vector3 _screenMousePos, Definition._Issue.Issue _issue)
		{
			//RectTransform cRect = _rootCanvas.pixelRect;
			//RectTransform cRect = _rootCanvas.GetComponent<RectTransform>();

			// 4. 호버 패널
			GameObject hPanel = m_general.m_panelHover.m_hoverPanel;
			//GameObject hPanel = m_hoverPanel;
			hPanel.transform.SetParent(transform.parent);
			// 4-1. 호버 패널 Rect
			RectTransform hRect = hPanel.GetComponent<RectTransform>();

			// 5. 호버 텍스트
			TextMeshProUGUI hText1 = m_general.m_panelHover.m_hoverText1;
			TextMeshProUGUI hText2 = m_general.m_panelHover.m_hoverText2;
			TextMeshProUGUI hText3 = m_general.m_panelHover.m_hoverText3;
			TextMeshProUGUI hText4 = m_general.m_panelHover.m_hoverText4;
			TextMeshProUGUI hText5 = m_general.m_panelHover.m_hoverText5;

			hPanel.SetActive(true);

			//Debug.Log(_rootCanvas.pixelRect);

			//hRect.position = _screenMousePos;
			HoverPanel_SetPanelPos(_rootCanvas.pixelRect, hRect, _screenMousePos, new Vector2(10, 10));

			HoverPanel_SetPanelText(hText1, hText2, hText3, hText4, hText5,
				_issue);
			// 1. 캔버스
			// 2. 마우스 위치 (스크린 위치)
			// 3. 손상정보
		}

		public void HoverPanel_OffHover()
		{
			m_general.m_panelHover.m_hoverPanel.SetActive(false);
		}

		/// <summary>
		/// 호버 패널의 위치 할당
		/// </summary>
		/// <param name="_canvasRect"></param>
		/// <param name="_rectT"></param>
		/// <param name="_screenPos"></param>
		/// <param name="offset"></param>
		private void HoverPanel_SetPanelPos(Rect _canvasRect, RectTransform _rectT, Vector3 _screenPos, Vector2 offset)
		{
			Vector2 finalPosition = SetPanelPos(_canvasRect, _screenPos, _rectT.sizeDelta, offset);
			_rectT.position = finalPosition;
		}

		/// <summary>
		/// 패널의 위치를 지정한다.
		/// </summary>
		/// <param name="_cRect"></param>
		/// <param name="_screenPos"></param>
		/// <param name="_rectSizeDelta"></param>
		/// <param name="_offset"></param>
		/// <returns></returns>
		private Vector2 SetPanelPos(Rect _cRect, Vector3 _screenPos, Vector2 _rectSizeDelta, Vector2 _offset)
		{
			// 시계열로 배치 우선순위 반환
			// -1, -1 왼쪽 아래
			// -1, 1 왼쪽 위
			// 1, 1 오른쪽 위
			// 1, -1 오른쪽 아래
			Vector2 result = new Vector2(0, 0);

			Vector2 MIN = _cRect.min;
			Vector2 MAX = _cRect.max;

			Vector2 min = default(Vector2);
			Vector2 max = default(Vector2);

			Vector2 finalPosition = default(Vector2);
			if (IsAblePosition(_cRect, _screenPos, _rectSizeDelta, _offset, new Vector2(1, 1), out finalPosition)) { }
			else if (IsAblePosition(_cRect, _screenPos, _rectSizeDelta, _offset, new Vector2(1, -1), out finalPosition)) { }
			else if (IsAblePosition(_cRect, _screenPos, _rectSizeDelta, _offset, new Vector2(-1, -1), out finalPosition)) { }
			else if (IsAblePosition(_cRect, _screenPos, _rectSizeDelta, _offset, new Vector2(-1, 1), out finalPosition)) { }

			return finalPosition;
		}

		/// <summary>
		/// 특정 위치값이 배치 가능한지 확인한다.
		/// </summary>
		/// <param name="_cRect"></param>
		/// <param name="_screenPos"></param>
		/// <param name="_rectSizeDelta"></param>
		/// <param name="_offset"></param>
		/// <returns></returns>
		private bool IsAblePosition(Rect _cRect, Vector3 _screenPos, Vector2 _rectSizeDelta, Vector2 _offset, Vector2 _SIGN, out Vector2 _position)
		{
			bool result = false;
			_position = default(Vector2);

			// 기호의 방향으로 가감 연산을 수행한다.
			Vector2 __pos = new Vector2(
				_screenPos.x + (_rectSizeDelta.x / 2 + _offset.x) * _SIGN.x,
				_screenPos.y + (_rectSizeDelta.y / 2 + _offset.y) * _SIGN.y);

			Vector2 min = new Vector2(
				__pos.x - _rectSizeDelta.x / 2,
				__pos.y - _rectSizeDelta.y / 2
				);

			Vector2 max = new Vector2(
				__pos.x + _rectSizeDelta.x / 2,
				__pos.y + _rectSizeDelta.y / 2
				);

			bool xConfirm = false;
			bool yConfirm = false;

			if (min.x >= _cRect.min.x && max.x <= _cRect.max.x)
			{
				xConfirm = true;
			}
			else
			{
				xConfirm = false;
			}

			if (min.y >= _cRect.min.y && max.y <= _cRect.max.y)
			{
				yConfirm = true;
			}
			else
			{
				yConfirm = false;
			}

			//Debug.Log($"sign : {_SIGN}, xConf : {xConfirm}, yConf : {yConfirm}");

			result = xConfirm & yConfirm;
			if (result)
			{
				_position = __pos;
			}

			return result;
		}

		private void HoverPanel_SetPanelText(TextMeshProUGUI _text1, TextMeshProUGUI _text2, TextMeshProUGUI _text3, TextMeshProUGUI _text4, TextMeshProUGUI _text5,
			Definition._Issue.Issue _issue)
		{
			//StringBuilder sb = new StringBuilder();

			//sb.AppendLine($"등록자 : {_issue.NmUser}");
			//sb.AppendLine($"손상 분류 : {_issue.IssueCode}");
			//sb.AppendLine($"손상 부재 : {_issue.CdBridgeParts}");
			//sb.AppendLine($"손상 등급 : {_issue.IssueStatus}");
			//sb.AppendLine($"등록 일자 : {_issue.DateDmg}");

			//_text.text = sb.ToString();

			_text1.text = _issue.NmUser;
			_text2.text = _issue.IssueCode.ToString();
			_text3.text = _issue.CdBridgeParts.ToString();
			_text4.text = _issue.IssueStatus.ToString();
			_text5.text = _issue.DateDmg;

		}
	}
}
