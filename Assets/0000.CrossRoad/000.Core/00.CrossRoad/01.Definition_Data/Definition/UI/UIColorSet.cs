using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="ScriptableObjects/UIColorSet", order = 1)]
public class UIColorSet : ScriptableObject
{
    /// <summary>
    /// 기본 색상코드 템플릿
    /// </summary>
    public Color color_default;

    /// <summary>
    /// 호버링 색상코드 템플릿
    /// </summary>
    public Color color_hover;

    /// <summary>
    /// 선택 생상코드 템플릿
    /// </summary>
    public Color color_select;

    /// <summary>
    /// 선택 해제 색상코드 템플릿
    /// </summary>
    public Color color_deselect;

}
