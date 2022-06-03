using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BackgroundColorController : MonoBehaviour
{
    private Camera _mainCamera;
    private Color _endColor;
    private float _timer;

    private void Start()
    {
        _mainCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        if (_timer <= 0) return;
        _mainCamera.backgroundColor = Color.Lerp(_mainCamera.backgroundColor, _endColor, _timer * Time.deltaTime);
        _timer -= Time.deltaTime;
    }
        
    public void SetBackgroundColor()
    {
        _endColor = new Color(Random.Range(0.2f, 0.88f), Random.Range(0.2f, 0.92f), 1f);
        _timer = 3f;
    }
}
