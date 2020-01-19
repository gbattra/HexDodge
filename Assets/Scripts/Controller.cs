using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public bool North => Input.GetKeyDown(KeyCode.UpArrow);
    public bool East => Input.GetKeyDown(KeyCode.RightArrow);
    public bool South => Input.GetKeyDown(KeyCode.DownArrow);
    public bool Enter => Input.GetKeyDown(KeyCode.Return);
    public bool MouseDown => Input.GetMouseButtonDown(0);
    public Vector2 TouchPosition => Input.mousePosition;
}
