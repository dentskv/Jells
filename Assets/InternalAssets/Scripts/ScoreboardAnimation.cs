using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class ScoreboardAnimation : MonoBehaviour
{
    [SerializeField] private Transform scoreboardPanelTransform;
    [SerializeField] private Transform panelStartPoint;
    [SerializeField] private Transform panelEndPoint;

    private void OnEnable()
    {
        SetDefaultTransform();
        scoreboardPanelTransform.DOMoveX(panelEndPoint.position.x, 0.9f).SetEase(Ease.OutElastic, 1.4f, 1.6f);
    }

    private void OnDisable()
    {
        SetDefaultTransform();
    }

    private void SetDefaultTransform()
    {
        scoreboardPanelTransform.position = panelStartPoint.position;
    }
}
