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

        }

        // Update is called once per frame
        void Update()
        {

        }

        void TakeDamage(int damage)
        {


        }

        void Die()
        {

        }

        void Move()
        {

        }
    }
}
