using System;
using UnityEngine;

namespace GamePlay
{
    public class EnemyData : ScriptableObject
    {
        [SerializeField]
        [Tooltip("The HP of the enemy")]
        private int _hp = 3;
        [SerializeField]
        [Tooltip("The firing loop")]
        private FiringScript _firingScript = null;

        public int hp => _hp;
        public FiringScript firingScript => _firingScript;
    }

    [Serializable]
    public class FiringScript
    {
        [SerializeField]
        [Min(0.01f)]
        [Tooltip("The initial delay for starting the first action [0.01, Inf]")]
        private float _initialDelay = 0.1f;
        [SerializeField]
        [Tooltip("The actions in the firing loop")]
        private FiringAction[] _actions = { new FiringAction() };

        public float initialDelay => _initialDelay;
        public FiringAction[] actions => _actions;
    }

    [Serializable]
    public class FiringAction
    {
        [SerializeField]
        [Tooltip("The bullets to be fired in this action")]
        private FiringBullet[] _bullets = { new FiringBullet() };
        [SerializeField]
        [Tooltip("The time interval for cooling down after this action")]
        private float _coolDownTime = 0.5f;

        public FiringBullet[] bullets => _bullets;
        public float coolDownTime => _coolDownTime;
    }

    [Serializable]
    public class FiringBullet
    {
        [SerializeField]
        [Tooltip("The prefab of the bullet to be shot")]
        private GameObject _prefab = null;
        [SerializeField]
        [Tooltip("The shooting direction relative to the forward direction of the enemy")]
        private float _degree = 0.0f;

        public GameObject prefab => _prefab;
        public float degree => _degree;
    }
}
