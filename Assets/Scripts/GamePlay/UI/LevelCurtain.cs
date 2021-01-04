using System;
using TMPro;
using UnityEngine;

namespace GamePlay.UI
{
    public class LevelCurtain : MonoBehaviour
    {
        private Animator _animator;
        [SerializeField]
        private TextMeshProUGUI _msgText = null;

        private Action _onCurtainClosedCallback;
        private Action _onCurtainOpenedCallback;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        /// <summary>
        /// Close the level curtain<para />
        /// When the curtain has been closed, <c>onCurtainClosed</c> will be invoked.
        /// </summary>
        /// <param name="msgText">The message to be shown while closing curtain</param>
        /// <param name="onCurtainClosed">
        /// The callback to be invoked when the curtain has been closed
        /// </param>
        public void CloseCurtain(string msgText, Action onCurtainClosed)
        {
            if (!isActiveAndEnabled)
                gameObject.SetActive(true);

            _onCurtainClosedCallback = onCurtainClosed;
            _msgText.text = msgText;
            _animator.Play("Level Curtain In");
        }

        /// <summary>
        /// Open the level curtain<para />
        /// When the curtain has been opened, <c>onCurtainOpened</c> will be invoked.
        /// </summary>
        /// <param name="msgText">The message to be shown while opening curtain</param>
        /// <param name="onCurtainOpened">
        /// The callback to be invoked when the curtain has been opened
        /// </param>
        public void OpenCurtain(string msgText, Action onCurtainOpened)
        {
            if (!isActiveAndEnabled)
                gameObject.SetActive(true);

            _onCurtainOpenedCallback = onCurtainOpened;
            _msgText.text = msgText;
            _animator.Play("Level Curtain Out");
        }

        private void OnCurtainClosed()
        {
            _onCurtainClosedCallback?.Invoke();
        }

        private void OnCurtainOpened()
        {
            gameObject.SetActive(false);
            _onCurtainOpenedCallback?.Invoke();
        }
    }
}
