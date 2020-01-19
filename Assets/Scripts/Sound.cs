using System;
using UnityEngine;

[Serializable]
public class Sound
{
    public AudioClip Clip;
    public AudioSource Source;

    public string Name;
    public float Volume;
    public float Pitch;
}
