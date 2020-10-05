using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay.UI
{
    public class LevelCurtain : MonoBehaviour
    {
        private Animator _animator;
        [SerializeField]
        private Text _msgText = null;

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

            _msgText.text = ExpandText(msgText);
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

            _msgText.text = ExpandText(msgText);
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

        /// <summary>
        /// Insert a space between each text
        /// </summary>
        /// <param name="originText">The origin string text</param>
        /// <returns>The expanded text</returns>
        private static string ExpandText(string originText)
        {
            var builder = new StringBuilder("");
            foreach (var ch in originText) {
                // Append one more space for the space
                if (ch == ' ') {
                    builder.Append(' ');
                }

                builder.Append($"{ch} ");
            }

            // Remove the last space
            builder.Remove(builder.Length - 1, 1);

            return builder.ToString();
        }
    }
}
