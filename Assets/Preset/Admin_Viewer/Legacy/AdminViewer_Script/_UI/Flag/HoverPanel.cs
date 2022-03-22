using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

public class HoverPanel : MonoBehaviour
{
    //[SerializeField] TextMeshProUGUI text;
    //[SerializeField] TextMeshProUGUI background;

    [SerializeField] Text backText;
    [SerializeField] Text targetText;

    private void Start()
    {
        SetText("Hello");
    }

    public void SetText(string value)
    {
        StringBuilder builder = new StringBuilder();
        if(value != "")
        {
            builder.Append(" ");
            builder.Append(value);
            builder.Append(" ");
            
            backText.text = builder.ToString();
            targetText.text = builder.ToString();
        }
        else
        {
            backText.text = value;
            targetText.text = value;
        }
    }
}
