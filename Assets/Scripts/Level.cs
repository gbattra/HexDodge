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
    public GameObject PlayerPrefab;
    public GameObject CameraPrefab;

    private GameObject Map => _map;
    private GameObject _map;

    public GameObject Player => _player;
    private GameObject _player;
    
    private GameObject Camera => _camera;
    private GameObject _camera;

    [SerializeField]
    public LevelCanvas LevelCanvas;
    
    public int TileCount => _tileCount;
    private int _tileCount;
    
    public Stopwatch Timer => _timer;
    private Stopwatch _timer = new Stopwatch();

    private int _highScore;
    
    private string DirectionMoved;

    public bool GameOver =>
        IsTimed && TimeRemaining <= 0 ||
        _player != null && !_player.GetComponent<Player>().Alive;

    public void Awake()
    {
        _highScore = PlayerPrefs.GetInt(
            $"{SceneManager.GetActiveScene().name}_highScore", 0);
        _map = Instantiate(MapPrefab, transform, true);
        _map.transform.parent = transform;
        _player = Instantiate(
            PlayerPrefab, 
            _map.GetComponent<Map>().CurrentTile.transform.position,
            PlayerPrefab.transform.rotation);
        _player.transform.parent = transform;
        
        _camera = Instantiate(CameraPrefab,
            transform.position,
            Quaternion.identity);
        _camera.transform.parent = transform;
        _camera.GetComponent<FollowCamera>().Follow = _player;
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
            LevelCanvas.SetTime($"{min}:{sec}");

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
        if (_player.GetComponent<Player>().IsMoving)
            return;
        
        var map = _map.GetComponent<Map>();
        var selectedTile = map.SelectedTile;
        map.SetLastDirectionMoved(selectedTile);
        map.MoveToTile();
        _player.GetComponent<Player>().MoveTo(selectedTile);
        IncrementTileCount();
    }

    public void HandleTileImpact()
    {
        if (_player.GetComponent<Player>().IsMoving)
            return;
        
        var player = _player.GetComponent<Player>();
        var selectedTile = _map.GetComponent<Map>().SelectedTile;
        if (!player.CanHandleTile(selectedTile))
            StartCoroutine(_camera.GetComponent<FollowCamera>().Shake());
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
