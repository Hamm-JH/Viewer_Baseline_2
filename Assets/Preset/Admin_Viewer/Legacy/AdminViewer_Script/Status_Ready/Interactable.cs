using MGS.UCamera;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//using MODBS_Library;

public class Interactable : MonoBehaviour
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
                _partName = _partName.Split('_')[1];

                if (Enum.TryParse<CodeLv4>(_partName, out _lv4))
                {
                    lv4 = _lv4;
                }
            }
            // 보수정보일 경우
            else if (_issues[i].GetType().Equals(typeof(Issue.RecoveredIssue)))
            {
                charIndex = 'R';

                CodeLv4 _lv4 = CodeLv4.Null;
                string _partName = _issues[i].BridgePartName;
                _partName = _partName.Split('_')[1];

                if (Enum.TryParse<CodeLv4>(_partName, out _lv4))
                {
                    lv4 = _lv4;
                }
            }

            // lv4 코드가 할당 완료된 경우
            if (lv4 != null)
            {
                // 손상/보수정보일 경우
                if (charIndex == 'D' || charIndex == 'R')
                {
                    _index[charIndex][(CodeLv4)lv4]++;
                }
            }
        }

        SetData(_index);
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
