using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tracker : MonoBehaviour
{
    public string LabelText;
    public string ValueText;
    
    public GameObject BackgroundPrefab;
    public GameObject LabelPrefab;
    public GameObject ValuePrefab;

    public GameObject Background => _background;
    private GameObject _background;

    public GameObject Label => _label;
    private GameObject _label;

    public GameObject Value => _value;
    private GameObject _value;

    public void Awake()
    {
        _background = Instantiate(BackgroundPrefab, transform, false);
        _label = Instantiate(LabelPrefab, transform, false);
        _value = Instantiate(ValuePrefab, transform, false);
        
        _label.GetComponent<Text>().text = LabelText;
        _value.GetComponent<Text>().text = ValueText;
    }

    public void SetLabelAndValue(string label, string value)
    {
        _label.GetComponent<Text>().text = label;
        _value.GetComponent<Text>().text = value;
    }
}
