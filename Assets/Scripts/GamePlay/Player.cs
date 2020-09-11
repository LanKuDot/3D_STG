using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GamePlay
{
    public class Player : Character
    {
        public PlayerData playerData;
        public Camera targetCamera;

        private Vector2 _movingDirection;
        private Vector2 _cameraCenterPos;
        private float _lookingDeg;
        private bool _isFiring = false;
        private Coroutine _lastFiringCoroutine;

        private void Start()
        {
            characterData = playerData.characterData;
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

        public void OnFire(InputAction.CallbackContext context)
        {
            if (context.performed) {
                // Prevent the last coroutine from living while it's waiting
                // and the _isFiring flag becomes true again.
                if (_lastFiringCoroutine != null)
                    StopCoroutine(_lastFiringCoroutine);
                _isFiring = true;
                _lastFiringCoroutine = StartCoroutine(Fire());
            } else if (context.canceled) {
                _isFiring = false;
            }
        }

        private void Update()
        {
            Move(_movingDirection);
            Look(_lookingDeg);
        }

        private IEnumerator Fire()
        {
            while (_isFiring) {
                var direction =
                    Quaternion.Euler(0, transform.eulerAngles.y, 0) * Vector3.forward;
                base.Fire(direction / direction.magnitude, 0);
                yield return new WaitForSeconds(playerData.firingInterval);
            }

            _lastFiringCoroutine = null;
        }
    }
}
