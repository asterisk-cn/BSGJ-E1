using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Players
{
    public class PlayerParameters
    {
        public float moveSpeed;
        public int health;
        public float powerGauge;
    }

    public class PlayerCore : MonoBehaviour
    {
        public bool isAlive;
        PlayerInputs _inputs;
        PlayerParameters _defaultParameters;
        PlayerParameters _currentParameters;

        // Start is called before the first frame update
        void Start()
        {
            _inputs = GetComponent<PlayerInputs>();
            _defaultParameters = new PlayerParameters();
            _currentParameters = new PlayerParameters();

            _defaultParameters.moveSpeed = 0.1f;
        }

        // Update is called once per frame
        void Update()
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

        void TakeDamage(int damage)
        {


        }

        void Die()
        {

        }

        void LeftMove()
        {
            transform.localPosition += _inputs.leftMoveStick * _defaultParameters.moveSpeed;
        }

        void RightMove()
        {
            transform.localPosition += _inputs.rightMoveStick * _defaultParameters.moveSpeed; ;
        }
    }
}
