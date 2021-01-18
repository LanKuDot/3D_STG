using UnityEngine;

namespace GamePlay
{
    public static class CursorManager
    {
        public static void ConfineCursor(bool toConfine)
        {
            Cursor.lockState =
                toConfine ? CursorLockMode.Confined : CursorLockMode.None;
        }

        public static void VisibleCursor(bool visible)
        {
            Cursor.visible = visible;
        }
    }
}
