using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameModeMenu : MonoBehaviour
{
    public GameObject MainMenu;

    public TextMeshProUGUI BurstHighScore;
    public TextMeshProUGUI SprintHighScore;
    public TextMeshProUGUI LongDistanceHighScore;
    public TextMeshProUGUI MarathonHighScore;
    public TextMeshProUGUI EndlessHighScore;

    public void Awake()
    {
        BurstHighScore.text = PlayerPrefs.GetInt("Burst_highScore", 0).ToString();
        SprintHighScore.text = PlayerPrefs.GetInt("Sprint_highScore", 0).ToString();
        LongDistanceHighScore.text = PlayerPrefs.GetInt("LongDistance_highScore", 0).ToString();
        MarathonHighScore.text = PlayerPrefs.GetInt("Marathon_highScore", 0).ToString();
        EndlessHighScore.text = PlayerPrefs.GetInt("Endless_highScore", 0).ToString();
    }

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
