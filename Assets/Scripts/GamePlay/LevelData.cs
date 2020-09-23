using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GamePlay
{
    [CreateAssetMenu(fileName = "LevelData",
        menuName = "Scriptable Object/Level Data", order = 5)]
    public class LevelData : ScriptableObject
    {
        public AssetReference levelScene;
        public Vector3 playerSpawnPoint;
    }
}