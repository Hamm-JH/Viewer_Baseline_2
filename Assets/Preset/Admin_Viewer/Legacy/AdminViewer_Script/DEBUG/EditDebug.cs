#if UNITY_EDITOR

#define DEBUG_INITIALIZE

#define DEBUG_INITJSON
#define DEBUG_INITJSON_IMG
#define DEBUG_SCENESTATUS
#define DEBUG_LoadJSON
#define DEBUG_OBJ
#define DEBUG_PANEL
#define DEBUG_INDICATOR
#define DEBUG_ELEMENT
#define DEBUG_ELEMENT_BP3
#define DEBUG_CLICK

// Element 디버깅 선언
#define DEBUG_TP_EVENT
#define DEBUG_MP2_EVENT
#define DEBUG_BP1_EVENT
#define DEBUG_BP2_EVENT
#define DEBUG_BP3_EVENT
#define DEBUG_BPL1_EVENT
#define DEBUG_KM_EVENT
#define DEBUG_IMG_EVENT

// Event 디버깅 선언
#define EVENT_DEFAULT
#define EVENT_3DOBJECT

#define EVENT_DMG
#define EVENT_RCV

#define EVENT_TP
#define EVENT_MP2
#define EVENT_BP1
#define EVENT_BP2
#define EVENT_BP3

// Image 받아오기 디버깅 선언
#define DEBUG_JSON_IMAGE

//=======================================

//#undef DEBUG_INITIALIZE

//#undef DEBUG_INITJSON
#undef DEBUG_INITJSON_IMG
#undef DEBUG_SCENESTATUS
#undef DEBUG_LoadJSON
#undef DEBUG_OBJ
#undef DEBUG_PANEL
#undef DEBUG_INDICATOR
#undef DEBUG_ELEMENT
#undef DEBUG_ELEMENT_BP3
#undef DEBUG_CLICK

// Element 디버깅 해제
#undef DEBUG_TP_EVENT
#undef DEBUG_MP2_EVENT
#undef DEBUG_BP1_EVENT
#undef DEBUG_BP2_EVENT
#undef DEBUG_BP3_EVENT
#undef DEBUG_BPL1_EVENT
#undef DEBUG_KM_EVENT
#undef DEBUG_IMG_EVENT


// Event 디버깅 해제
#undef EVENT_DEFAULT
#undef EVENT_3DOBJECT

//#undef EVENT_DMG
//#undef EVENT_RCV

//#undef EVENT_TP
//#undef EVENT_MP2
//#undef EVENT_BP1
//#undef EVENT_BP2
//#undef EVENT_BP3

// Image 받아오기 디버깅 해제
//#undef DEBUG_JSON_IMAGE


#endif

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 에디터 단계에서 On/Off 가능한 디버깅 메서드 관리 클래스
/// </summary>
public static class EditDebug
{
    /// <summary>
    /// 초기화 단계 디버깅
    /// </summary>
    /// <param name="arg"></param>
    public static void PrintINITIALIZERoutine(string arg)
    {
#if DEBUG_INITIALIZE
        //Debug.Log(arg);
#endif
    }

    #region done
    /// <summary>
    /// 교량 초기화 타이밍에 URI 디버깅
    /// </summary>
    /// <param name="arg"></param>
    public static void PrintINITJSONRoutine(string arg)
    {
#if DEBUG_INITJSON
        Debug.Log(arg);
#endif
    }

    /// <summary>
    /// 이미지 수집용 데이터 분할정보 디버깅
    /// </summary>
    /// <param name="arg"></param>
    public static void PrintINITJSONIMGRoutine(string arg)
    {
#if DEBUG_INITJSON_IMG
        Debug.Log(arg);
#endif
    }

    /// <summary>
    /// SceneStatus의 상태 변화 디버깅
    /// </summary>
    /// <param name="arg"></param>
    public static void PrintStatusChangeRoutine(string arg)
    {
#if DEBUG_SCENESTATUS
        Debug.Log(arg);
#endif
    }

    /// <summary>
    /// 손상/보수 정보 받아오기 단계 확인 디버깅
    /// </summary>
    /// <param name="arg"></param>
    public static void PrintLoadJSONRoutine(string arg)
    {
#if DEBUG_LoadJSON
        Debug.Log(arg);
#endif
    }

