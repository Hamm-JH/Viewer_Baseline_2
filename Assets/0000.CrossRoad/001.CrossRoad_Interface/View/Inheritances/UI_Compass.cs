﻿using System.Collections;
using System.Collections.Generic;

namespace View
{
    using Definition;
    using Module.Graphic;
    using Module.UI;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class UI_Compass : Interactable,
        IPointerEnterHandler, IPointerExitHandler
    {
        #region Overrides
        public override GameObject Target
        {
            get { return gameObject; }
        }

        public override List<GameObject> Targets => throw new System.NotImplementedException();


        /// <summary>
        /// 값 변경시 실행
        /// </summary>
        /// <param name="_value"></param>
        public override void OnChangeValue(float _value)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
		/// 선택 해제
		/// </summary>
        public override void OnDeselect()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
		/// 선택 해제
		/// </summary>
        public override void OnDeselect<T1, T2>(T1 t1, T2 t2)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
		/// 선택 해제
		/// </summary>
        public override void OnSelect()
        {
            //Debug.Log($"I am {gameObject.name} click");
            RootUI.GetUIEvent<Compass_EventType>(eType_compass, this);
        }

        #endregion

        [SerializeField] private AUI rootUI;
        public AUI RootUI { get => rootUI; set => rootUI = value; }


        public UIColor m_uiColor;

        public Compass_EventType eType_compass;

        Button m_btn;

        public void Init(Compass_EventType _eType, AUI _aui, Module_Graphic _graphic)
        {
            eType_compass = _eType;
            // TODO :: 0607
            RootUI = _aui;

            if(_eType == Compass_EventType.Compass_Prev)
            {
                // TODO :: CHECK :: Module_Graphic 활성화 1 : 색 지정
                m_uiColor.color_default = new Color(0x2b / 255f, 0x70 / 255f, 0xc6 / 255f, 1);
                m_uiColor.color_hover = new Color(0x1a / 255f, 0x5f / 255f, 0xac / 255f, 0.8f);
                m_uiColor.color_select = new Color(0x1a / 255f, 0x5f / 255f, 0xac / 255f, 1);
                m_uiColor.color_deSelect = new Color(0xba / 255f, 0xba / 255f, 0xba / 255f, 1);
            }
            else if(_eType == Compass_EventType.Compass_Next)
            {
                m_uiColor.color_default     = Color.black;
                m_uiColor.color_hover       = Color.black;
                m_uiColor.color_select      = Color.black;
                m_uiColor.color_deSelect    = Color.black;
            }
        }   

        // Start is called before the first frame update
        void Start()
        {
            if (gameObject.TryGetComponent<Button>(out m_btn))
            {
                m_btn.onClick.AddListener(new UnityEngine.Events.UnityAction(OnSelect));
            }

            OnSwitchColor_buttonState();
        }

        private void OnEnable()
        {
            OnSwitchColor_buttonState();
        }

        /// <summary>
        /// 객체 호버링 진입
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerEnter(PointerEventData eventData)
        {
            //Debug.Log($"I am {gameObject.name} enter");
            m_btn.image.color = m_uiColor.color_hover;
        }

        /// <summary>
        /// 객체 호버링 종료 개치
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerExit(PointerEventData eventData)
        {
            //Debug.Log($"I am {gameObject.name} exit");
            OnSwitchColor_buttonState();
        }

        /// <summary>
        /// 버튼 상태에 따라 색상 변경
        /// </summary>
        private void OnSwitchColor_buttonState()
        {
            if (m_btn == null) return;

            if (m_btn != null)
            {
                if (m_btn.enabled)
                {
                    m_btn.image.color = m_uiColor.color_default;
                }
                else
                {
                    m_btn.image.color = m_uiColor.color_deSelect;
                }
            }
            else
            {
                m_btn.image.color = m_uiColor.color_default;
            }
        }
    }
}
