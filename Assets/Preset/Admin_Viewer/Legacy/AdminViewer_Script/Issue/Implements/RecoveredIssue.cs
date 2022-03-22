using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Issue
{
    public class RecoveredIssue : AIssue
    {
        #region 변수
        private string dtStart;
        private string dtEnd;

        private List<Dictionary<KeyOfRecovers, string>> imgIndexes;
        #endregion

        #region 속성
        public string StartDate
        {
            get => dtStart;
            set => dtStart = value;
        }

        public string EndDate
        {
            get => dtEnd;
            set => dtEnd = value;
        }

        public List<Dictionary<KeyOfRecovers, string>> ImgIndexes
        {
            get => imgIndexes;
            set => imgIndexes = value;
        }

        #endregion

        protected override void init()
        {
            StartDate = "";
            EndDate = "";

            ImgIndexes = new List<Dictionary<KeyOfRecovers, string>>();
        }

        public override void SetObject<T>(T _data)
        {
            switch(_data)
            {
                case Dictionary<KeyOfRecovers, string> _dic:
                    {
                        ParseValue(_dic);
                    }
                    break;

                case List<Dictionary<KeyOfRecovers, string>> _imgIndexes:
                    {
                        ParseValue(_imgIndexes);
                    }
                    break;
            }
        }

        private void ParseValue(Dictionary<KeyOfRecovers, string> _dic)
        {
            for (int i = 0; i < Enum.GetValues(typeof(KeyOfRecovers)).Length; i++)
            {
                if (_dic.ContainsKey((KeyOfRecovers)i))
                {
                    switch((KeyOfRecovers)i)
                    {
                        case KeyOfRecovers.cdTunnel:
                            cdBridge = _dic[KeyOfRecovers.cdTunnel];
                            break;

                        case KeyOfRecovers.cdTunnelRecover:
                            issueOrderCode = _dic[KeyOfRecovers.cdTunnelRecover];
                            
                            break;

                        case KeyOfRecovers.fgDA001:
                            fgDA001 = ParseIssueCode(_dic[KeyOfRecovers.fgDA001]);
                            break;

                        case KeyOfRecovers.nmUser:
                            nmUser = _dic[KeyOfRecovers.nmUser];
                            break;

                        case KeyOfRecovers.cdTunnelParts:
                            cdBridgeParts = _dic[KeyOfRecovers.cdTunnelParts];
                            break;

                        case KeyOfRecovers.dcDamageMemberSurface:
                            dcDamageMemberSurface = Parse6Surface(_dic[KeyOfRecovers.dcDamageMemberSurface]);
                            break;

                        case KeyOfRecovers.dcLocation:
                            //Debug.Log(_dic[KeyOfRecovers.dcLocation]);
                            //dcLocation = ParseLocation(_dic[KeyOfRecovers.dcLocation].Split('"')[1]);
                            dcLocation = int.Parse(_dic[KeyOfRecovers.dcLocation]);
                            break;

                        case KeyOfRecovers.dcPinLocation:
                            dcPinLocation = ParseVector(_dic[KeyOfRecovers.dcPinLocation]);
                            break;

                        case KeyOfRecovers.dtStart:
                            dtStart = _dic[KeyOfRecovers.dtStart];
                            break;

                        case KeyOfRecovers.dtEnd:
                            dtEnd = _dic[KeyOfRecovers.dtEnd];
                            break;

                        case KeyOfRecovers.dcGrade:
                            IssueGrade = _dic[KeyOfRecovers.dcGrade];
                            break;

                        case KeyOfRecovers.noDamageWidth:
                            IssueWidth = ParseValue(_dic[KeyOfRecovers.noDamageWidth]);
                            break;

                        case KeyOfRecovers.noDamageHeight:
                            IssueHeight = ParseValue(_dic[KeyOfRecovers.noDamageHeight]);
                            break;

                        case KeyOfRecovers.noDamageDepth:
                            IssueDepth = ParseValue(_dic[KeyOfRecovers.noDamageDepth]);
                            break;

                        case KeyOfRecovers.dcRemark:
                            Description = _dic[KeyOfRecovers.dcRemark];
                            break;
                    }
                }
            }
        }

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

        private void ParseValue(List<Dictionary<KeyOfRecovers, string>> _imgIndexes)
        {
            ImgIndexes = _imgIndexes;
        }

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
