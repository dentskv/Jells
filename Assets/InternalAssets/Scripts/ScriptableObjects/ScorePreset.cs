using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "ScorePreset", fileName = "ScorePreset")]
    
    public class ScorePreset : ScriptableObject
    {
        [SerializeField] public int coins;
        [SerializeField] public int level;
        [SerializeField] public int score;
    }
}