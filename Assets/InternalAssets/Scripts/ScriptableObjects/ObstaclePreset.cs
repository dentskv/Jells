using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "Obstacle", fileName = "Obstacle")]
    
    public class ObstaclePreset : ScriptableObject
    {
        [SerializeField] public List<PoolComponent> obstaclePrefabPoolComponents;
    }
}
