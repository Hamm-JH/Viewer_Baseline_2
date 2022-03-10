using UnityEngine;

[System.Serializable]
public class BlurOutlineSettings
{
    [SerializeField, ColorUsage(true, true)]
    private Color Color = Color.green;

    [SerializeField, Range(0, 32)]
    private float Width = 10;

    [SerializeField]
    private bool Blur = true;

    [SerializeField, Range(0, 64)]
    private float Intensity = 1;

    public float intensity
    {
        get { return (Blur) ? Intensity : 100f; }
        set { Intensity = value; }
    }

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

    public bool blur
    {
        get { return Blur; }
        set { Blur = value; }
    }
}