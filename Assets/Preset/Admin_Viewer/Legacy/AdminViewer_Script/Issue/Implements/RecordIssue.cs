using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using SimpleJSON;

namespace Issue
{
    public class RecordIssue : AIssue
    {
        public string koreanName;

        public string dcRemark;

        private List<Dictionary<KeyOfRecords, string>> imgIndexes;

        public override void SetObject<T>(T _data)
        {
            //switch(_data)
            //{
            //    case JSONNode __node:
            //        ParseValue(__node);
            //        break;
            //}
        }

        public void SetKoreanName()
        {
            string _bridgePartName = Tunnel.TunnelConverter.GetName(BridgePartName); // MODBS_Library.BridgeCodeConverter.ConvertCode(BridgePartName);
            string _6surfName = Convert6Surface(Issue6Surfaces);
            string _9Location = Convert9Location(dcLocation);
            string _issueName = ConvertIssueCode(IssueCodes);

            koreanName = string.Format($"{0} {1} {2} {3}", _bridgePartName, _6surfName, _9Location, _issueName);
        }

        //private void ParseValue(JSONNode _node)
        //{
        //    for (int i = 0; i < Enum.GetValues(typeof(Issue.KeyOfRecords)).Length; i++)
        //    {
        //        switch((Issue.KeyOfRecords)i)
        //        {
        //            case Issue.KeyOfRecords.cdTunnelDamaged:
        //                IssueOrderCode = _node[KeyOfRecords.cdTunnelDamaged.ToString()].Value;
        //                break;

        //            case Issue.KeyOfRecords.fgDA001:
        //                IssueCodes = ParseIssueCode(_node[KeyOfRecords.fgDA001.ToString()].Value);
        //                break;

        //            case Issue.KeyOfRecords.cdTunnelParts:
        //                BridgePartName = _node[KeyOfRecords.cdTunnelParts.ToString()].Value;
        //                break;

        //            case Issue.KeyOfRecords.dcDamageMemberSurface:
        //                dcDamageMemberSurface = Parse6Surface(_node[KeyOfRecords.dcDamageMemberSurface.ToString()].Value);
        //                break;

        //            case Issue.KeyOfRecords.dcLocation:
        //                dcLocation = int.Parse(_node[KeyOfRecords.dcLocation.ToString()].Value);
        //                break;

        //            case Issue.KeyOfRecords.dcPinLocation:
        //                dcPinLocation = ParseVector(_node[KeyOfRecords.dcPinLocation.ToString()].Value);
        //                break;

        //            case Issue.KeyOfRecords.dcRemark:
        //                Description = _node[KeyOfRecords.dcRemark.ToString()].Value;
        //                break;

        //            case Issue.KeyOfRecords.dcGrade:
        //                dcGrade = _node[KeyOfRecords.dcGrade.ToString()].Value;
        //                break;

        //            case Issue.KeyOfRecords.noDamageWidth:
        //                IssueWidth = ParseValue(_node[KeyOfRecords.noDamageWidth.ToString()].Value);
        //                break;

        //            case Issue.KeyOfRecords.noDamageHeight:
        //                IssueHeight = ParseValue(_node[KeyOfRecords.noDamageHeight.ToString()].Value);
        //                break;

        //            case Issue.KeyOfRecords.noDamageDepth:
        //                IssueDepth = ParseValue(_node[KeyOfRecords.noDamageDepth.ToString()].Value);
        //                break;

        //            case Issue.KeyOfRecords.fgroup:
        //                FGroup = _node[KeyOfRecords.fgroup.ToString()].Value;
        //                break;

        //            case Issue.KeyOfRecords.dtCheck:
        //                DTCheck = _node[KeyOfRecords.dtCheck.ToString()].Value;
        //                break;

        //            case Issue.KeyOfRecords.dtEnd:
        //                DTEnd = _node[KeyOfRecords.dtEnd.ToString()].Value;
        //                break;

        //            case Issue.KeyOfRecords.nmUser:
        //                NmUser = _node[KeyOfRecords.nmUser.ToString()].Value;
        //                break;

        //            case Issue.KeyOfRecords.files:
        //                JSONArray _files = _node[KeyOfRecords.files.ToString()].AsArray;
        //                Dictionary<KeyOfRecords, string> _indic;

        //                if(ImgIndexes == null)
        //                {
        //                    ImgIndexes = new List<Dictionary<KeyOfRecords, string>>();
        //                }

        //                if(_files != null)
        //                {
        //                    int index = _files.Count;
        //                    for (int ii = 0; ii < index; ii++)
        //                    {
        //                        _indic = new Dictionary<KeyOfRecords, string>();
        //                        _indic.Add(KeyOfRecords.fid, _files[ii][KeyOfRecords.fid.ToString()].Value);
        //                        _indic.Add(KeyOfRecords.ftype, _files[ii][KeyOfRecords.ftype.ToString()].Value);
        //                        ImgIndexes.Add(_indic);
        //                    }
        //                }
        //                break;
        //        }
        //    }
        //}

        #region 6면 정보
        private Issue.Issue6Surface Parse6Surface(string code)
        {
            switch (code)
            {
                case "Top": return Issue.Issue6Surface.Top;
                case "Bottom": return Issue.Issue6Surface.Bottom;
                case "Front": return Issue.Issue6Surface.Front;
                case "Back": return Issue.Issue6Surface.Back;
                case "Left": return Issue.Issue6Surface.Left;
                case "Right": return Issue.Issue6Surface.Right;
                default: return Issue.Issue6Surface.Null;
            }
        }

        private string Convert6Surface(Issue.Issue6Surface code)
        {
            switch (code)
            {
                case Issue6Surface.Top:     return "윗면";
                case Issue6Surface.Bottom:  return "아랫면";
                case Issue6Surface.Front:   return "앞면";
                case Issue6Surface.Back:    return "뒷면";
                case Issue6Surface.Left:    return "왼쪽면";
                case Issue6Surface.Right:   return "오른쪽면";
                default: return "";
            }
        }
        #endregion

        #region 9면정보
        private int ParseValue(string text)
        {
            int result = 0;

            if (int.TryParse(text, out result))
            {
                return result;
            }
            else
            {
                return 0;
            }
        }

        private string Convert9Location(int code)
        {
            string result = $"{code}번위치";
            return result;
        }
        #endregion

        private Issue.IssueCode ParseIssueCode(string code)
        {
            switch (code)
            {
                case "0001": return IssueCode.Crack;
                case "0007": return IssueCode.Efflorescense;
                case "0009": return IssueCode.Spalling;
                case "0013": return IssueCode.Scour_Erosion;
                case "0022": return IssueCode.Breakage;
                default: return IssueCode.Null;
            }
        }

        private string ConvertIssueCode(Issue.IssueCode code)
        {
            switch(code)
            {
                case IssueCode.Crack:           return "균열";
                case IssueCode.Efflorescense:   return "박리박락";
                case IssueCode.Spalling:        return "백태";
                case IssueCode.Breakage:        return "파손";
                case IssueCode.Scour_Erosion:   return "세굴,침식";
                default:    return "";
            }
        }


        private Vector3 ParseVector(string stringVector)
        {
            string[] args = stringVector.Split(',');

            Vector3 result = new Vector3(
                float.Parse(args[0]),
                float.Parse(args[1]),
                float.Parse(args[2])
                );

            return result;
        }
    }
}
