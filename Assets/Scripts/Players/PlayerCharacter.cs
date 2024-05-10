using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;


namespace Players
{
    [System.Serializable]
    public class CharacterParameters
    {
        public float moveSpeed;
        public int health;
    }

    public class PlayerCharacter : MonoBehaviour
    {
        bool isAlive;

        [SerializeField] private CharacterParameters _defaultParameters;
        private CharacterParameters _currentParameters;

        // Start is called before the first frame update
        void Start()
        {
            _currentParameters = _defaultParameters;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void TakeDamage(int damage)
        {


        }

        public void Move(Vector3 direction)
        {
            transform.position += direction * _currentParameters.moveSpeed;
        }

        public void Die()
        {

        }
    }
}

