using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class AwardAnimation : MonoBehaviour
{
    [SerializeField] private Transform endPoint;
    [SerializeField] private Transform defaultPoint;

    public bool IsMoving { get; set; }

    private void Update()
    {
        if(IsMoving) MoveToPoint();
    }

    public void MoveToPoint()
    {
        //transform.DOMove(endPoint.position, 0.8f).SetEase(Ease.InCubic);
        transform.DOJump(endPoint.position, 1f, 1, 0.8f);
        IsMoving = false;
    }
    
    private void OnDisable()
    {
        transform.position = defaultPoint.position;
        IsMoving = false;
    }
}
