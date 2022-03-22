using MGS.UCamera;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//using MODBS_Library;

public enum CodeLv4
{
    
    //GD = 1,
    //CB = 2,
    //SL = 3,
    //DS = 4,
    //GR = 5,
    //GF = 6,
    //JI = 7,
    //AB = 8,
    //PI = 9,
    //BE = 10,
    //FT = 11,

    /// <summary>
    /// 공동구
    /// </summary>
    _1UnderLine = 0,

    /// <summary>
    /// 날개벽
    /// </summary>
    _2SideWall = 1,

    /// <summary>
    /// 라이닝
    /// </summary>
    _3Lining = 2,

    /// <summary>
    /// 면벽
    /// </summary>
    _4LineWall = 3,

    /// <summary>
    /// 제트팬
    /// </summary>
    _5JetFan = 4,

    /// <summary>
    /// 배수로
    /// </summary>
    _6WaterLine = 5,

    /// <summary>
    /// 벽 내곽
    /// </summary>
    _7InWall = 6,

    /// <summary>
    /// 비상전화
    /// </summary>
    _8EmCall = 7,

    /// <summary>
    /// 사면
    /// </summary>
    _9Slope = 8,

    /// <summary>
    /// 소화전
    /// </summary>
    _10Fireplug = 9,

    /// <summary>
    /// 옹벽
    /// </summary>
    _11BaseWall = 10,

    /// <summary>
    /// 조명
    /// </summary>
    _12Light = 11,

    /// <summary>
    /// 중앙분리대
    /// </summary>
    _13Central = 12,

    /// <summary>
    /// 천장
    /// </summary>
    _14Ceiling = 13,

    /// <summary>
    /// 평행사면
    /// </summary>
    _15ParSlope = 14,

    /// <summary>
    /// 포장
    /// </summary>
    _16Paving = 15,

    /// <summary>
    /// 피난연결통로
    /// </summary>
    _17EmExit = 16,

    Null = 17,
}

public class BridgeRoot : MonoBehaviour
{

    public void SetCameraCenter()
    {
        Collider _collider = gameObject.GetComponent<Collider>();
        Transform _mainTarget = RuntimeData.RootContainer.Instance.mainCamTarget;
        AroundAlignCamera mainAroundCam = RuntimeData.RootContainer.Instance.mainCam;

        _mainTarget.position = _collider.bounds.center;

        float maxValue = 0f;
        if (maxValue <= _collider.bounds.size.x)
        {
            maxValue = _collider.bounds.size.x;
        }
        if (maxValue <= _collider.bounds.size.y)
        {
            maxValue = _collider.bounds.size.y;
        }
        if (maxValue <= _collider.bounds.size.z)
        {
            maxValue = _collider.bounds.size.z;
        }

        mainAroundCam.targetDistance = maxValue;
    }

    public void SetIssueData()
    {
        List<Issue.AIssue> _issues = RuntimeData.RootContainer.Instance.IssueObjectList;

        // 타입 받기
        // 부재 받기
        //CodeLv4
        Dictionary<char, Dictionary<CodeLv4, int>> _index = new Dictionary<char, Dictionary<CodeLv4, int>>();
        _index.Add('D', new Dictionary<CodeLv4, int>());
        _index.Add('R', new Dictionary<CodeLv4, int>());

        // 인덱싱용 Dictionary 생성
        int index = Enum.GetValues(typeof(CodeLv4)).Length;
        for (int i = 0; i < index; i++)
        {
            _index['D'].Add((CodeLv4)i, 0);
            _index['R'].Add((CodeLv4)i, 0);
        }

        index = _issues.Count;
        for (int i = 0; i < index; i++)
        {
            char charIndex = ' ';
            CodeLv4? lv4 = null;

            // 손상정보일 경우
            if (_issues[i].GetType().Equals(typeof(Issue.DamagedIssue)))
            {
                charIndex = 'D';

                CodeLv4 _lv4 = CodeLv4.Null;
                string _partName = _issues[i].BridgePartName;

                //Debug.Log(_partName);
                lv4 = GetCode(_partName);


                //char splitter = '_';
                //if(Manager.MainManager.Instance.AppUseCase == Manager.Definition.UseCase.Bridge)
				//{
                //    splitter = '_';
				//}
                //else if(Manager.MainManager.Instance.AppUseCase == Manager.Definition.UseCase.Tunnel)
				//{
                //    splitter = ',';
				//}
                //_partName = _partName.Split(splitter)[1];

                //if (Enum.TryParse<CodeLv4>(_partName, out _lv4))
                //{
                //    lv4 = _lv4;
                //}

                if(lv4 != CodeLv4.Null)
				{
                    _index[charIndex][(CodeLv4)lv4]++;
                }
            }
            // 보수정보일 경우
            else if (_issues[i].GetType().Equals(typeof(Issue.RecoveredIssue)))
            {
                charIndex = 'R';

                CodeLv4 _lv4 = CodeLv4.Null;
                string _partName = _issues[i].BridgePartName;

                lv4 = GetCode(_partName);

                //char splitter = '_';
                //if (Manager.MainManager.Instance.AppUseCase == Manager.Definition.UseCase.Bridge)
                //{
                //    splitter = '_';
                //}
                //else if (Manager.MainManager.Instance.AppUseCase == Manager.Definition.UseCase.Tunnel)
                //{
                //    splitter = ',';
                //}
                //_partName = _partName.Split(splitter)[1];
                //
                //if (Enum.TryParse<CodeLv4>(_partName, out _lv4))
                //{
                //    lv4 = _lv4;
                //}

                if(lv4 != CodeLv4.Null)
				{
                    _index[charIndex][(CodeLv4)lv4]++;
                }
            }

            // lv4 코드가 할당 완료된 경우
            //if (lv4 != null)
            //{
            //    // 손상/보수정보일 경우
            //    if (charIndex == 'D' || charIndex == 'R')
            //    {
            //        _index[charIndex][(CodeLv4)lv4]++;
            //    }
            //}
        }

        SetData(_index);
    }

