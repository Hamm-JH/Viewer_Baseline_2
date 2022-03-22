using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeFitter : MonoBehaviour
{
    public RectTransform target;
    public RectTransform source;

    // Update is called once per frame
    void Update()
    {
        float x = source.sizeDelta.x - 2;
        target.sizeDelta = new Vector2(x, source.sizeDelta.y);
    }
}
