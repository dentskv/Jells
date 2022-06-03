using Assets.Scripts.Managers;
using Firebase.Database;
using Managers;
using UnityEngine;
using Zenject;
using ViewControllers;

public class LoginMonoInstaller : MonoInstaller
{
    [SerializeField] private FirebaseManager firebaseManager;
    
    public override void InstallBindings()
    {
        Container.BindInstance(firebaseManager);
    }
}
