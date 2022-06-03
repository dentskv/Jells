using System;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Managers;
using UnityEngine;

public class FirebaseManager : MonoBehaviour
{
    private DatabaseReference _databaseReference;
    private DatabaseReference _databaseReferenceId;
    private DatabaseReference _databaseReferenceScore;
    private DatabaseReference _databaseReferencePlayerName;
    private FirebaseAuth _firebaseAuth;
    private FirebaseUser _user;
    
    public event Action<string> OnGetErrorText;
    public event Action OnLoadPlayerData;
    public event Action OnAuthPlayer;
    public event Action OnEndLoadingData;
    
    public string GetUserId => _firebaseAuth.CurrentUser.UserId;

    public void LogOut()
    {
        _firebaseAuth.SignOut();
        Debug.Log("User logged out " + _firebaseAuth.CurrentUser);
    }

    private void Awake()
    {
        _firebaseAuth = FirebaseAuth.DefaultInstance;
        _databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        GetDatabaseReferences();
        
        if (FirebaseAuth.DefaultInstance.CurrentUser != null && PlayerPrefs.GetString("Reconnect") == "False")
        {
            FirebaseAuth.DefaultInstance.SignOut();
        }
        
        _firebaseAuth.StateChanged += OnStateChanged;
    }

    private void OnEnable()
    {
        OnAuthPlayer += GetDatabaseReferences;
        OnAuthPlayer += LoadDataAsync;
    }

    private void GetDatabaseReferences()
    {
        try
        {
            _databaseReferenceId = FirebaseDatabase.DefaultInstance.GetReference($"Players/{GetUserId}");
            _databaseReferenceScore = FirebaseDatabase.DefaultInstance.GetReference($"Players/{GetUserId}/Score");
            _databaseReferencePlayerName = FirebaseDatabase.DefaultInstance.GetReference($"Players/{GetUserId}/playerName");
        }
        catch (Exception e)
        {
            Debug.Log("FirebaseManager.cs Error: Player hasn't logged in yet");
        }
    }

    private void OnDestroy()
    {
        if (PlayerPrefs.GetString("Reconnect") != "True") return;
        
        _firebaseAuth.StateChanged -= OnStateChanged;
        _firebaseAuth = null;
    }

    private void OnStateChanged(object sender, EventArgs e)
    {
        if (_firebaseAuth.CurrentUser != _user) 
        {
            var signedIn = _user != _firebaseAuth.CurrentUser && _firebaseAuth.CurrentUser != null;
            
            if (!signedIn && _user != null) 
            {
                Debug.Log("Signed out " + _user.UserId);
            }
            
            _user = _firebaseAuth.CurrentUser;
            
            if (signedIn) 
            {
                Debug.Log("Signed in " + _user.UserId);
                OnAuthPlayer?.Invoke();
            }
        }
    }

    public async void LoadScoreboardDataAsync(GameObject playerScorePrefab, Transform contentTransform)
    {
        try
        {
            var task = await _databaseReference.Child("Players").OrderByChild("Score/playerScore").LimitToLast(5).GetValueAsync();
            var i = 5;
            
            foreach (var child in task.Children)
            {
                var playerName = child.Child("playerName").Value.ToString();
                var playerScore = Convert.ToInt32(child.Child("Score").Child("playerScore").Value);
                var scoreboardElement = Instantiate(playerScorePrefab, contentTransform);
                scoreboardElement.GetComponent<ScoreboardElementController>().SetPlayerScore(i, playerName, playerScore);
                i--;
                if (child.Key == _firebaseAuth.CurrentUser.UserId) scoreboardElement.GetComponent<ScoreboardElementController>().SetNicknameColor();
            }
            
            OnEndLoadingData?.Invoke();
        }
        catch (Exception e)
        {
            Debug.Log("FirebaseManager.cs LoadScoreboardDataAsync() exception: " + e);
        }
    }

    public void AddPlayerData(Score playerScore)
    {
        var playerJson = JsonUtility.ToJson(playerScore);
        _databaseReferenceScore.SetRawJsonValueAsync(playerJson);
        Debug.Log("id:" + _firebaseAuth.CurrentUser.UserId);
    }
    
    public async void LoadDataAsync()
    {
        try
        {
            var user = await _databaseReferenceScore.GetValueAsync();
            Debug.Log(user.GetRawJsonValue());
            UserManager.Deserialize(user.GetRawJsonValue());
            OnLoadPlayerData?.Invoke();
        }
        catch (Exception e)
        {
            Debug.Log("FirebaseManager.cs LoadDataAsync() exception: " + e);
        }
    }

    public void LoginPlayer(string email, string password)
    {
        _firebaseAuth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            
            if (task.IsCanceled) 
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            
            if (task.IsFaulted)
            {
                if (task.Exception == null) return;
                
                var firebaseException = task.Exception.Flatten().InnerException as FirebaseException;
                OnGetErrorText?.Invoke(firebaseException != null ? firebaseException.Message : "Undefined error");

                return;
            }

            var newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);
        });
    }

    public void RegistrationPlayer(string email, string password, string nickname)
    {
        _firebaseAuth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            
            if (task.IsFaulted)
            {
                if (task.Exception == null) return;
                
                var firebaseException = task.Exception.Flatten().InnerException as FirebaseException;
                OnGetErrorText?.Invoke(firebaseException != null ? firebaseException.Message : "Something goes wrong");

                return;
            }
            
            UserManager.SetDefaultScore();
            OnLoadPlayerData?.Invoke();
        });
    }

    public void ChangeNickname(string nickname)
    {
        _databaseReferencePlayerName.SetValueAsync(nickname);
    }
}
