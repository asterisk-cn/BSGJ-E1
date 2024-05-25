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

        private CharacterController _characterController;

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
            direction.y = direction.y + (Physics.gravity.y * Time.deltaTime);
            _characterController.Move(direction * _currentParameters.moveSpeed);
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<PlayerCharacter>(out var player))
            {
                player.UnitePartial(this);
                Destroy(gameObject);
            }
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent<PlayerCharacter>(out var player))
            {
                player.UnitePartial(this);
                Destroy(gameObject);
            }
        }

        void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (hit.collider.TryGetComponent<PlayerCharacter>(out var player))
            {
                player.UnitePartial(this);
                Destroy(gameObject);
            }
        }
    }
}
