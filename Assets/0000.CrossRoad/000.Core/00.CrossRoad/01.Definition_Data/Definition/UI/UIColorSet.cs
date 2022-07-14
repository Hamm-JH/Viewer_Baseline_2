using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="ScriptableObjects/UIColorSet", order = 1)]
public class UIColorSet : ScriptableObject
{
    /// <summary>
    /// �⺻ �����ڵ� ���ø�
    /// </summary>
    public Color color_default;

    /// <summary>
    /// ȣ���� �����ڵ� ���ø�
    /// </summary>
    public Color color_hover;

    /// <summary>
    /// ���� �����ڵ� ���ø�
    /// </summary>
    public Color color_select;

    /// <summary>
    /// ���� ���� �����ڵ� ���ø�
    /// </summary>
    public Color color_deselect;

}
