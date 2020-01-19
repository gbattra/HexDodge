using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject TutorialPage;
    
    public void Play()
    {
        SceneManager.LoadScene("Endless");
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
