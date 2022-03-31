using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace UI
{
    public class GraphCode : MonoBehaviour
    {
        public TextMeshProUGUI indexText;

        public IEnumerator SetIndexText(int index)
        {
            yield return new WaitForEndOfFrame();

            indexText.text = string.Format("{0:00}", index);

            yield break;
        }
    }
}
