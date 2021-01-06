using System;
using TMPro;
using UnityEngine;

namespace GamePlay.UI
{
    public class HPText : MonoBehaviour
    {
        [SerializeField]
        [ShowOnly]
        private TextMeshProUGUI _text = null;

        private void Reset()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }

        private void Start()
        {
            Player.Instance.OnHPChanged += UpdateHP;
        }

        /// <summary>
        /// Update the displaying value of HP
        /// </summary>
        /// <param name="hp">The HP value</param>
        private void UpdateHP(int hp)
        {
            _text.text = $"HP: {hp}";
        }
    }
}
