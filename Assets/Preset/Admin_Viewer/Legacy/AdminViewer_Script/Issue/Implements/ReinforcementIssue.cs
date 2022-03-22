using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Issue
{
    public class ReinforcementIssue : AIssue
    {
        public override void SetObject<T>(T _data)
        {
            Debug.Log("(임시) 보강 객체 정보생성");
        }
    }
}
