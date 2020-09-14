using UnityEngine;

namespace GamePlay
{
    [CreateAssetMenu(fileName = "PlayerData",
        menuName = "ScriptableObject/PlayerData", order = 1)]
    public class PlayerData : CharacterData
    {
        public float firingInterval = 0.5f;
    }
}
