using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;


namespace Players
{
    public class PlayerPartial : MonoBehaviour
    {
        bool isAlive;

        [SerializeField] private CharacterParameters _defaultParameters;
        private CharacterParameters _currentParameters;

        private PlayerCore _core;

        private CharacterController _characterController;

        private Vector3 _velocity = Vector3.zero;

        void Awake()
        {
            _currentParameters = _defaultParameters;
            _characterController = GetComponent<CharacterController>();
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
                    if (_velocity.magnitude < 0.001f) { _velocity = Vector3.zero; }
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
                Destroy(gameObject);
            }
        }
    }
}
