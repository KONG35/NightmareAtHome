using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultItemUI : MonoBehaviour
{
    public Image Icon;
    public TextMeshProUGUI txt;


    public void SetItem(Sprite image, string text)
    {
        Icon.sprite = image;
        txt.text = text;
    }
}
