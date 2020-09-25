using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GamePlay
{
    [CreateAssetMenu(fileName = "LevelData",
        menuName = "Scriptable Object/Level Data", order = 5)]
    public class LevelData : ScriptableObject
    {
        [SerializeField]
        private int _defaultLevelID = 0;

        [SerializeField]
        private Level[] _levels = { new Level() };

        public int defaultLevelID => _defaultLevelID;
        public int Length => _levels.Length;

        public AssetReference GetLevelScene(int levelID)
        {
            return _levels[levelID].levelScene;
        }

        public Vector3 GetPlayerSpawnPoint(int levelID)
        {
            return _levels[levelID].playerSpawnPoint;
        }
    }

    [Serializable]
    public class Level
    {
        public AssetReference levelScene;
        public Vector3 playerSpawnPoint;
    }
}
