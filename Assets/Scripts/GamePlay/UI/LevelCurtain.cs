using TMPro;
using UnityEngine;

namespace GamePlay.UI
{
    public class LevelCurtain : MonoBehaviour
    {
        private Animator _animator;
        [SerializeField]
        private TextMeshProUGUI _msgText = null;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        /// <summary>
        /// Close the level curtain<para />
        /// When the curtain has been closed, <c>OnCurtainClosed</c> will be invoked.
        /// </summary>
        /// <param name="msgText">The message to be shown while closing curtain</param>
        public void CloseCurtain(string msgText)
        {
            if (!isActiveAndEnabled)
                gameObject.SetActive(true);

            _msgText.text = msgText;
            _animator.Play("Level Curtain In");
        }

        /// <summary>
        /// Open the level curtain<para />
        /// When the curtain has been opened, <c>OnCurtainOpened</c> will be invoked.
        /// </summary>
        /// <param name="msgText">The message to be shown while opening curtain</param>
        public void OpenCurtain(string msgText)
        {
            if (!isActiveAndEnabled)
                gameObject.SetActive(true);

            _msgText.text = msgText;
            _animator.Play("Level Curtain Out");
        }

        private void OnCurtainClosed()
        {
            LevelManager.Instance.LoadLevel();
        }

        private void OnCurtainOpened()
        {
            gameObject.SetActive(false);
            LevelManager.Instance.StartLevel();
        }
    }
}
