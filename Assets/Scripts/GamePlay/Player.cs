using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GamePlay
{
    public class Player : Character
    {
        public static Player Instance { get; private set; }

        /// <summary>
        /// The event will be invoked when the player's HP is changed
        /// </summary>
        public event Action<int> OnHPChanged;

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

        public bool takeNoDamage { set; get; } = false;

        private void Awake()
        {
            _cameraCenterPos = new Vector2(Screen.width, Screen.height) / 2;
            _smoothMove = new SmoothMove(
                _data.movingVelocity, _data.movingAccelTime, _data.rotatingAccelTime);
            _hp = _data.hp;
            _protector.GetComponent<Protector>().Initialize(_data.noDamageInterval);
            // Default look action is controlled by the mouse
            _lookAction = LookAtPointer;

            Instance = this;
        }

        #region Input System Event

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

        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Bullet"))
                GetDamage();
        }

        private void FixedUpdate()
        {
            MoveAndLook();
        }

        #region Player Logic

        public void ResetPlayer(Vector3 respawnPoint)
        {
            _hp = _data.hp;
            OnHPChanged?.Invoke(_hp);
            transform.position = respawnPoint;
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            if (!isActiveAndEnabled)
                gameObject.SetActive(true);
            if (_protector.activeSelf)
                _protector.SetActive(false);
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
            // If the protector is activated, get no damage
            if (_protector.activeSelf)
                return;

            if (!takeNoDamage && --_hp == 0) {
                LevelManager.Instance.GameOver();
                gameObject.SetActive(false);
            } else
                _protector.SetActive(true);

            OnHPChanged?.Invoke(_hp);
        }

        #endregion
    }
}
