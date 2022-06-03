using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SettingsPanelAnimation : MonoBehaviour
{
    [SerializeField] private Transform settingsPanelTransform;
    [SerializeField] private Transform panelStartPoint;
    [SerializeField] private Transform panelEndPoint;

    private void OnEnable()
    {
        SetDefaultTransform();
        settingsPanelTransform.DOMoveX(panelEndPoint.position.x, 0.9f).SetEase(Ease.OutElastic, 1.4f, 1.6f);
    }

    private void OnDisable()
    {
        SetDefaultTransform();
    }

    private void SetDefaultTransform()
    {
        settingsPanelTransform.position = panelStartPoint.position;
    }
}
