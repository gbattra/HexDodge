using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject TutorialPage;
    public GameObject HomeMenu;
    // public GameObject GameModeMenu;
    public TextMeshProUGUI HighScore;

    public void Awake()
    {
        HighScore.text = PlayerPrefs.GetInt("Endless_highScore", 0).ToString();
    }

    public void Play()
    {
        SceneManager.LoadScene("Endless");
        // gameObject.SetActive(false);
        // GameModeMenu.SetActive(true);
    }

    public void Tutorial()
    {
        HomeMenu.SetActive(false);
        TutorialPage.SetActive(true);
    }

    public void GotIt()
    {
        HomeMenu.SetActive(true);
        TutorialPage.SetActive(false);
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
