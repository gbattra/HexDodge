using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    public TextMeshProUGUI Text;

    public void SetText(string text)
    {
        Text.text = text;
    }
}
