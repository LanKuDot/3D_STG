using UnityEngine;

namespace GamePlay
{
    [CreateAssetMenu(fileName = "Data_Player",
        menuName = "ScriptableObject/PlayerData", order = 1)]
    public class PlayerData : ScriptableObject
    {
        public CharacterData characterData = new CharacterData();
        public int id = 1;
    }
}
