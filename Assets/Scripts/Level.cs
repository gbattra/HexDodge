using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Timers;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

public class Level : MonoBehaviour
{
    public bool IsTimed;
    public float TotalTime;
    private double TimeRemaining => TotalTime - Timer.Elapsed.TotalSeconds;

    public GameObject MapPrefab;
    
    public GameObject Player;
    public GameObject Camera;

    private GameObject _map;

    [SerializeField]
    public LevelCanvas LevelCanvas;
    
    public int TileCount => _tileCount;
    private int _tileCount;
    
    public Stopwatch Timer => _timer;
    private Stopwatch _timer = new Stopwatch();

    private int _highScore;
    
    private string DirectionMoved;

    public bool GameOver =>
        IsTimed && TimeRemaining < 1 ||
        Player != null && !Player.GetComponent<Player>().Alive;

    public void Awake()
    {
        _highScore = PlayerPrefs.GetInt(
            $"{SceneManager.GetActiveScene().name}_highScore", 0);
        _map = Instantiate(MapPrefab, transform, true);
        _map.transform.parent = transform;
        Player.transform.position = _map.GetComponent<Map>().CurrentTile.transform.position;
    }

    public void Start()
    {
        Camera.transform.Rotate(Vector3.forward, -90);
    }

    public void HandleGameOver()
    {
        Timer.Stop();
        LevelCanvas.StopCountdown();
        LevelCanvas.GameOverSprite.SetActive(true);
        LevelCanvas.RestartButton.SetActive(true);
        if (_tileCount > _highScore)
        {
            PlayerPrefs.SetInt($"{SceneManager.GetActiveScene().name}_highScore", _tileCount);
            LevelCanvas.NewHighScoreText.gameObject.SetActive(true);
            FindObjectOfType<AudioManager>().Play("HappyGameOver");
        }
        else
        {
            FindObjectOfType<AudioManager>().Play("SadGameOver");
        }
    }

    public void StartTimer()
    {
        _timer.Start();
    }

    public void FixedUpdate()
    {
        if (!IsTimed)
            LevelCanvas.SetTime(Timer.Elapsed.ToString(@"m\:ss"));
        else
        {
            var min = Math.Floor(TimeRemaining / 60);
            var sec = Math.Floor(TimeRemaining % 60);
            if (min + sec > 0)
                LevelCanvas.SetTime($"{min}:{(sec < 10 ? "0" + sec : sec.ToString())}");
            else
            {
                LevelCanvas.SetTime("0:00");
            }

            if (min == 0 && sec <= 20 && sec > 10)
                LevelCanvas.SetTimerColor(Color.yellow);
            if (min == 0 && sec <= 10)
                LevelCanvas.SetTimerColor(Color.red);
        }

    }

    public void SetSelected(GameObject toTile)
    {
        _map.GetComponent<Map>().SetSelected(toTile);
    }

    public void MoveToTile()
    {
        if (Player.GetComponent<Player>().IsMoving)
            return;
        
        var map = _map.GetComponent<Map>();
        var selectedTile = map.SelectedTile;
        map.SetLastDirectionMoved(selectedTile);
        map.MoveToTile();
        Player.GetComponent<Player>().MoveTo(selectedTile);
        IncrementTileCount();
    }

    public void HandleTileImpact()
    {
        if (Player.GetComponent<Player>().IsMoving)
            return;
        
        var player = Player.GetComponent<Player>();
        var selectedTile = _map.GetComponent<Map>().SelectedTile;
        if (!player.CanHandleTile(selectedTile))
            StartCoroutine(Camera.GetComponent<FollowCamera>().Shake());
        player.HandleItemImpact(selectedTile);
        LevelCanvas.HandleItemImpact(player);
    }

    private void IncrementTileCount()
    {
        _tileCount += 1;
        if (_tileCount > _highScore)
            LevelCanvas.HasNewHighScore = true;
        LevelCanvas.SetTileCount(_tileCount);
    }
}