    /// <summary>
    /// 객체 생성 단계 확인 디버깅
    /// </summary>
    /// <param name="arg"></param>
    public static void PrintOBJRoutine(string arg)
    {
#if DEBUG_OBJ
        Debug.Log(arg);
#endif
    }

    /// <summary>
    /// 켜져있는 패널 확인 디버깅
    /// </summary>
    /// <param name="arg"></param>
    public static void PrintPANELRoutine(string arg)
    {
#if DEBUG_PANEL
        Debug.Log(arg);
#endif
    }

    /// <summary>
    /// 표시기 내부 실행여부 확인 디버깅
    /// </summary>
    /// <param name="arg"></param>
    public static void PrintINDICATORRoutine(string arg)
    {
#if DEBUG_INDICATOR
        Debug.Log(arg);
#endif
    }

    /// <summary>
    /// 표시기 내부 Element 실행확인 디버깅
    /// </summary>
    /// <param name="arg"></param>
    public static void PrintElementRoutine(string arg)
    {
#if DEBUG_ELEMENT
        Debug.Log(arg);
#endif
    }

    /// <summary>
    /// 3번 아래 표시기 Element정보 디버깅
    /// </summary>
    /// <param name="arg"></param>
    public static void PrintElementBP3Routine(string arg)
    {
#if DEBUG_ELEMENT_BP3
        Debug.Log(arg);
#endif
    }

    /// <summary>
    /// 클릭 인터페이스 선택 객체 확인 디버깅
    /// </summary>
    /// <param name="arg"></param>
    public static void PrintCLICKRoutine(string arg)
    {
#if DEBUG_CLICK
        Debug.Log(arg);
#endif
    }

    public static void PrintTPEventRoutine(string arg)
    {
#if DEBUG_TP_EVENT
        Debug.Log(arg);
#endif
    }

    public static void PrintMP2EventRoutine(string arg)
    {
#if DEBUG_MP2_EVENT
        Debug.Log(arg);
#endif
    }

    public static void PrintBP1EventRoutine(string arg)
    {
#if DEBUG_BP1_EVENT
        Debug.Log(arg);
#endif
    }

    public static void PrintBP2EventRoutine(string arg)
    {
#if DEBUG_BP2_EVENT
        Debug.Log(arg);
#endif
    }

    public static void PrintBP3EventRoutine(string arg)
    {
#if DEBUG_BP3_EVENT
        Debug.Log(arg);
#endif
    }

    public static void PrintBPL1EventRoutine(string arg)
    {
#if DEBUG_BPL1_EVENT
        Debug.Log(arg);
#endif
    }

    public static void PrintKMEventRoutine(string arg)
    {
#if DEBUG_KM_EVENT
        Debug.Log(arg);
#endif
    }
    #endregion

    #region Event

    public static void PrintEvent_DEFAULT(string arg)
    {
#if EVENT_DEFAULT
        Debug.Log(arg);
#endif
    }

    public static void PrintEVENT_3DOBJECT(string arg)
    {
#if EVENT_3DOBJECT
        Debug.Log(arg);
#endif
    }

    public static void PrintEVENT_DMG(string arg)
    {
#if EVENT_DMG
        Debug.Log(arg);
#endif
    }

    public static void PrintEVENT_RCV(string arg)
    {
#if EVENT_RCV
        Debug.Log(arg);
#endif
    }

    public static void PrintEVENT_TP(string arg)
    {
#if EVENT_TP
        Debug.Log(arg);
#endif
    }

    public static void PrintEVENT_MP2(string arg)
    {
#if EVENT_MP2
        Debug.Log(arg);
#endif
    }

    public static void PrintEVENT_BP1(string arg)
    {
#if EVENT_BP1
        Debug.Log(arg);
#endif
    }

    public static void PrintEVENT_BP2(string arg)
    {
#if EVENT_BP2
        Debug.Log(arg);
#endif
    }

    public static void PrintEVENT_BP3(string arg)
    {
#if EVENT_BP3
        Debug.Log(arg);
#endif
    }

    public static void PrintIMG(string arg)
    {
#if DEBUG_IMG_EVENT
        Debug.Log(arg);
#endif
    }

#endregion
}
