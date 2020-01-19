using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FollowCamera : MonoBehaviour
{
    public GameObject Follow;
    public GameObject PostFX;
    public GameObject Background;
    private GameObject _background;
    
    public Vector3 Offset;
    public Vector3 BackgroundOffset;
    
    public float SmoothSpeed;
    public float ShakeMagnitude;
    public float ShakeDuration;

    public void Start()
    {
        _background = Instantiate(Background, transform.position, Quaternion.identity);
        gameObject.AddComponent<AudioListener>();
        Instantiate(PostFX, transform, true);
        transform.position = Follow.transform.position + Offset;
        transform.LookAt(transform.position + -transform.up);
    }
    
    public void FixedUpdate()
    {
        if (Follow != null)
        {
            _background.transform.position = transform.position + BackgroundOffset;
            var desiredPosition = Follow.transform.position + Offset;
            var smoothedPosition = Vector3.Lerp(
                transform.position,
                desiredPosition,
                SmoothSpeed);
            transform.position = smoothedPosition;
            PostFX.transform.position = transform.position;
        }
    }
    
    public IEnumerator Shake()
    {
        var originalPos = transform.position;
        var elapsed = 0.0f;
        while (elapsed < ShakeDuration)
        {
            var x = Random.Range(-1f, 1f) * ShakeMagnitude;
            var z = Random.Range(-1f, 1f) * ShakeMagnitude;
            transform.position = new Vector3(
                transform.position.x + x,
                originalPos.y,
                transform.position.z + z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
