using UnityEngine;
using UnityEditor;

namespace GamePlay.Editor
{
    [CustomEditor(typeof(BarrierDestroyable))]
    [CanEditMultipleObjects]
    public class BarrierDestroyableEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawDefaultInspector();
            CheckBarrierType();
            serializedObject.ApplyModifiedProperties();
        }

        private void CheckBarrierType()
        {
            var condition = serializedObject.FindProperty("_condition").enumValueIndex;

            foreach (BarrierDestroyable obj in serializedObject.targetObjects) {
                var barrierProtector = obj.transform.GetChild(0).gameObject;

                if (condition == (int) BarrierDestroyable.Condition.AtStart)
                    barrierProtector.SetActive(false);
                else
                    barrierProtector.SetActive(true);
            }
        }
    }
}
