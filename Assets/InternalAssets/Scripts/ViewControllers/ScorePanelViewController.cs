using System;
using Assets.Scripts.Managers;
using Managers;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Zenject;

public class ScorePanelViewController : MonoBehaviour
{
    [SerializeField] private TMP_Text coinAmountTMP;
    [SerializeField] private TMP_Text scoreAmountTMP;
    [SerializeField] private TMP_Text levelNumberTMP;

    [Inject] private FirebaseManager firebaseManager;
    
    private void Start()
    {
        GetPlayerData();
    }
    
    private void OnEnable()
    {
        firebaseManager.OnLoadPlayerData += SetScoreAmount;
    }

    private void OnDisable()
    {
        firebaseManager.OnLoadPlayerData -= SetScoreAmount;
    }
    
    private void GetPlayerData()
    {
        firebaseManager.LoadDataAsync();
    }

    public void SetScoreAmount()
    {
        coinAmountTMP.text = "" + UserManager.GetCurrentScore.playerCoins;
        scoreAmountTMP.text = "" + UserManager.GetCurrentScore.playerScore;
        levelNumberTMP.text = "Level " + UserManager.GetCurrentScore.playerLevel;
    }
}
