using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ViewControllers;
using Zenject;

public class SettingsMenuViewController : MonoBehaviour
{
    [SerializeField] private GameObject changeNicknamePanel; 
    [SerializeField] private Toggle vibrationToggle;
    [SerializeField] private Toggle soundToggle;
    [SerializeField] private Button logOutButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button changeNicknameButton;

    [Inject] private FirebaseManager firebaseManager;

    private void Awake()
    {
        vibrationToggle.isOn = PlayerPrefs.GetInt("Vibration") == 1;
    }

    private void Start()
    {
        soundToggle.onValueChanged.AddListener(delegate { OnSoundChangeClick(soundToggle); });
        vibrationToggle.onValueChanged.AddListener(delegate { OnVibrationChangeClick(vibrationToggle); });
        closeButton.onClick.AddListener(OnCloseButtonClick);
        logOutButton.onClick.AddListener(OnLogOutButtonClick);
        changeNicknameButton.onClick.AddListener(OnChangeNicknameButtonClick);
    }

    private void OnChangeNicknameButtonClick()
    {
        changeNicknamePanel.SetActive(true);
    }

    private void OnSoundChangeClick(Toggle chosenToggle)
    {
        Debug.Log("Toggle was changed " + chosenToggle.isOn);
    }

    private void OnVibrationChangeClick(Toggle chosenToggle)
    {
        PlayerPrefs.SetInt("Vibration", chosenToggle.isOn ? 1 : 0);
    }

    private void OnLogOutButtonClick()
    {
        firebaseManager.LogOut();
        SceneManager.LoadScene(0);
        gameObject.SetActive(false);
    }
    
    private void OnCloseButtonClick()
    {
        gameObject.SetActive(false);
    }
}
