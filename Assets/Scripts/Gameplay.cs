using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using UnityEngine;


[RequireComponent(typeof(Gameplay))]
public class Gameplay : MonoBehaviour
{
    public bool CountdownFinished;
    public bool IsTimed;
    public float TotalTime;

    public Controller Controller;
    
    [SerializeField]
    public Level Level;
    
    private Controller _controller;

    public GameObject AudioManagerPrefab;
    public GameObject AudioManager => _audioManager;
    private GameObject _audioManager;
    
    public void Awake()
    {
        _controller = Controller;
        Level.IsTimed = IsTimed;
        Level.TotalTime = TotalTime;
        _audioManager = Instantiate(AudioManagerPrefab, transform, true);
    }

    public void Start()
    {
        StartCoroutine(Countdown());
    }

    public void Update()
    {
        if (Level.GameOver && !Level.LevelCanvas.GameOverSprite.active)
        {
            Level.HandleGameOver();
            Destroy(Level.Player);
        }
        else if (!Level.GameOver && CountdownFinished)
        {
            HandleController();
        }
    }
    
    public IEnumerator Countdown()
    {
        var i = 3;
        do
        {
            Level.LevelCanvas.Countdown.SetText($"{i}");
            FindObjectOfType<AudioManager>().Play("Beep");
            i -= 1;
            yield return new WaitForSeconds(1);
        } while ( i > 0 );

        if (!CountdownFinished)
            CountdownFinished = true;
        FindObjectOfType<AudioManager>().Play("Go");
        Level.LevelCanvas.Countdown.SetText("GO!");
        yield return new WaitForSeconds(1);
            
        Level.StartTimer();
        Destroy(Level.LevelCanvas.CountdownGameObject);
        yield return null;
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
                    Level.Player.transform.LookAt(hit.transform);
                    Level.SetSelected(hit.collider.gameObject);
                    Level.HandleTileImpact();
                    Level.MoveToTile();
                }
            }
        }
    }
}
