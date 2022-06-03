using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ViewControllers
{
    public class ScoreboardMenuViewController : MonoBehaviour
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private Transform contentTransform;
        [SerializeField] private GameObject playerScoreElement;
        [SerializeField] private TMP_Text loadingTMP;
    
        [Inject] private FirebaseManager firebaseManager;
        [Inject] private GUIViewController guiViewController;
        
        private void Start()
        {
            closeButton.onClick.AddListener(OnCloseButtonClick);
        }

        private void OnEnable()
        {
            SetScoreboard();
            firebaseManager.OnEndLoadingData += SetLoadingTMP;
        }

        private void OnDisable()
        {
            loadingTMP.text = "Loading...";
            firebaseManager.OnEndLoadingData -= SetLoadingTMP;
        }

        private void SetLoadingTMP()
        {
            loadingTMP.text = "";
        }
    
        private void OnCloseButtonClick()
        {
            guiViewController.SetActivePanel(guiViewController.GetMainMenu.name);
        }

        private void SetScoreboard()
        {
            ClearScoreboard();
            firebaseManager.LoadScoreboardDataAsync(playerScoreElement, contentTransform);
        }

        private void ClearScoreboard()
        {
            foreach (Transform child in contentTransform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
