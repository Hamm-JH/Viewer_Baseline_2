using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI
{
    public class DmgGraphCode : MonoBehaviour
    {
        public Image img;
        public TextMeshProUGUI indexText;

        public IEnumerator SetIndex(float distance, int index)
        {
            yield return new WaitForEndOfFrame();

            RectTransform imgRect = img.GetComponent<RectTransform>();

            imgRect.sizeDelta = new Vector2(
                distance,
                imgRect.sizeDelta.y
                );

            indexText.text = string.Format("{0:00}", index);
        }
    }
}