    public CodeLv4 GetCode(string name)
	{
        CodeLv4 lv4 = CodeLv4.Null;

        if (name.Contains("Etc"))
        {
            if (name.Contains("T"))
            {
                lv4 = CodeLv4._12Light;
                //partImage.sprite = Resources.Load<Sprite>("Tunnel/ICON/조명");
            }
            else if (name.Contains("Ex"))
            {
                lv4 = CodeLv4._17EmExit;
                //partImage.sprite = Resources.Load<Sprite>("Tunnel/ICON/피난연결통로");
            }
            else if (name.Contains("_F"))
            {
                lv4 = CodeLv4._10Fireplug;
                //partImage.sprite = Resources.Load<Sprite>("Tunnel/ICON/소화전");
            }
            else if (name.Contains("_Ec"))
            {
                lv4 = CodeLv4._8EmCall;
                //partImage.sprite = Resources.Load<Sprite>("Tunnel/ICON/비상전화");
            }
        }
        else if (name.Contains("S_") || name.Contains("E_"))
        {
            if (name.Contains("_Div"))
            {
                lv4 = CodeLv4._13Central;
                //partImage.sprite = Resources.Load<Sprite>("Tunnel/ICON/중앙분리대");
            }
            else if (name.Contains("C_Ga"))
            {
                lv4 = CodeLv4._4LineWall;
                //partImage.sprite = Resources.Load<Sprite>("Tunnel/ICON/면벽");
            }
            else if (name.Contains("C_Sl") || name.Contains("_SlIn") || name.Contains("_SlOut"))
            {
                lv4 = CodeLv4._15ParSlope;
                //partImage.sprite = Resources.Load<Sprite>("Tunnel/ICON/평행사면");
            }
            else if (name.Contains("_GaIn") || name.Contains("R_Ga"))
            {
                lv4 = CodeLv4._11BaseWall;
                //partImage.sprite = Resources.Load<Sprite>("Tunnel/ICON/옹벽");
            }
            else if (name.Contains("_rW"))
            {
                lv4 = CodeLv4._2SideWall;
                //partImage.sprite = Resources.Load<Sprite>("Tunnel/ICON/날개벽");
            }
            else if (name.Contains("_Sl"))
            {
                lv4 = CodeLv4._9Slope;
                //partImage.sprite = Resources.Load<Sprite>("Tunnel/ICON/사면");
            }
        }
        else if (name.Contains("M_"))
        {
            if (name.Contains("_Ce"))
            {
                lv4 = CodeLv4._14Ceiling;
                //partImage.sprite = Resources.Load<Sprite>("Tunnel/ICON/천장(도로터널)");
            }
            else if (name.Contains("_Sw"))
            {
                lv4 = CodeLv4._7InWall;
                //partImage.sprite = Resources.Load<Sprite>("Tunnel/ICON/벽 내곽");
            }
            else if (name.Contains("_P"))
            {
                lv4 = CodeLv4._16Paving;
                //partImage.sprite = Resources.Load<Sprite>("Tunnel/ICON/포장");
            }
            else if (name.Contains("_Co"))
            {
                lv4 = CodeLv4._1UnderLine;
                //partImage.sprite = Resources.Load<Sprite>("Tunnel/ICON/공동구");
            }
            else if (name.Contains("_D"))
            {
                lv4 = CodeLv4._6WaterLine;
                //partImage.sprite = Resources.Load<Sprite>("Tunnel/ICON/배수로");
            }
        }
        else if (name.Contains("L_"))
        {
            lv4 = CodeLv4._12Light;
			//partImage.sprite = Resources.Load<Sprite>("Tunnel/ICON/조명");
		}

		Debug.LogError($"name : {name}, code : {lv4.ToString()}");


		//Debug.Log(name);

		return lv4;
	}

    private void SetData(Dictionary<char, Dictionary<CodeLv4, int>> _index)
    {
        // Ex)

        if (_index.ContainsKey('D'))
        {
            int index = _index['D'].Keys.Count;
            for (int i = 0; i < index; i++)
            {
                string lv4 = ((CodeLv4)i).ToString();       // 4레벨 부재코드의 이름
                if (_index['D'][(CodeLv4)i] != 0)           // 손상 정보의 해당 4레벨 코드에 속한 개수가 1 이상이면 
                {
                    int count = _index['D'][(CodeLv4)i];    // 해당 4레벨 부재에 관련된 카운트 받음

                    // lv4 정보 / count를 조합해 목표한 정보에 데이터 할당
                }
            }
        }

        if (_index.ContainsKey('R'))
        {
            int index = _index['R'].Keys.Count;
            for (int i = 0; i < index; i++)
            {
                string lv4 = ((CodeLv4)i).ToString();       // 4레벨 부재코드의 이름
                if (_index['R'][(CodeLv4)i] != 0)           // 보수 정보의 해당 4레벨 코드에 속한 개수가 1 이상이면 
                {
                    int count = _index['R'][(CodeLv4)i];    // 해당 4레벨 부재에 관련된 카운트 받음

                    // lv4 정보 / count를 조합해 목표한 정보에 데이터 할당
                }
            }
        }
        Manager.PositionManager.Instance.CheckIssueCount(_index, this.name);
    }
}
