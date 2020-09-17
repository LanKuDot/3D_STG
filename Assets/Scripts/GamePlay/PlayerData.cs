using UnityEngine;

namespace GamePlay
{
    [CreateAssetMenu(fileName = "PlayerData",
        menuName = "ScriptableObject/PlayerData", order = 1)]
    public class PlayerData : ScriptableObject
    {
        public float movingVelocity = 10.0f;
        public float movingAccelTime = 0.1f;
        public float rotatingAccelTime = 0.1f;
        public float firingInterval = 0.5f;
    }
}
