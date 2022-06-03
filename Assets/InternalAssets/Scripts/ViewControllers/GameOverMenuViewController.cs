using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ViewControllers;
using Zenject;

public class GameOverMenuViewController : MonoBehaviour
{
    [Inject] private GUIViewController guiViewController;

    public event Action OnRestartButtonClick;
    
    private Button _restartButton;
    
    private void Start()
    {
        _restartButton = GetComponentInChildren<Button>();
        _restartButton.onClick.AddListener(RestartButtonClick);
    }

    private void RestartButtonClick()
    {
        OnRestartButtonClick?.Invoke();
        guiViewController.SetActivePanel(guiViewController.GetMainMenu.name); 
    }
}
