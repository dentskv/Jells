using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;
using Zenject;

public class CoinController : MonoBehaviour
{
    private MeshRenderer _meshRenderer;
    
    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public void TurnMeshRendererOff()
    {
        _meshRenderer.enabled = false;
        Invoke(nameof(TurnMeshRendererOn), 4f);
    }
    
    private void TurnMeshRendererOn()
    {
        _meshRenderer.enabled = true;
    }
}
