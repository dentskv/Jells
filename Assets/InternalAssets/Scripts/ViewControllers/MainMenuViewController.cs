using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ViewControllers;
using Zenject;

public class MainMenuViewController : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button scoreboardButton;
    
    [Inject] private GUIViewController _guiViewController;
    
    public event Action OnStartedGame;
    public event Action OnMainMenuEnabled;
    
    private void Start()
    {
        startButton.onClick.AddListener(OnStartButtonClick);
        settingsButton.onClick.AddListener(OnSettingsButtonClick);
        scoreboardButton.onClick.AddListener(OnScoreboardButtonClick);
    }

    private void OnEnable()
    {
        OnMainMenuEnabled?.Invoke();
    }

    private void OnScoreboardButtonClick()
    {
        _guiViewController.SetActivePanel(_guiViewController.GetScoreboardMenu.name);
    }
    
    private void OnStartButtonClick()
    {
        OnStartedGame?.Invoke();
        gameObject.SetActive(false);
    }

    private void OnSettingsButtonClick()
    {
        _guiViewController.GetSettingsMenu.gameObject.SetActive(true);
    }
}
