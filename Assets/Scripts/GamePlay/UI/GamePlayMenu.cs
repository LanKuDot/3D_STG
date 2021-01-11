using UnityEngine;
using UnityEngine.InputSystem;

namespace GamePlay.UI
{
    /// <summary>
    /// The menu in the game play scene
    /// </summary>
    public class GamePlayMenu : MonoBehaviour
    {
        [SerializeField]
        private GameObject _pauseText = null;

        private void Start()
        {
            // Make the cursor be trapped in the game view
            Cursor.lockState = CursorLockMode.Confined;
        }

        /// <summary>
        /// Toggle the game play menu and release the cursor if menu is shown
        /// </summary>
        public void ToggleMenu(InputAction.CallbackContext context)
        {
            // Cannot toggle menu when the level is loading
            if (!context.performed || LevelManager.isLoadingLevel)
                return;

            if (!_pauseText.activeSelf) {
                LevelManager.GamePause();
                Cursor.lockState = CursorLockMode.None;
            } else {
                LevelManager.GameResume();
                Cursor.lockState = CursorLockMode.Confined;
            }

            _pauseText.SetActive(!_pauseText.activeSelf);
        }
    }
}
