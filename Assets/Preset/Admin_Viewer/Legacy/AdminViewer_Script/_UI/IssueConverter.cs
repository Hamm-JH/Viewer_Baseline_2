using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public static class IssueConverter
    {
        public static Sprite GetMainIcon(int temp = 0)
        {
            Sprite result = null;

            result = Resources.Load<Sprite>("Icon/BotPanel/ICON_DR");

            return result;
        }

        public static Sprite GetMainIcon<T>() where T : Issue.AIssue
        {
            Sprite result = null;

            //if(typeof(T).Equals(typeof(Issue.DamagedIssue)))
            //{
            //    result = Resources.Load<Sprite>("Icon/BotPanel/ICON_DamageTitle");
            //}
            //else if(typeof(T).Equals(typeof(Issue.RecoveredIssue)))
            //{
            //    result = Resources.Load<Sprite>("Icon/BotPanel/ICON_rcv");
            //}
            //else if(typeof(T).Equals(typeof(Issue.ReinforcementIssue)))
            //{
            //    result = Resources.Load<Sprite>("Icon/BotPanel/ICON_ReinforcementTitle");
            //}

            return result;
        }

        public static string GetMainTitle(int temp = 0)
        {
            string result = "";

            result = "손상 상세정보";

            return result;
        }

        public static string GetMainTitle<T>() where T : Issue.AIssue
        {
            string result = "";

            //if (typeof(T).Equals(typeof(Issue.DamagedIssue)))
            //{
            //    result = "손상정보";
            //}
            //else if (typeof(T).Equals(typeof(Issue.RecoveredIssue)))
            //{
            //    result = "보수정보";
            //}
            //else if (typeof(T).Equals(typeof(Issue.ReinforcementIssue)))
            //{
            //    result = "보강정보";
            //}

            return result;
        }

        public static Color GetMainCountColor<T>() where T : Issue.AIssue
        {
            Color result = new Color(0, 0, 0, 1);

            //if (typeof(T).Equals(typeof(Issue.DamagedIssue)))
            //{
            //    result = new Color(253 / 255f, 17 / 255f, 110 / 255f, 1);
            //}
            //else if (typeof(T).Equals(typeof(Issue.RecoveredIssue)))
            //{
            //    result = new Color(0x25/255f, 0x8c/255f, 0xeb/255f, 1);
            //}
            //else if (typeof(T).Equals(typeof(Issue.ReinforcementIssue)))
            //{
            //    result = new Color(0x55/255f, 0xd2/255f, 0x3c/255f, 1);
            //}

            return result;
        }

        // ======================================

        //public static targetIssue GetTargetIssue(UI.PanelType panelType, Manager.ViewSceneStatus sceneStatus)
        //{
        //    targetIssue target = targetIssue.Null;

        //    //switch(panelType)
        //    //{
        //    //    case PanelType.BPMM:
        //    //        if(sceneStatus.Equals(Manager.ViewSceneStatus.ViewAllDamage))
        //    //        {
        //    //            target = targetIssue.Damage;
        //    //        }
        //    //        break;

        //    //    case PanelType.BPM1:
        //    //        if(sceneStatus.Equals(Manager.ViewSceneStatus.ViewPartDamage))
        //    //        {
        //    //            target = targetIssue.Damage;
        //    //        }
        //    //        else if(sceneStatus.Equals(Manager.ViewSceneStatus.ViewPart2R))
        //    //        {
        //    //            target = targetIssue.Recover;
        //    //        }
        //    //        else if(sceneStatus.Equals(Manager.ViewSceneStatus.ViewMaintainance))
        //    //        {
        //    //            target = targetIssue.Damage;
        //    //        }
        //    //        break;

        //    //    case PanelType.BPM2:
        //    //        if (sceneStatus.Equals(Manager.ViewSceneStatus.ViewPartDamage))
        //    //        {
        //    //            target = targetIssue.Image;
        //    //        }
        //    //        else if (sceneStatus.Equals(Manager.ViewSceneStatus.ViewPart2R))
        //    //        {
        //    //            target = targetIssue.Reinforcement;
        //    //        }
        //    //        else if (sceneStatus.Equals(Manager.ViewSceneStatus.ViewMaintainance))
        //    //        {
        //    //            target = targetIssue.R2;
        //    //        }
        //    //        break;
        //    //}

        //    return target;
        //}

        #region Set Column
        public static Sprite GetColumnIcon(ColumnType type)
        {
            Sprite result = null;
            string baseLoot = "Icon/BotPanel/ColumnIcon/";

            switch (type)
            {
                case ColumnType.DamageLocation:
                    result = Resources.Load<Sprite>($"{baseLoot}ICON_DamageLocation");
                    break;

                case ColumnType.DamageInfo:
                    result = Resources.Load<Sprite>($"{baseLoot}ICON_DamageInfo");
                    break;

                case ColumnType.DamagePart:
                    result = Resources.Load<Sprite>($"{baseLoot}ICON_DamagePart");
                    break;

                case ColumnType.Date:
                    result = Resources.Load<Sprite>($"{baseLoot}ICON_Date");
                    break;

                case ColumnType.Image:
                    result = Resources.Load<Sprite>($"{baseLoot}ICON_Image");
                    break;

                case ColumnType.RepairMethod:
                    result = Resources.Load<Sprite>($"{baseLoot}ICON_RepairMethod");
                    break;

                case ColumnType.ReinforcementMethod:
                    result = Resources.Load<Sprite>($"{baseLoot}ICON_ReinforcementMethod");
                    break;
            }

            return result;
        }

        public static string GetColumnName<T>(ColumnType type) where T : Issue.AIssue
        {
            string result = "";

            //if (typeof(T).Equals(typeof(Issue.DamagedIssue)))
            //{
            //    result = "손상";
            //}
            //else if (typeof(T).Equals(typeof(Issue.RecoveredIssue)))
            //{
            //    result = "보수";
            //}
            //else if (typeof(T).Equals(typeof(Issue.ReinforcementIssue)))
            //{
            //    result = "보강";
            //}

            //switch (type)
            //{
            //    case ColumnType.DamageLocation:
            //        result += "위치";
            //        break;

            //    case ColumnType.DamageInfo:
            //        result += "정보";
            //        break;

            //    case ColumnType.DamagePart:
            //        result += "부재";
            //        break;

            //    case ColumnType.Date:
            //        if(typeof(T).Equals(typeof(Issue.DamagedIssue)))
            //        {
            //            result = "확인날짜";
            //        }
            //        else
            //        {
            //            result += "날짜";
            //        }
            //        break;

            //    case ColumnType.Image:
            //        result += "사진";
            //        break;

            //    case ColumnType.RepairMethod:
            //        result += "공법";
            //        break;

            //    case ColumnType.ReinforcementMethod:
            //        result += "공법";
            //        break;
            //}

            return result;
        }

        #endregion

        #region Set Record

        public static Sprite GetRecordImageIcon()
        {
            Sprite result = null;

            result = Resources.Load<Sprite>("Icon/BotPanel/RecordIcon/ICON_Image");

            return result;
        }

        #endregion
    }
}
