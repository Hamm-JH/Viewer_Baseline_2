using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace UI
{
    /// <summary>
    /// 현재 패널과, 상태값을 받아, 보여줄 상태를 판별하는 열거변수
    /// </summary>
    public enum targetIssue
    {
        Null,
        Damage,
        Recover,
        Reinforcement,
        Image,
        R2
    }

    public enum ColumnType
    {
        DamageInfo,
        DamagePart,
        DamageLocation,
        Date,
        RepairMethod,
        ReinforcementMethod,
        Image
    }

    public enum PanelType
    {
        NULL,
        BPMM,
        BPM1,
        BPM2
    }

    public enum DragDirection
    {
        X,
        Y,
        TemplateX,
        TemplateY
    }

    public enum DragVector
    {
        TopToBottom,
        BottomToTop,
        LeftToRight,
        RightToLeft
    }

    public enum DragMethod
    {
        MouseWheel,
        ScrollBar
    }

    [System.Serializable]
    public class DragCall
    {
        public DragDirection direction;
        public DragVector vector;
        public DragMethod method;
        public RectTransform targetTransform;
        public float moveVelocity;
        public float dragResist;

        public float overTextHeight;

        public DragCall()
        {
            direction = DragDirection.X;
            vector = DragVector.TopToBottom;
            method = DragMethod.MouseWheel;
            targetTransform = null;
            moveVelocity = 0;
            dragResist = 20f;

            overTextHeight = 0;
        }

        //public DragCall(DragDirection _direction, DragVector _vector, RectTransform _target)
        //{
        //    direction = _direction;
        //    vector = _vector;
        //    targetTransform = _target;
        //}

        public void Set(DragDirection _direction, DragVector _vector, RectTransform _target, DragMethod _method, float _moveVelocity = 0, float _dragResist = 20f, float _overTextHeight = 0)
        {
            direction = _direction;
            vector = _vector;
            targetTransform = _target;
            method = _method;
            moveVelocity = _moveVelocity;
            dragResist = _dragResist;
            overTextHeight = _overTextHeight;
        }
    }
}