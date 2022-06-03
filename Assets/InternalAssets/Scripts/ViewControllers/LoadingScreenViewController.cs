using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ViewControllers;
using Zenject;

public class LoadingScreenViewController : MonoBehaviour
{
    [SerializeField] private Slider loadingBar;

    [Inject] private GUIViewController _guiViewController;

    private void OnEnable()
    {
        loadingBar.value = 0;
    }

    private void Update()
    {
        loadingBar.value += Time.deltaTime/1.2f;
        if (loadingBar.value >= 1)
        {
            LoadMainMenu();
        }
    }

    private void OnDisable()
    {
        loadingBar.value = 1;
    }

    private void LoadMainMenu()
    {
        _guiViewController.SetActivePanel(_guiViewController.GetMainMenu.name);
    }
}
