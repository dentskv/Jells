using System;
using System.Collections;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Random = UnityEngine.Random;

namespace ViewControllers
{
    public class ResultMenuViewController : MonoBehaviour
    {
        [Inject] private GUIViewController _guiViewController;
    
        [SerializeField] private AwardAnimation[] awardAnimations;
        [SerializeField] private TMP_Text scoreAwardAmountTMP;
        [SerializeField] private TMP_Text coinAwardAmountTMP;
        [SerializeField] private Button doubleButton;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button testButton;

        [Inject] private AdvertisementManager advertisementManager;
    
        public event Action OnNextLevelClick;
        public event Action OnShowResults;

        private void Start()
        {
            nextButton.onClick.AddListener(OnNextButtonClick);
            doubleButton.onClick.AddListener(OnDoubleButtonClick);
            testButton.onClick.AddListener(OnTestButtonClick);
        }

        private void OnEnable()
        {
            doubleButton.enabled = true;
        }

        private void OnTestButtonClick()
        {
            StartCoroutine(AnimateAwards());
        }
    
        private void OnNextButtonClick()
        {
            StartCoroutine(CountResults());
        }

        private IEnumerator CountResults()
        {
            StartCoroutine(AnimateAwards());
            yield return new WaitForSeconds(1.1f);
            OnShowResults?.Invoke();
            yield return new WaitForSeconds(0.4f);
            OnNextLevelClick?.Invoke();
            var chance = Random.Range(0, 5);
            if (chance == 0) advertisementManager.ShowInterstitialAd();
            _guiViewController.SetActivePanel(_guiViewController.GetMainMenu.gameObject.name);
        }

        private void OnDoubleButtonClick()
        {
            advertisementManager.UserChoseToWatchAd();
            doubleButton.enabled = false;
        }

        private IEnumerator AnimateAwards()
        {
            for (var i = 0; i < awardAnimations.Length; i++)
            {
                awardAnimations[i].IsMoving = true;
                yield return new WaitForSeconds(0.06f);
            }
        }
    
        public void SetAwardText(int coinAward, int scoreAward)
        {
            scoreAwardAmountTMP.text = "" + scoreAward;
            coinAwardAmountTMP.text = "" + coinAward;
        }
    }
}
