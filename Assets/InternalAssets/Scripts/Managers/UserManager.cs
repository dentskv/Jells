using System;
using UnityEngine;

namespace Managers
{
    public static class UserManager
    {
        private static Score _currentScore;

        public static Score GetCurrentScore => _currentScore;

        public static void Deserialize(string json)
        {
            var scoreData = (Score) JsonUtility.FromJson(json, typeof(Score));
            _currentScore = scoreData;
        }
        
        public static void SetDefaultScore()
        {
            _currentScore.playerScore = 0;
            _currentScore.playerCoins = 0;
            _currentScore.playerLevel = 1;
        }
    }

    [Serializable]
    public class UserData
    {
        public Score Score;
        public string playerName;

        public UserData(Score score, string name)
        {
            this.Score = score;
            this.playerName = name;
        }
    }
    
    [Serializable]
    public class Score
    {
        public int playerCoins;
        public int playerLevel;
        public int playerScore;
        
        public Score(int score, int coins, int level)
        {
            this.playerCoins = coins;
            this.playerScore = score;
            this.playerLevel = level;
        }
    }
}