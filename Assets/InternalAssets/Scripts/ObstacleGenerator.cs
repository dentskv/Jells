using UnityEngine;
using Random = UnityEngine.Random;

public class ObstacleGenerator : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;

    private readonly string[] _poolNames = {"TallArch", "MiddleArch", "WideArch", "WideArches", "CoinsHorizontal", "CoinsVertical"};
    private int _amountOfPossibleObstacles;
    private int _maxCoins;

    private void Start()
    {
        _amountOfPossibleObstacles = spawnPoints.Length;
        GenerateObstacles();
    }

    private void GenerateObstacles()
    {
        for (var i = 0; i < _amountOfPossibleObstacles; i++)
        {
            var spawnChance = Random.Range(0, 5);
            var currentPlatform = _maxCoins < 1 ? _poolNames[Random.Range(0, _poolNames.Length)] : _poolNames[Random.Range(0, _poolNames.Length - 2)];
            if (currentPlatform == "CoinsHorizontal" || currentPlatform == "CoinsVertical") _maxCoins++;
            if (spawnChance == 0) continue;
            PoolManager.GetPoolObject(currentPlatform, spawnPoints[i].position, spawnPoints[i].localRotation);
        }
    }

    private void OnDisable()
    {
        _maxCoins = 0;
    }
}
