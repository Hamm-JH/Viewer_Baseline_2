using Issue;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Indicator.Element
{
    public class MP2_ViewMaintainanceElement : AElement
    {
        #region 변수
        public TextMeshProUGUI partsName;
        public TextMeshProUGUI dateAndUser;

        public GameObject triangleObj;
        public GameObject mirrorMesh;
        public Camera overlayCamera;
        public Vector3[] vertices;
        public GameObject[] triangleVertices;
        public GameObject[] inputVertices;
        public TextMeshProUGUI[] dmgValues;
        public TextMeshProUGUI[] lengthUnit;
        public float[] dmgData = new float[3];
        //public GameObject TriangleMask;

        public Image icon;

        private string dmgRcvCheck = "";

        //전달 받을 데이터
        private string bridgePartName;
        private Issue6Surface issue6Surfaces;
        private string issue9Location;
        private string issueCodes;
        private string date;
        private string nmUser;


        private string issuePartsName;
        private string issueGrade;
        private float issueWidth;
        private float issueHeight;
        private float issueDepth;
        private string issueDescription;
        #endregion

        #region 이슈정보 Get & Set
        public override void SetElement(AIssue _issue, params object[] arg)
        {
            if(_issue.GetType().Equals(typeof(Issue.DamagedIssue)))
            {
                dmgRcvCheck = "damaged";
                Issue.DamagedIssue dmgIssue = _issue as Issue.DamagedIssue;

                date = dmgIssue.dtCheck;
                issueDescription = dmgIssue.Description;
                nmUser = dmgIssue.NmUser;
            }
            else if(_issue.GetType().Equals(typeof(Issue.RecoveredIssue)))
            {
                dmgRcvCheck = "recover";
                Issue.RecoveredIssue rcvIssue = _issue as Issue.RecoveredIssue;

                date = rcvIssue.EndDate;
                issueDescription = rcvIssue.Description;
                nmUser = rcvIssue.NmUser;
            }
            // 이 부분에서 레코드이슈가 데미지인지 리커버인지 분류해야됨
            else if (_issue.GetType().Equals(typeof(Issue.RecordIssue)))
            {
                if(_issue.IssueKinds == "D")
                {
                    dmgRcvCheck = "damaged";
                    Issue.RecordIssue dmgIssue = _issue as Issue.RecordIssue;

                    date = dmgIssue.DTCheck;
                    issueDescription = dmgIssue.Description;
                    nmUser = dmgIssue.NmUser;
                }
                else if(_issue.IssueKinds == "R")
                {
                    dmgRcvCheck = "recover";
                    Issue.RecordIssue rcvIssue = _issue as Issue.RecordIssue;

                    date = rcvIssue.DTEnd;
                    issueDescription = rcvIssue.Description;
                    nmUser = rcvIssue.NmUser;
                }
            }

            bridgePartName = Tunnel.TunnelConverter.GetName(_issue.BridgePartName); // MODBS_Library.BridgeCodeConverter.ConvertCode(_issue.BridgePartName).Split(' ')[0];
            issue6Surfaces = _issue.Issue6Surfaces;
            issue9Location = ConvertDamageLocation(_issue.Issue9Location);
            issueCodes = ConvertDamageType(_issue.IssueCodes.ToString());

            issueGrade = _issue.IssueGrade;
            issueWidth = _issue.IssueWidth;
            issueHeight = _issue.IssueHeight;
            issueDepth = _issue.IssueDepth;

            //부재에 맞게 ICON 변경
            //if (arg.Length != 0 && arg[0].GetType() == typeof(Dictionary<MODBS_Library.CodeLv4, Sprite>))
            //{
            //    SetIcon(_issue.BridgePartName, arg[0] as Dictionary<MODBS_Library.CodeLv4, Sprite>);
            //}

        }
        #endregion

        void Start()
        {
            //전달 받은 정보 배치
            SetInputData();

            int[] triangles = new[] { 0, 1, 2 };
            Mesh mesh = new Mesh();
            Vector2[] uvs = new[]
            {
            new Vector2(0f, 1f),
            new Vector2(0.5f, 0f),
            new Vector2(-0.5f, 0f)
        };

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uvs;
            triangleObj.AddComponent<MeshFilter>();
            triangleObj.AddComponent<MeshRenderer>();
            triangleObj.GetComponent<MeshFilter>().mesh = mesh;

            triangleObj.GetComponent<MeshRenderer>().material =
                Resources.Load<Material>(string.Format("Materials/Bridge/Triangle"));

            mirrorMesh.AddComponent<MeshFilter>();
            mirrorMesh.AddComponent<MeshRenderer>();
            mirrorMesh.GetComponent<MeshFilter>().sharedMesh = mesh;

            mirrorMesh.GetComponent<MeshRenderer>().material =
                Resources.Load<Material>(string.Format("Materials/Bridge/Triangle"));

            //SetTopText();
            SetDataTriangle();
            SetTriangle();
            //Instantiate(mirrorMesh, GameObject.Find("MirrorMeshPoint").transform);
            Instantiate(mirrorMesh, this.transform.GetChild(3).GetChild(2).GetChild(0));


            float screenHeight = Screen.height;

            int fhdHeight = 1080;
            float fhdOrthographicSize = 125f;

            float result = screenHeight * fhdOrthographicSize / fhdHeight;

            overlayCamera.orthographicSize = result;
        }

        #region 메서드
        //상단 제목, 날짜, 등록자 정보를 텍스트에 할당
        //private void SetTopText()
        //{
        //    string partName = MODBS_Library.BridgeCodeConverter.ConvertCodeDetailCode(
        //        argument: bridgePartName,
        //        surface6: issue6Surfaces.ToString(),
        //        location9: issue9Location,
        //        issueCode: issueCodes.ToString(), 
        //        MODBS_Library.OutOption.AdView_MP2_ViewIssueElement
        //        );

        //    partsName.text = partName;/* string.Format("{0}_{1}_{2}_{3}", bridgePartName, issue6Surfaces, issue9Location, issueCodes);*/
        //    dateAndUser.text = string.Format("{0} {1}", date, nmUser);
        //}

        //부재 아이콘 할당
        //private void SetIcon(string partName, Dictionary<MODBS_Library.CodeLv4, Sprite> dic)
        //{
        //    string[] args = partName.Split('_');

        //    MODBS_Library.CodeLv4 code4 = MODBS_Library.CodeLv4.Null;
        //    if (Enum.TryParse<MODBS_Library.CodeLv4>(args[1], out code4))
        //    {
        //        icon.sprite = dic[code4];
        //    }
        //}

        //이슈정보를 알맞은 텍스트에 할당
        public void SetInputData()
        {
            dmgData[0] = issueWidth;
            dmgData[1] = issueHeight;
            dmgData[2] = issueDepth;

            dmgValues[0].text = issueGrade;
            dmgValues[1].text = issueWidth.ToString();
            dmgValues[2].text = issueHeight.ToString();
            dmgValues[3].text = issueDepth.ToString();
            if(dmgRcvCheck == "damaged")
                dmgValues[4].text = string.Format("손상 내용\n {0}", issueDescription);
            else
                dmgValues[4].text = string.Format("보수 내용\n {0}", issueDescription);
        }

        //이슈정보의 가로, 세로, 깊이 값을 삼각형 꼭지점에 할당
        public void SetDataTriangle()
        {
            float maxData = dmgData[0];
            float distance1 = 0;
            float distance2 = 0;
            float distance3 = 0;

            inputVertices[0].transform.localPosition = new Vector3(0, -35f, 0);
            inputVertices[1].transform.localPosition = new Vector3(0, -35f, 0);
            inputVertices[2].transform.localPosition = new Vector3(0, -35f, 0);
            
            //데이터가 입력되는 점이 이동할 라인의 끝점을 바라보도록 세팅
            inputVertices[0].transform.LookAt(triangleVertices[0].transform, Vector3.forward);
            inputVertices[1].transform.LookAt(triangleVertices[1].transform, Vector3.forward);
            inputVertices[2].transform.LookAt(triangleVertices[2].transform, Vector3.forward);

            float line1Max = (float)Math.Sqrt(Math.Pow(inputVertices[0].transform.position.x - triangleVertices[0].transform.position.x, 2)
                + Math.Pow(inputVertices[0].transform.position.y - triangleVertices[0].transform.position.y, 2));
            float line2Max = (float)Math.Sqrt(Math.Pow(inputVertices[1].transform.position.x - triangleVertices[1].transform.position.x, 2)
                + Math.Pow(inputVertices[0].transform.position.y - triangleVertices[1].transform.position.y, 2));
            float line3Max = (float)Math.Sqrt(Math.Pow(inputVertices[2].transform.position.x - triangleVertices[2].transform.position.x, 2)
                + Math.Pow(inputVertices[0].transform.position.y - triangleVertices[2].transform.position.y, 2));

            //가장 큰 값으로 단위 재설정
            for (int i = 0; i < 3; i++)
                if (dmgData[i] > maxData)
                    maxData = dmgData[i];

            if (maxData == 0)
                maxData = 120f;

            //길이 변환
            distance1 = dmgData[0] * line1Max / maxData;
            distance2 = dmgData[1] * line2Max / maxData;
            distance3 = dmgData[2] * line3Max / maxData;
            SetLengthUnits(maxData);

            inputVertices[0].transform.Translate(Vector3.forward * distance1);
            inputVertices[1].transform.Translate(Vector3.forward * distance2);
            inputVertices[2].transform.Translate(Vector3.forward * distance3);
        }

        //단위값 설정
        private void SetLengthUnits(float maxunit)
        {
            lengthUnit[0].text = maxunit.ToString();
            lengthUnit[1].text = (maxunit * 3f / 4f).ToString();
            lengthUnit[2].text = (maxunit * 2f / 4f).ToString();
            lengthUnit[3].text = (maxunit * 1f / 4f).ToString();
        }

        //할당 받은 꼭지점으로 삼각형을 재배치
        private void SetTriangle()
        {
            //전달 받은 정보를 바탕으로 삼각형의 3꼭지점을 재배치
            vertices[0] = inputVertices[0].transform.localPosition;
            vertices[1] = inputVertices[1].transform.localPosition;
            vertices[2] = inputVertices[2].transform.localPosition;
            triangleObj.GetComponent<MeshFilter>().mesh.vertices = vertices;

            vertices[0] = inputVertices[0].transform.localPosition;
            vertices[1] = inputVertices[1].transform.localPosition;
            vertices[2] = inputVertices[2].transform.localPosition;
            mirrorMesh.GetComponent<MeshFilter>().sharedMesh.vertices = vertices;

            inputVertices[0].transform.rotation = Quaternion.Euler(0, 0, 0);
            inputVertices[1].transform.rotation = Quaternion.Euler(0, 0, 0);
            inputVertices[2].transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        //6면 정보 한글 변환
        public string ConvertDamageLocation(int issue6Surfaces)
        {
            switch (issue6Surfaces)
            {
                case 1:
                    return "상단좌측";
                case 2:
                    return "상단";
                case 3:
                    return "상단우측";
                case 4:
                    return "중앙좌측";
                case 5:
                    return "중앙";
                case 6:
                    return "중앙우측";
                case 7:
                    return "하단좌측";
                case 8:
                    return "하단";
                case 9:
                    return "하단우측";
            }
            return null;
        }

        //손상 종류 한글 변환
        public string ConvertDamageType(string IssueCodes)
        {
            switch (IssueCodes)
            {
                case "Crack":
                    return "균열";
                case "Efflorescense":
                    return "박리/박락";
                case "Spalling":
                    return "백태";
                case "Scour_Erosion":
                    return "세굴/침식";
                case "Breakage":
                    return "파손";
            }
            return null;
        }
    }
    #endregion
}
