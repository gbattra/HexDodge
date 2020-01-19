using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject MainMenu;

    public void GotIt()
    {
        gameObject.SetActive(false);
        MainMenu.SetActive(true);
    }
}
