using UnityEngine;

namespace GamePlay
{
    [CreateAssetMenu(fileName = "PlayerData",
        menuName = "ScriptableObject/PlayerData", order = 1)]
    public class PlayerData : ScriptableObject
    {
        public CharacterData characterData = new CharacterData();
        public float firingInterval = 0.5f;
    }
}
