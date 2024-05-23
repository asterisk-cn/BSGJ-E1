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

        private CharacterController _controller;

        void Awake()
        {
            _currentParameters = _defaultParameters;
            _controller = GetComponent<CharacterController>();
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
            transform.position += direction * _currentParameters.moveSpeed;
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
