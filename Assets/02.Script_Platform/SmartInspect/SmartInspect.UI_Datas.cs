﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmartInspect
{
    using TMPro;
    using UnityEngine.UI;

    /// <summary>
    /// 단일 프로세스 메뉴
    /// </summary>
    [System.Serializable]
    public class RProcessMenu
    {
        public Button btn_menu;
        public List<Image> imgs;
        public List<TextMeshProUGUI> txts;
    }

    #region Module elements

    [System.Serializable]
    public class ModuleElement
    {
        public List<GameObject> m_mElements;

        public DamageElement m_dmgElement;
        public RecoverElement m_rcvElement;
        public AdministElement m_admElement;
    }

    [System.Serializable]
    public class DamageElement
    {
        public GameObject root;

        /// <summary>
        /// 부재별 손상정보 패널
        /// </summary>
        public GameObject m_dmgCount;

        /// <summary>
        /// 부재별 손상정보 리스트 패널
        /// </summary>
        public GameObject m_dmgList;

        /// <summary>
        /// 부재별 특정 손상의 자세한 정보 패널
        /// </summary>
        public GameObject m_dmgInformation;

        /// <summary>
        /// 왼쪽 버튼 바 리스트
        /// </summary>
        public List<RProcessMenu> m_leftbar;

        /// <summary>
        /// 리스트 요소들
        /// </summary>
        //public List<ListElement> m_listElements;

        /// <summary>
        /// 점검정보 파트별 카운트 패널
        /// </summary>
        public List<IssueCountPanel> m_issueCountPanels;

        /// <summary>
        /// 점검정보 리스트 요소들
        /// </summary>
        public List<IssueListPanel> m_issueListPanels;

        /// <summary>
        /// 자세한 점검정보 패널
        /// </summary>
        public List<IssueDetailPanel> m_issueDetailPanels;
    }

    [System.Serializable]
    public class RecoverElement
    {
        public GameObject root;

        /// <summary>
        /// 부재별 보수정보 패널
        /// </summary>
        public GameObject m_rcvCount;

        /// <summary>
        /// 부재별 보수 리스트 패널
        /// </summary>
        public GameObject m_rcvList;

        /// <summary>
        /// 부재별 보강정보 패널
        /// </summary>
        public GameObject m_reinCount;

        /// <summary>
        /// 부재별 보강 리스트 패널
        /// </summary>
        public GameObject m_reinList;

        /// <summary>
        /// 왼쪽 버튼 바 리스트
        /// </summary>
        public List<RProcessMenu> m_leftbar;

        /// <summary>
        /// 리스트 요소들
        /// </summary>
        //public List<ListElement> m_listElements;

        /// <summary>
        /// 점검정보 파트별 카운트 패널
        /// </summary>
        public List<IssueCountPanel> m_issueCountPanels;

        /// <summary>
        /// 점검정보 리스트 요소들
        /// </summary>
        public List<IssueListPanel> m_issueListPanels;
    }

    [System.Serializable]
    public class AdministElement
	{
        public GameObject root;

        /// <summary>
        /// 왼쪽 버튼 바 리스트
        /// </summary>
        public List<RProcessMenu> m_leftbar;

        /// <summary>
        /// 종류별 집계
        /// </summary>
        public GameObject m_TotalMaintenance;

        /// <summary>
        /// 이력
        /// </summary>
        public GameObject m_MaintenanceTimeline;

        /// <summary>
        /// 종류별 집계 패널 인스턴스
        /// </summary>
        public TotalIssueCountPanel m_issueCountPanel;

        /// <summary>
        /// 타임라인 패널 인스턴스
        /// </summary>
        public TimelinePanel m_timelinePanel;
    }
    
    /// <summary>
    /// 점검정보 파트별 카운트 패널
    /// </summary>
    [System.Serializable]
    public class IssueCountPanel
    {
        public GameObject root;

        public ListElement m_listElement;
    }

    /// <summary>
    /// 점검정보 리스트 패널
    /// </summary>
    [System.Serializable]
    public class IssueListPanel
    {
        public GameObject root;

        public TextMeshProUGUI title;
        public string baseTitleName;

        public ListElement m_listElement;

    }

    /// <summary>
    /// 자세한 점검정보 패널
    /// </summary>
    [System.Serializable]
    public class IssueDetailPanel
    {
        public GameObject root;
        public TextMeshProUGUI m_titlePartName;
        public TextMeshProUGUI m_width;
        public TextMeshProUGUI m_height;
        public TextMeshProUGUI m_depth;
        public TextMeshProUGUI m_description;
    }

    /// <summary>
    /// 점검정보 종류별 집계
    /// </summary>
    [System.Serializable]
    public class TotalIssueCountPanel
	{
        public GameObject root;

        public List<Inspect_BarChart> m_barCharts;
	}

    /// <summary>
    /// 이력
    /// </summary>
    [System.Serializable]
    public class TimelinePanel
	{
        public GameObject root;

        public List<GraphChartFeed> m_graphFeeds;
    }

    #endregion

    #region Base elements

    /// <summary>
    /// 이벤트 베이스
    /// </summary>
    [System.Serializable]
    public class EventBase
    {
        /// <summary>
        /// 현재 인덱스
        /// </summary>
        [Header("모듈 상태정보")]
        public int m_index;
    }

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
    public class Zoom
    {
        public GameObject root;
        public GameObject zoomIn;
        public GameObject zoomOut;
    }

    #endregion

    #region General elements


    [System.Serializable]
    public class GeneralElement
    {
        /// <summary>
        /// 일반적인 패널들
        /// </summary>
        public List<GameObject> m_generalPanels;

        public Setting m_setting;
        public ObjectStatus m_objStatus;
        public ImagePopup m_imgPopup;
    }

    [System.Serializable]
    public class Setting
    {
        public GameObject root;

    }

    /// <summary>
    /// 시설물 현황정보창
    /// </summary>
    [System.Serializable]
    public class ObjectStatus
    {
        public GameObject root;
        public RawImage mainImage;
        public TextMeshProUGUI addressText;
    }

    /// <summary>
    /// 특정 부재의 보수 또는 보강 이미지 팝업 리스트 패널
    /// </summary>
    [System.Serializable]
    public class ImagePopup
    {
        public GameObject root;

        public TextMeshProUGUI m_date;

        public ListElement m_issueImagePanel;

    }

    #endregion
}
