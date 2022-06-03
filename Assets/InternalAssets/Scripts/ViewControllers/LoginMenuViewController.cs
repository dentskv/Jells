using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;
using Button = UnityEngine.UI.Button;

namespace ViewControllers
{
    public class LoginMenuViewController : MonoBehaviour
    {
        [Inject] private FirebaseManager firebaseManager;
    
        [SerializeField] private TMP_InputField passwordTextField;
        [SerializeField] private TMP_InputField nicknameTextField;
        [SerializeField] private TMP_InputField emailTextField;
        [SerializeField] private TMP_Text errorText;
        [SerializeField] private Button registrationButton;
        [SerializeField] private Button loginButton;
        [SerializeField] private Toggle rememberToggle;

        private bool _isReconnect;
        
        private void Start()
        {
            if(PlayerPrefs.GetString("Reconnect") == null) PlayerPrefs.SetString("Reconnect", _isReconnect.ToString());
            firebaseManager.OnAuthPlayer += EnterPlayer;
            loginButton.onClick.AddListener(OnLoginButtonClick);
            registrationButton.onClick.AddListener(OnRegistrationButtonClick);
            rememberToggle.onValueChanged.AddListener(delegate { OnRememberToggleChange(rememberToggle); });
            rememberToggle.isOn = PlayerPrefs.GetString("Reconnect") == "True";
            errorText.text = "";
        }

        private void OnEnable()
        {
            firebaseManager.OnGetErrorText += SetError;
        }

        private void OnDisable()
        {
            firebaseManager.OnGetErrorText -= SetError;
        }

        private void OnRememberToggleChange(Toggle toggle)
        {
            _isReconnect = toggle.isOn;
            PlayerPrefs.SetString("Reconnect", _isReconnect.ToString());
        }
        
        private void OnLoginButtonClick()
        {
            errorText.text = "";
            ConnectPlayer();
        }

        private void OnRegistrationButtonClick()
        {
            errorText.text = "";
            AddNewPlayer();
        }

        private void ConnectPlayer()
        {
            if (emailTextField.Equals(""))
            {
                SetError("Email field is empty. Write the email");
            }
            else if (passwordTextField.Equals(""))
            {
                SetError("Password field is empty. Write the password");
            }
            else
            {
                firebaseManager.LoginPlayer(emailTextField.text, passwordTextField.text);
            }
        }

        private void SetError(string error)
        {
            errorText.text = error;
        }
    
        private void AddNewPlayer()
        {
            if (nicknameTextField.text.Equals(""))
            {
                SetError("Nickname field is empty. Write the nickname");
            }
            else if (emailTextField.text.Equals(""))
            {
                SetError("Email field is empty. Write the email");
            }
            else if (passwordTextField.text.Equals(""))
            {
                SetError("Password field is empty. Write the password");
            }
            else if (passwordTextField.text.Length < 6)
            {
                SetError("Too short password");
            }
            else
            {
                firebaseManager.RegistrationPlayer(emailTextField.text, passwordTextField.text, nicknameTextField.text);
            }
        }

        private void EnterPlayer()
        {
            SceneManager.LoadScene(1);
            //guiViewController.SetActivePanel(guiViewController.GetLoadingScreen.name);
        }
    }
}
