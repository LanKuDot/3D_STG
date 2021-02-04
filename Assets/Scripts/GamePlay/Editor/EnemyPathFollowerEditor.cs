using UnityEditor;
using UnityEngine;

namespace GamePlay.Editor
{
    [CustomEditor(typeof(EnemyPathFollower))]
    public class EnemyPathFollowerEditor : UnityEditor.Editor
    {
        private bool _showAnchorInfo = true;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            _showAnchorInfo = EditorGUILayout.Foldout(
                _showAnchorInfo, "Anchor Time Information",
                EditorStyles.foldoutHeader);
            if (_showAnchorInfo) {
                ++EditorGUI.indentLevel;
                ShowAnchorTimeInfo();
                --EditorGUI.indentLevel;
            }
        }

        /// <summary>
        /// Show the anchor time information
        /// </summary>
        private void ShowAnchorTimeInfo()
        {
            var pathFollower = target as EnemyPathFollower;

            if (pathFollower.path == null) {
                EditorGUILayout.HelpBox("No path attached", MessageType.Warning);
                return;
            }

            var pathTransform = pathFollower.path.transform;
            var bPath = pathFollower.path.bezierPath;
            var vPath = pathFollower.path.path;
            var index = 0;

            EditorGUILayout.LabelField(
                new GUIContent(
                    $"Loop time\t{vPath.length / pathFollower.avgMovingVelocity}",
                    "The estimated time to go through the loop"));

            for (var i = 0; i < bPath.NumPoints; i += 3) {
                var worldPoint = pathTransform.TransformPoint(bPath[i]);
                var time = vPath.GetClosestTimeOnPath(worldPoint);
                EditorGUILayout.LabelField($"Anchor {index}\t{time}");
                ++index;
            }
        }
    }
}
