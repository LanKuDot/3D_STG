using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GamePlay
{
    public class Player : Character
    {
        public static Player Instance { get; private set; }

        [SerializeField]
        private PlayerData _data = null;
        [SerializeField]
        private Camera _targetCamera = null;
        private Vector2 _cameraCenterPos;

        private int _hp;
        private SmoothMove _smoothMove;
        private Vector2 _movingDirection;
        private float _lookingDeg;
        private bool _isFiring = false;
        private Coroutine _lastFiringCoroutine;

        private new void Awake()
        {
            base.Awake();
            _cameraCenterPos = _targetCamera.pixelRect.size / 2;
            _smoothMove = new SmoothMove(
                _data.movingVelocity, _data.movingAccelTime, _data.rotatingAccelTime);
            _hp = _data.hp;

            Instance = this;
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
                _lastFiringCoroutine = StartCoroutine(FireControl());
            } else if (context.canceled) {
                _isFiring = false;
            }
        }

        // This function is invoked only when it's hit by the bullet.
        private void OnTriggerEnter(Collider other)
        {
            GetDamage();
        }

        private void FixedUpdate()
        {
            MoveAndLook();
        }

        private void MoveAndLook()
        {
            var deltaDistance =
                _smoothMove.MoveDelta(_movingDirection, Time.fixedDeltaTime);
            var deltaDeg =
                _smoothMove.RotateDelta(transform.eulerAngles.y, _lookingDeg, Time.fixedDeltaTime);

            Move(new Vector3(deltaDistance.x, 0, deltaDistance.y));
            Look(deltaDeg);
        }

        private IEnumerator FireControl()
        {
            while (_isFiring) {
                base.Fire("playerBullet", Vector3.forward, 1.5f);
                yield return new WaitForSeconds(_data.firingInterval);
            }

            _lastFiringCoroutine = null;
        }

        private void GetDamage()
        {
            if (--_hp == 0) {
                gameObject.SetActive(false);
            }
        }
    }
}
