using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public enum ViewSceneStatus
    {
        Awake = 0,
        LoadObjects = 1,
        InitializeObject = 2,

        Ready = 3,
        ViewAllDamage = 4,      // 2 : 교량 손상 정보

        ViewPartDamage = 5,     // 3 : 부재별 손상 정보
        ViewPart2R = 6,         // 4 : 부재별 보수보강 정보
        ViewMaintainance = 7,   // 5 : 교량 유지관리 정보
    }
}
 