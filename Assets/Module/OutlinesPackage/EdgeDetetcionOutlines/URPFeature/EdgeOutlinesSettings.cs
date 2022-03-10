using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EdgeOutlinesSettings 
{
    [SerializeField, ColorUsage(true, true)]
    private Color Color = Color.green;

    [SerializeField, Range(0, 5)]
    private float Width = 2;

    [SerializeField, Range(0.2f, 40f)]
    private float DepthThreshold = 10;

    [SerializeField, Range(-2f, 10f)]
    private float SteepAngleThreshold = 10;

    [SerializeField, Range(1, 10f)]
    private float SteepAngleMultiplier = 10;

    [SerializeField, Range(0f, 3f)]
    private float NormalThreshold = 1;

    public Color color
    {
        get { return Color; }
        set { Color = value; }
    }

    public float width
    {
        get { return Width; }
        set { Width = value; }
    }

    public float depthThreshold
    {
        get { return DepthThreshold; }
        set { DepthThreshold = value; }
    }

    public float normalThreshold
    {
        get { return NormalThreshold; }
        set { NormalThreshold = value; }
    }

    public float steepAngleThreshold
    {
        get { return SteepAngleThreshold; }
        set { SteepAngleThreshold = value; }
    }

    public float steepAngleMultiplier
    {
        get { return SteepAngleMultiplier; }
        set { SteepAngleMultiplier = value; }
    }
}
