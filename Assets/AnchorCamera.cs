using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorCamera : MonoBehaviour
{
    public GameObject FollowCameraPrefab;

    public GameObject FollowCamera => _followCamera;
    private GameObject _followCamera;

    public void Awake()
    {
        _followCamera = Instantiate(FollowCameraPrefab,
            transform.position,
            Quaternion.identity);
        _followCamera.transform.parent = transform;
    }
}
