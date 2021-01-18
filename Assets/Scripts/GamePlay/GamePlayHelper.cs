using UnityEngine;
using UnityEngine.InputSystem;

namespace GamePlay
{
    /// <summary>
    /// The debugging UI for the gameplay
    /// </summary>
    [ExecuteInEditMode]
    public class GamePlayHelper : MonoBehaviour
    {
#if UNITY_EDITOR

        [SerializeField]
        private bool _activateAtStart = true;
        private bool _isHelperActivated;

        private bool _noPlayerDamage = false;
        private int _curLevelID = -1;
        private int _desiredLevelID = 0;
        private int _numOfLevel = 0;

        private void Awake()
        {
            // Add F12 key to the input system for triggering helper UI
            var action = new InputAction("Debug", binding: "<Keyboard>/F12");
            action.performed += context =>
            {
                _isHelperActivated = !_isHelperActivated;
            };
            action.Enable();

            _isHelperActivated = _activateAtStart;
        }

        private void Start()
        {
            if (LevelManager.Instance == null)
                return;

            _numOfLevel = LevelManager.Instance.numOfLevel;
            LevelManager.Instance.OnLevelStarted +=
                levelID => _desiredLevelID = _curLevelID = levelID;
        }

        private void OnGUI()
        {
            if (!_isHelperActivated)
                return;

            DrawHelperUI(new Rect(10, 10, 200, 140));
            DrawGamePlayInformation(new Rect(10, 145, 300, 110));
        }

        private void DrawHelperUI(Rect area)
        {
            GUILayout.BeginArea(area);
            GUILayout.BeginVertical("Gameplay Helper", "box");

            GUILayout.Label("");
            GUILayout.Label($"Level ID: {_curLevelID}");

            #region Level Selector

            GUILayout.Label($"Desired Level: {_desiredLevelID}");
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("+"))
                _desiredLevelID = (_desiredLevelID + 1) % _numOfLevel;
            if (GUILayout.Button("-"))
                _desiredLevelID = (_desiredLevelID - 1 + _numOfLevel) % _numOfLevel;
            if (GUILayout.Button("Load") && Application.isPlaying) {
                LevelManager.Instance.LevelChange(_desiredLevelID);
            }
            GUILayout.EndHorizontal();

            #endregion

            var newStatus = GUILayout.Toggle(
                _noPlayerDamage, "No Damage On Player");
            if (newStatus != _noPlayerDamage) {
                _noPlayerDamage = newStatus;
                Player.Instance.takeNoDamage = newStatus;
            }

            GUILayout.EndVertical();
            GUILayout.EndArea();
        }

        /// <summary>
        /// Show the gameplay information
        /// </summary>
        /// <param name="area"></param>
        private void DrawGamePlayInformation(Rect area)
        {
            GUILayout.BeginArea(area);
            GUILayout.BeginVertical("Information", "box");

            GUILayout.Label("");
            GUILayout.Label("Player");
            GUILayout.Label($"Direction: {Player.Instance.movingDirection}");
            GUILayout.Label($"Degree: {Player.Instance.lookingDeg}");

            GUILayout.EndVertical();
            GUILayout.EndArea();
        }

#endif
    }
}
