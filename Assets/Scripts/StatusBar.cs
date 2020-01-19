using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatusBar : MonoBehaviour
{
    public List<GameObject> Items = new List<GameObject>();
    public int ActiveCount;

    public void Awake()
    {
        ActiveCount = Items.Count;
    }

    public void SetActive(int count)
    {
        foreach (var item in Items)
        {
            item.SetActive(false);
        }
        
        for (var i = 0; i < count; i++)
        {
            Items[i].SetActive(true);
        }
    }
}
