using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameModeMenu : MonoBehaviour
{
    public GameObject MainMenu;

    public void GoBack()
    {
        gameObject.SetActive(false);
        MainMenu.SetActive(true);
    }
    
    public void LoadBurstMode()
    {
        SceneManager.LoadScene("Burst");
    }

    public void LoadSprintMode()
    {
        SceneManager.LoadScene("Sprint");
    }

    public void LoadLongDistanceMode()
    {
        SceneManager.LoadScene("LongDistance");
    }

    public void LoadMarathonMode()
    {
        SceneManager.LoadScene("Marathon");
    }

    public void LoadEndless()
    {
        SceneManager.LoadScene("Endless");
    }
}
