using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tracker : MonoBehaviour
{
    public GameObject Label;
    public GameObject Value;

    public void SetLabelAndValue(string label, string value)
    {
        Label.GetComponent<TextMeshProUGUI>().text = label;
        Value.GetComponent<TextMeshProUGUI>().text = value;
    }

    public void SetColor(Color color)
    {
        Value.GetComponent<TextMeshProUGUI>().color = color;
    }
}
