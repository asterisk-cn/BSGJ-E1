using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Players
{
    public class PlayerCore : MonoBehaviour
    {
        public bool isAlive;
        PlayerInputs _inputs;

        [SerializeField] private PlayerCharacter _character1;
        [SerializeField] private PlayerCharacter _character2;

        // Start is called before the first frame update
        void Start()
        {
            _inputs = GetComponentInParent<PlayerInputs>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void FixedUpdate()
        {
            Move();
        }

        void Move()
        {
            _character1.Move(_inputs.leftMoveStick);
            _character2.Move(_inputs.rightMoveStick);
        }

        void Attack()
        {

        }
    }
}
