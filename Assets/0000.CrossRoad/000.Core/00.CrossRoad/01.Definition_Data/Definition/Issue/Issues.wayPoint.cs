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
        private static void Setup(bool _isOnMain, bool _isOnFx, Definition._Issue.Issue _target)
        {
            _target.Waypoint.IssueWayPoint.ToggleMain(_isOnMain);
            _target.Waypoint.IssueWayPoint.ToggleFx(_isOnFx);
        }

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

        public static void WP_Setup()
        {
            Module_Model model = ContentManager.Instance.Module<Module_Model>();
            List<Definition._Issue.Issue> dmgs = model.DmgData;
            List<Definition._Issue.Issue> rcvs = model.RcvData;
            List<Definition._Issue.Issue> all = model.AllIssues;

            EventStatement.State_DemoWebViewer state = EventManager.Instance._Statement.GetState_DemoWebViewer(0);

            WP_Setup(dmgs, rcvs, all, state.IsDmgTab);
        }

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
        /// <param name="_dmgs"></param>
        /// <param name="_rcvs"></param>
        /// <param name="_all"></param>
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
        /// <param name="_dmgs"></param>
        /// <param name="_rcvs"></param>
        /// <param name="_all"></param>
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
        /// <param name="_dmgs"></param>
        /// <param name="_rcvs"></param>
        /// <param name="_all"></param>
        /// <param name="_targetName"></param>
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
        /// <param name="_dmgs"></param>
        /// <param name="_rcvs"></param>
        /// <param name="_all"></param>
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
        /// <param name="_dmgs"></param>
        /// <param name="_rcvs"></param>
        /// <param name="_all"></param>
        /// <param name="_targetName"></param>
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

        /// <summary>
        /// 특정객체 On 
        /// TODO :: Issues.wayPoint :: WP_Setup_Object :: 존재 검토
        /// </summary>
        /// <param name="_dmgs"></param>
        /// <param name="_rcvs"></param>
        /// <param name="_all"></param>
        /// <param name="_target"></param>
        public static void WP_Setup_Object(
            List<Definition._Issue.Issue> _dmgs,
            List<Definition._Issue.Issue> _rcvs,
            List<Definition._Issue.Issue> _all,
            GameObject _target)
        {

        }
    }
}
