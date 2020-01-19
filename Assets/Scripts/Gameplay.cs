using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;


[RequireComponent(typeof(Gameplay))]
public class Gameplay : MonoBehaviour
{
    public bool IsTimed;
    public float TotalTime;

    public Controller Controller;
    public GameObject Level;
    
    private Controller _controller;
    private GameObject _level;

    public GameObject AudioManagerPrefab;
    public GameObject AudioManager => _audioManager;
    private GameObject _audioManager;
    
    public void Awake()
    {
        _controller = Controller;
        _level = Instantiate(Level, transform, true);
        _level.GetComponent<Level>().IsTimed = IsTimed;
        _level.GetComponent<Level>().TotalTime = TotalTime;
        _audioManager = Instantiate(AudioManagerPrefab, transform, true);
    }

    public void Update()
    {
        var level = _level.GetComponent<Level>();
        var canvas = level.LevelCanvas.GetComponent<LevelCanvas>();
        if (level.GameOver && !canvas.GameOverSprite.active)
        {
            FindObjectOfType<AudioManager>().Play("GameOver");
            level.Timer.Stop();
            canvas.GameOverSprite.SetActive(true);
            canvas.RestartButton.SetActive(true);
            Destroy(level.Player);
        }
        else if (!level.GameOver)
        {
            HandleController();
        }
    }

    public void HandleController()
    {
        if (_controller.MouseDown)
        {
            var ray = Camera.main.ScreenPointToRay(_controller.TouchPosition);
            if (Physics.Raycast(ray, out var hit))
            {
                if (hit.collider.CompareTag("Tile") && hit.collider.GetComponent<Tile>().IsAvailable)
                {
                    _level.GetComponent<Level>().Player.transform.LookAt(hit.transform);
                    _level.GetComponent<Level>().SetSelected(hit.collider.gameObject);
                    _level.GetComponent<Level>().HandleTileImpact();
                    _level.GetComponent<Level>().MoveToTile();
                }
            }
        }
    }
}
