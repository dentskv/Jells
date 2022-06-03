using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class NicknameController : MonoBehaviour
{
    [SerializeField] private TMP_InputField nicknameInputField;
    [SerializeField] private TMP_Text errorText;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button okButton;

    [Inject] private FirebaseManager firebaseManager;
    
    private void Start()
    {
        okButton.onClick.AddListener(OnOKButtonClick);
        closeButton.onClick.AddListener(OnCancelButtonClick);
    }

    private void OnOKButtonClick()
    {
        if (nicknameInputField.text.Equals(""))
        {
            errorText.color = Color.red;
            errorText.text = "Write the nickname";
            
            return;
        }
        
        firebaseManager.ChangeNickname(nicknameInputField.text);
        errorText.color = Color.green;
        errorText.text = "Success";
    }

    private void OnCancelButtonClick()
    {
        errorText.text = "";
        gameObject.SetActive(false);
    }
}
