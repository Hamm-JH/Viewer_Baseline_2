using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition
{
    using Management;
    using Management.Events;
    using Module.Model;

    public static partial class Issues
    {
        /// <summary>
        /// 손상 정보 표시준비
        /// </summary>
        /// <param name="_isOnMain">WayPointIndicator에서 주 아이콘인가? true : 맞음</param>
        /// <param name="_isOnFx">WayPointIndicator에서 이펙트 아이콘인가? true : 맞음</param>
        /// <param name="_target">목표 손상정보 인스턴스</param>
        private static void Setup(bool _isOnMain, bool _isOnFx, Definition._Issue.Issue _target)
        {
            _target.Waypoint.IssueWayPoint.ToggleMain(_isOnMain);
            _target.Waypoint.IssueWayPoint.ToggleFx(_isOnFx);
        }

        /// <summary>
        /// WayPoint 인스턴스의 표시 목표에 한정해서 아이콘 상태 업데이트
        /// </summary>
        /// <param name="_targetName">목표 객체명</param>
        public static void WP_Setup_target(string _targetName)
        {
            Module_Model model = ContentManager.Instance.Module<Module_Model>();
            List<Definition._Issue.Issue> dmgs = model.DmgData;
            List<Definition._Issue.Issue> rcvs = model.RcvData;
            List<Definition._Issue.Issue> all = model.AllIssues;

            EventStatement.State_DemoWebViewer state = EventManager.Instance._Statement.GetState_DemoWebViewer(0);

            //Debug.Log($"targetName ; {_targetName}");
            //all.ForEach(x => Debug.Log($"partName : {x.CdBridgeParts}"));
            
            WP_Setup_target(dmgs, rcvs, all, state.IsDmgTab, _targetName);
        }

        /// <summary>
        /// WayPoint 인스턴스의 상태를 할당한다.
        /// </summary>
        public static void WP_Setup()
        {
            Module_Model model = ContentManager.Instance.Module<Module_Model>();
            List<Definition._Issue.Issue> dmgs = model.DmgData;
            List<Definition._Issue.Issue> rcvs = model.RcvData;
            List<Definition._Issue.Issue> all = model.AllIssues;

            EventStatement.State_DemoWebViewer state = EventManager.Instance._Statement.GetState_DemoWebViewer(0);

            WP_Setup(dmgs, rcvs, all, state.IsDmgTab);
        }

        /// <summary>
        /// WayPoint 인스턴스 리스트의 개별 요소의 활성화 여부를 처리한다.
        /// </summary>
        /// <param name="_dmgs">손상 POI 리스트</param>
        /// <param name="_rcvs">보수 POI 리스트</param>
        /// <param name="_all">모든 POI 리스트</param>
        /// <param name="_isDmg">현재 손상 정보를 활성화해야 하는가? true : 예</param>
        /// <param name="_targetName">특정 활성화 대상객체의 이름</param>
        public static void WP_Setup_target(
            List<Definition._Issue.Issue> _dmgs,
            List<Definition._Issue.Issue> _rcvs,
            List<Definition._Issue.Issue> _all,
            bool _isDmg,
            string _targetName)
        {
            // 손상인 경우
            if(_isDmg)
            {
                WP_Setup_Dmgs_WithTarget(_dmgs, _rcvs, _all, _targetName);
            }
            // 보수인 경우
            else
            {
                WP_Setup_Rcvs_WithTarget(_dmgs, _rcvs, _all, _targetName);
            }
        }

        /// <summary>
        /// 모든 객체 On/Off
        /// </summary>
        /// <param name="_dmgs">손상 POI 리스트</param>
        /// <param name="_rcvs">보수 POI 리스트</param>
        /// <param name="_all">모든 POI 리스트</param>
        public static void WP_Setup_ALL(
            List<Definition._Issue.Issue> _dmgs,
            List<Definition._Issue.Issue> _rcvs,
            List<Definition._Issue.Issue> _all)
        {
            _all.ForEach(x =>
            {
                Setup(true, true, x);
            });
        }

        /// <summary>
        /// _isDmg 값에 따라 손상 또는 보수의 이펙트를 On/Off한다
        /// </summary>
        /// <param name="_dmgs">손상 POI 리스트</param>
        /// <param name="_rcvs">보수 POI 리스트</param>
        /// <param name="_all">모든 POI 리스튼</param>
        /// <param name="_isDmg">손상인가? true : 손상 false : 보수</param>
        public static void WP_Setup(
            List<Definition._Issue.Issue> _dmgs,
            List<Definition._Issue.Issue> _rcvs,
            List<Definition._Issue.Issue> _all,
            bool _isDmg)
        {
            if(_isDmg)
            {
                WP_Setup_Dmgs(_dmgs, _rcvs, _all);
            }
            else
            {
                WP_Setup_Rcvs(_dmgs, _rcvs, _all);
            }
        }

        #region DMG

        /// <summary>
        /// 손상정보 On
        /// </summary>
        /// <param name="_dmgs">손상 POI 리스트</param>
        /// <param name="_rcvs">보수 POI 리스트</param>
        /// <param name="_all">모든 POI 리스트</param>
        public static void WP_Setup_Dmgs(
            List<Definition._Issue.Issue> _dmgs,
            List<Definition._Issue.Issue> _rcvs,
            List<Definition._Issue.Issue> _all)
        {
            _dmgs.ForEach(x =>
            {
                Setup(true, true, x);
            });

            _rcvs.ForEach(x =>
            {
                Setup(true, false, x);
            });
        }

        /// <summary>
        /// 손상정보 & 특정 객체 On
        /// </summary>
        /// <param name="_dmgs">손상 POI 리스트</param>
        /// <param name="_rcvs">보수 POI 리스트</param>
        /// <param name="_all">모든 POI 리스트</param>
        /// <param name="_targetName">특정 객체명</param>
        public static void WP_Setup_Dmgs_WithTarget(
            List<Definition._Issue.Issue> _dmgs,
            List<Definition._Issue.Issue> _rcvs,
            List<Definition._Issue.Issue> _all,
            string _targetName)
        {
            _dmgs.ForEach(x =>
            {
                if(_targetName != null)
                {
                    if(_targetName.Contains(x.CdBridgeParts))
                    //if(_targetName == x.CdBridgeParts)
                    {
                        Setup(true, true, x);
                    }
                    else
                    {
                        Setup(false, false, x);
                    }
                }
                else
                {
                    // 웹에서 탭을 눌렀을때 선택된 개체가 없는 경우가 있다.
                    //Debug.LogError($"name is null");
                    Setup(true, true, x);
                }
            });

            _rcvs.ForEach(x =>
            {
                if(_targetName != null)
                {
                    if(_targetName.Contains(x.CdBridgeParts))
                    //if(_targetName == x.CdBridgeParts)
                    {
                        Setup(true, false, x);
                    }
                    else
                    {
                        Setup(false, false, x);
                    }
                }
                else
                {
                    // 웹에서 탭을 눌렀을때 선택된 개체가 없는 경우가 있다.
                    //Debug.LogError($"name is null");
                    Setup(true, false, x);
                }
            });
        }

        #endregion

        #region RCV

        /// <summary>
        /// 보수정보 On
        /// </summary>
        /// <param name="_dmgs">손상 POI 리스트</param>
        /// <param name="_rcvs">보수 POI 리스트</param>
        /// <param name="_all">모든 POI 리스트</param>
        public static void WP_Setup_Rcvs(
            List<Definition._Issue.Issue> _dmgs,
            List<Definition._Issue.Issue> _rcvs,
            List<Definition._Issue.Issue> _all)
        {
            _dmgs.ForEach(x =>
            {
                x.Waypoint.IssueWayPoint.ToggleMain(true);
                x.Waypoint.IssueWayPoint.ToggleFx(false);
            });

            _rcvs.ForEach(x =>
            {
                x.Waypoint.IssueWayPoint.ToggleMain(true);
                x.Waypoint.IssueWayPoint.ToggleFx(true);
            });
        }

        /// <summary>
        /// 보수정보 & 특정 객체 On
        /// </summary>
        /// <param name="_dmgs">손상 POI 리스트</param>
        /// <param name="_rcvs">보수 POI 리스트</param>
        /// <param name="_all">모든 POI 리스트</param>
        /// <param name="_targetName">목표 객체명</param>
        public static void WP_Setup_Rcvs_WithTarget(
            List<Definition._Issue.Issue> _dmgs,
            List<Definition._Issue.Issue> _rcvs,
            List<Definition._Issue.Issue> _all,
            string _targetName)
        {
            _dmgs.ForEach(x =>
            {
                if(_targetName != null)
                {
                    if(_targetName == x.CdBridgeParts)
                    {
                        Setup(true, false, x);
                    }
                    else
                    {
                        Setup(false, false, x);
                    }
                }
                else
                {
                    // 웹에서 탭을 눌렀을때 선택된 개체가 없는 경우가 있다.
                    //Debug.LogError($"name is null");
                    Setup(true, false, x);
                }
            });

            _rcvs.ForEach(x =>
            {
                if(_targetName != null)
                {
                    if(_targetName == x.CdBridgeParts)
                    {
                        Setup(true, true, x);
                    }
                    else
                    {
                        Setup(false, false, x);
                    }
                }
                else
                {
                    // 웹에서 탭을 눌렀을때 선택된 개체가 없는 경우가 있다.
                    //Debug.LogError($"name is null");
                    Setup(true, true, x);
                }
            });
        }

        #endregion 
    }
}
