using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelCanvas : MonoBehaviour
{
    [SerializeField]
    public Countdown Countdown;
    public GameObject CountdownGameObject;

    public bool HasNewHighScore
    {
        get => _hasNewHighScore;
        set
        {
            if (value && !_hasNewHighScore)
            {
                StartCoroutine(AnnounceHighScore());
                FindObjectOfType<AudioManager>().Play("AnnounceHighScore");
                TileTracker.GetComponent<Tracker>().Value.GetComponent<Text>().color = Color.cyan;
                _hasNewHighScore = value;
            }
        }
    }
    private bool _hasNewHighScore;
    
    public GameObject BoostBar;
    public GameObject RocketBar;
    public GameObject HealthBar;
    public GameObject TimeTracker;
    public GameObject TileTracker;
    public GameObject RestartButton;
    
    public GameObject NewHighScoreText;
    public GameObject MuteButton;
    public GameObject UnmuteButton;

    public TextMeshProUGUI TimeRemainingText;

    public GameObject GameOverSprite;

    private bool IsYellow;
    private bool IsRed;


    public void Awake()
    {
        TimeTracker.GetComponent<Tracker>().SetLabelAndValue("TIMER", "0:00");
        TileTracker.GetComponent<Tracker>().SetLabelAndValue("TILES", "0");
        RestartButton.GetComponent<Button>().onClick.AddListener(Restart);
    }

    public void SetTimerColor(Color color)
    {
        if (!IsYellow && color == Color.yellow)
        {
            StartCoroutine(AnnounceTimeRemaining(20));
            TimeTracker.GetComponent<Tracker>().SetColor(color);
            IsYellow = true;
        }

        if (!IsRed && color == Color.red)
        {
            StartCoroutine(AnnounceTimeRemaining(10));
            StartCoroutine("FinalCountdown");
            TimeTracker.GetComponent<Tracker>().SetColor(color);
            IsRed = true;
        }
    }

    public IEnumerator AnnounceTimeRemaining(int seconds)
    {
        FindObjectOfType<AudioManager>().Play("Alarm");
        TimeRemainingText.text = $"{seconds} SECONDS REMAINING!";
        TimeRemainingText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        TimeRemainingText.gameObject.SetActive(false);
        TimeRemainingText.text = "";
    }

    public void StopCountdown()
    {
        StopCoroutine("FinalCountdown");
    }

    private IEnumerator FinalCountdown()
    {
        var i = 10;
        do
        {
            FindObjectOfType<AudioManager>().Play("Beep");
            yield return new WaitForSeconds(1f);
            i -= 1;
        } while (i > 0);
        yield return null;
    }

    public IEnumerator AnnounceHighScore()
    {
        NewHighScoreText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        NewHighScoreText.gameObject.SetActive(false);
    }

    public void Restart()
    {
        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void Exit()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Mute()
    {
        UnmuteButton.SetActive(true);
        MuteButton.SetActive(false);
        FindObjectOfType<AudioManager>().IsMuted = true;
    }

    public void Unmute()
    {
        MuteButton.SetActive(true);
        UnmuteButton.SetActive(false);
        FindObjectOfType<AudioManager>().IsMuted = false;
    }

    public void SetTileCount(int count)
    {
        TileTracker.GetComponent<Tracker>().SetLabelAndValue("TILES", count.ToString());
    }

    public void SetTime(string value)
    {
        TileTracker.GetComponent<Tracker>().SetLabelAndValue(
            "TIMER",
            value);
    }

    public void HandleItemImpact(Player player)
    {
        BoostBar.GetComponent<StatusBar>().SetActive(player.BoostCount);
        RocketBar.GetComponent<StatusBar>().SetActive(player.RocketCount);
        HealthBar.GetComponent<StatusBar>().SetActive(player.HealthCount);
    }
}
