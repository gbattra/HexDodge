using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject TutorialPage;
    public GameObject GameModeMenu;
    
    public void Play()
    {
        SceneManager.LoadScene("Endless");
        // gameObject.SetActive(false);
        // GameModeMenu.SetActive(true);
    }

    public void Tutorial()
    {
        gameObject.SetActive(false);
        TutorialPage.SetActive(true);
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
