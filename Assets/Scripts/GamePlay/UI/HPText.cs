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
        [SerializeField]
        private int _lowHP = 1;
        [SerializeField]
        private Color _lowHPColor = Color.red;

        private Color _origColor;

        private void Reset()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }

        private void Awake()
        {
            _origColor = _text.color;
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
            _text.color = hp <= _lowHP ? _lowHPColor : _origColor;
        }
    }
}
