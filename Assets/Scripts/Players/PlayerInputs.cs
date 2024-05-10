using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Players
{
    public class PlayerInputs : MonoBehaviour
    {
        public Vector3 leftMoveStick;
        public Vector3 rightMoveStick;
        public bool attack;

        [SerializeField] private bool useJoycon = false;

        void FixedUpdate()
        {
            attack = false;

            if (useJoycon)
            {
                UpdateJoyconInputs();
                if (leftAccelaration.magnitude > 0.1f)
                {
                    attack = true;
                }
            }
        }

        public Vector3 leftAccelaration;
        public Vector3 rightAccelaration;

        private List<Joycon> _joycons;
        private float _minDeadZone = 0.125f;
        private float _maxDeadZone = 0.925f;

        void Start()
        {
            _joycons = JoyconManager.Instance.j;
        }

        void OnLeftMove(InputValue value)
        {
            var axis = value.Get<Vector2>();

            leftMoveStick = new Vector3(axis.x, 0, axis.y);
        }

        void OnRightMove(InputValue value)
        {
            var axis = value.Get<Vector2>();

            rightMoveStick = new Vector3(axis.x, 0, axis.y);
        }

        void OnFire(InputValue value)
        {
            attack = value.isPressed;
        }

        void UpdateJoyconInputs()
        {
            if (_joycons.Count > 0)
            {
                Joycon joycon = _joycons[0];

                Vector2 _tmp = new Vector2(joycon.GetStick()[0], joycon.GetStick()[1]);
                if (_tmp.magnitude < _minDeadZone)
                {
                    _tmp = Vector2.zero;
                }
                else if (_tmp.magnitude > _maxDeadZone)
                {
                    _tmp.Normalize();
                }
                leftMoveStick = new Vector3(_tmp.x, 0, _tmp.y);

                leftAccelaration = joycon.GetAccel();

            }

            if (_joycons.Count > 1)
            {
                Joycon joycon = _joycons[1];

                Vector2 _tmp = new Vector2(joycon.GetStick()[0], joycon.GetStick()[1]);
                if (_tmp.magnitude < _minDeadZone)
                {
                    _tmp = Vector2.zero;
                }
                else if (_tmp.magnitude > _maxDeadZone)
                {
                    _tmp.Normalize();
                }
                rightMoveStick = new Vector3(_tmp.x, 0, _tmp.y);

                rightAccelaration = joycon.GetAccel();
            }
        }

    }
}
