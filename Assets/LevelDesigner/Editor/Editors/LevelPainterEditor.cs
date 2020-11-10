using LevelDesigner.Runtime;
using UnityEditor;
using UnityEngine;

namespace LevelDesigner.Editor
{
    public class LevelPainterEditor : UnityEditor.Editor
    {
        /// <summary>
        /// Create a new sector as the child object of the level painter
        /// </summary>
        [MenuItem("GameObject/Sector", false, 10)]
        private static void CreateSector(MenuCommand menuCommand)
        {
            var context = menuCommand.context as GameObject;
            var numOfExistingSector =
                context.GetComponentsInChildren<Sector>(true).Length;
            var newSector = new GameObject(
                $"Sector {numOfExistingSector + 1}", typeof(Sector));

            GameObjectUtility.SetParentAndAlign(newSector, context);
            Undo.RegisterCreatedObjectUndo(newSector, "Create new sector");
        }

        /// <summary>
        /// Check if the selected game object has Level Painter component
        /// </summary>
        [MenuItem("GameObject/Sector", true)]
        private static bool CreateSectorValidate()
        {
            var selection = Selection.activeGameObject;
            return selection && selection.GetComponent<LevelPainter>();
        }
    }
}
