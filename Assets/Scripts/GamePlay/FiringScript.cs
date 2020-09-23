using System;
using UnityEngine;

namespace GamePlay
{
    /// <summary>
    /// The predefined firing script for the enemy to firing bullets repeatedly
    /// </summary>
    [CreateAssetMenu(fileName = "FiringScript",
        menuName = "Scriptable Object/Firing Script", order = 4)]
    public class FiringScript : ScriptableObject
    {
        /// <summary>
        /// The delay time interval before starting the firing loop
        /// </summary>
        public float initialDelay = 0.1f;

        /// <summary>
        /// The actions in the firing loop
        /// </summary>
        public FiringAction[] actions = { new FiringAction() };
    }

    /// <summary>
    /// The action for a firing
    /// </summary>
    [Serializable]
    public class FiringAction
    {
        /// <summary>
        /// The data of bullets to be fired in this action
        /// </summary>
        public FireData[] data = { new FireData() };

        /// <summary>
        /// The time interval of cooling down after this action
        /// </summary>
        public float coolDownTime = 0.5f;
    }

    /// <summary>
    /// The detailed information for firing a bullet
    /// </summary>
    [Serializable]
    public class FireData
    {
        /// <summary>
        /// The bullet name in the ObjectPool
        /// </summary>
        public string bulletNameInPool = "bullet";
        /// <summary>
        /// The initial direction relative to the firing source
        /// </summary>
        public Vector3 direction = Vector3.up;
    }
}
