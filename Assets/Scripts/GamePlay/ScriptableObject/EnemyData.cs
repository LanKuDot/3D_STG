using System;
using UnityEngine;

namespace GamePlay
{
    public class EnemyData : ScriptableObject
    {
        [Tooltip("The HP of the enemy")]
        [SerializeField]
        private int _hp = 3;
        [Tooltip("The firing loop")]
        [SerializeField]
        private FiringScript _firingScript = null;

        public int hp => _hp;
        public FiringScript firingScript => _firingScript;
    }

    [Serializable]
    public class FiringScript
    {
        [Tooltip("The initial delay for starting the first action")]
        [SerializeField] private float _initialDelay = 0.1f;
        [Tooltip("The actions in the firing loop")]
        [SerializeField] private FiringAction[] _actions = { new FiringAction() };

        public float initialDelay => _initialDelay;
        public FiringAction[] actions => _actions;
    }

    [Serializable]
    public class FiringAction
    {
        [Tooltip("The bullets to be fired in this action")]
        [SerializeField] private FiringBullet[] _bullets = { new FiringBullet() };
        [Tooltip("The time interval for cooling down after this action")]
        [SerializeField] private float _coolDownTime = 0.5f;

        public FiringBullet[] bullets => _bullets;
        public float coolDownTime => _coolDownTime;
    }

    [Serializable]
    public class FiringBullet
    {
        [Tooltip("The prefab of the bullet to be shot")]
        [SerializeField] private GameObject _prefab = null;
        [Tooltip("The shooting direction relative to the forward direction of the enemy")]
        [SerializeField] private float _degree = 0.0f;

        public GameObject prefab => _prefab;
        public float degree => _degree;
    }
}
