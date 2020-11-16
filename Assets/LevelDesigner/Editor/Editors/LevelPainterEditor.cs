using LevelDesigner.Runtime;
using UnityEditor;
using UnityEngine;

namespace LevelDesigner.Editor
{
    public class LevelPainterEditor : UnityEditor.Editor
    {
        /// <summary>
        /// Create a level painter with a sector to the scene
        /// </summary>
        [MenuItem("GameObject/LevelDesigner/LevelPainter", false, 10)]
        public static void CreateLevelDesigner(MenuCommand menuCommand)
        {
            var newObj = new GameObject("Level Painter", typeof(LevelPainter));
            var newSector = new GameObject("Sector 1", typeof(Sector));
            GameObjectUtility.SetParentAndAlign(newSector, newObj);
            Undo.RegisterCreatedObjectUndo(newObj, "Create level painter");
        }

        /// <summary>
        /// Create a new sector as the child object of the level painter
        /// </summary>
        [MenuItem("GameObject/LevelDesigner/Sector", false, 10)]
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
        [MenuItem("GameObject/LevelDesigner/Sector", true)]
        private static bool CreateSectorValidate()
        {
            var selection = Selection.activeGameObject;
            return selection && selection.GetComponent<LevelPainter>();
        }
    }
}
