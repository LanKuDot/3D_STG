using System;
using UnityEngine;

namespace GamePlay
{
    [Serializable]
    public class CharacterData
    {
        [SerializeField]
        public float movingVelocity;
        [SerializeField]
        public float movingAccelTime;
        [SerializeField]
        public float rotatingAccelTime;

        public CharacterData()
        {
            movingVelocity = 10.0f;
            movingAccelTime = 0.1f;
            rotatingAccelTime = 0.1f;
        }
    }
}
