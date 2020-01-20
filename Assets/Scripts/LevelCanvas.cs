using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelCanvas : MonoBehaviour
{
    public GameObject BoostBar;
    public GameObject RocketBar;
    public GameObject HealthBar;
    public GameObject TrackerPrefab;
    public GameObject RestartButtonPrefab;

    public GameObject MuteButton;
    public GameObject UnmuteButton;
    
    public Vector3 TimerPosition;
    public Vector3 CounterPosition;
    public Vector3 RestartButtonPosition;

    public GameObject Timer => _timer;
    private GameObject _timer;

    public GameObject Counter => _counter;
    private GameObject _counter;

    public GameObject RestartButton => _restartButton;
    private GameObject _restartButton;

    public GameObject GameOverPrefab;
    public GameObject GameOverSprite => _gameOverSprite;

    public float BoostBarOffsetX;
    public float RocketBarOffsetX;
    public float HealthBarOffsetY;

    private GameObject _boostBar;
    private GameObject _rocketBar;
    private GameObject _healthBar;
    private GameObject _gameOverSprite;

    public void Awake()
    {
        _boostBar = Instantiate(BoostBar, transform, false);
        _boostBar.transform.position += new Vector3(BoostBarOffsetX, 0, 0);
        
        _rocketBar = Instantiate(RocketBar, transform, false);
        _rocketBar.transform.position += new Vector3(RocketBarOffsetX, 0, 0);
         
        _healthBar = Instantiate(HealthBar, transform, false);
        _healthBar.transform.position += new Vector3(0, HealthBarOffsetY, 0);

        _gameOverSprite = Instantiate(GameOverPrefab, transform, false);
        _gameOverSprite.SetActive(false);

        _timer = Instantiate(TrackerPrefab, transform, false);
        _timer.GetComponent<Tracker>().SetLabelAndValue("TIMER", "0:00");
        _timer.transform.position += TimerPosition;

        _counter = Instantiate(TrackerPrefab, transform, false);
        _counter.GetComponent<Tracker>().SetLabelAndValue("TILES", "0");
        _counter.transform.position += CounterPosition;

        _restartButton = Instantiate(RestartButtonPrefab, transform, false);
        _restartButton.transform.position += RestartButtonPosition;
        _restartButton.GetComponent<Button>().onClick.AddListener(Restart);
        _restartButton.SetActive(false);
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
        _counter.GetComponent<Tracker>().SetLabelAndValue("TILES", count.ToString());
    }

    public void SetTime(TimeSpan elapsed)
    {
        _timer.GetComponent<Tracker>().SetLabelAndValue(
            "TIMER",
            elapsed.ToString(@"m\:ss"));
    }

    public void HandleItemImpact(Player player)
    {
        _boostBar.GetComponent<StatusBar>().SetActive(player.BoostCount);
        _rocketBar.GetComponent<StatusBar>().SetActive(player.RocketCount);
        _healthBar.GetComponent<StatusBar>().SetActive(player.HealthCount);
    }
}
