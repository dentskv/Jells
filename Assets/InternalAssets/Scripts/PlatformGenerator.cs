using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class PlatformGenerator : MonoBehaviour
{
    [SerializeField] private PoolComponent firstPlatform;
    [SerializeField] private GameObject finishPlatformPrefab;
    [SerializeField] private int amountOfObstaclesOnPlatforms = 4;
    [SerializeField] private int amountOfPlatforms = 4;

    private PlatformPreset _platformPreset;
    private ObstaclePreset _obstaclePreset;
    private PoolComponent _tempPlatform;
    private GameObject _finishPlatform;
    private readonly string[] _poolPlatformNames = {"PlatformPool", "LeftPlatformPool", "RightPlatformPool", "RailsPlatform", "RailsUpPlatform"};
    private readonly string[] _poolObstacleNames = { "MiddleArch", "TallArch",  "WideArch", "WideArches", "CoinsHorizontal", "CoinsVertical"};
    private string _currentPool;
    
    private List<PoolComponent> _generatedPlatform = new List<PoolComponent>();

    private void Awake()
    {
        PoolManager.Initialize();
        _platformPreset = Resources.Load<PlatformPreset>("Platforms");
        _obstaclePreset = Resources.Load<ObstaclePreset>("Obstacle");
    }

    private void Start()
    {
        GeneratePools();
        _generatedPlatform.Add(firstPlatform);
        GenerateLevel();
    }

    public void GeneratePools()
    {
        for (var i = 0; i < _poolPlatformNames.Length; i++)
        {
            PoolManager.CreatePool(_platformPreset.platformsPoolComponent[i], _poolPlatformNames[i], amountOfPlatforms, false);
            
        }

        for (var i = 0; i < _poolObstacleNames.Length; i++)
        {
            PoolManager.CreatePool(_obstaclePreset.obstaclePrefabPoolComponents[i], _poolObstacleNames[i], amountOfObstaclesOnPlatforms*2, false);
        }
    }
    
    private void SpawnPlatform()
    {
        _currentPool = _poolPlatformNames[Random.Range(0, _poolPlatformNames.Length)];

        if (_currentPool == "LeftPlatformPool")
        {
            _tempPlatform = PoolManager.GetPoolObject(_currentPool, Vector3.zero, Quaternion.identity);
            _tempPlatform.transform.position = _generatedPlatform[_generatedPlatform.Count - 1].GetEndPoint.position - _tempPlatform.GetStartPoint.position;
            _generatedPlatform.Add(_tempPlatform);
        }
        else
        {
            _tempPlatform = PoolManager.GetPoolObject(_currentPool, Vector3.zero, Quaternion.identity);
            _tempPlatform.transform.position = _generatedPlatform[_generatedPlatform.Count - 1].GetEndPoint.position + new Vector3(-3, 0, 0) - _tempPlatform.GetStartPoint.position;
            _generatedPlatform.Add(_tempPlatform);
        }
    }

    public void GenerateLevel()
    {
        for (var i = 0; i < amountOfPlatforms; i++)
        {
            SpawnPlatform();
        }

        _finishPlatform = Instantiate(finishPlatformPrefab, Vector3.zero, Quaternion.identity);
        _finishPlatform.transform.position = _generatedPlatform[_generatedPlatform.Count - 1].GetEndPoint.position - _finishPlatform.GetComponent<PoolComponent>().GetStartPoint.position;
    }

    public void ReturnObjectsToPool()
    {
        for (var i = 0; i < _poolObstacleNames.Length; i++)
        {
            PoolManager.ReturnPoolObject(_poolObstacleNames[i]);
        }
        
        for (var i = 0; i < _poolPlatformNames.Length; i++)
        {
            PoolManager.ReturnPoolObject(_poolPlatformNames[i]);
        }

        Destroy(_finishPlatform);
        _generatedPlatform.Clear();
        _generatedPlatform.Add(firstPlatform);
        
    }
    
    public void DestroyLevel()
    {
        for (var i = 0; i < _poolPlatformNames.Length; i++)
        {
            PoolManager.DestroyPool(_poolPlatformNames[i]);
        }

        for (var i = 0; i < _poolObstacleNames.Length; i++)
        {
            PoolManager.DestroyPool(_poolObstacleNames[i]);
        }
        
        Destroy(_finishPlatform);
        _generatedPlatform.Clear();
        _generatedPlatform.Add(firstPlatform);
    }
}
