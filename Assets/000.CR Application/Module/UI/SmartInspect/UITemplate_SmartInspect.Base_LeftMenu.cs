using Definition;
using SmartInspect;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using View;

namespace Module.UI
{
    public partial class UITemplate_SmartInspect : AUI
    {
        /// <summary>
        /// Base LeftMenu dmg 버튼 클릭시 메서드
        /// </summary>
        private void LeftMenu_SetDmg()
        {
            // dmg 상태변경
            int currIndex = LeftMenu_StatusChange(0);
            
            // dmg panel 켠다.
            LeftMenu_SetTogglePanel(currIndex);

            // dmg button 상태 활성화
            LeftMenu_SetToggleButton(currIndex);

        }

        /// <summary>
        /// Base LeftMenu rcv 버튼 클릭시 메서드
        /// </summary>
        /// 
        private void LeftMenu_SetRcv()
        {
            // rcv 상태변경
            int currIndex = LeftMenu_StatusChange(1);
            
            // rcv panel 켠다.
            LeftMenu_SetTogglePanel(currIndex);

            // rcv button 상태 활성화
            LeftMenu_SetToggleButton(currIndex);

        }

        /// <summary>
        /// Base LeftMenu setting 버튼 클릭시 메서드
        /// </summary>
        private void LeftMenu_SetSetting()
        {
            int currIndex = LeftMenu_StatusChange(2);
            LeftMenu_SetTogglePanel(currIndex);
            LeftMenu_SetToggleButton(currIndex);
        }

        #region Panel On/Off

        /// <summary>
        /// index ::
        /// 0 : dmg on
        /// 1 : rcv on
        /// 2 : set on
        /// </summary>
        /// <param name="index"></param>
        private void LeftMenu_SetTogglePanel(int index)
        {
            LeftMenu_AllOnOffPanel(false);

            switch(index)
            {
                case 0:
                    m_moduleElements.m_dmgElement.root.SetActive(true);
                    break;

                case 1:
                    m_moduleElements.m_rcvElement.root.SetActive(true);
                    break;

                case 2:

                    break;
            }
        }

        /// <summary>
        /// all module elements on/off
        /// </summary>
        /// <param name="isOn"></param>
        private void LeftMenu_AllOnOffPanel(bool isOn)
        {
            m_moduleElements.m_mElements.ForEach(x => x.SetActive(isOn));
        }

        #endregion

        #region Button Set on/off

        private void LeftMenu_SetToggleButton(int index)
        {
            LeftMenu_AllOnOffButton(false);

            switch(index)
            {
                case 0:
                    LeftMenu_SetSingleButton(m_basePanel.m_processMenus.menus[0], true, 0);
                    break;

                case 1:
                    LeftMenu_SetSingleButton(m_basePanel.m_processMenus.menus[1], true, 1);
                    break;

                case 2:
                    LeftMenu_SetSingleButton(m_basePanel.m_processMenus.menus[2], true, 2);
                    break;
            }
        }

        private void LeftMenu_AllOnOffButton(bool isOn)
        {
            m_basePanel.m_processMenus.menus.ForEach(x =>
            {
                LeftMenu_SetSingleButton(x, isOn, 9);
            });
        }

        private void LeftMenu_SetSingleButton(ProcessMenu _menu, bool isOn, int debugIndex)
        {
            // TODO SmartInspect UI LeftMenu 이미지세팅 바꾸기 예정
            Debug.LogError($"LeftMenu [{debugIndex}] 이미지세팅 바꾸기 예정");

            //_menu.btn_menu = null;    // 버튼 메뉴 변경
            //_menu.img_main = null;    // 주 이미지 변경
            //_menu.img_side = null;    // 사이드바 이미지 변경
            //_menu.txt_desc = null;    // 설명 텍스트 변경
        }

        #endregion

        #region Status Change

        private int LeftMenu_StatusChange(int index)
        {
            int result = -2;
            int currIndex = m_eventBase.m_index;

            // 같은 인덱스를 또 눌렀을 경우
            if(currIndex == index)
            {
                // 어떤 모듈도 선택하지 않은 기본 상태로 전환
                currIndex = -1;
            }
            // 다른 인덱스를 또 눌렀을 경우
            else
            {
                currIndex = index;
            }

            SetLeftMenu_Images(currIndex);

            // 이벤트 상태 업데이트
            m_eventBase.m_index = currIndex;
            return currIndex;
        }

        private void SetLeftMenu_Images(int index)
		{
            Image m1_bg = m_basePanel.m_processMenus.menus[0].btn_menu.GetComponent<Image>();
            Image m1_main = m_basePanel.m_processMenus.menus[0].img_main;
            Image m1_side = m_basePanel.m_processMenus.menus[0].img_side;

            Image m2_bg = m_basePanel.m_processMenus.menus[1].btn_menu.GetComponent<Image>();
            Image m2_main = m_basePanel.m_processMenus.menus[1].img_main;
            Image m2_side = m_basePanel.m_processMenus.menus[1].img_side;

            Image m3_bg = m_basePanel.m_processMenus.menus[2].btn_menu.GetComponent<Image>();
            Image m3_main = m_basePanel.m_processMenus.menus[2].img_main;
            Image m3_side = m_basePanel.m_processMenus.menus[2].img_side;

            m_uiResources.m_processMenu.SetImage(index,
                m1_bg, m1_main, m1_side,
                m2_bg, m2_main, m2_side,
                m3_bg, m3_main, m3_side);
        }

        #endregion
    }
}
