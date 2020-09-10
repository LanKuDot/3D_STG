using UnityEngine;
using UnityEngine.InputSystem;

namespace GamePlay
{
    public class Player : Character
    {
        public Camera targetCamera;

        private Vector2 _movingDirection;
        private float _lookingDeg;
        private Vector2 _cameraCenterPos;

        private void Start()
        {
            _cameraCenterPos = targetCamera.pixelRect.size / 2;
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _movingDirection = context.ReadValue<Vector2>();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            var pointerPos = context.ReadValue<Vector2>();
            _lookingDeg = -Vector2.SignedAngle(Vector2.up, pointerPos - _cameraCenterPos);
        }

        private void Update()
        {
            Move(_movingDirection);
            Look(_lookingDeg);
        }
    }
}
