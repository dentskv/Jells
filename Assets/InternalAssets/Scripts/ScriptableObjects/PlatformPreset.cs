using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "Platforms", fileName = "Platforms")]

    public class PlatformPreset : ScriptableObject
    {
        [SerializeField] public List<PoolComponent> platformsPoolComponent;
    }
}
