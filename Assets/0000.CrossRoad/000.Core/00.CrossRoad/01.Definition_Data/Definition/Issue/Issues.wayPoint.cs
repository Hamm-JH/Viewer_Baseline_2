using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition
{
    public static partial class Issues
    {
        private static void Setup(bool _isOnMain, bool _isOnFx, Definition._Issue.Issue _target)
        {
            _target.Waypoint.IssueWayPoint.ToggleMain(_isOnMain);
            _target.Waypoint.IssueWayPoint.ToggleFx(_isOnFx);
        }

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

        public static void WP_Setup_Dmgs_WithTarget(
            List<Definition._Issue.Issue> _dmgs,
            List<Definition._Issue.Issue> _rcvs,
            List<Definition._Issue.Issue> _all,
            GameObject _target)
        {
            _dmgs.ForEach(x =>
            {
                if(_target != null)
                {
                    if(_target.name == x.CdBridgeParts)
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
                    Setup(true, true, x);
                }
            });

            _rcvs.ForEach(x =>
            {
                Setup(false, false, x);
            });
        }

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

        public static void WP_Setup_Rcvs_WithTarget(
            List<Definition._Issue.Issue> _dmgs,
            List<Definition._Issue.Issue> _rcvs,
            List<Definition._Issue.Issue> _all,
            GameObject _target)
        {
            _dmgs.ForEach(x =>
            {
                Setup(false, false, x);
            });

            _rcvs.ForEach(x =>
            {
                if(_target != null)
                {
                    if(_target.name == x.CdBridgeParts)
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
                    Setup(true, true, x);
                }
            });
        }

        public static void WP_Setup_Object(
            List<Definition._Issue.Issue> _dmgs,
            List<Definition._Issue.Issue> _rcvs,
            List<Definition._Issue.Issue> _all,
            GameObject _target)
        {

        }
    }
}
