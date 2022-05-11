using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="ScriptableObjects/UIColorSet", order = 1)]
public class UIColorSet : ScriptableObject
{

    public Color color_default;
    public Color color_hover;
    public Color color_select;
    public Color color_deselect;

}
