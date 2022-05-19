using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition
{
    public static partial class Issues
    {
        public static void WP_Setup_ALL(
            List<Definition._Issue.Issue> _dmgs,
            List<Definition._Issue.Issue> _rcvs,
            List<Definition._Issue.Issue> _all)
        {
            _all.ForEach(x =>
            {
                x.Waypoint.IssueWayPoint.ToggleMain(true);
                x.Waypoint.IssueWayPoint.ToggleFx(true);
            });
        }

        public static void WP_Setup_Dmgs(
            List<Definition._Issue.Issue> _dmgs,
            List<Definition._Issue.Issue> _rcvs,
            List<Definition._Issue.Issue> _all)
        {
            _dmgs.ForEach(x =>
            {
                x.Waypoint.IssueWayPoint.ToggleMain(true);
                x.Waypoint.IssueWayPoint.ToggleFx(true);
            });

            _rcvs.ForEach(x =>
            {
                x.Waypoint.IssueWayPoint.ToggleMain(true);
                x.Waypoint.IssueWayPoint.ToggleFx(false);
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

        public static void WP_Setup_Object(
            List<Definition._Issue.Issue> _dmgs,
            List<Definition._Issue.Issue> _rcvs,
            List<Definition._Issue.Issue> _all,
            GameObject _target)
        {

        }
    }
}
