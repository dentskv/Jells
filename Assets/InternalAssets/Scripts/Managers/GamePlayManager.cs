using System;
using DG.Tweening;
using Managers;
using ScriptableObjects;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using ViewControllers;
using Zenject;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Managers
{
    public class GamePlayManager : MonoBehaviour
    {
        [SerializeField] private PlayerPhysicsController playerPhysicsController;
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private BackgroundColorController backgroundColorController;

        [Inject] private GUIViewController guiViewController;
        [Inject] private AdvertisementManager advertisementManager;
        [Inject] private FirebaseManager firebaseManager;

        public event Action<Score> OnChangeScore;

        private PlatformGenerator _platformGenerator;
        private Color _endColor;
        private float _timer;
        private int _coinForfeit;
        private int _scoreForfeit;
        private int _obstacleScoreAward;
        private int _obstacleCoinAward;
        private int _levelScoreAward;
        private int _levelCoinAward;
        private int _scoreCount;
        private int _coinCount;
        private int _crossCounter;
        private int _coins;

        private void Awake()
        {
            advertisementManager.Initialize();
            DOTween.Init();
        }

        private void Start()
        {
            _platformGenerator = GetComponent<PlatformGenerator>();
        }

        private void OnEnable()
        {
            firebaseManager.OnLoadPlayerData += SetLevelAward;
            OnChangeScore += firebaseManager.AddPlayerData;
            
            playerPhysicsController.OnTakeCoin += IncreaseCoins;
            playerPhysicsController.OnPlayerWon += CountFinalAward;
            playerPhysicsController.OnPlayerWon += SetGamePanelState;
            //playerPhysicsController.OnPlayerFell += SetGamePanelState;
            playerPhysicsController.OnPlayerFell += ClearLevelResults;
            playerPhysicsController.OnBumpIntoObstacle += ReduceObstacleAward;
            playerPhysicsController.OnCrossOneObstacle += Vibrate;
            playerPhysicsController.OnCrossTenObstacles += IncreaseObstacleAward;
            playerPhysicsController.OnCrossTenObstacles += backgroundColorController.SetBackgroundColor;
            
            guiViewController.GetGameOverMenu.OnRestartButtonClick += playerMovement.SetStartTransform;
            guiViewController.GetResultMenu.OnNextLevelClick += LoadNextLevel;
            guiViewController.GetResultMenu.OnNextLevelClick += SetAwards;
            guiViewController.GetMainMenu.OnStartedGame += SetGamePanelState;
            
            advertisementManager.OnEarnedReward += RecountFinalAward;
        }

        private void OnDisable()
        {
            firebaseManager.OnLoadPlayerData -= SetLevelAward;
            OnChangeScore -= firebaseManager.AddPlayerData;
            
            playerPhysicsController.OnTakeCoin -= IncreaseCoins;
            playerPhysicsController.OnPlayerWon -= CountFinalAward;
            playerPhysicsController.OnPlayerWon -= SetGamePanelState;
            //playerPhysicsController.OnPlayerFell -= SetGamePanelState;
            playerPhysicsController.OnPlayerFell -= ClearLevelResults;
            playerPhysicsController.OnBumpIntoObstacle -= ReduceObstacleAward;
            playerPhysicsController.OnCrossOneObstacle -= Vibrate;
            playerPhysicsController.OnCrossTenObstacles -= IncreaseObstacleAward;
            playerPhysicsController.OnCrossTenObstacles -= backgroundColorController.SetBackgroundColor;
            
            guiViewController.GetGameOverMenu.OnRestartButtonClick -= playerMovement.SetStartTransform;
            guiViewController.GetResultMenu.OnNextLevelClick -= LoadNextLevel;
            guiViewController.GetResultMenu.OnNextLevelClick -= SetAwards;
            guiViewController.GetMainMenu.OnStartedGame -= SetGamePanelState;
            
            advertisementManager.OnEarnedReward -= RecountFinalAward;
        }

        private void SetGamePanelState()
        {
            guiViewController.GetGamePanel.SetActive(!guiViewController.GetGamePanel.activeSelf);
        }

        private void ReduceObstacleAward()
        {
            _crossCounter++;
            if (_obstacleCoinAward != 0) _obstacleCoinAward -= 5;
            if (_obstacleScoreAward != 0) _obstacleScoreAward -= 20;
        }

        private void IncreaseCoins()
        {
            _coins++;
        }

        private void Vibrate()
        {
            if (PlayerPrefs.GetInt("Vibration") != 1) return;
            Vibration.Vibrate(150);
        }

        private void IncreaseObstacleAward()
        {
            _obstacleCoinAward += 5;
            _obstacleScoreAward += 20;
        }

        private void LoadNextLevel()
        {
            _platformGenerator.DestroyLevel();
            _platformGenerator.GeneratePools();
            _platformGenerator.GenerateLevel();
        }

        private void SetLevelAward()
        {
            _levelCoinAward = 10 + UserManager.GetCurrentScore.playerLevel;
            _levelScoreAward = 50 + UserManager.GetCurrentScore.playerLevel * 2;
            _coinForfeit = UserManager.GetCurrentScore.playerLevel;
            _scoreForfeit = 25 + _coinForfeit;
        }

        private void CountFinalAward()
        {
            Debug.Log("coins: " + _coins);
            _scoreCount = _levelScoreAward + _obstacleScoreAward;
            _coinCount = _levelCoinAward + _obstacleCoinAward + _coins;
            if (_crossCounter > 3)
            {
                _scoreCount -= _scoreForfeit;
                _coinCount -= _coinForfeit;
            }

            _crossCounter = 0;
            guiViewController.GetResultMenu.SetAwardText(_coinCount, _scoreCount);
        }

        private void RecountFinalAward()
        {
            _coinCount *= 2;
            guiViewController.GetResultMenu.SetAwardText(_coinCount, _scoreCount);
        }

        private void SetAwards()
        {
            UserManager.GetCurrentScore.playerScore += _scoreCount; 
            UserManager.GetCurrentScore.playerCoins += _coinCount;
            UserManager.GetCurrentScore.playerLevel++;
            OnChangeScore?.Invoke(UserManager.GetCurrentScore);
            guiViewController.GetScorePanel.SetScoreAmount();
            ClearAllResults();
        }

        private void ClearLevelResults()
        {
            _obstacleScoreAward = 0;
            _obstacleCoinAward = 0;
            _coins = 0;
            SetLevelAward();
        }

        private void ClearAllResults()
        {
            _obstacleScoreAward = 0;
            _obstacleCoinAward = 0;
            _levelScoreAward = 0;
            _levelCoinAward = 0;
            _scoreCount = 0;
            _coinCount = 0;
            _coins = 0;
            SetLevelAward();
        }
    }
}