using UnityEditor;

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
            var bPath = pathFollower.path.bezierPath;
            var vPath = pathFollower.path.path;
            var index = 0;

            for (var i = 0; i < bPath.NumPoints; i += 3) {
                var time = vPath.GetClosestTimeOnPath(bPath[i]);
                EditorGUILayout.LabelField($"Anchor {index}\t{time}");
                ++index;
            }
        }
    }
}
