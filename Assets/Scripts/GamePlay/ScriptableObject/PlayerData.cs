﻿using UnityEngine;

namespace GamePlay
{
    [CreateAssetMenu(
        fileName = "PlayerData",
        menuName = "Scriptable Object/Player Data",
        order = 1)]
    public class PlayerData : ScriptableObject
    {
        public int hp = 3;
        public float movingVelocity = 10.0f;
        public float movingAccelTime = 0.1f;
        public float rotatingAccelTime = 0.1f;
        public GameObject bullet;
        public float firingInterval = 0.5f;
    }
}