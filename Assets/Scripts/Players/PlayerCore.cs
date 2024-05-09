using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Players
{
    public class PlayerCore : MonoBehaviour
    {
        public bool isAlive;
        PlayerInputs _inputs;

        

        private PlayerCharacter _character1;
        private PlayerCharacter _character2;

        CharacterParameters _defaultParameters;
        CharacterParameters _currentParameters;

        // Start is called before the first frame update
        void Start()
        {
            _inputs = GetComponentInParent<PlayerInputs>();

            _defaultParameters = new CharacterParameters();
            _currentParameters = new CharacterParameters();

            _defaultParameters.moveSpeed = 0.1f;
            _defaultParameters.health = 10;

        }

        // Update is called once per frame
        void Update()
        {
            
        }

        

        private void FixedUpdate()
        {
            if (this.gameObject.CompareTag("Player"))
            {
                LeftMove();
            }

            if (this.gameObject.CompareTag("Player2"))
            {
                RightMove();
            }
        }


        void Move()
        {

        }
       

        void LeftMove()
        {
            transform.localPosition += _inputs.leftMoveStick * _defaultParameters.moveSpeed;
        }

        void RightMove()
        {
            transform.localPosition += _inputs.rightMoveStick * _defaultParameters.moveSpeed;
        }

        void Attack()
        {

        }
    }
}
