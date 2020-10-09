using System;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GamePlay
{
    public class Player : Character
    {
        public static Player Instance { get; private set; }

        private Action _onPlayerDestroyed;
        private Action<Vector2> _lookAction;

        [SerializeField]
        private PlayerData _data = null;
        [SerializeField]
        private GameObject _protector = null;
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
            _cameraCenterPos = new Vector2(Screen.width, Screen.height) / 2;
            _smoothMove = new SmoothMove(
                _data.movingVelocity, _data.movingAccelTime, _data.rotatingAccelTime);
            _hp = _data.hp;
            // Default look action is controlled by the mouse
            _lookAction = LookAtPointer;

            Instance = this;
        }

        private void Start()
        {
            _onPlayerDestroyed += LevelManager.Instance.GameOver;
        }

        public void OnInputDeviceChanged(PlayerInput input)
        {
            if (input.currentControlScheme.Equals("Gamepad"))
                _lookAction = LookByStick;
            else
                _lookAction = LookAtPointer;
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _movingDirection = context.ReadValue<Vector2>();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            _lookAction(context.ReadValue<Vector2>());
        }

        private void LookAtPointer(Vector2 pointerPos)
        {
            _lookingDeg = -Vector2.SignedAngle(Vector2.up, pointerPos - _cameraCenterPos);
        }

        private void LookByStick(Vector2 direction)
        {
            // Prevent the huge value when the stick is at the reset position
            if (Mathf.Abs(direction.x) > 1.0f)
                direction.x = 0.0f;
            if (Mathf.Abs(direction.y) > 1.0f)
                direction.y = 0.0f;

            if (direction.magnitude < 0.1)
                return;

            _lookingDeg = -Vector2.SignedAngle(Vector2.up, direction);
        }

        public void OnFire(InputAction.CallbackContext context)
        {
            if (context.started) {
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

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Bullet"))
                GetDamage();
        }

        private void FixedUpdate()
        {
            MoveAndLook();
        }

        public void ResetPlayer(Vector3 respawnPoint)
        {
            _hp = _data.hp;
            transform.position = respawnPoint;
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            if (!isActiveAndEnabled)
                gameObject.SetActive(true);
        }

        private void MoveAndLook()
        {
            var deltaDistance =
                _smoothMove.MoveDelta(_movingDirection, Time.deltaTime);
            var deltaDeg =
                _smoothMove.RotateDelta(transform.eulerAngles.y, _lookingDeg, Time.deltaTime);

            Move(new Vector3(deltaDistance.x, 0, deltaDistance.y));
            Look(deltaDeg);
        }

        private IEnumerator FireControl()
        {
            while (_isFiring) {
                Fire(_data.bullet.name, Vector3.forward, 1.5f);
                yield return new WaitForSeconds(_data.firingInterval);
            }

            _lastFiringCoroutine = null;
        }

        private void GetDamage()
        {
            if (--_hp == 0) {
                _onPlayerDestroyed();
                gameObject.SetActive(false);
            } else
                _protector.SetActive(true);
        }
    }
}
