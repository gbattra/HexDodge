using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Timers;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Level : MonoBehaviour
{
    public bool IsTimed;
    public float TotalTime;
    private float TimeRemaining => TotalTime * 60f - Timer.Elapsed.Seconds;
    
    public GameObject MapPrefab;
    public GameObject PlayerPrefab;
    public GameObject CameraPrefab;
    public GameObject LevelCanvasPrefab;

    private GameObject Map => _map;
    private GameObject _map;

    public GameObject Player => _player;
    private GameObject _player;
    
    private GameObject Camera => _camera;
    private GameObject _camera;

    public GameObject LevelCanvas => _levelCanvas;
    private GameObject _levelCanvas;
    
    public int TileCount => _tileCount;
    private int _tileCount;
    
    public Stopwatch Timer => _timer;
    private Stopwatch _timer = new Stopwatch();
    
    private string DirectionMoved;

    public bool GameOver => _player != null && !_player.GetComponent<Player>().Alive;

    public void Awake()
    {
        _levelCanvas = Instantiate(LevelCanvasPrefab);
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
        _timer.Start();
    }

    public void FixedUpdate()     
    {
        Debug.Log(TimeRemaining);
        _levelCanvas.GetComponent<LevelCanvas>().SetTime(Timer.Elapsed);
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
        _levelCanvas.GetComponent<LevelCanvas>().HandleItemImpact(player);
    }

    private void IncrementTileCount()
    {
        _tileCount += 1;
        _levelCanvas.GetComponent<LevelCanvas>().SetTileCount(_tileCount);
    }
}
