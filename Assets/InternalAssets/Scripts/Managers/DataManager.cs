using System.IO;
using UnityEngine;

namespace Managers
{
    public static class DataManager
    {
        private static readonly string PATH = Application.dataPath + "/Saves/" + "save.txt";

        public static void SaveData(string json)
        {
            File.WriteAllText(PATH, json);
        }

        public static string LoadData()
        {
            var json = File.ReadAllText(PATH);
            return json;
        }
    }
}

