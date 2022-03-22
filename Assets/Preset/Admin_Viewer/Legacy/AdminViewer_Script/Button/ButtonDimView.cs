using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonDimView : MonoBehaviour
{
    public Image image;

    public void SetButtonImage(bool isViewSpec)
    {
        // 부재보기 단계로 돌아갈 경우
        if (isViewSpec)
        {
            image.sprite = Resources.Load<Sprite>("Icon/SceneChangeMenu/ICON_Ruler");
        }
        // 이력보기 단계로 진행할 경우
        else
        {
            image.sprite = Resources.Load<Sprite>("Icon/SceneChangeMenu/ICON_SelectRuler");
        }
    }
}
