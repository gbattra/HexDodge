using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject BoostEffect;
    public GameObject RocketEffect;
    public GameObject DestroyEffect;
    
    public float Speed;
    
    public int StartingBoostCount;
    public int StartingRocketCount;
    public int StartingHealthCount;
    
    public int MaxBoost;
    public int MaxRocket;
    public int MaxHealth;
    

    public bool Alive => _alive;
    private bool _alive => HealthCount > 0;

    public bool IsMoving => _isMoving;
    private bool _isMoving;

    private int _boostCount;
    public int BoostCount
    {
        get => _boostCount;
        set
        {
            if (value < 0)
            {
                FindObjectOfType<AudioManager>().Play("HealthDown");
                _healthCount -= 1;
            }
            else
            {
                _boostCount = Mathf.Clamp(value, 0, MaxBoost);
            }
        }
    }
    
    private int _rocketCount;
    public int RocketCount
    {
        get => _rocketCount;
        set
        {
            if (value < 0)
            {
                FindObjectOfType<AudioManager>().Play("HealthDown");
                _healthCount -= 1;
            }
            else
            {
                _rocketCount = Mathf.Clamp(value, 0, MaxRocket);
            }
        }
    }

    private int _healthCount;
    public int HealthCount
    {
        get => _healthCount;
        set => _healthCount = Mathf.Clamp(value, 0, MaxHealth);
    }

    public void Awake()
    {
        _boostCount = StartingBoostCount;
        _rocketCount = StartingRocketCount;
        _healthCount = StartingHealthCount;
    }

    public void OnDestroy()
    {
        if (DestroyEffect != null)
            Instantiate(DestroyEffect, transform.position, transform.rotation);
    }

    public void MoveTo(GameObject tile)
    {
        if (_isMoving)
            return;
        
        StartCoroutine(MoveTo(tile.transform));
    }

    public bool CanHandleTile(GameObject tile)
    {
        var item = tile.GetComponent<Tile>().Item.tag;
        if (item == "Shield")
            return BoostCount > 0;

        if (item == "Mine")
            return RocketCount > 0;

        return true;
    }

    public void HandleItemImpact(GameObject tile)
    {
        var item = tile.GetComponent<Tile>().Item;
        if (item.CompareTag("Boost"))
        {
            item.GetComponent<Item>().ShouldPlayDestroyEffect = BoostCount < StartingBoostCount;
            item.GetComponent<Item>().ShouldPlaySoundEffect = BoostCount < StartingBoostCount;
            BoostCount += 1;
            Destroy(item);
        }
        if (item.CompareTag("Rocket"))
        {
            item.GetComponent<Item>().ShouldPlayDestroyEffect = RocketCount < StartingRocketCount;
            item.GetComponent<Item>().ShouldPlaySoundEffect = RocketCount < StartingRocketCount;
            RocketCount += 1;
            Destroy(item);
        }
        if (item.CompareTag("Health"))
        {
            item.GetComponent<Item>().ShouldPlayDestroyEffect = HealthCount < StartingHealthCount;
            item.GetComponent<Item>().ShouldPlaySoundEffect = HealthCount < StartingHealthCount;
            HealthCount += 1;
            Destroy(item);
        }
        if (item.CompareTag("Shield"))
        {
            item.GetComponent<Item>().ShouldPlayDestroyEffect = true;
            item.GetComponent<Item>().ShouldPlaySoundEffect = true;

            if (BoostCount > 0)
            {
                Instantiate(BoostEffect, transform.position, transform.rotation);
                FindObjectOfType<AudioManager>().Play("BoostFire");
            }

            BoostCount -= 1;
            Destroy(item);
        }
        if (item.CompareTag("Mine"))
        {
            item.GetComponent<Item>().ShouldPlayDestroyEffect = true;
            item.GetComponent<Item>().ShouldPlaySoundEffect = true;
            if (RocketCount > 0)
            {
                Instantiate(RocketEffect, transform.position, transform.rotation);
                FindObjectOfType<AudioManager>().Play("RocketFire");
            }

            RocketCount -= 1;
            Destroy(item);
        }
        FindObjectOfType<AudioManager>().Play("MoveOne");
    }
    
    private IEnumerator MoveTo(Transform target)
    {
        while (Vector3.Distance(transform.position, target.position) > 0f)
        {
            _isMoving = true;
            transform.position =
                Vector3.MoveTowards(
                    transform.position, 
                    target.position, 
                    Time.deltaTime * Speed);
            yield return null;
        }
        _isMoving = false;
        yield return null;
    }
}
