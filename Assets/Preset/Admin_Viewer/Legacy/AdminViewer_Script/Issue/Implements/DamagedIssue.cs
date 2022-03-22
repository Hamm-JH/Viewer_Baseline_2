using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Issue
{
    public class DamagedIssue : AIssue
    {
        #region 변수
        //public string dtCheck;

        private List<Dictionary<KeyOfDamages, string>> imgIndexes;
        #endregion

        #region 속성
        public string CheckDate
        {
            get => dtCheck;
            set => dtCheck = value;
        }

        public List<Dictionary<KeyOfDamages, string>> ImgIndexes
        {
            get => imgIndexes;
            set => imgIndexes = value;
        } // 이미지 인덱스 리스트
        #endregion

        protected override void init()
        {
            CheckDate = "";
            ImgIndexes = new List<Dictionary<KeyOfDamages, string>>();
        }

        public override void SetObject<T>(T _data)
        {
            switch(_data)
            {
                case Dictionary<KeyOfDamages, string> _dic:
                    {
                        ParseValue(_dic);
                    }
                    break;

                case List<Dictionary<KeyOfDamages, string>> _imgIndexes:
                    {
                        ParseValue(_imgIndexes);
                    }
                    break;
            }
        }

        private void ParseValue(Dictionary<KeyOfDamages, string> _dic)
        {
            for (int i = 0; i < Enum.GetValues(typeof(KeyOfDamages)).Length; i++)
            {
                if(_dic.ContainsKey((KeyOfDamages)i))
                {
                    switch((KeyOfDamages)i)
                    {
                        case KeyOfDamages.cdTunnel:
                            cdBridge = _dic[KeyOfDamages.cdTunnel];
                            break;

                        case KeyOfDamages.cdTunnelDamaged:
                            issueOrderCode = _dic[KeyOfDamages.cdTunnelDamaged];
                            break;

                        case KeyOfDamages.fgDA001:
                            fgDA001 = ParseIssueCode(_dic[KeyOfDamages.fgDA001]);
                            break;

                        case KeyOfDamages.nmUser:
                            nmUser = _dic[KeyOfDamages.nmUser];
                            break;

                        case KeyOfDamages.cdTunnelParts:
                            cdBridgeParts = _dic[KeyOfDamages.cdTunnelParts];
                            break;

                        case KeyOfDamages.dcDamageMemberSurface:
                            dcDamageMemberSurface = Parse6Surface(_dic[KeyOfDamages.dcDamageMemberSurface]);
                            break;

                        case KeyOfDamages.dcLocation:
                            dcLocation = int.Parse(_dic[KeyOfDamages.dcLocation]);
                            break;

                        case KeyOfDamages.dcPinLocation:
                            dcPinLocation = ParseVector(_dic[KeyOfDamages.dcPinLocation]);
                            break;

                        case KeyOfDamages.dtCheck:
                            dtCheck = _dic[KeyOfDamages.dtCheck];
                            break;

                        case KeyOfDamages.fgroup:
                            fgroup = _dic[KeyOfDamages.fgroup];
                            break;

                        case KeyOfDamages.dcGrade:
                            IssueGrade = _dic[KeyOfDamages.dcGrade];
                            break;

                        case KeyOfDamages.noDamageWidth:
                            IssueWidth = ParseValue(_dic[KeyOfDamages.noDamageWidth]);
                            break;

                        case KeyOfDamages.noDamageHeight:
                            IssueHeight = ParseValue(_dic[KeyOfDamages.noDamageHeight]);
                            break;

                        case KeyOfDamages.noDamageDepth:
                            IssueDepth = ParseValue(_dic[KeyOfDamages.noDamageDepth]);
                            break;

                        case KeyOfDamages.dcRemark:
                            Description = _dic[KeyOfDamages.dcRemark];
                            break;
                    }
                }
            }
        }

        private int ParseValue(string text)
        {
            int result = 0;

            if(int.TryParse(text, out result))
            {
                return result;
            }
            else
            {
                return 0;
            }
        }

        private void ParseValue(List<Dictionary<KeyOfDamages, string>> _imgIndexes)
        {
            ImgIndexes = _imgIndexes;
        }

        private Issue.IssueCode ParseIssueCode(string code)
        {
            switch(code)
            {
                case "0001":    return IssueCode.Crack;
                case "0007":    return IssueCode.Efflorescense;
                case "0009":    return IssueCode.Spalling;
                case "0013":    return IssueCode.Scour_Erosion;
                case "0022":    return IssueCode.Breakage;
                default:        return IssueCode.Null;
            }
        }

        private Issue.Issue6Surface Parse6Surface(string code)
        {
            switch(code)
            {
                case "Top":     return Issue.Issue6Surface.Top;
                case "Bottom": return Issue.Issue6Surface.Bottom;
                case "Front": return Issue.Issue6Surface.Front;
                case "Back": return Issue.Issue6Surface.Back;
                case "Left": return Issue.Issue6Surface.Left;
                case "Right": return Issue.Issue6Surface.Right;
                default:        return Issue.Issue6Surface.Null;
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
