using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmartInspect
{
    using TMPro;
    using UnityEngine.UI;

    #region Module elements

    [System.Serializable]
    public class ModuleElement
    {
        public List<GameObject> m_mElements;

        public DamageElement m_dmgElement;
        public RecoverElement m_rcvElement;
    }

    [System.Serializable]
    public class DamageElement
    {
        public GameObject root;
    }

    [System.Serializable]
    public class RecoverElement
    {
        public GameObject root;
    }

    #endregion

    #region Base elements

    /// <summary>
    /// 베이스
    /// </summary>
    [System.Serializable]
    public class Base
    {
        public Header m_header;
        public ProcessMenus m_processMenus;
        public ProfilePopup m_profilePopup;
    }

    /// <summary>
    /// 헤더
    /// </summary>
    [System.Serializable]
    public class Header
    {
        /// <summary>
        /// 시설물 텍스트 배경
        /// </summary>
        public GameObject objectBackground;

        /// <summary>
        /// 시설물 이름
        /// </summary>
        public TextMeshProUGUI objectName;

        /// <summary>
        /// 부재 텍스트 배경
        /// </summary>
        public GameObject partBackground;

        /// <summary>
        /// 부재 이름
        /// </summary>
        public TextMeshProUGUI partName;

        /// <summary>
        /// 프로필 버튼
        /// </summary>
        public Button btn_profile;
    }

    /// <summary>
    /// 프로세스 메뉴들
    /// </summary>
    [System.Serializable]
    public class ProcessMenus
    {
        public List<ProcessMenu> menus;
    }

    /// <summary>
    /// 단일 프로세스 메뉴
    /// </summary>
    [System.Serializable]
    public class ProcessMenu
    {
        public Button btn_menu;
        public Image img_main;
        public Image img_side;
        public TextMeshProUGUI txt_desc;
    }

    /// <summary>
    /// 프로필 팝업창
    /// </summary>
    [System.Serializable]
    public class ProfilePopup
    {
        /// <summary>
        /// 프로필 버튼박스
        /// </summary>
        public GameObject profileBox;

        /// <summary>
        /// 프로필 버튼
        /// </summary>
        public Button btn_profile;

        /// <summary>
        /// 프로필 로그아웃
        /// </summary>
        public Button btn_logOut;
    }

    #endregion

    #region BottomBar elements

    [System.Serializable]
    public class BottomBar
    {
        public List<GameObject> m_subPanels;

        public ViewPort m_viewport;
        public Orthographic m_orthographic;
        public Setting m_setting;
        public Zoom m_zoom;
    }

    [System.Serializable]
    public class ViewPort
    {
        public GameObject root;
        public GameObject btn_1ISO;
        public GameObject btn_2Top;
        public GameObject btn_3Side;
        public GameObject btn_4Bottom;
    }

    [System.Serializable]
    public class Orthographic
    {
        public GameObject root;
        public GameObject btn_1Orthographic;
        public GameObject btn_2Perspective;
    }

    [System.Serializable]
    public class Setting
    {
        public GameObject root;

    }

    [System.Serializable]
    public class Zoom
    {
        public GameObject root;
        public GameObject zoomIn;
        public GameObject zoomOut;
    }

    #endregion
}
