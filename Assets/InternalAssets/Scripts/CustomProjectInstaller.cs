using ScriptableObjects;
using UnityEngine;
using Zenject;

[CreateAssetMenu(menuName = "PresetProjectInstaller", fileName = "PresetProjectInstaller")]

public class CustomProjectInstaller : ScriptableObjectInstaller<CustomProjectInstaller>
{
    [SerializeField] private ObstaclePreset _obstaclePreset;
    [SerializeField] private PlatformPreset _platformPreset;
    
    public override void InstallBindings()
    {
        Container.BindInstance(_obstaclePreset);
        Container.BindInstance(_platformPreset);
    }
}
