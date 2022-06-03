using Assets.Scripts.Managers;
using Firebase.Database;
using Managers;
using UnityEngine;
using Zenject;
using ViewControllers;

public class GameMonoInstaller : MonoInstaller
{
    [SerializeField] private GamePlayManager gamePlayManager;
    [SerializeField] private GUIViewController guiViewController;
    [SerializeField] private FirebaseManager firebaseManager;

    public override void InstallBindings()
    {
        Container.Bind<AdvertisementManager>().AsSingle();
        
        Container.BindInstance(gamePlayManager);
        Container.BindInstance(guiViewController);
        Container.BindInstance(firebaseManager);
    }
}
