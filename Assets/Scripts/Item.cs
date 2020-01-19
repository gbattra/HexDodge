using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public GameObject DestroyEffect;
    public string DestroySoundName;
    public bool ShouldPlayDestroyEffect;
    public bool ShouldPlaySoundEffect;
    
    public void OnDestroy()
    {
        if (ShouldPlayDestroyEffect)
        {
            if (DestroyEffect != null)
                Instantiate(DestroyEffect, transform.position, transform.rotation);
        }
        
        if (ShouldPlaySoundEffect)
            FindObjectOfType<AudioManager>().Play(DestroySoundName);
    }
}
