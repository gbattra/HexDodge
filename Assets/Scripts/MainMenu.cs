using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject TutorialPage;
    public GameObject GameModeMenu;
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
        gameObject.SetActive(false);
        TutorialPage.SetActive(true);
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
