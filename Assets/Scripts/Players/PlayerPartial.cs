using GameManagers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;


namespace Players
{
    public class PlayerPartial : MonoBehaviour
    {
        [Header("調整用パラメータ")]
        [Header("プレイヤーパラメータ")]
        [SerializeField] private CharacterParameters _defaultParameters;

        bool isAlive;

        private CharacterParameters _currentParameters;

        private PlayerCore _core;

        private CharacterController _characterController;

        private Vector3 _velocity = Vector3.zero;

        public bool isOnFloor = false;

        void Awake()
        {
            _currentParameters = _defaultParameters;
            _characterController = GetComponent<CharacterController>();
            _characterController.enabled = false;
        }

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Move(Vector3 direction)
        {
            if (!_characterController.enabled) return;
            var maxSpeed = _currentParameters.maxSpeed;
            var acceleration = _currentParameters.acceleration;
            var deceleration = _currentParameters.deceleration;

            if (direction.magnitude > 0)
            {
                _velocity += direction * acceleration * Time.deltaTime;
                if (_velocity.magnitude > maxSpeed)
                {
                    _velocity = _velocity.normalized * maxSpeed;
                }
            }
            else
            {
                if (_velocity.magnitude > 0)
                {
                    _velocity -= _velocity.normalized * deceleration * Time.deltaTime;
                    if (_velocity.magnitude <= 0.01f) { _velocity = Vector3.zero; }
                }
            }

            // direction.y = direction.y + (Physics.gravity.y * Time.deltaTime);
            // _characterController.Move(direction * _currentParameters.moveSpeed);

            var gravity = Physics.gravity.y * Time.deltaTime;
            Vector3 velocity = _velocity + new Vector3(0, gravity, 0);
            _characterController.Move(velocity);
        }

        public void TakeDamage(int damage)
        {
            _core.TakePartialDamage(damage);
        }

        public void SetCore(PlayerCore core)
        {
            _core = core;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<PlayerCharacter>(out var player))
            {
                player.UnitePartial(this);
            }
        }

        public void OnEnableCharacterController()
        {
            _characterController.enabled = true;
            isOnFloor = true;
            AudioManager.Instance.PlaySE("Main＿Soul＿On＿Floor");
        }
    }
}
