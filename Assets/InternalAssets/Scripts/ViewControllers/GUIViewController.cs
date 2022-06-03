using UnityEngine;

namespace ViewControllers
{
    public class GUIViewController : MonoBehaviour
    {
        [SerializeField] private ScoreboardMenuViewController scoreboardMenu;
        [SerializeField] private LoadingScreenViewController loadingScreen;
        [SerializeField] private GameOverMenuViewController gameOverMenu;
        [SerializeField] private SettingsMenuViewController settingsMenu;
        [SerializeField] private ResultMenuViewController resultMenu;
        [SerializeField] private ScorePanelViewController scorePanel;
        [SerializeField] private MainMenuViewController mainMenu;
        [SerializeField] private GameObject pauseMenu;
        [SerializeField] private GameObject gamePanel;

        private PlayerMovement _playerMovement;

        public ScoreboardMenuViewController GetScoreboardMenu => scoreboardMenu;
        public LoadingScreenViewController GetLoadingScreen => loadingScreen;
        public SettingsMenuViewController GetSettingsMenu => settingsMenu;
        public GameOverMenuViewController GetGameOverMenu => gameOverMenu;
        public ResultMenuViewController GetResultMenu => resultMenu;
        public ScorePanelViewController GetScorePanel => scorePanel;
        public MainMenuViewController GetMainMenu => mainMenu;
        public GameObject GetPauseMenu => pauseMenu;
        public GameObject GetGamePanel => gamePanel;

        private void Start()
        {
            _playerMovement = FindObjectOfType<PlayerMovement>();
            _playerMovement.GetPhysicsController.OnPlayerFell += ShowGameOverMenu;
            resultMenu.OnNextLevelClick += _playerMovement.SetStartTransform;
            resultMenu.OnShowResults += scorePanel.SetScoreAmount;
        }

        public void SetActivePanel(string activePanel)
        {
            scoreboardMenu.gameObject.SetActive(activePanel.Equals(scoreboardMenu.name));
            loadingScreen.gameObject.SetActive(activePanel.Equals(loadingScreen.name));
            gameOverMenu.gameObject.SetActive(activePanel.Equals(gameOverMenu.name));
            resultMenu.gameObject.SetActive(activePanel.Equals(resultMenu.name));
            mainMenu.gameObject.SetActive(activePanel.Equals(mainMenu.name));
            pauseMenu.SetActive(activePanel.Equals(pauseMenu.name));
        }

        private void ShowGameOverMenu()
        {
            gameOverMenu.gameObject.SetActive(true);
        }
    }
}
